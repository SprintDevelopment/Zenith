using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Zenith.Assets.Extensions;
using Zenith.Models;
using Zenith.Repositories;
using System.Reactive.Linq;
using Zenith.Assets.Values.Enums;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Attributes;
using Zenith.Views.CreateOrUpdateViews;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using AutoMapper;
using Zenith.Assets.Utils;
using DynamicData.Binding;
using System.Threading.Tasks;

namespace Zenith.ViewModels.ListViewModels
{
    public class BaseListViewModel<T> : BaseViewModel<T> where T : Model, new()
    {
        public BaseListViewModel(Repository<T> repository, SearchBaseDto searchModel, IObservable<Func<T, bool>> criteria, PermissionTypes permissionType)
        {
            var singleMultipleTitles = new
            {
                single = (string)App.Current.Resources[$"SingleResources.{typeof(T).Name}"],
                multiple = (string)App.Current.Resources[$"MultipleResources.{typeof(T).Name}"]
            };

            ViewTitle = singleMultipleTitles.multiple;
            Repository = repository;
            SearchModel = searchModel;

            repository._context.ChangeTracker.DetectedEntityChanges += (s, e) =>
            {
                if (e.Entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified && e.Entry.Entity.GetType() == typeof(T))
                {
                    //e.Entry.Reload();
                    RefreshSourceList(((T)e.Entry.Entity).GetKeyPropertyValue());
                }
            };

            var allItemsSelectedString = App.MainViewModel.Language == AppLanguages.English ?
                " , all items selected)" : " ، تمامی موارد انتخاب شده)";

            var nItemsSelectedStringFormat = App.MainViewModel.Language == AppLanguages.English ?
                " , {0:n0} item(s) selected)" : " ، {0:n0} مورد انتخاب شده)";

            SourceList.Connect()
                .Filter(criteria, ListFilterPolicy.ClearAndReplace)
                .Transform((item, i) => { item.DisplayOrder = i + 1; return item; })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(ActiveList)
                .Do(changeSet => { if (IsBusy) ItemsStatistics = $"({ActiveList.Count:n0} {singleMultipleTitles.single})"; })
                .Subscribe();

            //SourceList.AddRange(Repository.All());

            SelectAllCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                SelectionMode = SelectionMode != SelectionModes.AllItemsSelected ? SelectionModes.AllItemsSelected : SelectionModes.NoItemSelected;
                foreach (var item in ActiveList)
                {
                    item.IsSelected = SelectionMode == SelectionModes.AllItemsSelected;
                }
            });

            SelectOneCommand = ReactiveCommand.Create<T>(item =>
            {
                item.IsSelected = !item.IsSelected;
            });

            SelectAllCommand.Merge(SelectOneCommand).Subscribe(_ =>
            {
                searchModel.OnlyForRefreshAfterUpdate++;

                var itemsCount = ActiveList.Count();
                var selectedItemsCount = ActiveList.Count(item => item.IsSelected);

                ItemsStatistics = $"({itemsCount:n0} {singleMultipleTitles.single}";
                if (itemsCount == selectedItemsCount || selectedItemsCount == 0)
                {
                    ItemsStatistics += selectedItemsCount > 0 ? allItemsSelectedString : ")";
                    SelectionMode = selectedItemsCount == 0 ? SelectionModes.NoItemSelected : SelectionModes.AllItemsSelected;
                }
                else
                {
                    ItemsStatistics += string.Format(nItemsSelectedStringFormat, selectedItemsCount);
                    SelectionMode = SelectionModes.SomeItemsSelected;
                }
            });

            IDisposable removeDisposable = null;
            RemoveCommand = ReactiveCommand.Create<T, bool>(_ =>
            {
                var selectedItemsCount = ActiveList.Count(x => x.IsSelected);
                var titleTextAndChoices = new
                {
                    titleSingle = App.MainViewModel.Language == AppLanguages.English ? $"Deleting one {singleMultipleTitles.single}" : $"حذف یک {singleMultipleTitles.single}",
                    titleMultiple = App.MainViewModel.Language == AppLanguages.English ? $"Deleting multiple {singleMultipleTitles.multiple}" : $"حذف چند {singleMultipleTitles.single}",
                    textSingle = App.MainViewModel.Language == AppLanguages.English ? $"Are you sure to delete {ActiveList.FirstOrDefault(x => x.IsSelected)} ?" : $"آیا از حذف {ActiveList.FirstOrDefault(x => x.IsSelected)} اطمینان دارید ؟",
                    textMultiple = App.MainViewModel.Language == AppLanguages.English ? $"Are you sure to delete {selectedItemsCount:n0} {singleMultipleTitles.multiple} ?" : $"آیا از حذف {selectedItemsCount:n0} {singleMultipleTitles.single} اطمینان دارید ؟",
                    contentYes = App.MainViewModel.Language == AppLanguages.English ? "Yes, delete" : "بله، حذف شود",
                    contentNo = App.MainViewModel.Language == AppLanguages.English ? "No, cancel" : "خیر، حذف نشود"
                };

                var dialogDto = new DialogDto()
                {
                    DialogType = DialogTypes.Danger,
                    Title = selectedItemsCount == 1 ? titleTextAndChoices.titleSingle : titleTextAndChoices.titleMultiple,
                    Text = selectedItemsCount == 1 ? titleTextAndChoices.textSingle : titleTextAndChoices.textMultiple,
                    Choices = new List<DialogChoiceDto>()
                    {
                        new DialogChoiceDto { DialogResult = DialogResults.Yes, Text = titleTextAndChoices.contentYes },
                        new DialogChoiceDto { DialogResult = DialogResults.No, Text = titleTextAndChoices.contentNo },
                    }
                };

                removeDisposable?.Dispose();
                App.MainViewModel.ShowDialogCommand.Execute(dialogDto).Subscribe();
                removeDisposable = App.MainViewModel.WhenAnyValue(vm => vm.DialogResult).Where(dr => dr == DialogResults.Yes).Subscribe(dialogResult =>
                {
                    var toRemoveItems = ActiveList.Where(x => x.IsSelected);
                    Repository.RemoveRange(toRemoveItems);
                    //Repository.SaveChanges();

                    SourceList.RemoveMany(toRemoveItems);
                });
                return false;
            }, this.WhenAnyValue(vm => vm.SelectionMode)
                .Select(selectionMode => selectionMode != SelectionModes.NoItemSelected)
                .CombineLatest(App.MainViewModel.WhenAnyValue(mvm => mvm.LoggedInUser)
                .Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == permissionType && p.HasDeleteAccess)))
                .Select(combined => combined.First && combined.Second));

            IDisposable createUpdateDisposable = null;
            CreateCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = CreateUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    if (!changeSet.IsNullOrEmpty())
                    {
                        SourceList.AddRange(changeSet);
                    }
                    App.MainViewModel.CreateUpdatePageReturnedCommand.Execute().Subscribe();
                });

                CreateUpdatePage.ViewModel.PrepareCommand.Execute().Subscribe();
                App.MainViewModel.ShowCreateUpdatePageCommand.Execute(CreateUpdatePage).Subscribe();
            }, App.MainViewModel.WhenAnyValue(mvm => mvm.LoggedInUser)
                .Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == permissionType && p.HasCreateAccess)));

            UpdateCommand = ReactiveCommand.Create<T>(itemToUpdate =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = CreateUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    //RefreshSourceList(changeSet);

                    App.MainViewModel.CreateUpdatePageReturnedCommand.Execute().Subscribe();
                });

                CreateUpdatePage.ViewModel.PrepareCommand.Execute(itemToUpdate.GetKeyPropertyValue()).Subscribe();
                App.MainViewModel.ShowCreateUpdatePageCommand.Execute(CreateUpdatePage).Subscribe();
            }, App.MainViewModel.WhenAnyValue(mvm => mvm.LoggedInUser)
                .Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == permissionType && p.HasUpdateAccess)));

            SearchCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                App.MainViewModel.IsSearchVisible = !App.MainViewModel.IsSearchVisible;
            });

            //searchModel.Title = $"Search in {modelAttributes.MultipleResourceName}";
            InitiateSearchCommand = ReactiveCommand.CreateFromObservable<Unit, SearchBaseDto>(_ => App.MainViewModel.InitiateSearchCommand.Execute(searchModel));

            DisposeCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                SelectAllCommand?.Dispose();
                SelectOneCommand?.Dispose();
                CreateCommand?.Dispose();
                UpdateCommand?.Dispose();
                RemoveCommand?.Dispose();
                SearchCommand?.Dispose();
                removeDisposable?.Dispose();
            });

            LoadAll = ReactiveCommand.CreateRunInBackground<Unit>(async _ =>
            {
                IsBusy = true;
                if (Repository is SaleRepository saleRepository)
                    await foreach (var item in Repository.AllAsync())
                    {
                        SourceList.Add(item);
                    }
                IsBusy = false;
            });
        }

        private void RefreshSourceList(dynamic entityId)
        {
            T updatedEntity = Repository.Single(entityId);
            //if (!changeSet.IsNullOrEmpty())
            //{
            //    var changeSetKeyValues = changeSet.Select(c =>
            //    {
            var itemToUpdate = SourceList.Items.FirstOrDefault(item => item.GetKeyPropertyValue().ToString() == updatedEntity.GetKeyPropertyValue().ToString());
            if (itemToUpdate != null)
            {
                //changeSet.FirstOrDefault().IsSelected = itemToUpdate.IsSelected;
                //SourceList.Replace(itemToUpdate, changeSet.FirstOrDefault());
                updatedEntity.IsSelected = itemToUpdate.IsSelected;
                SourceList.Replace(itemToUpdate, updatedEntity);
            }

            //    return c;
            //}).ToList();

            SearchModel.OnlyForRefreshAfterUpdate++;
            //}
        }

        [Reactive]
        public bool IsBusy { get; set; }

        public SourceList<T> SourceList { get; set; } = new SourceList<T>();
        public IObservableCollection<T> ActiveList { get; } = new ObservableCollectionExtended<T>();

        public ReactiveCommand<Unit, Unit> LoadAll { get; set; }
        public ReactiveCommand<Unit, Unit> SelectAllCommand { get; set; }
        public ReactiveCommand<T, Unit> SelectOneCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateCommand { get; set; }
        public ReactiveCommand<T, Unit> UpdateCommand { get; set; }
        public ReactiveCommand<T, bool> RemoveCommand { get; set; }
        public ReactiveCommand<Unit, SearchBaseDto> InitiateSearchCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; set; }
        public ReactiveCommand<Unit, Unit> DisposeCommand { get; set; }

        [Reactive]
        public SelectionModes SelectionMode { get; set; }

        public SearchBaseDto SearchModel { get; set; }
        public BaseCreateOrUpdatePage<T> CreateUpdatePage { get; set; }
        public Repository<T> Repository { get; set; }

        [Reactive]
        public T SummaryItem { get; set; }
    }
}

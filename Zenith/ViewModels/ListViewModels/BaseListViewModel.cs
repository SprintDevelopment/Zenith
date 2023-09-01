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

namespace Zenith.ViewModels.ListViewModels
{
    public class BaseListViewModel<T> : BaseViewModel<T> where T : Model, new()
    {
        public BaseListViewModel(Repository<T> repository, SearchBaseDto searchModel, IObservable<Func<T, bool>> criteria, PermissionTypes permissionType)
        {
            var modelAttributes = typeof(T).GetAttribute<ModelAttribute>();
            ViewTitle = modelAttributes.MultipleName;
            Repository = repository;

            SourceList.AddRange(Repository.All());
            void calculate()
            {
                var itemsCount = ActiveList.Count();
                var selectedItemsCount = ActiveList.Count(item => item.IsSelected);

                ItemsStatistics = $"({itemsCount:n0} {modelAttributes.SingleName}";
                if (itemsCount == selectedItemsCount || selectedItemsCount == 0)
                {
                    ItemsStatistics += selectedItemsCount > 0 ? $" , all itmes selected)" : ")";
                    SelectionMode = selectedItemsCount == 0 ? SelectionModes.NoItemSelected : SelectionModes.AllItemsSelected;
                }
                else
                {
                    ItemsStatistics += $" , {selectedItemsCount:n0} item(s) selected)";
                    SelectionMode = SelectionModes.SomeItemsSelected;
                }
            };

            SourceList.Connect()
                .Filter(criteria, ListFilterPolicy.ClearAndReplace)
                .Transform((item, i) => { item.DisplayOrder = i + 1; return item; })
                .Bind(out ActiveList)
                .Do(_ => calculate())
                .Subscribe();

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
                calculate();
            });

            IDisposable removeDisposable = null;
            RemoveCommand = ReactiveCommand.Create<T, bool>(_ =>
            {
                var selectedItemsCount = ActiveList.Count(x => x.IsSelected);
                var dialogDto = new DialogDto()
                {
                    DialogType = DialogTypes.Danger,
                    Title = selectedItemsCount == 1 ? $"Deleting one {modelAttributes.SingleName}" : $"Deleting multiple {modelAttributes.SingleName}",
                    Text = selectedItemsCount == 1 ? $"Are you sure to delete {ActiveList.FirstOrDefault(x => x.IsSelected)} ?" : $"Are you sure to delete {selectedItemsCount:n0} {modelAttributes.SingleName} items ?",
                    Choices = new List<DialogChoiceDto>()
                    {
                        new DialogChoiceDto { DialogResult = DialogResults.Yes, Text = "Yes, delete" },
                        new DialogChoiceDto { DialogResult = DialogResults.No, Text = "No, cancel" },
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
                    if (!changeSet.IsNullOrEmpty())
                    {
                        MapperUtil.Mapper.Map(changeSet.FirstOrDefault(), itemToUpdate);
                    }

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

            searchModel.Title = $"Search in {modelAttributes.MultipleName}";
            InitiateSearchCommand = ReactiveCommand.CreateFromObservable<Unit, SearchBaseDto>(_ =>App.MainViewModel.InitiateSearchCommand.Execute(searchModel));

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
        }

        public SourceList<T> SourceList { get; set; } = new SourceList<T>();
        public ReadOnlyObservableCollection<T> ActiveList;

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

        public T SearchModel { get; set; }
        public BaseCreateOrUpdatePage<T> CreateUpdatePage { get; set; }
        public Repository<T> Repository { get; set; }
    }
}

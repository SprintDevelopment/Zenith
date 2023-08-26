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
        public BaseListViewModel(Repository<T> repository, SearchBaseDto searchModel, IObservable<Func<T, bool>> criteria)
        {
            var modelAttributes = typeof(T).GetAttribute<ModelAttribute>();
            ViewTitle = $"لیست {modelAttributes.MultipleName}";
            Repository = repository;

            SourceList.AddRange(Repository.All());
            void calculate()
            {
                var itemsCount = ActiveList.Count();
                var selectedItemsCount = ActiveList.Count(item => item.IsSelected);

                ItemsStatistics = $"({itemsCount:n0} {modelAttributes.SingleName}";
                if (itemsCount == selectedItemsCount || selectedItemsCount == 0)
                {
                    ItemsStatistics += selectedItemsCount > 0 ? $" ، همه موارد انتخاب شده)" : ")";
                    SelectionMode = selectedItemsCount == 0 ? SelectionModes.NoItemSelected : SelectionModes.AllItemsSelected;
                }
                else
                {
                    ItemsStatistics += $" ، {selectedItemsCount:n0} مورد انتخاب شده)";
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
                    Title = selectedItemsCount == 1 ? $"حذف یک {modelAttributes.SingleName}" : $"حذف چند {modelAttributes.SingleName}",
                    Text = selectedItemsCount == 1 ? $"آیا می خواهید {ActiveList.FirstOrDefault(x => x.IsSelected)} حذف شود ؟" : $"آیا می خواهید تعداد {selectedItemsCount:n0} {modelAttributes.SingleName} حذف شود ؟",
                    Choices = new List<DialogChoiceDto>()
                    {
                        new DialogChoiceDto { DialogResult = DialogResults.Yes, Text = "بله، حذف شود" },
                        new DialogChoiceDto { DialogResult = DialogResults.No, Text = "خیر، حذف نشود" },
                    }
                };

                removeDisposable?.Dispose();
                App.MainViewModel.ShowDialog.Execute(dialogDto).Subscribe();
                removeDisposable = App.MainViewModel.WhenAnyValue(vm => vm.DialogResult).Where(dr => dr == DialogResults.Yes).Subscribe(dialogResult =>
                {
                    var toRemoveItems = ActiveList.Where(x => x.IsSelected);
                    Repository.RemoveRange(toRemoveItems);
                    //Repository.SaveChanges();

                    SourceList.RemoveMany(toRemoveItems);
                });
                return false;
            }, this.WhenAnyValue(vm => vm.SelectionMode).Select(selectionMode => selectionMode != SelectionModes.NoItemSelected));

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
                    App.MainViewModel.CreateUpdatePageReturned.Execute().Subscribe();
                });

                CreateUpdatePage.ViewModel.PrepareCommand.Execute().Subscribe();
                App.MainViewModel.ShowCreateUpdatePage.Execute(CreateUpdatePage).Subscribe();
            });

            UpdateCommand = ReactiveCommand.Create<T>(itemToUpdate =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = CreateUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    if (!changeSet.IsNullOrEmpty())
                    {
                        MapperUtil.Mapper.Map(changeSet.FirstOrDefault(), itemToUpdate);
                    }

                    App.MainViewModel.CreateUpdatePageReturned.Execute().Subscribe();
                });

                CreateUpdatePage.ViewModel.PrepareCommand.Execute(itemToUpdate.GetKeyPropertyValue()).Subscribe();
                App.MainViewModel.ShowCreateUpdatePage.Execute(CreateUpdatePage).Subscribe();
            });

            SearchCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                App.MainViewModel.IsSearchVisible = !App.MainViewModel.IsSearchVisible;
            });

            searchModel.Title = $"جستجو در لیست {modelAttributes.MultipleName}";
            InitiateSearchCommand = ReactiveCommand.CreateFromObservable<Unit, SearchBaseDto>(_ =>App.MainViewModel.InitiateSearch.Execute(searchModel));

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

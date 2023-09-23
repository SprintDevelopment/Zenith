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

namespace Zenith.ViewModels.ReportViewModels
{
    public class BaseReportViewModel<T> : BaseViewModel<T> where T : Model, new()
    {
        public BaseReportViewModel(Repository<T> repository, SearchBaseDto searchModel, IObservable<Func<T, bool>> criteria, PermissionTypes permissionType)
        {
            var singleMultipleTitles = new
            {
                single = (string)App.Current.Resources[$"SingleResources.{typeof(T).Name}"],
                multiple = (string)App.Current.Resources[$"MultipleResources.{typeof(T).Name}"]
            };

            ViewTitle = singleMultipleTitles.multiple;
            Repository = repository;
            SearchModel = searchModel;

            //var allItemsSelectedString = App.MainViewModel.Language == AppLanguages.English ?
            //    " , all items selected)" : " ، تمامی موارد انتخاب شده)";

            //var nItemsSelectedStringFormat = App.MainViewModel.Language == AppLanguages.English ?
            //    " , {0:n0} item(s) selected)" : " ، {0:n0} مورد انتخاب شده)";

            //SourceList.AddRange(Repository.All());
            //void calculate()
            //{
            //    var itemsCount = ActiveList.Count();
            //    var selectedItemsCount = ActiveList.Count(item => item.IsSelected);

            //    ItemsStatistics = $"({itemsCount:n0} {singleMultipleTitles.single}";
            //    if (itemsCount == selectedItemsCount || selectedItemsCount == 0)
            //    {
            //        ItemsStatistics += selectedItemsCount > 0 ? allItemsSelectedString : ")";
            //        SelectionMode = selectedItemsCount == 0 ? SelectionModes.NoItemSelected : SelectionModes.AllItemsSelected;
            //    }
            //    else
            //    {
            //        ItemsStatistics += string.Format(nItemsSelectedStringFormat,selectedItemsCount);
            //        SelectionMode = SelectionModes.SomeItemsSelected;
            //    }
            //};

            SourceList.Connect()
                .Filter(criteria, ListFilterPolicy.ClearAndReplace)
                .Transform((item, i) => { item.DisplayOrder = i + 1; return item; })
                .Bind(ActiveList)
                .Do(_ => calculate())
                .Subscribe();

            ActiveList.ObserveCollectionChanges().Do(_ => { }).Subscribe();

            //SelectAllCommand = ReactiveCommand.Create<Unit>(_ =>
            //{
            //    SelectionMode = SelectionMode != SelectionModes.AllItemsSelected ? SelectionModes.AllItemsSelected : SelectionModes.NoItemSelected;
            //    foreach (var item in ActiveList)
            //    {
            //        item.IsSelected = SelectionMode == SelectionModes.AllItemsSelected;
            //    }
            //});

            //SelectOneCommand = ReactiveCommand.Create<T>(item =>
            //{
            //    item.IsSelected = !item.IsSelected;
            //});

            //SelectAllCommand.Merge(SelectOneCommand).Subscribe(_ =>
            //{
            //    searchModel.OnlyForRefreshAfterUpdate++;

            //    calculate();
            //});

            IDisposable viewItemDisposable = null;
            ViewCommand = ReactiveCommand.Create<T>(itemToUpdate =>
            {
                viewItemDisposable?.Dispose();

                viewItemDisposable = CreateUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                });

                CreateUpdatePage.ViewModel.PrepareCommand.Execute(itemToUpdate.GetKeyPropertyValue()).Subscribe();
                App.MainViewModel.ShowCreateUpdatePageCommand.Execute(CreateUpdatePage).Subscribe();
            }, App.MainViewModel.WhenAnyValue(mvm => mvm.LoggedInUser)
                .Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == permissionType && p.HasUpdateAccess)));

            SearchCommand = ReactiveCommand.Create<Unit>(_ =>
            {
            });

            //searchModel.Title = $"Search in {modelAttributes.MultipleResourceName}";
            //InitiateSearchCommand = ReactiveCommand.CreateFromObservable<Unit, SearchBaseDto>(_ =>App.MainViewModel.InitiateSearchCommand.Execute(searchModel));

            DisposeCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                //SelectAllCommand?.Dispose();
                //SelectOneCommand?.Dispose();
                ViewCommand?.Dispose();
                SearchCommand?.Dispose();
            });
        }

        public SourceList<T> SourceList { get; set; } = new SourceList<T>();
        public IObservableCollection<T> ActiveList { get; } = new ObservableCollectionExtended<T>();

        //public ReactiveCommand<Unit, Unit> SelectAllCommand { get; set; }
        //public ReactiveCommand<T, Unit> SelectOneCommand { get; set; }
        public ReactiveCommand<T, Unit> ViewCommand { get; set; }
        //public ReactiveCommand<Unit, SearchBaseDto> InitiateSearchCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; set; }
        public ReactiveCommand<Unit, Unit> DisposeCommand { get; set; }

        [Reactive]
        public SelectionModes SelectionMode { get; set; }

        public SearchBaseDto SearchModel { get; set; }
        public BaseCreateOrUpdatePage<T> CreateUpdatePage { get; set; }
        public Repository<T> Repository { get; set; }
    }
}

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
using Zenith.Models.ReportModels;
using Zenith.Repositories.ReportRepositories;

namespace Zenith.ViewModels.ReportViewModels
{
    public class BaseReportViewModel<T> : BaseViewModel<T> where T : ReportModel, new()
    {
        public BaseReportViewModel(ReportRepository<T> repository, BaseDto searchModel, PermissionTypes permissionType)
        {
            ViewTitle = (string)App.Current.Resources[$"ReportViewTitleResources.{typeof(T).Name}"];
            SearchGridTitle = (string)App.Current.Resources[$"ReportSearchGridTitleResources.{typeof(T).Name}"];

            Repository = repository;
            SearchModel = searchModel;

            //var allItemsSelectedString = App.MainViewModel.Language == AppLanguages.English ?
            //    " , all items selected)" : " ، تمامی موارد انتخاب شده)";

            //var nItemsSelectedStringFormat = App.MainViewModel.Language == AppLanguages.English ?
            //    " , {0:n0} item(s) selected)" : " ، {0:n0} مورد انتخاب شده)";

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
                .Transform((item, i) => { item.DisplayOrder = i + 1; return item; })
                .Bind(ActiveList)
                //.Do(_ => calculate())
                .Subscribe();

            ActiveList.ObserveCollectionChanges().Do(_ => { }).Subscribe();

            IDisposable viewItemDisposable = null;
            ViewCommand = ReactiveCommand.Create<T>(itemToUpdate =>
            {
                //viewItemDisposable?.Dispose();

                //viewItemDisposable = CreateUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                //{
                //});

                //CreateUpdatePage.ViewModel.PrepareCommand.Execute(itemToUpdate.GetKeyPropertyValue()).Subscribe();
                //App.MainViewModel.ShowCreateUpdatePageCommand.Execute(CreateUpdatePage).Subscribe();
            }, App.MainViewModel.WhenAnyValue(mvm => mvm.LoggedInUser)
                .Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == permissionType && p.HasUpdateAccess)));

            CreateReportCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                SourceList.Clear();
                SourceList.AddRange(Repository.Find(searchModel));

                IsInSearchMode = false;
            }, searchModel.ValidationContext.WhenAnyValue(context => context.IsValid));

            HideSearchGridCommand = ReactiveCommand.Create<Unit>(_ => IsInSearchMode = false);

            DisposeCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                ViewCommand?.Dispose();
                CreateReportCommand?.Dispose();
            });
        }

        public SourceList<T> SourceList { get; set; } = new SourceList<T>();
        public IObservableCollection<T> ActiveList { get; } = new ObservableCollectionExtended<T>();

        public ReactiveCommand<T, Unit> ViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> HideSearchGridCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateReportCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PrintCommand { get; set; }
        public ReactiveCommand<Unit, Unit> DisposeCommand { get; set; }

        [Reactive]
        public string SearchGridTitle { get; set; }

        [Reactive]
        public bool IsInSearchMode { get; set; }
        public BaseDto SearchModel { get; set; }
        //public BaseCreateOrUpdatePage<T> CreateUpdatePage { get; set; }
        public ReportRepository<T> Repository { get; set; }
    }
}

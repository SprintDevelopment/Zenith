﻿using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Models.SearchModels;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for BuyListPage.xaml
    /// </summary>
    public partial class BuyListPage : BaseListPage<Buy>
    {
        public BuyListPage()
        {
            InitializeComponent();
            var searchModel = new BuySearchModel();

            IObservable<Func<Buy, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Buy, bool>(buy =>
                    (s.CompanyId == 0 || buy.CompanyId == s.CompanyId) &&
                    (s.DateRange == DateRanges.DontCare || buy.DateTime.IsInDateRange(s.DateRange))));

            ViewModel = new BaseListViewModel<Buy>(new BuyRepository(), searchModel, dynamicFilter, PermissionTypes.Buys)
            {
                CreateUpdatePage = new BuyPage()
            };

            this.WhenActivated(d => 
            { 
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                ViewModel.SummaryItem = new Buy();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.Price = ViewModel.ActiveList.Sum(i => i.Price);
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}

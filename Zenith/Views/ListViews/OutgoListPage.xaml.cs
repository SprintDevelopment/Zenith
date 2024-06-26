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
    /// Interaction logic for OutgoListPage.xaml
    /// </summary>
    public partial class OutgoListPage : BaseListPage<Outgo>
    {
        public OutgoListPage()
        {
            InitializeComponent();
            var searchModel = new OutgoSearchModel();

            IObservable<Func<Outgo, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Outgo, bool>(outgo =>
                    (s.OutgoCategoryId == 0 || outgo.OutgoCategoryId == s.OutgoCategoryId) &&
                    (s.CompanyId == 0 || outgo.CompanyId == s.CompanyId) &&
                    (s.DateRange == DateRanges.DontCare || outgo.DateTime.IsInDateRange(s.DateRange))));

            ViewModel = new OutgoListViewModel(new OutgoRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new OutgoPage()
            };

            this.WhenActivated(d => 
            { 
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                ViewModel.SummaryItem = new Outgo();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.TotalOutgoValue = ViewModel.ActiveList.Where(o => o.OutgoType != OutgoTypes.UseConsumables).Sum(i => i.TotalOutgoValue);
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}

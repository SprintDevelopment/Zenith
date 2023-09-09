﻿using ReactiveUI;
using System;
using System.Linq;
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

            IObservable<Func<Buy, bool>> dynamicFilter = searchModel.WhenAnyValue(b => b.CompanyId, n => n.OnlyForRefreshAfterUpdate)
                .Select(x => x.Item1)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(companyId => new Func<Buy, bool>(b => companyId == 0 || b.CompanyId == companyId));

            ViewModel = new BaseListViewModel<Buy>(new BuyRepository(), searchModel, dynamicFilter, PermissionTypes.Buys)
            {
                CreateUpdatePage = new BuyPage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

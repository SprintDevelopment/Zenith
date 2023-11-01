using DynamicData;
using DynamicData.Binding;
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
    /// Interaction logic for SaleListPage.xaml
    /// </summary>
    public partial class SaleListPage : BaseListPage<Sale>
    {
        public SaleListPage()
        {
            InitializeComponent();
            var searchModel = new SaleSearchModel();

            IObservable<Func<Sale, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Sale, bool>(sale =>
                    (s.CompanyId == 0 || sale.CompanyId == s.CompanyId) &&
                    (s.DateRange == DateRanges.DontCare || sale.DateTime.IsInDateRange(s.DateRange))));

            ViewModel = new SaleListViewModel(new SaleRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new SalePage()
            };

            this.WhenActivated(d =>
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                ViewModel.SummaryItem = new Sale();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.Price = ViewModel.ActiveList.Sum(i => i.Price);
                        ViewModel.SummaryItem.DeliveryFee = ViewModel.ActiveList.Sum(i => i.DeliveryFee);
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}

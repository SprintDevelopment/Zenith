using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Models.SearchModels;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for DeliveryListPage.xaml
    /// </summary>
    public partial class DeliveryListPage : BaseListPage<Delivery>
    {
        public DeliveryListPage()
        {
            InitializeComponent();

            var searchModel = new DeliverySearchModel();

            IObservable<Func<Delivery, bool>> dynamicFilter = searchModel
            .WhenAnyPropertyChanged()
            .WhereNotNull()
            .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
            .Select(s => new Func<Delivery, bool>(delivery =>
                (s.DeliveryNumber.IsNullOrWhiteSpace() || delivery.DeliveryNumber.Contains(s.DeliveryNumber)) &&
                (s.LpoNumber.IsNullOrWhiteSpace() || (!delivery.LpoNumber.IsNullOrWhiteSpace() && delivery.LpoNumber.Contains(s.LpoNumber))) &&
                (s.MaterialId == 0 || delivery.SaleItem.MaterialId == s.MaterialId) &&
                (s.CompanyId == 0 || delivery.Site.CompanyId == s.CompanyId) &&
                (s.SiteId == 0 || delivery.SiteId == s.SiteId) &&
                (s.DriverId == 0 || delivery.DeliveryId == s.DriverId) &&
                (s.MachineId == 0 || delivery.MachineId == s.MachineId) &&
                (s.DateRange == DateRanges.DontCare || delivery.DateTime.IsInDateRange(s.DateRange))));

            ViewModel = new BaseListViewModel<Delivery>(new DeliveryRepository(), searchModel, dynamicFilter, PermissionTypes.Deliveries)
            {
                CreateUpdatePage = new DeliveryPage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

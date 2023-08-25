using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Zenith.Assets.Extensions;
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
        public DeliveryListPage(Sale relatedSale)
        {
            InitializeComponent();

            var searchModel = new DeliverySearchModel();

            IObservable<Func<Delivery, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(SynchronizationContext.Current)
                .Select(subject => new Func<Delivery, bool>(oc => true));

            ViewModel = new DeliveryListViewModel(new DeliveryRepository(), searchModel, dynamicFilter, relatedSale)
            {
                CreateUpdatePage = new DeliveryPage()
            };

            var CastedViewModel = (DeliveryListViewModel)ViewModel;

            this.WhenActivated(d => { listItemsControl.ItemsSource = CastedViewModel.SaleItemsActiveList; });
        }
    }
}

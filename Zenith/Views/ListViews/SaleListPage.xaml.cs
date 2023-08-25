using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Zenith.Assets.Extensions;
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

            IObservable<Func<Sale, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(SynchronizationContext.Current)
                .Select(subject => new Func<Sale, bool>(oc => true));

            ViewModel = new SaleListViewModel(new SaleRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new SalePage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

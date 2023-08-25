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
    /// Interaction logic for BuyListPage.xaml
    /// </summary>
    public partial class BuyListPage : BaseListPage<Buy>
    {
        public BuyListPage()
        {
            InitializeComponent();
            var searchModel = new BuySearchModel();

            IObservable<Func<Buy, bool>> dynamicFilter = searchModel.WhenAnyValue(b => b.CompanyId)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(SynchronizationContext.Current)
                .Select(companyId => new Func<Buy, bool>(b => companyId == 0 || b.CompanyId == companyId));

            ViewModel = new BaseListViewModel<Buy>(new BuyRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new BuyPage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

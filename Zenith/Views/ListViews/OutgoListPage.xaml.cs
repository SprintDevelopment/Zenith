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
    /// Interaction logic for OutgoListPage.xaml
    /// </summary>
    public partial class OutgoListPage : BaseListPage<Outgo>
    {
        public OutgoListPage()
        {
            InitializeComponent();
            var searchModel = new OutgoSearchModel();

            IObservable<Func<Outgo, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(subject => new Func<Outgo, bool>(oc => true));

            ViewModel = new BaseListViewModel<Outgo>(new OutgoRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new OutgoPage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

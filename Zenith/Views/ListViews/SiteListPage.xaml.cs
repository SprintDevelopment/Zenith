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
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for SiteListPage.xaml
    /// </summary>
    public partial class SiteListPage : BaseListPage<Site>
    {
        public SiteListPage()
        {
            InitializeComponent();
            var searchModel = new SiteSearchModel();

            IObservable<Func<Site, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Site, bool>(site =>
                    (s.Name.IsNullOrWhiteSpace() || site.Name.Contains(s.Name)) &&
                    (s.CompanyId == 0 || site.CompanyId == s.CompanyId)));

            ViewModel = new BaseListViewModel<Site>(new SiteRepository(), searchModel, dynamicFilter, PermissionTypes.Sites)
            {
                CreateUpdatePage = new SitePage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

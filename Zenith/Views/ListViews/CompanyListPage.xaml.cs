using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Zenith.Models.SearchModels;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;
using ReactiveUI;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for CompanyListPage.xaml
    /// </summary>
    public partial class CompanyListPage : BaseListPage<Company>
    {
        public CompanyListPage()
        {
            InitializeComponent();
            var searchModel = new CompanySearchModel();

            IObservable<Func<Company, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Name, n => n.OnlyForRefreshAfterUpdate)
                .Select(x => x.Item1)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(name => new Func<Company, bool>(p => name.IsNullOrWhiteSpace() || p.Name.Contains(name)));

            ViewModel = new BaseListViewModel<Company>(new CompanyRepository(), searchModel, dynamicFilter, PermissionTypes.Companies)
            {
                CreateUpdatePage = new CompanyPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}

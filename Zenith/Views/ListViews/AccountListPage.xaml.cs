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
    /// Interaction logic for AccountListPage.xaml
    /// </summary>
    public partial class AccountListPage : BaseListPage<Account>
    {
        public AccountListPage()
        {
            InitializeComponent();
            var searchModel = new AccountSearchModel();

            IObservable<Func<Account, bool>> dynamicFilter = searchModel.WhenAnyValue(n => n.OnlyForRefreshAfterUpdate)
                //.Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(name => new Func<Account, bool>(p => true));

            ViewModel = new BaseListViewModel<Account>(new AccountRepository(), searchModel, dynamicFilter, PermissionTypes.Companies)
            {
                //CreateUpdatePage = new AccountPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}

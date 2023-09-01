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
    /// Interaction logic for UserListPage.xaml
    /// </summary>
    public partial class UserListPage : BaseListPage<User>
    {
        public UserListPage()
        {
            InitializeComponent();
            var searchModel = new UserSearchModel();

            IObservable<Func<User, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(subject => new Func<User, bool>(oc => true));

            ViewModel = new BaseListViewModel<User>(new UserRepository(), searchModel, dynamicFilter, PermissionTypes.Users)
            {
                CreateUpdatePage = new UserPage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

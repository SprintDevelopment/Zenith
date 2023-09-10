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
    /// Interaction logic for MachineOutgoListPage.xaml
    /// </summary>
    public partial class MachineOutgoListPage : BaseListPage<MachineOutgo>
    {
        public MachineOutgoListPage()
        {
            InitializeComponent();
            var searchModel = new MachineOutgoSearchModel();

            IObservable<Func<MachineOutgo, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title, n => n.OnlyForRefreshAfterUpdate)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(subject => new Func<MachineOutgo, bool>(oc => true));

            ViewModel = new BaseListViewModel<MachineOutgo>(new MachineOutgoRepository(), searchModel, dynamicFilter, PermissionTypes.MachineOutgoes)
            {
                CreateUpdatePage = new MachineOutgoPage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

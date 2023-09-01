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
    /// Interaction logic for MixtureListPage.xaml
    /// </summary>
    public partial class MixtureListPage : BaseListPage<Mixture>
    {
        public MixtureListPage()
        {
            InitializeComponent();
            var searchModel = new MixtureSearchModel();

            IObservable<Func<Mixture, bool>> dynamicFilter = searchModel.WhenAnyValue(m => m.Title)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(title => new Func<Mixture, bool>(b => true));

            ViewModel = new BaseListViewModel<Mixture>(new MixtureRepository(), searchModel, dynamicFilter, PermissionTypes.Mixtures)
            {
                CreateUpdatePage = new MixturePage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

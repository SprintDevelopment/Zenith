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
    /// Interaction logic for OutgoCategoryListPage.xaml
    /// </summary>
    public partial class OutgoCategoryListPage : BaseListPage<OutgoCategory>
    {
        public OutgoCategoryListPage()
        {
            InitializeComponent();
            var searchModel = new OutgoCategorySearchModel();

            IObservable<Func<OutgoCategory, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<OutgoCategory, bool>(c => true));

            ViewModel = new BaseListViewModel<OutgoCategory>(new OutgoCategoryRepository(), searchModel, dynamicFilter, PermissionTypes.OutgoCategories)
            {
                CreateUpdatePage = new OutgoCategoryPage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

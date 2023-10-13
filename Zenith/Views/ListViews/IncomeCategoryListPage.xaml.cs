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
    /// Interaction logic for IncomeCategoryListPage.xaml
    /// </summary>
    public partial class IncomeCategoryListPage : BaseListPage<IncomeCategory>
    {
        public IncomeCategoryListPage()
        {
            InitializeComponent();
            var searchModel = new IncomeCategorySearchModel();

            IObservable<Func<IncomeCategory, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<IncomeCategory, bool>(c => true));

            ViewModel = new BaseListViewModel<IncomeCategory>(new IncomeCategoryRepository(), searchModel, dynamicFilter, PermissionTypes.IncomeCategories)
            {
                CreateUpdatePage = new IncomeCategoryPage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}

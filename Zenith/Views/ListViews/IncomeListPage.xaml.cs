using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
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
    /// Interaction logic for IncomeListPage.xaml
    /// </summary>
    public partial class IncomeListPage : BaseListPage<Income>
    {
        public IncomeListPage()
        {
            InitializeComponent();
            var searchModel = new IncomeSearchModel();

            IObservable<Func<Income, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Income, bool>(income =>
                    (s.IncomeCategoryId == 0 || income.IncomeCategoryId == s.IncomeCategoryId) &&
                    (s.CompanyId == 0 || income.CompanyId == s.CompanyId) &&
                    (s.DateRange == DateRanges.DontCare || income.DateTime.IsInDateRange(s.DateRange))));

            ViewModel = new BaseListViewModel<Income>(new IncomeRepository(), searchModel, dynamicFilter, PermissionTypes.Incomes)
            {
                CreateUpdatePage = new IncomePage()
            };

            this.WhenActivated(d => 
            { 
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                ViewModel.SummaryItem = new Income();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.Value = ViewModel.ActiveList.Sum(i => i.Value);
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}

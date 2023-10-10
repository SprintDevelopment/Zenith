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
using DynamicData.Binding;
using System.Reactive.Disposables;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for CashListPage.xaml
    /// </summary>
    public partial class CashListPage : BaseListPage<Cash>
    {
        public CashListPage()
        {
            InitializeComponent();
            var searchModel = new CashSearchModel();

            IObservable<Func<Cash, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Cash, bool>(c => 
                    (c.Value > 0) && 
                    (s.CompanyId == 0 || c.CompanyId == s.CompanyId) &&
                    (s.MoneyTransactionType == MoneyTransactionTypes.DontCare || c.MoneyTransactionType == s.MoneyTransactionType) &&
                    (s.CostCenter == CostCenters.DontCare || c.CostCenter == s.CostCenter)));

            ViewModel = new CashListViewModel(new CashRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new CashPage()
            };

            this.WhenActivated(d =>
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                ViewModel.SummaryItem = new Cash();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.Value = ViewModel.ActiveList.Sum(i => i.Value * i.MoneyTransactionType.ToChangeCoefficient().CompCredCoeff);
                    }).Do(_ =>
                    {
                        if (ViewModel.SummaryItem.Value < 0)
                            ViewModel.SummaryItem.Value *= -1;
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}

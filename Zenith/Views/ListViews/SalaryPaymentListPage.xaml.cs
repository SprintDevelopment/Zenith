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

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for SalaryPaymentListPage.xaml
    /// </summary>
    public partial class SalaryPaymentListPage : BaseListPage<SalaryPayment>
    {
        public SalaryPaymentListPage()
        {
            InitializeComponent();
            var searchModel = new SalaryPaymentSearchModel();

            IObservable<Func<SalaryPayment, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<SalaryPayment, bool>(salaryPayment =>
                    (s.PersonId == 0 || salaryPayment.PersonId == s.PersonId) &&
                    (s.CostCenter == CostCenters.DontCare || salaryPayment.CostCenter == s.CostCenter) &&
                    (s.DateRange == DateRanges.DontCare || salaryPayment.DateTime.IsInDateRange(s.DateRange))));


            ViewModel = new SalaryPaymentListViewModel(new SalaryPaymentRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new SalaryPaymentPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}

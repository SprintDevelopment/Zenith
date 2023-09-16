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
    /// Interaction logic for SalaryPaymentListPage.xaml
    /// </summary>
    public partial class SalaryPaymentListPage : BaseListPage<SalaryPayment>
    {
        public SalaryPaymentListPage()
        {
            InitializeComponent();
            var searchModel = new SalaryPaymentSearchModel();

            IObservable<Func<SalaryPayment, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title, n => n.OnlyForRefreshAfterUpdate)
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new { Title = s }).Select(s => new Func<SalaryPayment, bool>(p => true));

            ViewModel = new BaseListViewModel<SalaryPayment>(new SalaryPaymentRepository(), searchModel, dynamicFilter, PermissionTypes.SalaryPayments)
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

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
    /// Interaction logic for ChequeListPage.xaml
    /// </summary>
    public partial class ChequeListPage : BaseListPage<Cheque>
    {
        public ChequeListPage()
        {
            InitializeComponent();
            var searchModel = new ChequeSearchModel();

            IObservable<Func<Cheque, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Cheque, bool>(c =>
                    (c.Value > 0) &&
                    (s.TransferDirection == TransferDirections.DontCare || c.TransferDirection == s.TransferDirection) &&
                    (s.CompanyId == 0 || c.CompanyId == s.CompanyId) &&
                    (s.MoneyTransactionType == MoneyTransactionTypes.DontCare || c.MoneyTransactionType == s.MoneyTransactionType) &&
                    (s.CostCenter == CostCenters.DontCare || c.CostCenter == s.CostCenter)));

            ViewModel = new ChequeListViewModel(new ChequeRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new ChequePage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}

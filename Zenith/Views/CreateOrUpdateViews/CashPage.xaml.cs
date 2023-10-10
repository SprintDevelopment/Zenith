using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for CashPage.xaml
    /// </summary>
    public partial class CashPage : BaseCreateOrUpdatePage<Cash>
    {
        public CashPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Cash>(new CashRepository());

            moneyTransactionTypesComboBox.ItemsSource = typeof(MoneyTransactionTypes).ToCollection();

            this.WhenActivated(d =>
            {
                companyComboBox.ItemsSource = new CompanyRepository().All().ToList();
            });
        }
    }
}

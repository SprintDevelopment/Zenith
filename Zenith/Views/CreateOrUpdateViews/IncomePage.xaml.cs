using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for IncomeCategoryPage.xaml
    /// </summary>
    public partial class IncomePage : BaseCreateOrUpdatePage<Income>
    {
        public IncomePage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Income>(new IncomeRepository());

            cashStatesComboBox.ItemsSource = typeof(CashStates).ToCollection();
            companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType.HasFlag(CompanyTypes.RelatedToIncome)).ToList();
            this.WhenActivated(d =>
            {
                incomeCategoryComboBox.ItemsSource = new IncomeCategoryRepository().Find(oc => oc.CostCenter != CostCenters.Transportation).ToList();
            });
        }
    }
}

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
    /// Interaction logic for MachineIncomeCategoryPage.xaml
    /// </summary>
    public partial class MachineIncomePage : BaseCreateOrUpdatePage<MachineIncome>
    {
        public MachineIncomePage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<MachineIncome>(new MachineIncomeRepository());

            cashStatesComboBox.ItemsSource = typeof(CashStates).ToCollection();
            companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType.HasFlag(CompanyTypes.RelatedToIncome)).ToList();

            this.WhenActivated(d =>
            {
                incomeCategoryComboBox.ItemsSource = new IncomeCategoryRepository().Find(oc => oc.CostCenter != CostCenters.Workshop).ToList();
                machineComboBox.ItemsSource = new MachineRepository().All().ToList();

                this.OneWayBind(ViewModel, vm => vm.PageModel.RelatedOutgoPlusTransportId.HasValue, v => v.inputControlsStackPanel.IsEnabled).DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.PageModel.CompanyId)
                    .WhereNotNull()
                    .Do(ci => 
                    {
                        siteComboBox.ItemsSource = new SiteRepository().Find(s => s.CompanyId == ci).ToList();
                        siteComboBox.SelectedIndex = -1;
                    })
                    .Subscribe().DisposeWith(d);
            });
        }
    }
}

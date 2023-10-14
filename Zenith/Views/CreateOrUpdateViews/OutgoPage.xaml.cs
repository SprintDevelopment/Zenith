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
    /// Interaction logic for OutgoCategoryPage.xaml
    /// </summary>
    public partial class OutgoPage : BaseCreateOrUpdatePage<Outgo>
    {
        public OutgoPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Outgo>(new OutgoRepository());

            outgoTypeComboBox.ItemsSource = typeof(OutgoTypes).ToCollection();
            cashStatesComboBox.ItemsSource = typeof(CashStates).ToCollection();
            companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType == CompanyTypes.Other).ToList();

            this.WhenActivated(d =>
            {
                outgoCategoryComboBox.ItemsSource = new OutgoCategoryRepository().Find(oc => oc.CostCenter != CostCenters.Transportation).ToList();
                machineComboBox.ItemsSource = new MachineRepository().All().ToList();

                if (ViewModel.PageModel.OutgoType == OutgoTypes.UseConsumables)
                {
                    valueTextBox.IsEnabled = false;

                    ViewModel.PageModel.WhenAnyValue(pm => pm.OutgoCategoryId)
                        .Select(_ => ViewModel.PageModel.WhenAnyValue(pm => pm.Amount))
                        .Switch()
                        .Do(_ =>
                        {
                            var oc = outgoCategoryComboBox.SelectedItem as OutgoCategory ?? new OutgoCategory();
                            ViewModel.PageModel.Value = ViewModel.PageModel.Amount * oc.ApproxUnitPrice;
                        }).Subscribe().DisposeWith(d);
                }
                else
                    valueTextBox.IsEnabled = true;

                this.OneWayBind(ViewModel, vm => vm.PageModel.OutgoType, v => v.cashStatesComboBox.Visibility, ot => (ot != OutgoTypes.UseConsumables).Viz());
                this.OneWayBind(ViewModel, vm => vm.PageModel.OutgoType, v => v.companyComboBox.Visibility, ot => (ot != OutgoTypes.UseConsumables).Viz());
                this.OneWayBind(ViewModel, vm => vm.PageModel.OutgoType, v => v.machineComboBox.Visibility, ot => (ot == OutgoTypes.DirectIncludeTransportation).Viz());
                this.OneWayBind(ViewModel, vm => vm.PageModel.OutgoType, v => v.machineIncomeValueTextBox.Visibility, ot => (ot == OutgoTypes.DirectIncludeTransportation).Viz());
                this.OneWayBind(ViewModel, vm => vm.PageModel.OutgoType, v => v.factorNumberTextBox.Visibility, ot => (ot != OutgoTypes.UseConsumables).Viz());
            });
        }
    }
}

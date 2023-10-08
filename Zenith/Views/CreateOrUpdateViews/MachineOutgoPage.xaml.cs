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
    /// Interaction logic for MachineOutgoCategoryPage.xaml
    /// </summary>
    public partial class MachineOutgoPage : BaseCreateOrUpdatePage<MachineOutgo>
    {
        public MachineOutgoPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<MachineOutgo>(new MachineOutgoRepository());

            outgoTypeComboBox.ItemsSource = typeof(OutgoTypes).ToCollection();

            this.WhenActivated(d =>
            {
                outgoCategoryComboBox.ItemsSource = new OutgoCategoryRepository().Find(oc => oc.CostCenter != CostCenters.Workshop).ToList();
                companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType == CompanyTypes.Other).ToList();
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

                this.OneWayBind(ViewModel, vm => vm.PageModel.OutgoType, v => v.companyComboBox.Visibility, ot => (ot != OutgoTypes.UseConsumables).Viz());
                this.OneWayBind(ViewModel, vm => vm.PageModel.OutgoType, v => v.factorNumberTextBox.Visibility, ot => (ot != OutgoTypes.UseConsumables).Viz());
            });
        }
    }
}

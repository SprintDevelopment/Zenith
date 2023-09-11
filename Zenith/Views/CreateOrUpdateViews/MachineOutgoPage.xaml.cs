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

            this.WhenActivated(d =>
            {
                outgoCategoryComboBox.ItemsSource = new OutgoCategoryRepository().Find(oc => oc.CostCenter == CostCenters.Transportation).ToList();
                companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType == CompanyTypes.Other).ToList();
                machineComboBox.ItemsSource = new MachineRepository().All().ToList();
            });
        }
    }
}

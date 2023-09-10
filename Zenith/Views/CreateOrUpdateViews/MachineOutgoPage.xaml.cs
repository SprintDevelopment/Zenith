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
                outgoCategoryTreeView.ViewModel.ItemsSource = new OutgoCategoryRepository().All();
                companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType == CompanyTypes.Seller).ToList();
                machineComboBox.ItemsSource = new MachineRepository().All().ToList();

                this.Bind(ViewModel, vm => vm.PageModel.OutgoCategory, v => v.outgoCategoryTreeView.ViewModel.SelectedItem).DisposeWith(d);
            });
        }
    }
}

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
    /// Interaction logic for DeliveryPage.xaml
    /// </summary>
    public partial class DeliveryPage : BaseCreateOrUpdatePage<Delivery>
    {
        public DeliveryPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Delivery>(new DeliveryRepository());

            this.WhenActivated(d =>
            {
                machineComboBox.ItemsSource = new MachineRepository().All().ToList();
                driverComboBox.ItemsSource = new PersonRepository().All().ToList();
                siteComboBox.ItemsSource = new SiteRepository().Find(s => s.CompanyId == ViewModel.PageModel.SaleItem.Sale.CompanyId).ToList();

                ViewModel.PageModel.WhenAnyValue(pm => pm.InfoIsRelatedToMachine)
                    .Where(relation => relation)
                    .Select(_ => 
                        ViewModel.PageModel.WhenAnyValue(pm => pm.MachineId)
                            .Select(mi => machineComboBox.Items.OfType<Machine>().FirstOrDefault(m => m.MachineId == mi))
                            .WhereNotNull())
                    .Switch()
                    .Do(m =>
                    {
                        ViewModel.PageModel.Count = m.Capacity;
                        ViewModel.PageModel.DeliveryFee = m.DefaultDeliveryFee;
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}

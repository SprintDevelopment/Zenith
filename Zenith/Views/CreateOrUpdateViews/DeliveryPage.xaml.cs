using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Dtos;
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

            //Should refactor, but temporary
            var configurationRepository = new ConfigurationRepository();

            ViewModel = new BaseCreateOrUpdateViewModel<Delivery>(new DeliveryRepository());

            this.WhenActivated(d =>
            {
                machineComboBox.ItemsSource = new MachineRepository().All().ToList();
                driverComboBox.ItemsSource = new PersonRepository().All().ToList();
                siteComboBox.ItemsSource = new SiteRepository().Find(s => s.CompanyId == ViewModel.PageModel.SaleItem.Sale.CompanyId).ToList();

                if (ViewModel.IsNew)
                {
                    ViewModel.WhenAnyValue(vm => vm.PageModel)
                        .Select(pm => pm.WhenAnyValue(pm => pm.AutoDeliveryNumberEnabled))
                        .Switch()
                        .Do(autoDeliveryEnabled =>
                        {
                            if (autoDeliveryEnabled)
                            {
                                if (int.TryParse(configurationRepository.Single(ConfigurationKeys.LastAutoDeliveryNumber).Value, out int lastAutoDeliveryNumber))
                                    ViewModel.PageModel.DeliveryNumber = $"{lastAutoDeliveryNumber + 1}";
                                else
                                    ViewModel.PageModel.DeliveryNumber = "800401";
                            }
                            else
                                ViewModel.PageModel.DeliveryNumber = "";
                        }).Subscribe().DisposeWith(d);
                }

                ViewModel.PageModel.WhenAnyValue(pm => pm.IsIndirectDelivery)
                    .Select(iid => iid.Viz())
                    .BindTo(this, v => v.sourceDeliveryInfoStackPanel.Visibility)
                    .DisposeWith(d);
            });
        }
    }
}

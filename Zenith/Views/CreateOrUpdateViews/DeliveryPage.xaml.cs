using ReactiveUI;
using System.Linq;
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
            });
        }
    }
}

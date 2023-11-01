using ReactiveUI;
using System.Linq;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for MachinePage.xaml
    /// </summary>
    public partial class MachinePage : BaseCreateOrUpdatePage<Machine>
    {
        public MachinePage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Machine>(new MachineRepository());

            this.WhenActivated(d =>
            {
                companyComboBox.ItemsSource = new CompanyRepository().All().ToList();
            });
        }
    }
}

using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for CompanyPage.xaml
    /// </summary>
    public partial class CompanyPage : BaseCreateOrUpdatePage<Company>
    {
        public CompanyPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Company>(new CompanyRepository());
        }
    }
}

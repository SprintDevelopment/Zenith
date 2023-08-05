using Zenith.Models;
using Zenith.Repositories;

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
        
            Initialize(new CompanyRepository());
        }
    }
}

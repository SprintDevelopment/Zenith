using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for MaterialPage.xaml
    /// </summary>
    public partial class MaterialPage : BaseCreateOrUpdatePage<Material>
    {
        public MaterialPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Material>(new MaterialRepository());
        }
    }
}

using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
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

            buyUnitComboBox.ItemsSource = typeof(CountUnits).ToCollection();
            saleUnitComboBox.ItemsSource = typeof(CountUnits).ToCollection();

        }
    }
}

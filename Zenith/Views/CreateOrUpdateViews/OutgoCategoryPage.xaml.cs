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
    /// Interaction logic for OutgoCategoryPage.xaml
    /// </summary>
    public partial class OutgoCategoryPage : BaseCreateOrUpdatePage<OutgoCategory>
    {
        public OutgoCategoryPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<OutgoCategory>(new OutgoCategoryRepository());
            
            costCenterComboBox.ItemsSource = typeof(CostCenters).ToCollection();

            this.WhenActivated(d =>
            {
                //parentOutgoCategoryTreeView.ViewModel.ItemsSource = new OutgoCategoryRepository().All();
                //this.Bind(ViewModel, vm => vm.PageModel.Parent, v => v.parentOutgoCategoryTreeView.ViewModel.SelectedItem).DisposeWith(d);
            });
        }
    }
}

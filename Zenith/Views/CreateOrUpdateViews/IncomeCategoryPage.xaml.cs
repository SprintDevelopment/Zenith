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
    /// Interaction logic for IncomeCategoryPage.xaml
    /// </summary>
    public partial class IncomeCategoryPage : BaseCreateOrUpdatePage<IncomeCategory>
    {
        public IncomeCategoryPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<IncomeCategory>(new IncomeCategoryRepository());
            
            costCenterComboBox.ItemsSource = typeof(CostCenters).ToCollection();

            this.WhenActivated(d =>
            {
                //parentIncomeCategoryTreeView.ViewModel.ItemsSource = new IncomeCategoryRepository().All();
                //this.Bind(ViewModel, vm => vm.PageModel.Parent, v => v.parentIncomeCategoryTreeView.ViewModel.SelectedItem).DisposeWith(d);
            });
        }
    }
}

using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for SalePage.xaml
    /// </summary>
    public partial class SalePage : BaseCreateOrUpdatePage<Sale>
    {
        public SalePage()
        {
            InitializeComponent();

            ViewModel = new SaleCreateOrUpdateViewModel(new SaleRepository());

            cashStatesComboBox.ItemsSource = typeof(CashStates).ToCollection();
            companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType.HasFlag(CompanyTypes.Buyer)).ToList();
            sellerCompanyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType.HasFlag(CompanyTypes.Seller)).ToList();

            var CastedViewModel = (SaleCreateOrUpdateViewModel)ViewModel;

            this.WhenActivated(d =>
            {
                materialListItemsControl.ItemsSource = CastedViewModel.MaterialsActiveList;
                //itemsListItemsControl.ItemsSource = CastedViewModel.SaleItemsActiveList;

                CastedViewModel.WhenAnyValue(vm => vm.SearchedMaterialName)
                    .Select(s => s.IsNullOrWhiteSpace())
                    .Do(i => searchHintTextBlock.Visibility = i ? Visibility.Visible : Visibility.Collapsed)
                    .Subscribe().DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.PageModel.IsIndirectSale, v => v.sellerCompanyComboBox.Visibility, ot => (ot).Viz());
            });
        }
    }
}

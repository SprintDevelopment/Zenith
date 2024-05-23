using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
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
            companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType.HasFlag(CompanyTypes.RelatedToSale)).ToList();
            sellerCompanyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType.HasFlag(CompanyTypes.RelatedToBuy)).ToList();

            var CastedViewModel = (SaleCreateOrUpdateViewModel)ViewModel;

            this.WhenActivated(d =>
            {
                Observable.FromEventPattern(this, nameof(PreviewKeyDown))
                    .Merge(Observable.FromEventPattern(this, nameof(PreviewKeyUp)))
                    .Select(x => x.EventArgs as KeyEventArgs)
                    .Select(e => (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) && e.KeyStates.HasFlag(KeyStates.Down))
                    .BindTo(CastedViewModel, vm => vm.IsUnitSelectorVisible)
                    .DisposeWith(d);

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

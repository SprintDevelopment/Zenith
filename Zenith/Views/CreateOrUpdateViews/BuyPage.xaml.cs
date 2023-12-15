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
    /// Interaction logic for BuyPage.xaml
    /// </summary>
    public partial class BuyPage : BaseCreateOrUpdatePage<Buy>
    {
        public BuyPage()
        {
            InitializeComponent();

            ViewModel = new BuyCreateOrUpdateViewModel(new BuyRepository());
            
            cashStatesComboBox.ItemsSource = typeof(CashStates).ToCollection();
            companyComboBox.ItemsSource = new CompanyRepository().Find(c => c.CompanyType.HasFlag(CompanyTypes.Seller)).ToList();
           
            var CastedViewModel = (BuyCreateOrUpdateViewModel)ViewModel;

            this.WhenActivated(d =>
            {
                Observable.FromEventPattern(this, nameof(PreviewKeyDown))
                    .Merge(Observable.FromEventPattern(this, nameof(PreviewKeyUp)))
                    .Select(x => x.EventArgs as KeyEventArgs)
                    .Select(e => (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) && e.KeyStates.HasFlag(KeyStates.Down))
                    .BindTo(CastedViewModel, vm => vm.IsCountSelectorVisible)
                    .DisposeWith(d);

                materialListItemsControl.ItemsSource = CastedViewModel.MaterialsActiveList;
                //itemsListItemsControl.ItemsSource = CastedViewModel.BuyItemsActiveList;

                CastedViewModel.WhenAnyValue(vm => vm.SearchedMaterialName)
                    .Select(s => s.IsNullOrWhiteSpace())
                    .Do(i => searchHintTextBlock.Visibility = i ? Visibility.Visible : Visibility.Collapsed)
                    .Subscribe().DisposeWith(d);
            });
        }
    }
}

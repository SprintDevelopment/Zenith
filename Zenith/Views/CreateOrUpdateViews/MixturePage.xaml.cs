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
    /// Interaction logic for MixturePage.xaml
    /// </summary>
    public partial class MixturePage : BaseCreateOrUpdatePage<Mixture>
    {
        public MixturePage()
        {
            InitializeComponent();

            ViewModel = new MixtureCreateOrUpdateViewModel(new MixtureRepository());
            
            saleUnitComboBox.ItemsSource = typeof(CountUnits).ToCollection();
            
            var CastedViewModel = (MixtureCreateOrUpdateViewModel)ViewModel;

            this.WhenActivated(d =>
            {
                materialListItemsControl.ItemsSource = CastedViewModel.MaterialsActiveList;

                CastedViewModel.WhenAnyValue(vm => vm.SearchedMaterialName)
                    .Select(s => s.IsNullOrWhiteSpace())
                    .Do(i => searchHintTextBlock.Visibility = i ? Visibility.Visible : Visibility.Collapsed)
                    .Subscribe().DisposeWith(d);
            });
        }
    }
}

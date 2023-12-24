using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using Zenith.Assets.Extensions;
using Zenith.Assets.UI.BaseClasses;
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for UnitSelectorControl.xaml
    /// </summary>
    public partial class UnitSelectorControl : ActivatableUserControl, IViewFor<UnitSelectorViewModel>
    {
        public Visibility SelectionGridVisibility
        {
            get { return (Visibility)base.GetValue(SelectionGridVisibilityProperty); }
            set { base.SetValue(SelectionGridVisibilityProperty, value); }
        }
        public static readonly DependencyProperty SelectionGridVisibilityProperty = DependencyProperty.Register("SelectionGridVisibility", typeof(Visibility), typeof(UnitSelectorControl), new PropertyMetadata(Visibility.Collapsed));

        public UnitSelectorControl()
        {
            InitializeComponent();

            this.WhenActivated(d => 
            {
                ViewModel = DataContext as UnitSelectorViewModel;

                this.OneWayBind(ViewModel, vm => vm.SelectedCountUnit, v => v.selectedUnitTextBlock.Text, su => $"{App.Current.Resources[$"{su.GetType().Name}.{su}"]}").DisposeWith(d);

                Observable.Merge(
                Observable.FromEventPattern(select1MeterButton, nameof(Button.Click)).Select(_ => CountUnits.Meter),
                Observable.FromEventPattern(select3MeterButton, nameof(Button.Click)).Select(_ => CountUnits._3MeterTrip),
                Observable.FromEventPattern(select20MeterButton, nameof(Button.Click)).Select(_ => CountUnits._20MeterTrip),
                Observable.FromEventPattern(select35MeterButton, nameof(Button.Click)).Select(_ => CountUnits._35MeterTrip),
                Observable.FromEventPattern(select45MeterButton, nameof(Button.Click)).Select(_ => CountUnits._45MeterTrip))
                    .Do(scu => ViewModel.SelectedCountUnit = scu)
                    .Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.SelectedCountUnit)
                    .Select(scu =>
                    {
                        return scu switch
                        {
                            CountUnits.Meter => 0,
                            CountUnits._3MeterTrip => 1,
                            CountUnits._20MeterTrip => 2,
                            CountUnits._35MeterTrip => 3,
                            CountUnits._45MeterTrip => 4,
                            _ => 0,
                        };
                    })
                    .Do(col => unitIndentifierRect.SetValue(Grid.ColumnProperty, col))
                    .Subscribe().DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (UnitSelectorViewModel)value; }
        }

        public UnitSelectorViewModel ViewModel { get; set; }
    }

    public class UnitSelectorViewModel : ReactiveObject
    {
        [Reactive]
        public CountUnits SelectedCountUnit { get; set; }
    }
}

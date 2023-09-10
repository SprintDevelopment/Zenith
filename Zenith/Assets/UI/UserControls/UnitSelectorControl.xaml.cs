using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for UnitSelectorControl.xaml
    /// </summary>
    public partial class UnitSelectorControl : ActivatableUserControl, IViewFor<UnitSelectorViewModel>
    {
        public UnitSelectorControl()
        {
            InitializeComponent();

            this.WhenActivated(d => 
            {
                ViewModel = DataContext as UnitSelectorViewModel;

                this.OneWayBind(ViewModel, vm => vm.SelectedCountUnit, v => v.selectionButton.Content, su => $"{App.Current.Resources[$"{su.GetType().Name}.{su}.OnlyUnit"]}").DisposeWith(d);

                Observable.FromEventPattern(selectionButton, nameof(Button.Click))
                    .Do(_ => ViewModel.SelectedCountUnit = ViewModel.SelectedCountUnit == CountUnits.Meter ? CountUnits.Ton : CountUnits.Meter)
                    .Subscribe().DisposeWith(d);
                //this.Bind(ViewModel, vm => vm.IsLocked, v => v.lockToggleButton.IsChecked).DisposeWith(d);

                //Observable.Merge(
                //    Observable.FromEventPattern(selectTonButton, nameof(Button.Click)).Select(_ => CountUnits.Ton),
                //    Observable.FromEventPattern(selectMeterButton, nameof(Button.Click)).Select(_ => CountUnits.Meter))
                //    .Do(countUnit => ViewModel.SelectedCountUnit = countUnit)
                //    .Subscribe().DisposeWith(d);

                //ViewModel.WhenAnyValue(vm => vm.SelectedCountUnit)
                //    .Select(scu => (int)scu)
                //    .Do(intCu => unitIndentifierRect.SetValue(Grid.ColumnProperty, intCu))
                //    .Subscribe().DisposeWith(d);
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

        //[Reactive]
        //public bool IsLocked { get; set; } = true;
    }
}

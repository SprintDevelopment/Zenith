using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
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
using Zenith.Assets.Extensions;
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for CountSelectorControl.xaml
    /// </summary>
    public partial class CountSelectorControl : ActivatableUserControl, IViewFor<CountSelectorViewModel>
    {
        public CountSelectorControl()
        {
            InitializeComponent();
            //this.Bind(ViewModel, vm => vm.IsLocked, v => v.lockToggleButton.IsChecked).DisposeWith(d);
            this.WhenActivated(d =>
            {
                ViewModel = DataContext as CountSelectorViewModel;

                Observable.Merge(
                Observable.FromEventPattern(select3MeterButton, nameof(Button.Click)).Select(_ => 3f),
                Observable.FromEventPattern(select20MeterButton, nameof(Button.Click)).Select(_ => 20f),
                Observable.FromEventPattern(select35MeterButton, nameof(Button.Click)).Select(_ => 35f),
                Observable.FromEventPattern(select45MeterButton, nameof(Button.Click)).Select(_ => 45f))
                .Do(count => ViewModel.SelectedCount = count)
                .Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.SelectedCount)
                    .Select(sc => (sc == 3 || sc == 20 || sc == 35 || sc == 45).Viz())
                    .BindTo(this, v => v.unitIndentifierRect.Visibility)
                    .DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.SelectedCount)
                    .Select(sc =>
                    {
                        return sc switch
                        {
                            3f => 0,
                            20f => 1,
                            35f => 2,
                            45f => 3,
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
            set { ViewModel = (CountSelectorViewModel)value; }
        }

        public CountSelectorViewModel ViewModel { get; set; }
    }

    public class CountSelectorViewModel : ReactiveObject
    {
        [Reactive]
        public float SelectedCount { get; set; }
    }
}

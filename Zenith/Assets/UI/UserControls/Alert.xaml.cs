using ReactiveUI;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zenith.Assets.UI.Helpers;
using Zenith.Assets.Values.Enums;
using Zenith.ViewModels;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for Alert.xaml
    /// </summary>
    public partial class Alert : ActivatableUserControl, IViewFor<AlertViewModel>
    {
        public Alert()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;

                ViewModel.WhenAnyValue(vm => vm.DialogType)
                    .Select(dt => dt switch
                    {
                        DialogTypes.Success => new { foreground = Brushes.DarkGreen, borderBrush = Brushes.Green },
                        DialogTypes.Warning => new { foreground = Brushes.DarkOrange, borderBrush = Brushes.Orange },
                        DialogTypes.Danger => new { foreground = Brushes.DarkRed, borderBrush = Brushes.Red },
                        DialogTypes.Info => new { foreground = Brushes.DarkBlue, borderBrush = Brushes.Blue },
                        _ => new { foreground = Brushes.Transparent, borderBrush = Brushes.Transparent }
                    })
                    .Do(brushes => { alertBorder.BorderBrush = brushes.borderBrush; titleTextBlock.Foreground = descriptionTextBlock.Foreground = brushes.foreground; })
                    .Subscribe().DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.ActionCommand, v => v.actionButton.Visibility, a => a is null ? Visibility.Collapsed : Visibility.Visible).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.ActionCommand, v => v.actionButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.CloseCommand, v => v.closeButton).DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AlertViewModel)value; }
        }

        public AlertViewModel ViewModel { get; set; }
    }
}

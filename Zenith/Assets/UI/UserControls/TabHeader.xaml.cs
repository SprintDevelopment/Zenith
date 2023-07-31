using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Zenith.Assets.UI.Helpers;
using Zenith.ViewModels;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TabHeader.xaml
    /// </summary>
    public partial class TabHeader : ActivatableUserControl, IViewFor<TabViewModel>
    {
        public TabHeader()
        {
            InitializeComponent();
            
            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;

                headerBorder.InputBindings.Add(new MouseBinding(ViewModel.SelectCommand, new MouseGesture(MouseAction.LeftClick)));
                headerBorder.InputBindings.Add(new MouseBinding(ViewModel.CloseCommand, new MouseGesture(MouseAction.MiddleClick)));

                this.OneWayBind(ViewModel, vm => vm.IsSelected, v => v.headerBorder.Background, x => x ? Brushes.White : Brushes.Transparent).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.IsSelected, v => v.FontWeight, x => x ? FontWeights.SemiBold : FontWeights.Normal).DisposeWith(d);

                Observable.FromEventPattern(headerBorder, nameof(headerBorder.MouseEnter))
                    .Do(_ => { if (!ViewModel.IsSelected) headerBorder.Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)); })
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(headerBorder, nameof(headerBorder.MouseLeave))
                    .Do(_ => { if (!ViewModel.IsSelected) headerBorder.Background = Brushes.Transparent; })
                    .Subscribe().DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TabViewModel)value; }
        }

        public TabViewModel ViewModel { get; set; }
    }
}

using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using Zenith.Assets.UI.UserControls;

namespace Zenith.Assets.UI.BaseClasses
{
    public class TabbedWindow : Window, IActivationForViewFetcher, ICanActivate
    {
        #region Implements IActivationForViewFetcher
        public int GetAffinityForView(Type view) => 1;
        public IObservable<bool> GetActivationForView(IActivatableView view) => Observable.FromEventPattern(this, nameof(Loaded)).Select(x => true);
        #endregion

        #region Implements ICanActivate
        public IObservable<Unit> Activated => Observable.FromEventPattern(this, nameof(Loaded)).Select(x => Unit.Default);
        public IObservable<Unit> Deactivated => Observable.FromEventPattern(this, nameof(Unloaded)).Select(x => Unit.Default);
        #endregion
     
        public TitleBar TitleBar { get; private set; }

        public TabbedWindow()
        {
            base.Initialized += (sender, eventArgs) =>
            {
                WindowState = WindowState.Maximized;
                FontFamily = new FontFamily("Vazir FD");
                FontSize = 14;

                TitleBar = new TitleBar();
                TitleBar.Closed += (s, e) => this.Close();
                TitleBar.Minimized += (s, e) => this.WindowState = WindowState.Minimized;

                var titleBarPlaceHolder = new ContentControl { Content = TitleBar };
                titleBarPlaceHolder.MouseMove += (s, e) => { SendMessage(new WindowInteropHelper(this).Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); };
                titleBarPlaceHolder.MouseDoubleClick += (s, e) => { WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized; };

                var newContent = new Grid() { Background = Brushes.Transparent };
                newContent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(38, GridUnitType.Pixel) });
                newContent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                newContent.Children.Add(titleBarPlaceHolder);

                var preContent = this.Content as Grid;
                preContent.SetValue(Grid.RowProperty, 1);

                this.Content = null;

                newContent.Children.Add(preContent);

                var borderRectangle = new Rectangle() { Stroke = Brushes.Silver, Visibility = Visibility.Collapsed };
                borderRectangle.SetValue(Grid.RowSpanProperty, 2);
                newContent.Children.Add(borderRectangle);

                this.WhenAnyValue(w => w.WindowState).ObserveOn(SynchronizationContext.Current).Subscribe(ws =>
                {
                    if (ws == WindowState.Maximized)
                    {
                        TitleBar.Margin = new Thickness(8, 8, 8, 0);
                        preContent.Margin = new Thickness(8, 0, 8, 8);
                        borderRectangle.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        TitleBar.Margin = new Thickness(0);
                        preContent.Margin = new Thickness(0);
                        borderRectangle.Visibility = Visibility.Visible;
                    }
                });

                this.Content = newContent;
            };
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}

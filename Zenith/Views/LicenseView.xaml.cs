using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.ViewModels;

namespace Zenith.Views
{
    /// <summary>
    /// Interaction logic for LicenseView.xaml
    /// </summary>
    public partial class LicenseView : ActivatablePage, IViewFor<LicenseViewModel>
    {
        public LicenseView()
        {
            InitializeComponent();

            ViewModel = new LicenseViewModel();

            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;

                ViewModel.CheckAndApplyLicenseCommand
                    .Select(_ => ViewModel.License.IsLicenseValid ? validMessageBorder : invalidMessageBorder)
                    .Do(border => border.Visibility = Visibility.Visible)
                    .Throttle(TimeSpan.FromSeconds(5)).ObserveOn(RxApp.MainThreadScheduler)
                    .Do(border => border.Visibility = Visibility.Collapsed)
                    .Subscribe().DisposeWith(d);

                var modalBackRect = new Rectangle { Fill = new SolidColorBrush(Color.FromArgb(96, 0, 0, 0)) };
                ((Grid)Content).Children.Insert(0, modalBackRect);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LicenseViewModel)value; }
        }

        public LicenseViewModel ViewModel { get; set; }
    }
}

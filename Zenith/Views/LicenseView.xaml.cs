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

                ViewModel.GetAppLicenseCommand.Execute().Subscribe().Dispose();

                ViewModel.CheckAndApplyLicenseCommand
                    .Do(_ => messageBorder.Visibility = Visibility.Visible)
                    .Throttle(TimeSpan.FromSeconds(5)).ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ => messageBorder.Visibility = Visibility.Collapsed)
                    .Subscribe().DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.License.State, v => v.activateButton.Visibility, state => (state != AppLicenseStates.Valid).Viz()).DisposeWith(d); 

                this.OneWayBind(ViewModel, vm => vm.License.State, v => v.bottomCloseButton.Visibility, state => (state == AppLicenseStates.Valid).Viz()).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.License.State, v => v.startDateTextBox.Visibility, state => (state == AppLicenseStates.Valid).Viz()).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.License.State, v => v.endDateTextBox.Visibility, state => (state == AppLicenseStates.Valid).Viz()).DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.License.State)
                    .Do(x =>
                    {

                    }).Subscribe().DisposeWith(d);

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

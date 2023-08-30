using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Zenith.ViewModels;
using Zenith.Models;
using Zenith.ViewModels.CreateOrUpdateViewModels;
using Zenith.Repositories;
using ReactiveUI;
using System.Windows.Shapes;
using Zenith.Assets.UI.Helpers;
using Zenith.ViewModels.ListViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    public class BaseCreateOrUpdatePage<T> : ActivatablePage, IViewFor<BaseCreateOrUpdateViewModel<T>> where T : Model, new()
    {
        KeyEventHandler WindowPreviewKeyDownEventHandler;

        public BaseCreateOrUpdatePage()
        {
            var window = App.Current.MainWindow as MainWindow;

            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;

                ViewModel.WhenAnyValue(vm => vm.PageModel).Subscribe(_ =>
                {
                    ((Control)FindName("FirstEntryControl"))?.Focus();
                });

                var modalBackRect = new Rectangle { Fill = new SolidColorBrush(Color.FromArgb(96, 0, 0, 0)), Name = "modalBackRect" };
                modalBackRect.InputBindings.Add(new MouseBinding(ViewModel.ReturnCommand, new MouseGesture(MouseAction.LeftClick)));

                if(!((Grid)Content).Children.OfType<Rectangle>().Any(r => r.Name == "modalBackRect"))
                    ((Grid)Content).Children.Insert(0, modalBackRect);
            });

            //WindowPreviewKeyDownEventHandler = (s, e) => { CreateUpdateBasePage_PreviewKeyDown(s, e); };
            //this.Loaded += (s, e) => { window.PreviewKeyDown += WindowPreviewKeyDownEventHandler; };
            //this.Unloaded += (s, e) => { window.PreviewKeyDown -= WindowPreviewKeyDownEventHandler; };
        }

        private void CreateUpdateBasePage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && (e.Key == Key.Enter || e.Key == Key.Escape))
            {
                //if (typeof(T) != typeof(Shortcut))
                    switch (e.Key)
                    {
                        case Key.Enter:
                            var focusedControl = (DependencyObject)Keyboard.FocusedElement;
                            if (focusedControl != null && focusedControl is TextBox && (focusedControl as TextBox).AcceptsReturn)
                                return;

                            while (focusedControl != null)
                            {
                                //if (focusedControl is ComboBoxUC)
                                //    return;

                                focusedControl = VisualTreeHelper.GetParent(focusedControl);
                            }

                            //ViewModel.CreateCommand.Execute().Subscribe();
                            break;

                        case Key.Escape:
                            ViewModel.ReturnCommand.Execute().Subscribe();
                            break;

                        default:
                            return; // RETURN => e.Handled = false; => Propagate KeyDown Event
                    }

                e.Handled = true;
            }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (BaseCreateOrUpdateViewModel<T>)value; }
        }

        public BaseCreateOrUpdateViewModel<T> ViewModel { get; set; }
    }
}

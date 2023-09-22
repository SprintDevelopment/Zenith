using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;
using Zenith.Assets.UI.Helpers;

namespace Zenith.Views.ListViews
{
    public class BaseListPage<T> : ActivatablePage, IViewFor<BaseListViewModel<T>> where T : Model, new()
    {
        KeyEventHandler WindowPreviewKeyDownEventHandler;

        public BaseListPage()
        {
            var window = App.Current.MainWindow as MainWindow;

            this.WhenActivated(d =>
            {
                ViewModel.InitiateSearchCommand.Execute().Subscribe().Dispose();
                ViewModel.SearchModel.OnlyForRefreshAfterUpdate++;

                this.DataContext = ViewModel;
            });
            //WindowPreviewKeyDownEventHandler = (s, e) => { ListBasePage_PreviewKeyDown(s, e); };
            //this.Loaded += (s, e) => { window.PreviewKeyDown += WindowPreviewKeyDownEventHandler; };
            //this.Unloaded += (s, e) => { window.PreviewKeyDown -= WindowPreviewKeyDownEventHandler; };
        }

        private void ListBasePage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control || e.Key == Key.Delete || e.Key == Key.Escape)
            {
                switch (e.Key)
                {
                    case Key.N:
                        ViewModel.CreateCommand.Execute().Subscribe();
                        break;

                    case Key.Delete:
                        ViewModel.RemoveCommand.Execute().Subscribe();
                        break;

                    case Key.Escape:
                        //listBasePage.Close(null, null);
                        break;

                    case Key.P:
                        //ViewModel.PrintCommand.Execute().Subscribe();
                        break;

                    case Key.A:
                        ViewModel.SelectAllCommand.Execute().Subscribe();
                        break;

                    case Key.F:
                        ViewModel.SearchCommand.Execute().Subscribe();
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
            set { ViewModel = (BaseListViewModel<T>)value; }
        }

        public BaseListViewModel<T> ViewModel { get; set; }
    }
}

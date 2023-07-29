using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.Views.ListViews
{
    public class BaseListPage<T> : Page where T : Model, new()
    {
        public BaseListViewModel<T> ViewModel { get; private set; }
        KeyEventHandler WindowPreviewKeyDownEventHandler;

        public BaseListPage()
        {
            var window = App.Current.MainWindow as MainWindow;

            WindowPreviewKeyDownEventHandler = (s, e) => { ListBasePage_PreviewKeyDown(s, e); };
            this.Loaded += (s, e) => { window.PreviewKeyDown += WindowPreviewKeyDownEventHandler; };
            this.Unloaded += (s, e) => { window.PreviewKeyDown -= WindowPreviewKeyDownEventHandler; };
        }

        public void Initialize(BaseCreateOrUpdatePage<T> createUpdatePage, Repository<T> repository, SearchBaseDto searchModel, IObservable<Func<T, bool>> criteria)
        {
            ViewModel = new BaseListViewModel<T>(repository, searchModel, criteria)
            {
                CreateUpdatePage = createUpdatePage
            };
            this.Unloaded += (s, e) => { ViewModel.DisposeCommand.Execute().Subscribe(); };
            this.DataContext = ViewModel;
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
    }
}

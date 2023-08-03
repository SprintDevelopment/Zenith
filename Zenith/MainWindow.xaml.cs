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
using Zenith.Assets.UI.BaseClasses;
using Zenith.Assets.UI.CustomEventArgs;
using Zenith.ViewModels;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.ListViews;

namespace Zenith
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : TabbedWindow, IViewFor<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = App.MainViewModel;

            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;

                Observable.FromEventPattern(TitleBar, nameof(TitleBar.MenuClicked))
                    .Do(_ => ViewModel.IsMenuVisible = !ViewModel.IsLocked && !ViewModel.IsMenuVisible)
                    .Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.IsMenuVisible)
                    .Do(isMenuVisible =>
                    {
                        if (isMenuVisible)
                            ViewModel.IsSearchVisible = false;
                        var storyboard = Resources[isMenuVisible ? "ShowMenuStoryboard" : "HideMenuStoryboard"] as Storyboard;
                        storyboard.Begin();
                    }).Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.IsSearchVisible).Subscribe(isSearchVisible =>
                {
                    if (isSearchVisible)
                        ViewModel.IsMenuVisible = false;
                    var storyboard = Resources[isSearchVisible ? "ShowSearchStoryboard" : "HideSearchStoryboard"] as Storyboard;
                    storyboard.Begin();
                });

                ViewModel.InitiateSearch.Subscribe(searchModel =>
                {
                    this.searchUserControl.Initialize(searchModel);
                });

                IDisposable disposable = null;
                ViewModel.WhenAnyValue(vm => vm.DialogDto).Where(dto => dto != null).Subscribe(dialogDto =>
                {
                    disposable?.Dispose();
                    disposable = Observable.FromEventPattern(dialogUserControl, nameof(dialogUserControl.Returned))
                    .Select(ea => ((DialogEventArgs)ea.EventArgs).DialogResult).Subscribe(dialogResult =>
                    {
                        ViewModel.DialogResult = dialogResult;
                    });

                    dialogUserControl.Initialize(dialogDto);
                });

                ViewModel.WhenAnyValue(vm => vm.ListPage).SkipWhile(page => page == null).Subscribe(listPage =>
                {
                    Frame.RemoveBackEntry();
                    listPage.FontFamily = this.FontFamily;
                    Frame.Navigate(listPage);
                });

                ViewModel.WhenAnyValue(vm => vm.CreateUpdatePage).SkipWhile(page => page == null).Subscribe(createUpdatePage =>
                {
                    CreateUpdateFrame.RemoveBackEntry();
                    if (createUpdatePage == null)
                    {
                        CreateUpdateFrame.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        createUpdatePage.FontFamily = this.FontFamily;
                        CreateUpdateFrame.Navigate(createUpdatePage);
                        CreateUpdateFrame.Visibility = Visibility.Visible;
                    }
                });

                Frame.Navigate(new NoteListPage() { FontFamily = this.FontFamily });
                //CreateUpdateFrame.Navigate(new ContractPage() { FontFamily = this.FontFamily });
            });
        }


        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainViewModel)value; }
        }

        public MainViewModel ViewModel { get; set; }
    }
}

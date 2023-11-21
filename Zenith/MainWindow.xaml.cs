using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Zenith.Assets.UI.BaseClasses;
using Zenith.Assets.UI.CustomEventArgs;
using ZenithControls = Zenith.Assets.UI.UserControls;
using Zenith.ViewModels;
using Zenith.Assets.Extensions;
using DynamicData.Binding;
using Zenith.Assets.UI.UserControls;
using DynamicData;
using Zenith.Views;
using Zenith.Assets.Values.Enums;
using Zenith.Assets.Utils;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;

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
            //GlobalFontSettings.FontResolver = new JetBrainsFontResolver();
            //new DocumentUtil().GeneratePdfReport("Title", new string[] { "1234", "12345343", "sfda;ljfasdf" });

            var blankPage = new BlankPage();

            ViewModel = App.MainViewModel;

            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;

                ViewModel.CreateDatabaseCommand.Execute().Subscribe().Dispose();
                ViewModel.GetAppLicenseCommand.Execute().Subscribe().Dispose();

                ViewModel.WhenAnyValue(vm => vm.CreateUpdatePage)
                    .SkipWhile(page => page == null)
                    .Do(createUpdatePage =>
                    {
                        CreateUpdateFrame.RemoveBackEntry();
                        if (createUpdatePage == null)
                        {
                            CreateUpdateFrame.Navigate(blankPage);
                            CreateUpdateFrame.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            createUpdatePage.FontFamily = this.FontFamily;
                            createUpdatePage.FontSize = this.FontSize;
                            CreateUpdateFrame.Navigate(createUpdatePage);
                            CreateUpdateFrame.Visibility = Visibility.Visible;
                        }
                    }).Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.SecondCreateUpdatePage)
                    .SkipWhile(page => page == null)
                    .Do(secondCreateUpdatePage =>
                    {
                        SecondCreateUpdateFrame.RemoveBackEntry();
                        if (secondCreateUpdatePage == null)
                        {
                            SecondCreateUpdateFrame.Navigate(blankPage);
                            SecondCreateUpdateFrame.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            secondCreateUpdatePage.FontFamily = this.FontFamily;
                            secondCreateUpdatePage.FontSize = this.FontSize;
                            SecondCreateUpdateFrame.Navigate(secondCreateUpdatePage);
                            SecondCreateUpdateFrame.Visibility = Visibility.Visible;
                        }
                    }).Subscribe().DisposeWith(d);

                CompositeDisposable disposable = null;

                ViewModel.WhenAnyValue(vm => vm.LoggedInUser, vm => vm.AppLicense)
                    .Select(changes => new { loggedInUser = changes.Item1, appLicense = changes.Item2 })
                    .Do(changes =>
                    {
                        if (changes.loggedInUser == null)
                        {
                            ViewModel.IsSearchVisible = ViewModel.IsMenuVisible = false;
                            ViewModel.CreateUpdatePage = new LoginView() { FontFamily = this.FontFamily, FontSize = this.FontSize };
                            disposable?.Dispose();
                        }
                        else if (changes.appLicense is null || changes.appLicense.State != AppLicenseStates.Valid)
                        {
                            ViewModel.IsSearchVisible = ViewModel.IsMenuVisible = false;
                            ViewModel.CreateUpdatePage = new LicenseView() { FontFamily = this.FontFamily, FontSize = this.FontSize };
                            disposable?.Dispose();
                        }
                        else
                        {
                            disposable = new CompositeDisposable().DisposeWith(d);

                            Observable.FromEventPattern(TitleBar, nameof(TitleBar.MenuClicked))
                                .Do(_ => ViewModel.IsMenuVisible = !ViewModel.IsLocked && !ViewModel.IsMenuVisible)
                                .Subscribe().DisposeWith(disposable);

                            ViewModel.WhenAnyValue(vm => vm.IsMenuVisible)
                                .Do(isMenuVisible =>
                                {
                                    if (isMenuVisible)
                                    {
                                        ViewModel.SwapIndicatorZeroNoneOneMenuTwoSearch = ViewModel.IsSearchVisible ? 2 : 0;
                                        ViewModel.IsSearchVisible = false;
                                    }
                                    else
                                        ViewModel.IsSearchVisible = ViewModel.SwapIndicatorZeroNoneOneMenuTwoSearch > 0;

                                    var storyboard = Resources[isMenuVisible ? "ShowMenuStoryboard" : "HideMenuStoryboard"] as Storyboard;
                                    storyboard.Begin();
                                }).Subscribe().DisposeWith(disposable);

                            ViewModel.WhenAnyValue(vm => vm.IsSearchVisible)
                                .Do(isSearchVisible =>
                                {
                                    if (isSearchVisible)
                                    {
                                        ViewModel.SwapIndicatorZeroNoneOneMenuTwoSearch = ViewModel.IsMenuVisible ? 1 : 0;
                                        ViewModel.IsMenuVisible = false;
                                        ((StackPanel)searchUserControl.FindName("SearchMemberStackPanel")).Children.OfType<Control>().FirstOrDefault()?.Focus();
                                    }
                                    else
                                        ViewModel.IsMenuVisible = ViewModel.SwapIndicatorZeroNoneOneMenuTwoSearch > 0;

                                    var storyboard = Resources[isSearchVisible ? "ShowSearchStoryboard" : "HideSearchStoryboard"] as Storyboard;
                                    storyboard.Begin();
                                }).Subscribe().DisposeWith(disposable);

                            ViewModel.InitiateSearchCommand
                                .WhereNotNull()
                                .Do(searchModel =>
                                {
                                    this.searchUserControl.Initialize(searchModel);
                                }).Subscribe().DisposeWith(disposable);

                            IDisposable dailogDisposable = null;
                            ViewModel.WhenAnyValue(vm => vm.DialogDto).Where(dto => dto != null).Subscribe(dialogDto =>
                            {
                                dailogDisposable?.Dispose();
                                dailogDisposable = Observable.FromEventPattern(dialogUserControl, nameof(dialogUserControl.Returned))
                                .Select(ea => ((DialogEventArgs)ea.EventArgs).DialogResult).Subscribe(dialogResult =>
                                {
                                    ViewModel.DialogResult = dialogResult;
                                });

                                dialogUserControl.Initialize(dialogDto);
                            });

                            ViewModel.WhenAnyValue(vm => vm.ListPage)
                                .SkipWhile(page => page == null)
                                .Do(listPage =>
                                {
                                    Frame.RemoveBackEntry();
                                    listPage.FontFamily = this.FontFamily;
                                    listPage.FontSize = this.FontSize;
                                    Frame.Navigate(listPage);
                                }).Subscribe().DisposeWith(disposable);

                            var shortcutedMenuItems = menuContainerStackPanel.Children.OfType<ZenithControls.MenuItem>()
                                .Select(mi => mi.AdditionalContent as StackPanel)
                                .SelectMany(sp => sp.Children.OfType<ZenithControls.SubMenuItem>())
                                .Select(mi => new { mi.Shortcut, mi.Command, mi.CommandParameter })
                                .Where(mi => !mi.Shortcut.IsNullOrWhiteSpace())
                                .Select(mi => new
                                {
                                    modifiers = (mi.Shortcut.Contains("Ctrl") ? ModifierKeys.Control : ModifierKeys.None) | (mi.Shortcut.Contains("Shift") ? ModifierKeys.Shift : ModifierKeys.None),
                                    keyChar = char.ToUpper(mi.Shortcut.Last()),
                                    mi.Command,
                                    mi.CommandParameter
                                })
                                .ToList();

                            Observable.FromEventPattern(this, nameof(Window.PreviewKeyDown))
                                .Select(x => x.EventArgs as KeyEventArgs)
                                .Select(x => new { eventArgs = x, shortcutedItem = shortcutedMenuItems.SingleOrDefault(smi => smi.modifiers == Keyboard.Modifiers && smi.keyChar == x.Key.ToChar()) })
                                .Where(x => x.shortcutedItem is not null)
                                .Do(x =>
                                {
                                    x.eventArgs.Handled = true;
                                    if (x.shortcutedItem.Command.CanExecute(x.shortcutedItem.CommandParameter))
                                        x.shortcutedItem.Command.Execute(x.shortcutedItem.CommandParameter);
                                }).Subscribe().DisposeWith(disposable);

                            ViewModel.Alerts
                                .ToObservableChangeSet()
                                .ObserveOn(RxApp.MainThreadScheduler)
                                .Transform(tvm => new Alert { ViewModel = tvm })
                                .OnItemAdded(addedAlert => alertsStackPanel.Children.Add(addedAlert))
                                .OnItemRemoved(removedAlert => alertsStackPanel.Children.Remove(removedAlert))
                                .Subscribe().DisposeWith(disposable);

                            ViewModel.CreateUpdatePage = null;
                            ViewModel.IsMenuVisible = true;
                        }
                    })
                    .Subscribe().DisposeWith(d);
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

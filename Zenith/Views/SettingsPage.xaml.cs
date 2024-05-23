using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Zenith.Models.SearchModels;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;
using ReactiveUI;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using System.Reactive.Disposables;
using Zenith.Assets.UI.Helpers;
using Zenith.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Zenith.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : ActivatablePage, IViewFor<SettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();

            ViewModel = new SettingsViewModel();
            backupIntervalComboBox.ItemsSource = typeof(BackupIntervals).ToCollection();

            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;

                var modalBackRect = new Rectangle { Fill = new SolidColorBrush(Color.FromArgb(96, 0, 0, 0)) };
                ((Grid)Content).Children.Insert(0, modalBackRect);
                
                Observable.FromEventPattern(backupSettingsButton, nameof(Button.Click)).Select(_ => 0)
                    .Merge(Observable.FromEventPattern(reminderSettingsButton, nameof(Button.Click)).Select(_ => 1))
                    .Do(clickedIndex =>
                    {
                        ViewModel.IsBackupSettingsSectionVisible = clickedIndex == 0;
                        ViewModel.IsReminderSettingsSectionVisible = clickedIndex == 1;

                        selectedTabIndentifierBorder.SetValue(Grid.ColumnProperty, clickedIndex);
                    }).Subscribe().DisposeWith(d);

                Observable.FromEventPattern(selectPathButton, nameof(Button.Click))
                    .Select(_ => Observable.Using(() => new CommonOpenFileDialog() { IsFolderPicker = true, Title = "Select default backup folder ..." },
                                                    ofd => ofd.ShowDialog() != CommonFileDialogResult.Ok
                                                        ? Observable.Empty<string>()
                                                        : Observable.Return(ofd.FileName)))
                    .Switch()
                    .WhereNotNull()
                    .BindTo(ViewModel, vm => vm.BackupDefaultLocation)
                    .DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SettingsViewModel)value; }
        }

        public SettingsViewModel ViewModel { get; set; }
    }
}

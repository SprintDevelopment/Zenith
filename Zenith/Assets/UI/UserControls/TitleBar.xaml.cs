using DynamicData;
using System;
using System.Windows.Controls;
using Zenith.ViewModels;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();

            var tabControlViewModel = new TabControlViewModel();
            tabStackPanel.ViewModel = tabControlViewModel;

            addNewTabButton.Click += (s, e) => tabControlViewModel._tabs.Add(new TabViewModel { Guid = Guid.NewGuid(), Title = $"Title {DateTime.Now.ToString("T")}", IsSelected = true });
            menuButton.Click += (s, e) => MenuClicked?.Invoke(this, EventArgs.Empty);
            menuButton.PreviewMouseLeftButtonDown += (s, e) => e.Handled = e.ClickCount == 2; // Prevent MenuButton clicked by double clicking
            minimizeButton.Click += (s, e) => Minimized?.Invoke(this, EventArgs.Empty);
            closeButton.Click += (s, e) => Closed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler MenuClicked;
        public event EventHandler Minimized;
        public event EventHandler Closed;
    }
}

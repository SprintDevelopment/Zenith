using System;
using System.Windows.Controls;

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

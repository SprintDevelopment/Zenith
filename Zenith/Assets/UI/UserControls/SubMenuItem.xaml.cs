using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for SubMenuItem.xaml
    /// </summary>
    public partial class SubMenuItem : UserControl
    {
        public PagesOrOperations PageOrOperation
        {
            get { return (PagesOrOperations)base.GetValue(PageOrOperationProperty); }
            set { base.SetValue(PageOrOperationProperty, value); }
        }
        public static readonly DependencyProperty PageOrOperationProperty = DependencyProperty.Register("PageOrOperation", typeof(PagesOrOperations), typeof(SubMenuItem), new FrameworkPropertyMetadata(PagesOrOperations.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PageOrOperationChanged));

        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(SubMenuItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public NavigationLinkAttribute NavigationLink { get; set; } = new NavigationLinkAttribute();

        public static void PageOrOperationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var thisInstance = dependencyObject as SubMenuItem;

            var navigationLink = ((PagesOrOperations)e.NewValue).GetAttribute<NavigationLinkAttribute>();

            if (navigationLink != null)
            {
                thisInstance.NavigationLink.Title = navigationLink.Title;
                thisInstance.NavigationLink.Shortcut = navigationLink.Shortcut;
                thisInstance.NavigationLink.NavigationPageSource = navigationLink.NavigationPageSource;
                thisInstance.NavigationLink.Icon = navigationLink.Icon;
            }
        }


        public SubMenuItem()
        {
            InitializeComponent();
        }

        public event EventHandler Clicked;

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

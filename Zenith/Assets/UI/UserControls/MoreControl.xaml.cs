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

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for MoreControl.xaml
    /// </summary>
    public partial class MoreControl : UserControl
    {
        public ImageSource Icon
        {
            get { return (ImageSource)base.GetValue(IconProperty); }
            set { base.SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MoreControl), new PropertyMetadata(null));

        public object MoreContent
        {
            get { return (object)GetValue(MoreContentProperty); }
            set { SetValue(MoreContentProperty, value); }
        }
        public static readonly DependencyProperty MoreContentProperty = DependencyProperty.Register("MoreContent", typeof(object), typeof(MoreControl), new PropertyMetadata(null));
        
        public MoreControl()
        {
            InitializeComponent();

            openPopupButton.Click += (s, e) => { morePopup.IsOpen = true; };
        }
    }
}

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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for MenuItem.xaml
    /// </summary>
    public partial class MenuItem : UserControl
    {
        public ImageSource Icon
        {
            get { return (ImageSource)base.GetValue(IconProperty); }
            set { base.SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MenuItem), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MenuItem), new PropertyMetadata(""));

        public bool IsSubMenuVisibe
        {
            get { return (bool)base.GetValue(IsSubMenuVisibeProperty); }
            set { base.SetValue(IsSubMenuVisibeProperty, value); }
        }
        public static readonly DependencyProperty IsSubMenuVisibeProperty = DependencyProperty.Register("IsSubMenuVisibe", typeof(bool), typeof(MenuItem), new PropertyMetadata(false));

        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }
        public static readonly DependencyProperty AdditionalContentProperty = DependencyProperty.Register("AdditionalContent", typeof(object), typeof(MenuItem), new PropertyMetadata(null));

        Storyboard showStoryboard, hideStoryboard;

        public MenuItem()
        {
            InitializeComponent();
        }

        private void ChangeStoryboardValues()
        {
            var SubMenuItemCount = (this.AdditionalContent as StackPanel).Children.Count;

            showStoryboard = this.FindResource("ShowSubMenuStoryboard") as Storyboard;

            var showStoryboardDoubleAnimationUsingKeyFrames = showStoryboard.Children[0] as DoubleAnimationUsingKeyFrames;
            var lastEasingDoubleKeyFrame = showStoryboardDoubleAnimationUsingKeyFrames.KeyFrames[1] as EasingDoubleKeyFrame;


            lastEasingDoubleKeyFrame.Value = SubMenuItemCount * 46 + 5;

            hideStoryboard = this.FindResource("HideSubMenuStoryboard") as Storyboard;

            var hideStoryboardDoubleAnimationUsingKeyFrames = hideStoryboard.Children[0] as DoubleAnimationUsingKeyFrames;
            var firstEasingDoubleKeyFrame = hideStoryboardDoubleAnimationUsingKeyFrames.KeyFrames[0] as EasingDoubleKeyFrame;
            firstEasingDoubleKeyFrame.Value = SubMenuItemCount * 50 + 5;
        }

        private void ExpandCollapse_Click(object sender, RoutedEventArgs e)
        {
            ChangeStoryboardValues();
            //
            IsSubMenuVisibe = !IsSubMenuVisibe;

            if (IsSubMenuVisibe)
                showStoryboard.Begin();
            else
                hideStoryboard.Begin();
        }
    }
}
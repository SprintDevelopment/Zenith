using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Zenith.Assets.UI.BaseClasses
{
    public class JTransparentTextBox : TextBox
    {
        public JTransparentTextBox()
        {
            BorderBrush = Background = Brushes.Transparent;
            BorderThickness = new System.Windows.Thickness(0);

            this.GotFocus += (s, e) => { SelectAll(); };

            this.PreviewKeyDown += (s, e) =>
            {
                if ((e.Key == Key.Up || e.Key == Key.Down) && int.TryParse(Text, out int value))
                    Text = (value + (e.Key == Key.Up ? 1 : -1) * (Keyboard.Modifiers == ModifierKeys.Control ? 10 : 1)).ToString();
            };
        }

    }
}

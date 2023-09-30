using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Zenith.Assets.UI.BaseClasses;

namespace Zenith.Assets.UI.Helpers
{
    public class UiGenerator
    {
        public static JTextBox GetTextBox(string titleKey)
        {
            return new JTextBox { Title = (string)App.Current.Resources[titleKey], Margin = new Thickness(12,16,12,0), Height = 42 };
        }

        public static ComboBox GetComboBox(string titleKey)
        {
            return new ComboBox { Tag = (string)App.Current.Resources[titleKey], Margin = new Thickness(12,16,12,0), Height = 42 };
        }
    }
}

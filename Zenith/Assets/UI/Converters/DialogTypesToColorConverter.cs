using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.UI.Converters
{
    public class DialogTypesToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((DialogTypes)value)
            {
                case DialogTypes.Success:
                    return Brushes.Green;
                case DialogTypes.Warning:
                    return Brushes.Orange;
                case DialogTypes.Danger:
                    return Brushes.Red;
                case DialogTypes.Info:
                    return Brushes.Blue;
                default:
                    return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

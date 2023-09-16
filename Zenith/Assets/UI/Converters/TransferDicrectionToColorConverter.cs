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
    public class TransferDicrectionToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((TransferDirections)value)
            {
                case TransferDirections.FromCompnay:
                    return Brushes.Green;
                case TransferDirections.ToCompany:
                    return Brushes.Red;
                default:
                    return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

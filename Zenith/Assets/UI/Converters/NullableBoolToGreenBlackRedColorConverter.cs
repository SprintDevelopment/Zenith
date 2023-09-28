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
    public class NullableBoolToGreenBlackRedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullableBool = (bool?)value;
            return nullableBool.HasValue ? nullableBool.Value ? Brushes.Green : Brushes.Red : Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

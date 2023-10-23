using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Zenith.Assets.Extensions;

namespace Zenith.Assets.UI.Converters
{
    public class FlaggedEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsInt = (int)value;
            var enumType = (Type)parameter;
            
            return Enum.ToObject(enumType, valueAsInt);
        }
    }
}

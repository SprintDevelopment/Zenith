using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Zenith.Assets.Extensions;

namespace Zenith.Assets.UI.Converters
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsInt = (int)value;
            var enumType = (Type)parameter;
            if (enumType.GetAttribute<FlagsAttribute>() != null)
                return string.Join(" ، ", enumType.ToCollection().Where(item => ((int)item.Value | valueAsInt) == valueAsInt).Select(item => item.Description));

            return ((Enum)value).GetDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

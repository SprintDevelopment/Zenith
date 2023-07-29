using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Zenith.Assets.Values.Constants;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.UI.Converters
{
    public class SelectionModesToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectionMode = (SelectionModes)value;

            if (selectionMode == SelectionModes.AllItemsSelected)
                return ImageSourceConstants.ALL_ITEMS_SELECTED;
            return selectionMode == SelectionModes.SomeItemsSelected ? ImageSourceConstants.SOME_ITEMS_SELECTED : ImageSourceConstants.NO_ITEM_SELECTED;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

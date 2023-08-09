using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zenith.Assets.Extensions
{
    public static class ReactiveConverterExtensions
    {
        public static Visibility Viz(this bool boolValue) => boolValue ? Visibility.Visible : Visibility.Collapsed;
    }
}

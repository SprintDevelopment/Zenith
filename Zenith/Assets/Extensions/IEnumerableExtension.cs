using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Assets.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || list.Count() == 0;
        }

        public static ObservableCollection<Model> ToModelObservableCollection<T>(this IEnumerable<T> list) where T : Model
        {
            if (list.IsNullOrEmpty())
                return null;

            return new ObservableCollection<Model>(list.Select(item => (Model)item));
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            if (list.IsNullOrEmpty())
                return null;

            return new ObservableCollection<T>(list);
        }
    }
}

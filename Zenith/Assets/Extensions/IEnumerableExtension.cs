using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;

namespace Zenith.Assets.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || list.Count() == 0;
        }

        public static IEnumerable<TreeViewItemDto> GetHierarchyCollection<T>(this IEnumerable<T> rawEnumerable)
        {
            var keyProperty = typeof(T).GetKeyProperty();
            var parentProperty = typeof(T).GetProperty($"Parent{keyProperty.Name}");

            var grouppedEnumerable = rawEnumerable.GroupBy(x => parentProperty.GetValue(x));
            var result = new List<TreeViewItemDto>(grouppedEnumerable.Where(g => g.Key == null).SelectMany(g => g.Select(item => new TreeViewItemDto { Id = keyProperty.GetValue(item), Title = item.ToString() })));
            var queue = new Queue<TreeViewItemDto>(result);

            while(queue.Count > 0)
            {
                var item = queue.Dequeue();

                var thisItemGroup = grouppedEnumerable.FirstOrDefault(g => g.Key?.ToString() == item.Id.ToString());
                if (thisItemGroup != null)
                    item.Children = thisItemGroup.Select(x =>
                    {
                        var children = new TreeViewItemDto { Parent = item, Id = keyProperty.GetValue(x), Title = x.ToString() };
                        queue.Enqueue(children);

                        return children;
                    }).ToList();
            }

            return result;
        }
        public static TreeViewItemDto SearchInHierarchyCollection<T>(this IEnumerable<TreeViewItemDto> hierarchyEnumerable, T searchedItem)
        {
            return hierarchyEnumerable.SingleOrDefault(item => item.Id.ToString() == searchedItem.GetKeyPropertyValue().ToString())
                ?? (hierarchyEnumerable.Any() ? SearchInHierarchyCollection(hierarchyEnumerable.SelectMany(i => i.Children), searchedItem) : null);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            if (list.IsNullOrEmpty())
                return null;

            return new ObservableCollection<T>(list);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Assets.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static Tuple<T, string> SearchInHierarchicalCollection<T>(this ObservableCollection<T> entities, PropertyInfo keyProperty, object selectedId) where T : Model
        {
            var itemType = entities.FirstOrDefault().GetType();
            var nodeQueue = new Queue<Tuple<T, string>>();

            int index = 0;
            foreach (var item in entities)
                nodeQueue.Enqueue(new Tuple<T, string>(item, $"{index++}"));

            var childrenProperty = itemType.GetProperty("Children");

            while (nodeQueue.Count > 0)
            {
                var currentNode = nodeQueue.Dequeue();

                if (keyProperty.GetValue(currentNode.Item1).ToString() == selectedId.ToString())
                    return currentNode;

                dynamic children = childrenProperty.GetValue(currentNode.Item1);

                index = 0;
                if (children != null && children.Count > 0)
                    foreach (var item in children)
                        nodeQueue.Enqueue(new Tuple<T, string>(item, $"{currentNode.Item2},{index++}"));
            }

            return null;
        }

        public static List<T> GetHierarchyObservableCollection<T>(this IEnumerable<T> rawEnumerable)
        {
            var rawCollection = rawEnumerable.ToList();
            var keyProperty = typeof(T).GetKeyProperty();
            var parentProperty = typeof(T).GetProperty($"Parent{keyProperty.Name}");
            var childrenProperty = typeof(T).GetProperty("Children");

            var allRootNodes = rawCollection.Where(item => parentProperty.GetValue(item) == null).ToList();
            var nodeQueue = new Queue<T>();

            foreach (var node in allRootNodes)
            {
                rawCollection.Remove(node);
                nodeQueue.Enqueue(node);
            }

            while (nodeQueue.Count > 0)
            {
                var currentNode = nodeQueue.Dequeue();

                var currentNodeChildren = rawCollection.Where(item => parentProperty.GetValue(item).ToString() == keyProperty.GetValue(currentNode).ToString()).ToList();
                childrenProperty.SetValue(currentNode, currentNodeChildren);
                foreach (var node in currentNodeChildren)
                {
                    rawCollection.Remove(node);
                    nodeQueue.Enqueue(node);
                }
            }

            return allRootNodes;
        }
    }
}

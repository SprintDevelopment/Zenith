using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Dtos = Zenith.Assets.Values.Dtos;

namespace Zenith.Assets.Extensions
{
    public static class ModelExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return (TAttribute)type.GetCustomAttribute(typeof(TAttribute));
        }

        public static string GetSelector<T>(this T model)
        {
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            };

            var jsonString = JsonConvert.SerializeObject(model, settings);
            var properties = ((JObject)JsonConvert.DeserializeObject(jsonString)).Properties().Select(p => p.Name);
            return properties.IsNullOrEmpty() ? null : properties.Join(", ");
        }

        public static List<Dtos.KeyValuePair> GetKeyValuePairs<T>(this T model)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            };

            var jsonString = JsonConvert.SerializeObject(model, settings);
            return ((JObject)JsonConvert.DeserializeObject(jsonString)).Properties()
                .Select(p => new Dtos.KeyValuePair(p.Name, $"{p.Value}")).ToList();
        }

        public static string GetKeyPropertyName(this Type type)
        {
            return type.GetProperties().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null)?.Name;
        }

        public static PropertyInfo GetKeyProperty(this Type type)
        {
            return type.GetProperties().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
        }

        public static object GetKeyPropertyValue<T>(this T model)
        {
            return typeof(T).GetKeyProperty().GetValue(model);
        }
    }
}

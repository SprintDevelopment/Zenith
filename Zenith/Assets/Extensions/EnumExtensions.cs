using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;

namespace Zenith.Assets.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                            .First().GetCustomAttribute<TAttribute>();
        }

        public static ObservableCollection<EnumDto> ToCollection(this Type enumType, bool hasDontCareItem = false)
        {
            var enumValues = Enum.GetValues(enumType);

            return (from object enumValue in enumValues select new EnumDto(enumValue, ((Enum)enumValue).GetDescription())).OrderBy(item => item.Value).Where(item => hasDontCareItem || ((int)item.Value) >= 0).ToObservableCollection();
        }

        public static string GetDescription(this Enum enumValue)
        {
            return (string)App.Current.Resources[$"{enumValue.GetType().Name}.{enumValue}"];
        }
    }
}

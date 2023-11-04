using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison compareMode)
        {
            if (source.IsNullOrWhiteSpace() && !value.IsNullOrWhiteSpace())
                return false;

            return source.IndexOf(value, compareMode) >= 0;
        }

        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool IsNull(this string source)
        {
            return source == null;
        }

        public static string Join(this IEnumerable<string> source, string separator)
        {
            return source.IsNullOrEmpty() ? null : string.Join(separator, source);
        }

        public static bool HasOnlyOne(this string text, char character)
        {
            int count = 0;
            foreach (var ch in text)
            {
                if (ch == character)
                {
                    if (count == 1)
                        return false;

                    count++;
                }
            }

            return count == 1;
        }
    }
}

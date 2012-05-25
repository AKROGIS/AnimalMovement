using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalMovement
{
    static class ExtensionMethods
    {
        internal static string NullifyIfEmpty(this string s)
        {
            return String.IsNullOrEmpty(s) ? null : s;
        }

        internal static double? DoubleOrNull(this string s)
        {
            double result;
            if (Double.TryParse(s, out result))
                return result;
            return null;
        }

        internal static string BuildSqlList(this IEnumerable<string> items)
        {
            if (items == null)
                return null;
            string result =  "(" + String.Join(",", items.Select(i => "'" + i + "'")) + ")";
            return result == "()" ? null : result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WhetStone.Looping
{
    public static class @enum
    {
        public static IEnumerable<T> Enum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }
        public static IEnumerable<T> EnumFlags<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type with [Flags] attribute");
            }
            long shown = 0;
            foreach (T t in Enum<T>())
            {
                var i = t.ToInt64(CultureInfo.CurrentCulture);
                if ((shown & i) == i)
                    continue;
                shown |= i;
                yield return t;
            }
        }
        public static IEnumerable<T> EnumFlags<T>(this T filter) where T : struct, IConvertible
        {
            var f = (Enum)(dynamic)filter;
            return EnumFlags<T>().Cast<Enum>().Where(a => f.HasFlag(a)).Cast<T>();
        }
    }
}

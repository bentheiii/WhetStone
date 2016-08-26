using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class first
    {
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, T def)
        {
            return !@this.Any() ? def : @this.First();
        }
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, T def)
        {
            return @this.Any(cond) ? @this.First(cond) : def;
        }
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, out bool any)
        {
            foreach (T t in @this)
            {
                if (cond(t))
                {
                    any = true;
                    return t;
                }
            }
            any = false;
            return default(T);
        }
    }
}

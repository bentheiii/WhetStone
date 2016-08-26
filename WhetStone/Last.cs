using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class last
    {
        public static T LastOrDefault<T>(this IEnumerable<T> @this, T def)
        {
            return !@this.Any() ? def : @this.Last();
        }
        public static T LastOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, T def)
        {
            return @this.Any(cond) ? @this.Last(cond) : def;
        }
        public static T LastOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, out bool any)
        {
            T ret = default(T);
            any = false;
            foreach (T t in @this)
            {
                if (cond(t))
                {
                    any = true;
                    ret = t;
                }
            }
            return ret;
        }
    }
}

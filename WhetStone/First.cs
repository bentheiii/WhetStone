using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class first
    {
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, T def)
        {
            var tor = @this.GetEnumerator();
            return !tor.MoveNext() ? def : tor.Current;
        }
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, T def)
        {
            foreach (T t in @this)
            {
                if (cond(t))
                    return t;
            }
            return def;
        }
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, out bool any, T def = default(T))
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
            return def;
        }
    }
}

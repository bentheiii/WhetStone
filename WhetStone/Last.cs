using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class last
    {
        public static T LastOrDefault<T>(this IEnumerable<T> @this, T def)
        {
            var ret = def;
            foreach (T t in @this)
            {
                ret = t;
            }
            return ret;
        }
        public static T LastOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, T def = default(T))
        {
            var ret = def;
            foreach (T t in @this)
            {
                if (cond(t))
                    ret = t;
            }
            return ret;
        }
        public static T LastOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, out bool any, T def = default(T))
        {
            T ret;
            any = false;
            var tor = @this.GetEnumerator();
            while (true)
            {
                if (!tor.MoveNext())
                    return def;
                if (cond(tor.Current))
                {
                    any = true;
                    ret = tor.Current;
                    break;
                }
            }
            while (!tor.MoveNext())
            {
                if (cond(tor.Current))
                    ret = tor.Current;
            }
            return ret;
        }
    }
}

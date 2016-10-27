using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class split
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> @this, T divisor, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return Split(@this, a => comp.Equals(divisor, a));
        }
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> @this, Func<T, bool> divisorDetector)
        {
            var ret = new ResizingArray<T>();
            foreach (var t in @this)
            {
                if (divisorDetector(t))
                {
                    yield return ret.arr;
                    ret = new ResizingArray<T>();
                    continue;
                }
                ret.Add(t);
            }
        }
    }
}

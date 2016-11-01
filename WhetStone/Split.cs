using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class split
    {
        public static IEnumerable<IList<T>> Split<T>(this IList<T> @this, Func<IList<T>, T,bool> capture)
        {
            int indstart = 0;
            int len = 0;
            while (true)
            {
                if (indstart + len >= @this.Count)
                {
                    if (len != 0)
                        yield return @this.Slice(indstart, length: len);
                    yield break;
                }
                if (capture(@this.Slice(indstart, length: len), @this[indstart + len]))
                {
                    len++;
                }
                else
                {
                    yield return @this.Slice(indstart, length: len);
                    indstart = indstart + len;
                    len = 0;
                }
            }
        }
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

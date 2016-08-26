using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class longestCommonPrefix
    {
        public static IEnumerable<IEnumerable<T>> LongestCommonPrefix<T>(this IEnumerable<IEnumerable<T>> @this, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Zip().TakeWhile(a => a.AllEqual(comp));
        }
        public static IEnumerable<Tuple<T, T>> LongestCommonPrefix<T>(this IEnumerable<T> @this, IEnumerable<T> other, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Zip(other).TakeWhile(a => comp.Equals(a.Item1, a.Item2));
        }
    }
}

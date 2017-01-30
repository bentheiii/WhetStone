using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class startsWith
    {
        public static bool StartsWith<T>(this IEnumerable<T> @this, IEnumerable<T> prefix, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Select(a => Tuple.Create(a)).Zip(prefix.Select(a => Tuple.Create(a))).TakeWhile(x => x.Item2 != null).All(
                    x => x.Item1 != null && comp.Equals(x.Item1.Item1, x.Item2.Item1));
        }
    }
}

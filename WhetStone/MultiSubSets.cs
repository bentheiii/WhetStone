using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class multiSubSets
    {
        public static IEnumerable<IEnumerable<int>> MultiSubSets(this IEnumerable<int> @this, bool inclusive = true)
        {
            return @this.Select(a => (inclusive ? range.IRange(a) : range.Range(a)).ToArray()).ToArray().Join();
        }
        public static IEnumerable<IEnumerable<T>> MultiSubSets<T>(this IEnumerable<T> @this, bool inclusive = true)
        {
            return @this.Select(a => (inclusive ? range.IRange(a) : range.Range(a)).ToArray()).ToArray().Join();
        }
        public static IEnumerable<IEnumerable<Tuple<T, int>>> MultiSubSets<T>(this IEnumerable<Tuple<T, int>> @this, bool inclusive = true)
        {
            var items = @this.Select(a => a.Item1).ToArray();
            return @this.Select(a => a.Item2).MultiSubSets(inclusive).Select(x => items.Zip(x));
        }
        public static IEnumerable<IEnumerable<Tuple<T, G>>> MultiSubSets<T, G>(this IEnumerable<Tuple<T, G>> @this, bool inclusive = true)
        {
            var items = @this.Select(a => a.Item1);
            return @this.Select(a => a.Item2).MultiSubSets(inclusive).Select(x => items.Zip(x));
        }
    }
}

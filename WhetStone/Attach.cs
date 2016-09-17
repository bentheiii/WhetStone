using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class attach
    {
        public static IEnumerable<Tuple<T1, T2>> Attach<T1, T2>(this IEnumerable<T1> @this, Func<T1, T2> selector)
        {
            return @this.Select(a => Tuple.Create(a, selector(a)));
        }
        public static IEnumerable<Tuple<T1, T2>> Attach<T1, T2>(this IEnumerable<Tuple<T1>> @this, Func<T1, T2> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, selector(a.Item1)));
        }
        public static IEnumerable<Tuple<T1, T2, T3>> Attach<T1, T2, T3>(this IEnumerable<Tuple<T1, T2>> @this, Func<T1, T2, T3> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, selector(a.Item1, a.Item2)));
        }
        public static IEnumerable<Tuple<T1, T2, T3, T4>> Attach<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3>> @this, Func<T1, T2, T3, T4> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, a.Item3, selector(a.Item1, a.Item2, a.Item3)));
        }
        public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> Attach<T1, T2, T3, T4, T5>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this, Func<T1, T2, T3, T4, T5> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, a.Item3, a.Item4, selector(a.Item1, a.Item2, a.Item3, a.Item4)));
        }
        public static LockedList<Tuple<T1, T2>> Attach<T1, T2>(this IList<T1> @this, Func<T1, T2> selector)
        {
            return @this.Select(a => Tuple.Create(a, selector(a)));
        }
        public static LockedList<Tuple<T1, T2>> Attach<T1, T2>(this IList<Tuple<T1>> @this, Func<T1, T2> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, selector(a.Item1)));
        }
        public static LockedList<Tuple<T1, T2, T3>> Attach<T1, T2, T3>(this IList<Tuple<T1, T2>> @this, Func<T1, T2, T3> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, selector(a.Item1, a.Item2)));
        }
        public static LockedList<Tuple<T1, T2, T3, T4>> Attach<T1, T2, T3, T4>(this IList<Tuple<T1, T2, T3>> @this, Func<T1, T2, T3, T4> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, a.Item3, selector(a.Item1, a.Item2, a.Item3)));
        }
        public static LockedList<Tuple<T1, T2, T3, T4, T5>> Attach<T1, T2, T3, T4, T5>(this IList<Tuple<T1, T2, T3, T4>> @this, Func<T1, T2, T3, T4, T5> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, a.Item3, a.Item4, selector(a.Item1, a.Item2, a.Item3, a.Item4)));
        }
    }
}

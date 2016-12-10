using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class unZip
    {
        public static Tuple<IEnumerable<T1>, IEnumerable<T2>> UnZip<T1, T2>(this IEnumerable<Tuple<T1, T2>> @this)
        {
            return Tuple.Create(@this.Select(a => a.Item1), @this.Select(a => a.Item2));
        }
        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> UnZip<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> @this)
        {
            return Tuple.Create(@this.Select(a => a.Item1), @this.Select(a => a.Item2), @this.Select(a => a.Item3));
        }
        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> UnZip<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this)
        {
            return Tuple.Create(@this.Select(a => a.Item1), @this.Select(a => a.Item2), @this.Select(a => a.Item3), @this.Select(a=>a.Item4));
        }
        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>> UnZip<T1, T2, T3, T4, T5>(this IEnumerable<Tuple<T1, T2, T3, T4, T5>> @this)
        {
            return Tuple.Create(@this.Select(a => a.Item1), @this.Select(a => a.Item2), @this.Select(a => a.Item3), @this.Select(a => a.Item4), @this.Select(a => a.Item5));
        }
        public static Tuple<IList<T1>, IList<T2>> UnZip<T1, T2>(this IList<Tuple<T1, T2>> @this)
        {
            return Tuple.Create(@this.Select(a => a.Item1).AsList(), @this.Select(a => a.Item2).AsList());
        }
        public static Tuple<IList<T1>, IList<T2>, IList<T3>> UnZip<T1, T2, T3>(this IList<Tuple<T1, T2, T3>> @this)
        {
            return Tuple.Create(@this.Select(a => a.Item1).AsList(), @this.Select(a => a.Item2).AsList(), @this.Select(a => a.Item3).AsList());
        }
        public static Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>> UnZip<T1, T2, T3, T4>(this IList<Tuple<T1, T2, T3, T4>> @this)
        {
            return Tuple.Create(@this.Select(a => a.Item1).AsList(), @this.Select(a => a.Item2).AsList(), @this.Select(a => a.Item3).AsList(), @this.Select(a => a.Item4).AsList());
        }
        public static Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>, IList<T5>> UnZip<T1, T2, T3, T4, T5>(this IList<Tuple<T1, T2, T3, T4, T5>> @this)
        {
            return Tuple.Create(@this.Select(a => a.Item1).AsList(), @this.Select(a => a.Item2).AsList(), @this.Select(a => a.Item3).AsList(), @this.Select(a => a.Item4).AsList(), @this.Select(a => a.Item5).AsList());
        }
    }
}

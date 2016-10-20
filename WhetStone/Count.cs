﻿using System.Collections.Generic;
using System.Linq;
using WhetStone.Comparison;

namespace WhetStone.Looping
{
    public static class count
    {
        public static int Count<T>(this IEnumerable<T> @this, T query, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Count(a => comp.Equals(a, query));
        }
        public static int Count<T>(this IEnumerable<T> @this, IEnumerable<T> query, IEqualityComparer<IEnumerable<T>> comp = null)
        {
            comp = comp ?? new EnumerableEqualityCompararer<T>();
            return @this.Trail(query.Count()).Count(a => comp.Equals(@this,a));
        }
    }
}

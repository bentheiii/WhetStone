using System;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class selectMany
    {
        public static IList<R> SelectMany<T, R>(this IList<T> @this, Func<T, IList<R>> selector)
        {
            return @this.Select(selector).Concat();
        }
        public static LockedList<R> SelectMany<T,R>(this ICollection<T> @this, Func<T, IEnumerable<R>> selector)
        {
            return ((IList<IEnumerable<R>>)@this.Select(selector)).Concat();
        }
    }
}

using System;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class selectMany
    {
        public static IList<R> SelectMany<T, R>(this IList<T> @this, Func<T, IList<R>> selector, bool? samecount = false)
        {
            return @this.Select(selector).Concat(samecount);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone
{
    public static class isSorted
    {
        public static bool IsSorted<T>(this IEnumerable<T> @this, IComparer<T> comp = null, bool allowEquals = true)
        {
            comp = comp ?? Comparer<T>.Default;
            return
                @this.Trail(2).All(allowEquals
                    ? (Func<T[], bool>)(a => comp.Compare(a[0], a[1]) <= 0)
                    :                  (a => comp.Compare(a[0], a[1]) <  0));
        }
    }
}

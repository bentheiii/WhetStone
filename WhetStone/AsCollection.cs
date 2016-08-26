using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class asCollection
    {
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> @this)
        {
            var l = @this as ICollection<T>;
            if (l != null)
                return l;
            var r = @this as IReadOnlyCollection<T>;
            if (r != null)
                return r.ToLockedCollection();
            return @this.ToArray();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class asList
    {
        public static IList<T> AsList<T>(this IEnumerable<T> @this)
        {
            var l = @this as IList<T>;
            if (l != null)
                return l;
            var r = @this as IReadOnlyList<T>;
            if (r != null)
                return r.ToLockedList();
            var s = @this as string;
            if (s != null)
                return (IList<T>)new LockedListStringAdaptor(s);
            return @this.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class selectMany
    {
        public static LockedList<T> SelectMany<T>(this IList<T> @this, Func<T, IList<T>> selector)
        {
            return @this.Select(selector).Concat();
        }
        public static LockedList<T> SelectMany<T>(this ICollection<T> @this, Func<T, IEnumerable<T>> selector)
        {
            return ((IList<IEnumerable<T>>)@this.Select(selector)).Concat();
        }
    }
}

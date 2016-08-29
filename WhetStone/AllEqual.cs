using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    //todo: comparer extentions?
    public static class allEqual
    {
        public static bool AllEqual<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp = null)
        {
            if (!@this.Any())
                return true;
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.All(a => comp.Equals(a, @this.First()));
        }
    }
}

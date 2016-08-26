using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class startsWith
    {
        public static bool StartsWith<T>(this IEnumerable<T> @this, IEnumerable<T> prefix, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.CompareCount(prefix) >= 0 && @this.Zip(prefix, comp.Equals).All();
        }
    }
}

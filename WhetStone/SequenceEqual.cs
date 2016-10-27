using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class sequenceEqual
    {
        public static bool SequenceEqual<T>(this IEnumerable<T> @this, params T[] seq)
        {
            return @this.SequenceEqual(seq.AsEnumerable());
        }
    }
}

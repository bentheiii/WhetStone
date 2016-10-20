using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

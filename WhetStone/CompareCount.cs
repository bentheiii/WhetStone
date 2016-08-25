using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Arrays
{
    public static class compareCount
    {
        public static int CompareCount<T0, T1>(this IEnumerable<T0> @this, IEnumerable<T1> other)
        {
            int? rect = @this.RecommendSize();
            if (rect.HasValue)
            {
                int? reco = other.RecommendSize();
                if (reco.HasValue)
                    return rect.Value.CompareTo(reco.Value);
            }
            var tor = new IEnumerator[] { @this.GetEnumerator(), other.GetEnumerator() };
            var next = tor.Select(a => a.MoveNext()).ToArray();
            while (next.All(a => a))
            {
                next = tor.Select(a => a.MoveNext()).ToArray();
            }
            if (next[0] == next[1])
                return 0;
            return next[0] ? 1 : -1;
        }
    }
}

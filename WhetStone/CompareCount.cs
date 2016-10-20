using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class compareCount
    {
        public static int CompareCount<T0, T1>(this IEnumerable<T0> @this, IEnumerable<T1> other)
        {

            int? rect = @this.RecommendCount();
            int? reco = other.RecommendCount();

            if (reco.HasValue && rect.HasValue)
                return rect.Value.CompareTo(reco.Value);
            if (reco.HasValue) //rect is null
            {
                int c = 0;
                foreach (var t0 in @this)
                {
                    c++;
                    if (c >= reco.Value + 1)
                        return 1;
                }
                return c == reco.Value ? 0 : -1;
            }
            if (rect.HasValue) //reco is null
            {
                int c = 0;
                foreach (var t0 in other)
                {
                    c++;
                    if (c >= rect.Value + 1)
                        return -1;
                }
                return c == rect.Value ? 0 : 1;
            }


            var tor = new IEnumerator[] {@this.GetEnumerator(), other.GetEnumerator()}.AsEnumerable();
            var next = tor.Select(a => a.MoveNext()).ToArray();
            while (next.All())
            {
                next = tor.Select(a => a.MoveNext()).ToArray();
            }
            if (next[0] == next[1])
                return 0;
            return next[0] ? 1 : -1;
        }
    }
}

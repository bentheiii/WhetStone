using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class occurances
    {
        public static IEnumerable<Tuple<T,int>> ToOccurancesSorted<T>(this IEnumerable<T> @this, IEqualityComparer<T> c = null)
        {
            c = c ?? EqualityComparer<T>.Default;
            var tor = @this.GetEnumerator();
            int ret = 0;
            T mem = default(T);
            foreach (var t in @this)
            {
                if (ret == 0)
                {
                    ret = 1;
                    mem = t;
                    continue;
                }
                if (c.Equals(mem, t))
                {
                    ret++;
                }
                else
                {
                    yield return Tuple.Create(mem, ret);
                    mem = t;
                    ret = 1;
                }
            }
            if (ret != 0)
                yield return Tuple.Create(mem, ret);
        }
        public static IDictionary<T, int> ToOccurances<T>(this IEnumerable<T> arr, IEqualityComparer<T> c = null)
        {
            c = c ?? EqualityComparer<T>.Default;
            Dictionary<T, int> oc = new Dictionary<T, int>(c);
            foreach (T v in arr)
            {
                if (oc.ContainsKey(v))
                    oc[v]++;
                else
                    oc[v] = 1;
            }
            return oc;
        }
        public static IEnumerable<T> FromOccurances<T>(this IEnumerable<KeyValuePair<T, int>> d)
        {
            return d.SelectMany(a => a.Key.Enumerate().Repeat(a.Value));
        }
        public static IList<T> FromOccurances<T>(this IList<KeyValuePair<T, int>> d)
        {
            return d.SelectMany(a => a.Key.Enumerate(a.Value));
        }
    }
}

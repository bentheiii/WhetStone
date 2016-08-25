using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WhetStone.Looping;

namespace WhetStone.Arrays
{
    public static class occurances
    {
        public static IEnumerable<KeyValuePair<T,int>> ToOccurancesSorted<T>(this IEnumerable<T> @this, IEqualityComparer<T> c = null)
        {
            c = c ?? EqualityComparer<T>.Default;
            var tor = @this.GetEnumerator();
            while (tor.MoveNext())
            {
                int ret = 0;
                T member = tor.Current;
                while (c.Equals(member,tor.Current))
                {
                    ret++;
                    if (!tor.MoveNext())
                    {
                        yield return new KeyValuePair<T, int>(member,ret);
                        yield break;
                    }
                }
                yield return new KeyValuePair<T, int>(member, ret);
            }
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
            return d.Select(a => a.Key.Enumerate().Repeat(a.Value)).Concat();
        }
    }
}

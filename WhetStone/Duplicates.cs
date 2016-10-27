using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class duplicates
    {
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> arr, IEqualityComparer<T> comp = null, int minoccurances = 2)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            var occurances = new Dictionary<T, int>(comp);
            foreach (var t in arr)
            {
                if (occurances.EnsureValue(t) && occurances[t] == 0)
                    continue;
                occurances[t]++;
                if (occurances[t] >= minoccurances)
                {
                    yield return t;
                    occurances[t] = 0;
                }
            }
        }
        public static IEnumerable<T> DuplicatesSorted<T>(this IEnumerable<T> arr, IEqualityComparer<T> comp = null, int minoccurances = 2)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return arr.ToOccurancesSorted(comp).Where(a => a.Item2 >= minoccurances).Select(a => a.Item1);
        }
    }
}

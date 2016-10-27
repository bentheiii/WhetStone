using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class uniques
    {
        public static IEnumerable<T> Uniques<T>(this IEnumerable<T> arr, IEqualityComparer<T> comp = null, int maxoccurances = 1)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return arr.ToOccurances(comp).Where(a => a.Value <= maxoccurances).Select(a => a.Key);
        }
        public static IEnumerable<T> UniquesSorted<T>(this IEnumerable<T> arr, IEqualityComparer<T> comp = null, int maxoccurances = 1)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return arr.ToOccurancesSorted(comp).Where(a => a.Item2 <= maxoccurances).Select(a => a.Item1);
        }
    }
}

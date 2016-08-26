using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class getMedian
    {
        public static T GetMedian<T>(this IEnumerable<T> @this, IComparer<T> comparer, out int index)
        {
            if (!@this.Any())
                throw new ArgumentException("cannot be empty", nameof(@this));
            int c = @this.Count();
            var res = @this.CountBind().OrderBy(Comparer<Tuple<T, int>>.Create((a, b) => comparer.Compare(a.Item1, b.Item1))).ElementAt(c / 2);
            index = res.Item2;
            return res.Item1;
        }
        public static T GetMedian<T>(this IEnumerable<T> tosearch, IComparer<T> comparer = null)
        {
            int prox;
            return tosearch.GetMedian(comparer ?? Comparer<T>.Default, out prox);
        }
        public static T GetMedian<T>(this IEnumerable<T> tosearch, out int index) where T : IComparable<T>
        {
            return tosearch.GetMedian(Comparer<T>.Default, out index);
        }
    }
}

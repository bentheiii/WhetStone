using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Comparison;

namespace WhetStone.Looping
{
    public static class getMode
    {
        public static T GetMode<T>(this IEnumerable<T> tosearch, IEqualityComparer<T> comparer, out int index)
        {
            if (!tosearch.Any())
                throw new ArgumentException("cannot be empty", nameof(tosearch));
            var oc = tosearch.CountBind().ToOccurances(new EqualityFunctionComparer<Tuple<T, int>, T>(a => a.Item1, comparer));
            KeyValuePair<Tuple<T, int>, int> max = oc.GetMax(new FunctionComparer<KeyValuePair<Tuple<T, int>, int>>(a => a.Value));
            index = max.Key.Item2;
            return max.Key.Item1;
        }
        public static T GetMode<T>(this IEnumerable<T> tosearch, out int index)
        {
            return GetMode(tosearch, EqualityComparer<T>.Default, out index);
        }
        public static T GetMode<T>(this IEnumerable<T> tosearch)
        {
            int prox;
            return tosearch.GetMode(out prox);
        }
    }
}

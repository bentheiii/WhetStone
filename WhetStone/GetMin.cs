using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class getMin
    {
        public static T GetMin<T>(this IEnumerable<T> tosearch, IComparer<T> compare, out int index)
        {
            if (!tosearch.Any())
                throw new ArgumentException("cannot be empty", nameof(tosearch));
            T ret = tosearch.First();
            index = 0;
            int i = 1;
            foreach (T var in tosearch.Skip(1))
            {
                if (compare.Compare(var, ret) < 0)
                {
                    ret = var;
                    index = i;
                }
                i++;
            }
            return ret;
        }
        public static T GetMin<T>(this IEnumerable<T> tosearch, IComparer<T> compare = null)
        {
            int prox;
            return tosearch.GetMin(compare ?? Comparer<T>.Default, out prox);
        }
        public static T GetMin<T>(this IEnumerable<T> tosearch, out int index)
        {
            return tosearch.GetMin(Comparer<T>.Default, out index);
        }
    }
}

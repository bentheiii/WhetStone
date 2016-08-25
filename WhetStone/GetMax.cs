using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Comparison;

namespace WhetStone.Arrays
{
    public static class getMax
    {
        public static T GetMax<T>(this IEnumerable<T> tosearch, IComparer<T> compare, out int index)
        {
            return tosearch.GetMin(compare.Reverse(), out index);
        }
        public static T GetMax<T>(this IEnumerable<T> tosearch, IComparer<T> compare = null)
        {
            int prox;
            return tosearch.GetMax(compare ?? Comparer<T>.Default, out prox);
        }
        public static T GetMax<T>(this IEnumerable<T> tosearch, out int index)
        {
            return tosearch.GetMax(Comparer<T>.Default, out index);
        }
    }
}

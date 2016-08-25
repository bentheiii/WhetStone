using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class merge
    {
        public static IEnumerable<T> Merge<T>(this IEnumerable<IEnumerable<T>> @this, Func<T[], int> chooser)
        {
            var numerators = new List<IEnumerator<T>>(@this.Select(a => a.GetEnumerator()));
            numerators.RemoveAll(a => !a.MoveNext());
            while (numerators.Any())
            {
                int index = chooser(numerators.Select(a => a.Current).ToArray());
                yield return numerators[index].Current;
                if (!numerators[index].MoveNext())
                    numerators.RemoveAt(index);
            }
        }
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> @this, IEnumerable<T> other, IComparer<T> chooser = null)
        {
            chooser = chooser ?? Comparer<T>.Default;
            return new[] { @this, other }.Merge(a => a.Length > 1 ? (chooser.Compare(a[0], a[1]) < 0 ? 0 : 1) : 0);
        }
    }
}

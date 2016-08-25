using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Tuples
{
    public static class toEnumerable
    {
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T> @this)
        {
            yield return @this.Item1;
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T> @this)
        {
            yield return @this.Item1;
            yield return @this.Item2;
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T> @this)
        {
            yield return @this.Item1;
            yield return @this.Item2;
            yield return @this.Item3;
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T, T> @this)
        {
            yield return @this.Item1;
            yield return @this.Item2;
            yield return @this.Item3;
            yield return @this.Item4;
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T, T, T> @this)
        {
            yield return @this.Item1;
            yield return @this.Item2;
            yield return @this.Item3;
            yield return @this.Item4;
            yield return @this.Item5;
        }
    }
}

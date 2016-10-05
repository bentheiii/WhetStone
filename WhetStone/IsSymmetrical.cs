using System.Collections.Generic;
using System.Linq;
using WhetStone.NumbersMagic;

namespace WhetStone.Looping
{
    public static class isSymmetrical
    {
        public static bool IsSymmetrical<T>(this IList<T> @this)
        {
            return IsSymmetrical(@this, EqualityComparer<T>.Default);
        }
        public static bool IsSymmetrical<T>(this IList<T> @this, IEqualityComparer<T> c)
        {
            return range.Range(@this.Count / 2).All(i => c.Equals(@this[i], @this[(-i - 1).TrueMod(@this.Count)]));
        }
        public static bool IsSymmetrical<T>(this IEnumerable<T> @this, IEqualityComparer<T> c)
        {
            int? count = @this.RecommendCount();
            int len = count / 2 ?? int.MaxValue;
            return @this.Zip(@this.Reverse()).Take(len).All(a => c.Equals(a.Item1, a.Item2));
        }
        public static bool IsSymmetrical<T>(this IEnumerable<T> @this)
        {
            return IsSymmetrical(@this, EqualityComparer<T>.Default);
        }
    }
}

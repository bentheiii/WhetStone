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
    }
}

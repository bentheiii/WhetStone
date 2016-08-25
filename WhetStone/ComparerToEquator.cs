using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Comparison
{
    public static class ComparerToEquator
    {
        public static IEqualityComparer<T> ToEqualityComparer<T>(this IComparer<T> comp)
        {
            return ToEqualityComparer(comp, a => a.GetHashCode());
        }
        public static IEqualityComparer<T> ToEqualityComparer<T>(this IComparer<T> comp, Func<T, int> hash)
        {
            return new WrappedComparerclass<T>(comp, hash);
        }
        private class WrappedComparerclass<T> : IEqualityComparer<T>
        {
            private readonly IComparer<T> _comp;
            private readonly Func<T, int> _hash;
            public WrappedComparerclass(IComparer<T> c, Func<T, int> hash)
            {
                this._comp = c;
                _hash = hash;
            }
            public bool Equals(T x, T y)
            {
                return _comp.Compare(x, y) == 0;
            }
            public int GetHashCode(T obj)
            {
                return _hash(obj);
            }
        }
    }
}

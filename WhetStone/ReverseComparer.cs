using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Comparison
{
    public static class ReverseComparer
    {
        public static IComparer<T> Reverse<T>(this IComparer<T> comp)
        {
            return new ReverseComparerClass<T>(comp);
        }
        private class ReverseComparerClass<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return -this._comp.Compare(x, y);
            }
            private readonly IComparer<T> _comp;
            public ReverseComparerClass(IComparer<T> c)
            {
                this._comp = c;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Comparison
{
    public class FunctionComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _c;
        public int Compare(T x, T y)
        {
            return _c(x, y);
        }
        public FunctionComparer(Func<T, T, int> c)
        {
            this._c = c;
        }
        public FunctionComparer(Func<T, IComparable> c)
        {
            if (c == null)
                throw new ArgumentNullException();
            this._c = (a, b) => c(a).CompareTo(c(b));
        }
        public FunctionComparer(Func<T, object> f, IComparer c)
        {
            this._c = (a, b) => c.Compare(f(a), f(b));
        }
    }
    public class FunctionComparer<T, G> : IComparer<T>
    {
        private readonly Func<T, T, int> _c;
        public int Compare(T x, T y)
        {
            return _c(x, y);
        }
        public FunctionComparer(Func<T, G> f) : this(f, Comparer<G>.Default) { }
        public FunctionComparer(Func<T, G> f, IComparer<G> c)
        {
            this._c = (a, b) => c.Compare(f(a), f(b));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Comparison
{
    public class EqualityFunctionComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _func;
        private readonly Func<T, int> _hash;
        public EqualityFunctionComparer(Func<T, T, bool> func, Func<T, int> hash)
        {
            this._func = func;
            this._hash = hash;
        }
        public EqualityFunctionComparer(Func<T, object> c)
        {
            this._hash = a => c(a).GetHashCode();
            this._func = (a, b) => c(a).Equals(c(b));
        }
        public EqualityFunctionComparer(Func<T, object> c, IEqualityComparer e)
        {
            this._hash = a => c(a).GetHashCode();
            this._func = (a, b) => e.Equals(c(a), (c(b)));
        }
        public EqualityFunctionComparer(Func<T, object> c, IEqualityComparer<object> e)
        {
            this._hash = a => c(a).GetHashCode();
            this._func = (a, b) => e.Equals(c(a), (c(b)));
        }
        public bool Equals(T x, T y)
        {
            return this._func(x, y);
        }
        public int GetHashCode(T obj)
        {
            return _hash(obj);
        }
    }
    public class EqualityFunctionComparer<T, G> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _func;
        private readonly Func<T, int> _hash;
        public EqualityFunctionComparer(Func<T, G> c)
        {
            this._hash = a => c(a).GetHashCode();
            this._func = (a, b) => c(a).Equals(c(b));
        }
        public EqualityFunctionComparer(Func<T, G> c, IEqualityComparer<G> e)
        {
            this._hash = a => c(a).GetHashCode();
            this._func = (a, b) => e.Equals(c(a), (c(b)));
        }
        public bool Equals(T x, T y)
        {
            return this._func(x, y);
        }
        public int GetHashCode(T obj)
        {
            return _hash(obj);
        }
    }
}

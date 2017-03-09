using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Comparison
{
    /// <summary>
    /// A comparer that maps a value from one type to another, for either equality comparison or order comparison.
    /// </summary>
    /// <typeparam name="T">The original type to be compared.</typeparam>
    /// <typeparam name="G">The mapped type to compare.</typeparam>
    public class FunctionComparer<T, G> : IComparer<T>, IEqualityComparer<T>
    {
        private readonly Func<T, G> _f;
        private readonly IEqualityComparer<G> _e;
        private readonly IComparer<G> _c;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="f">The mapper function to map from <typeparamref name="T"/> to <typeparamref name="G"/>.</param>
        /// <param name="c">The internal <see cref="IComparer{T}"/> to compare mapped elements with. <see langword="null"/> for default.</param>
        /// <param name="e">the internal <see cref="IEqualityComparer{T}"/> to hash and compare equality of mapped elements with. <see langword="null"/> for <paramref name="c"/> to compare equality and default hasher.</param>
        public FunctionComparer(Func<T, G> f, IComparer<G> c = null, IEqualityComparer<G> e = null)
        {
            f.ThrowIfNull(nameof(f));
            _f = f;
            _c = c ?? Comparer<G>.Default;
            _e = e;
        }
        /// <inheritdoc />
        public int Compare(T x, T y)
        {
            return _c.Compare(_f(x), _f(y));
        }
        /// <inheritdoc />
        public bool Equals(T x, T y)
        {
            if (_e == null)
            {
                return Compare(x, y) == 0;
            }
            return _e.Equals(_f(x), _f(y));
        }
        /// <inheritdoc />
        public int GetHashCode(T obj)
        {
            var tohash = _f(obj);
            return (_e ?? EqualityComparer<G>.Default).GetHashCode(tohash);
        }
    }
}

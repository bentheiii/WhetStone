using System;
using System.Collections.Generic;

namespace WhetStone.Comparison
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> for <see cref="ValueTuple{T1,T2}"/>
    /// </summary>
    /// <typeparam name="T1">The first type of the tuple.</typeparam>
    /// <typeparam name="T2">The second type of the tuple.</typeparam>
    public class TupleEqualityComparer<T1, T2> : IEqualityComparer<(T1, T2)>
    {
        private readonly IEqualityComparer<T1> _c1;
        private readonly IEqualityComparer<T2> _c2;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="c1">The comparer between the first elements. <see langword="null"/> for default.</param>
        /// <param name="c2">The comparer between the second elements. <see langword="null"/> for default.</param>
        public TupleEqualityComparer(IEqualityComparer<T1> c1 = null, IEqualityComparer<T2> c2 = null)
        {
            _c1 = c1 ?? EqualityComparer<T1>.Default;
            _c2 = c2 ?? EqualityComparer<T2>.Default;
        }
        /// <inheritdoc />
        public bool Equals((T1, T2) x, (T1, T2) y)
        {
            return _c1.Equals(x.Item1, y.Item1) && _c2.Equals(x.Item2, x.Item2);
        }
        /// <inheritdoc />
        public int GetHashCode((T1, T2) obj)
        {
            return _c1.GetHashCode(obj.Item1) ^ _c2.GetHashCode(obj.Item2);
        }
    }
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> for <see cref="Tuple{T1,T2}"/>
    /// </summary>
    /// <typeparam name="T1">The first type of the tuple.</typeparam>
    /// <typeparam name="T2">The second type of the tuple.</typeparam>
    /// <typeparam name="T3">The third type of the tuple.</typeparam>
    public class TupleEqualityComparer<T1, T2, T3> : IEqualityComparer<Tuple<T1, T2, T3>>
    {
        private readonly IEqualityComparer<T1> _c1;
        private readonly IEqualityComparer<T2> _c2;
        private readonly IEqualityComparer<T3> _c3;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="c1">The comparer between the first elements. <see langword="null"/> for default.</param>
        /// <param name="c2">The comparer between the second elements. <see langword="null"/> for default.</param>
        /// <param name="c3">The comparer between the third elements. <see langword="null"/> for default.</param>
        public TupleEqualityComparer(IEqualityComparer<T1> c1 = null, IEqualityComparer<T2> c2 = null, IEqualityComparer<T3> c3 = null)
        {
            _c1 = c1 ?? EqualityComparer<T1>.Default;
            _c2 = c2 ?? EqualityComparer<T2>.Default;
            _c3 = c3 ?? EqualityComparer<T3>.Default;
        }
        /// <inheritdoc />
        public bool Equals(Tuple<T1, T2, T3> x, Tuple<T1, T2, T3> y)
        {
            return _c1.Equals(x.Item1, y.Item1) && _c2.Equals(x.Item2, x.Item2) && _c3.Equals(x.Item3, y.Item3);
        }
        /// <inheritdoc />
        public int GetHashCode(Tuple<T1, T2, T3> obj)
        {
            return _c1.GetHashCode(obj.Item1) ^ _c2.GetHashCode(obj.Item2) ^ _c3.GetHashCode(obj.Item3);
        }
    }
}

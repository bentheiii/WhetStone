using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;

namespace WhetStone.Comparison
{
    /// <summary>
    /// A comparer that compares <see cref="IEnumerable{T}"/>s, element-wise, then length-wise.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s to compare.</typeparam>
    public class EnumerableCompararer<T> : IComparer<IEnumerable<T>>, IEqualityComparer<IEnumerable<T>>
    {
        private readonly IComparer<T> _int;
        private readonly IEqualityComparer<T> _eq;
        private readonly int? _hashtake;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="i">The inner <see cref="IComparer{T}"/> to compare individual elements. <see langword="null"/> for default.</param>
        /// <param name="eq">The inner <see cref="IEqualityComparer{T}"/> to compare and hash individual elements. <see langword="null"/> to use <paramref name="i"/> to compare equality and disallow hashing.</param>
        /// <param name="hashTake">The maximum number of element to take for the hash function. <see langword="null"/> for no maximum.</param>
        public EnumerableCompararer(IComparer<T> i = null, IEqualityComparer<T> eq = null, int? hashTake = null)
        {
            this._int = i ?? Comparer<T>.Default;
            _eq = eq;
            _hashtake = hashTake;
        }
        /// <inheritdoc />
        public int Compare(IEnumerable<T> x, IEnumerable<T> y)
        {
            int ret = 0;
            foreach (var z in x.ZipUnBoundTuple(y))
            {
                if (z.Item1 == null)
                    return -1;
                if (z.Item2 == null)
                    return 1;
                ret = _int.Compare(z.Item1.Item1, z.Item2.Item1);
                if (ret != 0)
                    break;
            }
            return ret;
        }
        /// <inheritdoc />
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (_eq != null)
                return x.ZipUnBoundTuple(y).All(a =>
                {
                    if (a.Item1 == null || a.Item2 == null)
                        return false;
                    return _eq.Equals(a.Item1.Item1, a.Item2.Item1);
                });
            return Compare(x, y) == 0;
        }
        /// <inheritdoc />
        public int GetHashCode(IEnumerable<T> obj)
        {
            if (_eq == null)
                throw new NotSupportedException();
            if (_hashtake.HasValue)
                obj = obj.Take(_hashtake.Value);
            return obj.Select(a => _eq.GetHashCode(a)).Aggregate((a, b) => a ^ b);
        }
    }
}

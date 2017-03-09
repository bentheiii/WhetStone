using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Comparison
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class ReverseComparer
    {
        /// <summary>
        /// Get an <see cref="IComparer{T}"/> that is reverse of another.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IComparer{T}"/></typeparam>
        /// <param name="comp">The <see cref="IComparer{T}"/> to reverse.</param>
        /// <returns>A reversed <see cref="IComparer{T}"/> that creates an opposite order than <paramref name="comp"/>.</returns>
        public static IComparer<T> Reverse<T>(this IComparer<T> comp)
        {
            comp.ThrowIfNull(nameof(comp));
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

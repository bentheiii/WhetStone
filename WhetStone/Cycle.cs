using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class cycle
    {
        private class CycleList<T> : LockedList<T>
        {
            private readonly IList<T> _source;
            public CycleList(IList<T> source)
            {
                _source = source;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                while (true)
                {
                    foreach (T t in _source)
                    {
                        yield return t;
                    }
                }
            }
            public override int Count => int.MaxValue;
            public override T this[int index]
            {
                get
                {
                    return _source[index % _source.Count];
                }
            }
        }
        private class CycleEnumerable<T> : LockedList<T>
        {
            private readonly IEnumerable<T> _source;
            public CycleEnumerable(IEnumerable<T> source)
            {
                _source = source.CacheCount();
            }
            public override IEnumerator<T> GetEnumerator()
            {
                while (true)
                {
                    foreach (T t in _source)
                    {
                        yield return t;
                    }
                }
            }
            public override int Count => int.MaxValue;
            public override T this[int index]
            {
                get
                {
                    return _source.ElementAt(index % _source.Count());
                }
            }
        }
        /// <overloads>Gets an enumerable, repeating another enumerable indefinitely.</overloads>
        /// <summary>
        /// Gets a new <see cref="IList{T}"/>, that is <paramref name="this"/> repeated indefinitely.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to repeat.</param>
        /// <returns>An <see cref="IList{T}"/> that contains <paramref name="this"/>'s elements repeated indefinitely.</returns>
        public static IList<T> Cycle<T>(this IList<T> @this)
        {
            return new CycleList<T>(@this);
        }
        /// <summary>
        /// Gets a new <see cref="IEnumerable{T}"/>, that is <paramref name="this"/> repeated indefinitely.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to repeat.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains <paramref name="this"/>'s elements repeated indefinitely.</returns>
        /// <remarks>The underlying class of the return value implements a read-only <see cref="IList{T}"/> interface. This is to accelerate certain LINQ operations, and should not be accessed by the user.</remarks>
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> @this)
        {
            return new CycleEnumerable<T>(@this);
        }
    }
}

using System;
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
                    int c = 0;
                    foreach (var t in _source)
                    {
                        if (c == index)
                            return t;
                        c++;
                    }
                    return _source.ElementAt(index % c);
                }
            }
        }
        private class RepeatList<T> : LockedList<T>
        {
            private readonly IList<T> _source;
            private readonly int _count;
            public RepeatList(IList<T> source, int count)
            {
                _source = source;
                _count = count;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return range.Range(_count).SelectMany(i => _source).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _count * _source.Count;
                }
            }
            public override T this[int index]
            {
                get
                {
                    if (index >= Count)
                        throw new IndexOutOfRangeException();
                    return _source[index % _source.Count];
                }
            }
            public override bool Contains(T item)
            {
                return _source.Contains(item);
            }
            public override int IndexOf(T item)
            {
                return _source.IndexOf(item);
            }
        }
        private class RepeatEnumerable<T> : LockedList<T>
        {
            private readonly IEnumerable<T> _source;
            private readonly int _count;
            public RepeatEnumerable(IEnumerable<T> source, int count)
            {
                _source = source;
                _count = count;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return range.Range(_count).SelectMany(_ => _source).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _count * _source.Count();
                }
            }
            public override T this[int index]
            {
                get
                {
                    int c = 0;
                    foreach (var t in _source)
                    {
                        if (c == index)
                            return t;
                        c++;
                    }
                    if (index >= c*_count)
                        throw new IndexOutOfRangeException();
                    return _source.ElementAt(index % c);
                }
            }
        }
        /// <overloads>Gets an enumerable, repeating another enumerable.</overloads>
        /// <summary>
        /// Gets a new <see cref="IList{T}"/>, that is <paramref name="this"/> repeated.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to repeat.</param>
        /// <param name="amount">How many times to repeat enumeration, or <see langword="null"/> for infinite repetition.</param>
        /// <returns>An <see cref="IList{T}"/> that contains <paramref name="this"/>'s elements repeated.</returns>
        public static IList<T> Cycle<T>(this IList<T> @this, int? amount = null)
        {
            if (amount.HasValue)
                return new RepeatList<T>(@this,amount.Value);
            return new CycleList<T>(@this);
        }
        /// <summary>
        /// Gets a new <see cref="IEnumerable{T}"/>, that is <paramref name="this"/> repeated.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to repeat.</param>
        /// <param name="amount">How many times to repeat enumeration, or <see langword="null"/> for infinite repetition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains <paramref name="this"/>'s elements repeated.</returns>
        /// <remarks>The underlying class of the return value implements a read-only <see cref="IList{T}"/> interface. This is to accelerate certain LINQ operations, and should not be accessed by the user.</remarks>
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> @this, int? amount = null)
        {
            if (amount.HasValue)
                return new RepeatEnumerable<T>(@this, amount.Value);
            return new CycleEnumerable<T>(@this);
        }
    }
}

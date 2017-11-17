using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class concat
    {
        private class ConcatEnumerable<T> : LockedCollection<T>
        {
            private readonly IEnumerable<IEnumerable<T>> _source;
            public ConcatEnumerable(IEnumerable<IEnumerable<T>> source)
            {
                _source = source;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return _source.SelectMany(v => v).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _source.Sum(a => a.Count());
                }
            }
        }
        private class ConcatList<T> : LockedList<T>
        {
            private readonly IList<IEnumerable<T>> _source;
            public ConcatList(IList<IEnumerable<T>> source)
            {
                _source = source;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return _source.SelectMany(v => v).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _source.Sum(a => a.RecommendCount() ?? a.Count());
                }
            }
            public override T this[int index]
            {
                get
                {
                    foreach (var l in _source)
                    {
                        var c = l.Count();
                        if (index < c)
                        {
                            var li = l.AsList();
                            return li != null ? li[index] : l.ElementAt(index);
                        }
                        index -= c;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }
        private class ConcatListList<T> : IList<T>
        {
            private readonly IList<IList<T>> _source;
            public ConcatListList(IList<IList<T>> source)
            {
                _source = source;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return Enumerable.SelectMany(_source, v => v).GetEnumerator();
            }
            public void Add(T item)
            {
                if (_source.Last().IsReadOnly)
                    _source.Add(new List<T>(1));
                _source.Last().Add(item);
            }
            public void Clear()
            {
                foreach (var l in _source)
                {
                    l.Clear();
                }
                _source.Clear();
            }
            public bool Contains(T item)
            {
                return _source.Any(x => x.Contains(item));
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var l in _source)
                {
                    l.CopyTo(array, arrayIndex);
                    arrayIndex += l.Count;
                }
            }
            public bool Remove(T item)
            {
                return _source.FirstOrDefault(x => x.Remove(item), null) != null;
            }
            public int Count
            {
                get
                {
                    return _source.Sum(a => a.Count);
                }
            }
            public bool IsReadOnly => false;
            private Tuple<IList<T>, int> ind(int index)
            {
                foreach (var l in _source)
                {
                    var c = l.Count;
                    if (index < c)
                        return Tuple.Create(l, index);
                    index -= c;
                }
                throw new IndexOutOfRangeException();
            }
            public int IndexOf(T item)
            {
                int offset = 0;
                foreach (var l in _source)
                {
                    var i = l.IndexOf(item);
                    if (i != -1)
                        return i + offset;
                    offset += l.Count;
                }
                return -1;
            }
            public void Insert(int index, T item)
            {
                var ind = this.ind(index);
                ind.Item1.Insert(ind.Item2, item);
            }
            public void RemoveAt(int index)
            {
                var ind = this.ind(index);
                ind.Item1.RemoveAt(ind.Item2);
            }
            public T this[int index]
            {
                get
                {
                    var i = ind(index);
                    return i.Item1[i.Item2];
                }
                set
                {
                    var i = ind(index);
                    i.Item1[i.Item2] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private class ConcatListListSameCount<T> : LockedList<T>
        {
            private readonly IList<IList<T>> _source;
            public ConcatListListSameCount(IList<IList<T>> source)
            {
                _source = source;
            }
            private int smallCount => _source[0].Count;
            public override IEnumerator<T> GetEnumerator()
            {
                return Enumerable.SelectMany(_source, v => v).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return smallCount * _source.Count;
                }
            }
            public override T this[int index]
            {
                get
                {
                    return _source[index / smallCount][index % smallCount];
                }
            }
        }
        /// <overloads>Concatenates multiple enumerables.</overloads>
        /// <summary>
        /// Concatenates an <see cref="IList{T}"/> of <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="a">An <see cref="IList{T}"/> of <see cref="IEnumerable{T}"/>s to concatenate.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>, with all the elements in the elements of <paramref name="a"/>, concatenated.</returns>
        /// <remarks>The underlying type of the return value is <see cref="IList{T}"/>. However, many of the <see cref="IList{T}"/> operations are not constant time (and are only at worst better then LINQ implementation) and as such, should not be called explicitly by the user. Use <see cref="Enumerable.ElementAt{TSource}"/> and <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> for list operations.</remarks>
        public static IEnumerable<T> Concat<T>(this IList<IEnumerable<T>> a)
        {
            a.ThrowIfNull(nameof(a));
            return new ConcatList<T>(a);
        }
        /// <summary>
        /// Concatenates an <see cref="IList{T}"/> of <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="a">An <see cref="IList{T}"/> of <see cref="IList{T}"/>s to concatenate.</param>
        /// <param name="sameCount">Whether to optimize operations by assuming all sublists have he same lengths. a <see langword="null"/> value means that equal-length values should be checked for.</param>
        /// <returns>An <see cref="IList{T}"/>, with all the elements in the elements of <paramref name="a"/>, concatenated.</returns>
        /// <remarks>In cases where you are certain the lists will either have different or same length, set the <paramref name="sameCount"/> accordingly.</remarks>
        public static IList<T> Concat<T>(this IList<IList<T>> a, bool? sameCount = null)
        {
            a.ThrowIfNull(nameof(a));
            if (!a.Any())
                return new List<T>(0);
            if (sameCount == null)
            {
                var tal = a.Tally().TallyAggregateSelect((x, b) => b == -1 ? x.Count : (x.Count == b ? b : null), (int?)-1, x => x.HasValue, x=> x == null)
                    .TallyAny(x => x == null, true).Do();
                sameCount = tal.Item1;
                if (tal.Item2)
                    throw new ArgumentNullException(nameof(a)+" contains null elements.");
                
            }
            if (sameCount.Value)
                return new ConcatListListSameCount<T>(a);
            return new ConcatListList<T>(a);
        }
        /// <summary>
        /// Concatenates two <see cref="IList{T}"/>s.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The first <see cref="IList{T}"/> to concatenate.</param>
        /// <param name="other">The second <see cref="IList{T}"/> to concatenate.</param>
        /// <returns>A concatenated list of <paramref name="this"/> and <paramref name="other"/></returns>
        /// <remarks>This simply wraps <see cref="Concat{T}(IList{IList{T}}, bool?)"/>.</remarks>
        public static IList<T> Concat<T>(this IList<T> @this, IList<T> other)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            return new ConcatListList<T>(new[] { @this, other });
        }
        /// <summary>
        /// Concatenates an <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="a">An <see cref="IList{T}"/> of <see cref="IEnumerable{T}"/>s to concatenate.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>, with all the elements in the elements of <paramref name="a"/>, concatenated.</returns>
        /// <remarks>The underlying type of the return value is <see cref="ICollection{T}"/>. However, many of the <see cref="ICollection{T}"/> operations are not constant time (and are only at worst better then LINQ implementation) and as such, should not be called explicitly by the user. Use <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> for list operations.</remarks>
        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> a)
        {
            a.ThrowIfNull(nameof(a));
            return new ConcatEnumerable<T>(a);
        }
    }
}

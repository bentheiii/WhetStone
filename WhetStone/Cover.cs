using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class cover
    {
        /// <overloads>Gets an enumerable that has some members replaced by members of another enumerable</overloads>
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> that has members starting at <paramref name="start"/> replaced with <paramref name="cover"/>'s members.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to cover.</param>
        /// <param name="cover">The <see cref="IEnumerable{T}"/> to cover with.</param>
        /// <param name="start">The start of the covering. Inclusive.</param>
        /// <returns><paramref name="this"/> overlayed with <paramref name="cover"/> starting at index <paramref name="start"/>.</returns>
        /// <remarks>The returned enumerable is as long as <paramref name="this"/>, regardless of <paramref name="cover"/>'s length.</remarks>
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, IEnumerable<T> cover, int start = 0)
        {
            @this.ThrowIfNull(nameof(@this));
            cover.ThrowIfNull(nameof(cover));
            if (start < 0)
            {
                cover = cover.Skip(-start);
                start = 0;
            }
            using (var tor = @this.GetEnumerator())
            {
                foreach (var _ in range.Range(start))
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return tor.Current;
                }
                foreach (var c in cover)
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return c;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> that has members at <paramref name="coverindices"/> replaced with <paramref name="cover"/>'s members.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to cover.</param>
        /// <param name="cover">The <see cref="IEnumerable{T}"/> to cover with.</param>
        /// <param name="coverindices">The indices that will be covered.</param>
        /// <returns><paramref name="this"/> overlayed with <paramref name="cover"/> at indices <paramref name="coverindices"/>.</returns>
        /// <remarks>
        /// <para>The returned enumerable is as long as <paramref name="this"/>, regardless of <paramref name="cover"/>'s length.</para>
        /// <para>If <paramref name="coverindices"/> is longer than <paramref name="cover"/>, <paramref name="cover"/> will cycle over itself to provide adequate overlay.</para>
        /// <para>If <paramref name="coverindices"/> is shorter than <paramref name="cover"/>, the remaining elements of <paramref name="cover"/> will be ignored. The cover is only as long as <paramref name="coverindices"/>.</para>
        /// </remarks>
        /// <exception cref="ArgumentException">If <paramref name="cover"/> is empty.</exception>
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, IEnumerable<T> cover, IEnumerable<int> coverindices)
        {
            @this.ThrowIfNull(nameof(@this));
            cover.ThrowIfNull(nameof(cover));
            coverindices.ThrowIfNull(nameof(coverindices));
            using (var ctor = cover.GetEnumerator())
            {
                using (var itor = coverindices.GetEnumerator())
                {
                    int? nextCoverInd = null;
                    T nextCover = default(T);
                    if (!ctor.MoveNext())
                    {
                        if (itor.MoveNext())
                            throw new ArgumentException("empty cover");
                    }
                    else
                    {
                        itor.MoveNext();
                        nextCoverInd = itor.Current;
                        nextCover = ctor.Current;
                    }
                    foreach (var tuple in @this.CountBind())
                    {
                        if (nextCoverInd.HasValue && nextCoverInd.Value == tuple.Item2)
                        {
                            yield return nextCover;
                            if (!itor.MoveNext())
                            {
                                nextCoverInd = null;
                            }
                            else
                            {
                                if (!ctor.MoveNext())
                                {
                                    ctor.Reset();
                                    ctor.MoveNext();
                                }
                                nextCoverInd = itor.Current;
                                nextCover = ctor.Current;
                            }
                        }
                        else
                        {
                            yield return tuple.Item1;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> that has its first members replaced with <paramref name="cover"/>'s members.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to cover.</param>
        /// <param name="cover">The <see cref="IEnumerable{T}"/> to cover with.</param>
        /// <returns><paramref name="this"/> overlayed with <paramref name="cover"/> at index 0.</returns>
        /// <remarks>
        /// The returned enumerable is as long as <paramref name="this"/>, regardless of <paramref name="cover"/>'s length.
        /// </remarks>
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, params T[] cover)
        {
            @this.ThrowIfNull(nameof(@this));
            cover.ThrowIfNull(nameof(cover));
            return Cover(@this, cover, 0);
        }
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> that has member at <paramref name="start"/> replaced with <paramref name="cover"/> members.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to cover.</param>
        /// <param name="cover">The <see cref="IEnumerable{T}"/> to cover with.</param>
        /// <param name="start">The index of the cover.</param>
        /// <returns><paramref name="this"/> overlayed with <paramref name="cover"/> at index <paramref name="start"/>.</returns>
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, T cover, int start = 0)
        {
            @this.ThrowIfNull(nameof(@this));
            if (start < 0)
                return @this;
            return @this.Cover(cover.Enumerate(),start);
        }
        private class CoverList<T> : IList<T>
        {
            private readonly IList<T> _source;
            private readonly IList<T> _cover;
            private int _start;
            public CoverList(IList<T> source, IList<T> cover, int start)
            {
                _source = source;
                _cover = cover;
                _start = start;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _source.AsEnumerable().Cover(_cover,_start).GetEnumerator();
            }
            public void Add(T item)
            {
                _source.Add(item);
                if (covered(this.Count))
                    _cover[this.Count - _start] = item;
            }
            public void Clear()
            {
                _cover.Clear();
                _source.Clear();
                _start = 0;
            }
            private bool covered(int index)
            {
                return index >= _start && index - _start < _cover.Count;
            }
            public bool Contains(T item)
            {
                return IndexOf(item) >= 0;
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var t in this)
                {
                    array[arrayIndex++] = t;
                }
            }
            public bool Remove(T item)
            {
                var i = IndexOf(item);
                if (i == -1)
                    return false;
                RemoveAt(i);
                return true;
            }
            public int Count => _source.Count;
            public bool IsReadOnly => _source.IsReadOnly && _cover.IsReadOnly;
            public int IndexOf(T item)
            {
                var ic = _cover.IndexOf(item);
                if (ic >= 0 && ic + _start < _source.Count)
                    return ic + _start;
                var si = _source.IndexOf(item);
                if (si >= 0 && !covered(si))
                    return si;
                return -1;
            }
            public void Insert(int index, T item)
            {
                if (covered(index))
                {
                    _cover.Insert(index-_start,item);
                }
                else if (index < _start)
                    _start++;
                _source.Insert(index, item);
            }
            public void RemoveAt(int index)
            {
                if (covered(index))
                {
                    _source.RemoveAt(index);
                    _cover.RemoveAt(index - _start);
                }
                else
                {
                    _source.RemoveAt(index);
                    if (index < _start)
                        _start--;
                }
            }
            public T this[int index]
            {
                get
                {
                    if (index >= _start && index - _start < _cover.Count)
                        return _cover[index - _start];
                    return _source[index];
                }
                set
                {
                    if (index >= _start && index - _start < _cover.Count)
                        _cover[index - _start] = value;
                    _source[index] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private class CoverListIndices<T> : IList<T>
        {
            private readonly IList<T> _source;
            private readonly IList<T> _cover;
            private readonly IList<int> _coverindices;
            public CoverListIndices(IList<T> source, IList<T> cover, IList<int> coverindices)
            {
                _source = source;
                _cover = cover.Count >= coverindices.Count ? cover : cover.Cycle();
                _coverindices = coverindices;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _source.AsEnumerable().Cover(_cover, _coverindices).GetEnumerator();
            }
            public void Add(T item)
            {
                _source.Add(item);
                var ind = _coverindices.IndexOf(Count);
                if (ind >= 0)
                {
                    _cover[ind] = item;
                }
            }
            public void Clear()
            {
                _source.Clear();
                _cover.Clear();
                _coverindices.Clear();
            }
            public bool Contains(T item)
            {
                return IndexOf(item) >= 0;
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var t in this)
                {
                    array[arrayIndex++] = t;
                }
            }
            public bool Remove(T item)
            {
                var i = IndexOf(item);
                if (i == -1)
                    return false;
                RemoveAt(i);
                return true;
            }
            public int Count => _source.Count;
            public bool IsReadOnly => _source.IsReadOnly && _cover.IsReadOnly && _coverindices.IsReadOnly;
            public int IndexOf(T item)
            {
                var ci = _cover.IndexOf(item);
                if (ci >= 0 && _coverindices[ci] < _source.Count)
                    return _coverindices[ci];
                var si = _source.IndexOf(item);
                if (si >= 0 && _coverindices.IndexOf(si) == -1)
                    return si;
                return -1;
            }
            public void Insert(int index, T item)
            {
                var minindex = _coverindices.BinarySearch(a => a >= index,binarySearch.BooleanBinSearchStyle.GetFirstTrue);
                if (minindex > -1)
                    foreach (var i in _coverindices.Indices().Slice(minindex))
                    {
                        _coverindices[i]++;
                    }
                _source.Insert(index, item);
            }
            public void RemoveAt(int index)
            {
                _source.RemoveAt(index);
                var i = _coverindices.IndexOf(index);
                if (i >= 0)
                {
                    _cover.RemoveAt(i);
                    _coverindices.RemoveAt(i);
                }
                var minindex = _coverindices.BinarySearch(a => a >= index, binarySearch.BooleanBinSearchStyle.GetFirstTrue);
                if (minindex > -1)
                    foreach (var c in _coverindices.Indices().Slice(minindex))
                    {
                        _coverindices[c]--;
                    }
            }
            public T this[int index]
            {
                get
                {
                    var i = _coverindices.IndexOf(index);
                    if (i >= 0)
                        return _cover[i];
                    return _source[index];
                }
                set
                {
                    var i = _coverindices.IndexOf(index);
                    if (i >= 0)
                        _cover[i] = value;
                    _source[index] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> that has members at <paramref name="coverindices"/> replaced with <paramref name="cover"/>'s members.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to cover.</param>
        /// <param name="cover">The <see cref="IList{T}"/> to cover with.</param>
        /// <param name="coverindices">The indices that will be covered.</param>
        /// <returns><paramref name="this"/> overlayed with <paramref name="cover"/> at indices <paramref name="coverindices"/>.</returns>
        /// <remarks>
        /// <para>The returned enumerable is as long as <paramref name="this"/>, regardless of <paramref name="cover"/>'s length.</para>
        /// <para>If <paramref name="coverindices"/> is longer than <paramref name="cover"/>, <paramref name="cover"/> will cycle over itself to provide adequate overlay.</para>
        /// <para>If <paramref name="coverindices"/> is shorter than <paramref name="cover"/>, the remaining elements of <paramref name="cover"/> will be ignored. The cover is only as long as <paramref name="coverindices"/>.</para>
        /// <para>The returned <see cref="IList{T}"/> is mutability passing (assuming all <paramref name="this"/>, <paramref name="cover"/>, <paramref name="coverindices"/> are mutable), however, its mutating methods are undefined and untested, and should be considered experimental.</para>
        /// </remarks>
        /// <exception cref="ArgumentException">If <paramref name="cover"/> is empty.</exception>
        public static IList<T> Cover<T>(this IList<T> @this, IList<T> cover, IList<int> coverindices)
        {
            @this.ThrowIfNull(nameof(@this));
            cover.ThrowIfNull(nameof(cover));
            coverindices.ThrowIfNull(nameof(coverindices));
            if (!cover.Any() && coverindices.Any())
                throw new ArgumentException("Empty cover.");
            return new CoverListIndices<T>(@this, cover, coverindices);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> that has members starting at <paramref name="start"/> replaced with <paramref name="cover"/>'s members.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to cover.</param>
        /// <param name="cover">The <see cref="IList{T}"/> to cover with.</param>
        /// <param name="start">The start of the covering. Inclusive.</param>
        /// <returns><paramref name="this"/> overlayed with <paramref name="cover"/> starting at index <paramref name="start"/>.</returns>
        /// <remarks>
        /// <para>The returned enumerable is as long as <paramref name="this"/>, regardless of <paramref name="cover"/>'s length.</para>
        /// <para>The returned <see cref="IList{T}"/> is mutability passing (assuming both <paramref name="this"/>, <paramref name="cover"/> are mutable), however, its mutating methods are undefined and untested, and should be considered experimental.</para>
        /// </remarks>
        public static IList<T> Cover<T>(this IList<T> @this, IList<T> cover, int start = 0)
        {
            @this.ThrowIfNull(nameof(@this));
            cover.ThrowIfNull(nameof(cover));
            if (start < 0)
            {
                cover = cover.Skip(-start);
                start = 0;
            }
            return new CoverList<T>(@this,cover,start);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> that has its first members replaced with <paramref name="cover"/>'s members.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to cover.</param>
        /// <param name="cover">The <see cref="IList{T}"/> to cover with.</param>
        /// <returns><paramref name="this"/> overlayed with <paramref name="cover"/> at index 0.</returns>
        /// <remarks>
        /// <para>The returned enumerable is as long as <paramref name="this"/>, regardless of <paramref name="cover"/>'s length.</para>
        /// <para>The returned <see cref="IList{T}"/> is mutability passing (assuming both <paramref name="this"/>, <paramref name="cover"/> are mutable), however, its mutating methods are undefined and untested, and should be considered experimental.</para>
        /// </remarks>
        public static IList<T> Cover<T>(this IList<T> @this, params T[] cover)
        {
            @this.ThrowIfNull(nameof(@this));
            cover.ThrowIfNull(nameof(cover));
            return @this.Cover(cover.AsList());
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> that has member at <paramref name="start"/> replaced with <paramref name="cover"/> members.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to cover.</param>
        /// <param name="cover">The <see cref="IList{T}"/> to cover with.</param>
        /// <param name="start">The index of the cover.</param>
        /// <returns><paramref name="this"/> overlayed with <paramref name="cover"/> at index <paramref name="start"/>.</returns>
        /// /// <remarks>
        /// <para>The returned <see cref="IList{T}"/> is mutability passing (assuming both <paramref name="this"/>, <paramref name="cover"/> are mutable), however, its mutating methods are undefined and untested, and should be considered experimental.</para>
        /// </remarks>
        public static IList<T> Cover<T>(this IList<T> @this, T cover, int start)
        {
            @this.ThrowIfNull(nameof(@this));
            if (start < 0)
                return @this;
            return @this.Cover(cover.Enumerate(),start);
        }
    }
}

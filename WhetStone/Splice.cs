using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class splice
    {
        /// <overloads>Insert an enumerable into another.</overloads>
        /// <summary>
        /// Create an <see cref="IEnumerable{T}"/> with an element inserted into it.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to insert to.</param>
        /// <param name="slice">The element to insert.</param>
        /// <param name="spliceStart">The index of the inserted element.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/>, like <paramref name="this"/> but with <paramref name="slice"/> inserted at index <paramref name="spliceStart"/>.</returns>
        public static IEnumerable<T> Splice<T>(this IEnumerable<T> @this, T slice, int spliceStart)
        {
            return @this.Splice(slice.Enumerate(), spliceStart);
        }
        /// <summary>
        /// Create an <see cref="IEnumerable{T}"/> with another <see cref="IEnumerable{T}"/> inserted into it.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to insert to.</param>
        /// <param name="slice">The <see cref="IEnumerable{T}"/> to insert.</param>
        /// <param name="spliceStart">The first index of the inserted <see cref="IEnumerable{T}"/>.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/>, like <paramref name="this"/> but with <paramref name="slice"/> inserted at index <paramref name="spliceStart"/>.</returns>
        public static IEnumerable<T> Splice<T>(this IEnumerable<T> @this, IEnumerable<T> slice, int spliceStart)
        {
            using (var tor = @this.GetEnumerator())
            {
                foreach (int i in range.Range(spliceStart))
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return tor.Current;
                }
                foreach (var s in slice)
                {
                    yield return s;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        private class SpliceList<T> : IList<T>
        {
            private readonly IList<T> _source;
            private readonly IList<T> _slice;
            private int _spliceStart;
            public SpliceList(IList<T> source, IList<T> slice, int spliceStart)
            {
                _source = source;
                _slice = slice;
                _spliceStart = spliceStart;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _source.AsEnumerable().Splice(_slice, _spliceStart).GetEnumerator();
            }
            private bool isSpliced(int index)
            {
                return index >= _spliceStart && index < _slice.Count + _spliceStart;
            }
            public void Add(T item)
            {
                if (isSpliced(Count))
                    _slice.Add(item);
                _source.Add(item);
            }
            public void Clear()
            {
                _slice.Clear();
                _source.Clear();
            }
            public bool Contains(T item)
            {
                return _source.Contains(item) || _slice.Contains(item);
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var v in this)
                {
                    array[arrayIndex++] = v;
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
            public int Count => _source.Count + _slice.Count;
            public bool IsReadOnly => _source.IsReadOnly && _slice.IsReadOnly;
            public int IndexOf(T item)
            {
                var io = _source.IndexOf(item);
                if (io < _spliceStart)
                    return io;
                var si = _slice.IndexOf(item);
                return si != -1 ? si : io;
            }
            public void Insert(int index, T item)
            {
                if (index < _spliceStart)
                {
                    _source.Insert(index,item);
                    _spliceStart++;
                }
                else if (index < _slice.Count + _spliceStart)
                {
                    _slice.Insert(index-_spliceStart,item);
                }
                else
                {
                    _source.Insert(index - _slice.Count,item);
                }
            }
            public void RemoveAt(int index)
            {
                if (index < _spliceStart)
                {
                    _source.RemoveAt(index);
                    _spliceStart--;
                }
                else if (index < _slice.Count + _spliceStart)
                {
                    _slice.RemoveAt(index - _spliceStart);
                }
                else
                {
                    _source.RemoveAt(index - _slice.Count);
                }
            }
            public T this[int index]
            {
                get
                {
                    if (index < _spliceStart)
                        return _source[index];
                    if (index < _slice.Count + _spliceStart)
                        return _slice[index - _spliceStart];
                    return _source[index - _slice.Count];
                }
                set
                {
                    if (index < _spliceStart)
                        _source[index] = value;
                    if (index < _slice.Count + _spliceStart)
                        _slice[index - _spliceStart] = value;
                    _source[index - _slice.Count] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        /// <summary>
        /// Create an <see cref="IList{T}"/> with another <see cref="IList{T}"/> inserted into it.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to insert to.</param>
        /// <param name="slice">The <see cref="IList{T}"/> to insert.</param>
        /// <param name="spliceStart">The first index of the inserted <see cref="IList{T}"/>.</param>
        /// <returns>A mutability-passing <see cref="IList{T}"/>, like <paramref name="this"/> but with <paramref name="slice"/> inserted at index <paramref name="spliceStart"/>.</returns>
        public static IList<T> Splice<T>(this IList<T> @this, IList<T> slice, int spliceStart)
        {
            return new SpliceList<T>(@this,slice,spliceStart);
        }
        /// <summary>
        /// Create an <see cref="IList{T}"/> with an element inserted into it.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to insert to.</param>
        /// <param name="slice">The element to insert.</param>
        /// <param name="spliceStart">The index of the inserted element.</param>
        /// <returns>A mutability-passing <see cref="IList{T}"/>, like <paramref name="this"/> but with <paramref name="slice"/> inserted at index <paramref name="spliceStart"/>.</returns>
        public static IList<T> Splice<T>(this IList<T> @this, T slice, int spliceStart)
        {
            return @this.Splice(slice.Enumerate(),spliceStart);
        }
    }
}

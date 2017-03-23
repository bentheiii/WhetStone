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
    public static class reverse
    {
        /// <summary>
        /// Get a reversed version of an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to reverse.</param>
        /// <returns>A mutability-passing <see cref="IList{T}"/> that includes <paramref name="this"/>'s elements in reverse order.</returns>
        public static IList<T> Reverse<T>(this IList<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new ReverseList<T>(@this);
        }
        private class ReverseList<T> : IList<T>
        {
            private readonly IList<T> _source;
            public ReverseList(IList<T> source)
            {
                _source = source;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return Enumerable.Select(_source.Indices(), i => _source[_source.Count - i - 1]).GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            public void Add(T item)
            {
                _source.Insert(0,item);
            }
            public void Clear()
            {
                _source.Clear();
            }
            public bool Contains(T item)
            {
                return _source.Contains(item);
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach ((var t, int i) in this.CountBind(arrayIndex))
                {
                    array[i] = t;
                }
            }
            public bool Remove(T item)
            {
                var ind = IndexOf(item);
                if (ind == -1)
                    return false;
                RemoveAt(ind);
                return true;
            }
            public int Count => _source.Count;
            public bool IsReadOnly => _source.IsReadOnly;
            public int IndexOf(T item)
            {
                return this.CountBind().FirstOrDefault(a => a.Equals(item), Tuple.Create(default(T), -1)).Item2;
            }
            public void Insert(int index, T item)
            {
                if (index == 0)
                    _source.Add(item);
                _source.Insert(_source.Count - index,item);
            }
            public void RemoveAt(int index)
            {
                _source.RemoveAt(_source.Count - index - 1);
            }
            public T this[int index]
            {
                get
                {
                    return _source[_source.Count - 1 - index];
                }
                set
                {
                    _source[_source.Count - 1 - index] = value;
                }
            }
        }
    }
}

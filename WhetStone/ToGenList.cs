using System;
using System.Collections;
using System.Collections.Generic;

namespace WhetStone.SystemExtensions
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class toGenList
    {
        /// <summary>
        /// Wraps an <see cref="IList{T}"/> in a non-generic <see cref="IList"/> wrapper.
        /// </summary>
        /// <typeparam name="T">The type of the original <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">the <see cref="IList{T}"/> to wrap.</param>
        /// <returns><paramref name="this"/> wrapped in a non-generic <see cref="IList"/> wrapper.</returns>
        /// <remarks>The wrapper still wraps a generic <see cref="IList{T}"/>. Use with caution.</remarks>
        public static IList ToGeneral<T>(this IList<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new GenListWrapper<T>(@this);
        }
        private class GenListWrapper<T> : IList
        {
            private readonly IList<T> _inner;
            public GenListWrapper(IList<T> inner)
            {
                _inner = inner;
            }
            public IEnumerator GetEnumerator()
            {
                return ((IEnumerable)_inner).GetEnumerator();
            }
            public void CopyTo(Array array, int index)
            {
                foreach (var t in _inner)
                {
                    array.SetValue(t,index++);
                }
            }
            public int Count => _inner.Count;
            public object SyncRoot => this;
            public bool IsSynchronized => false;
            public int Add(object value)
            {
                _inner.Add((T)value);
                return _inner.Count - 1;
            }
            public bool Contains(object value)
            {
                return value is T t && _inner.Contains(t);
            }
            public void Clear()
            {
                _inner.Clear();
            }
            public int IndexOf(object value)
            {
                if (!(value is T))
                    return -1;
                return _inner.IndexOf((T)value);
            }
            public void Insert(int index, object value)
            {
                _inner.Insert(index,(T)value);
            }
            public void Remove(object value)
            {
                _inner.Remove((T)value);
            }
            public void RemoveAt(int index)
            {
                _inner.RemoveAt(index);
            }
            public object this[int index]
            {
                get
                {
                    return _inner[index];
                }
                set
                {
                    _inner[index] = (T)value;
                }
            }
            public bool IsReadOnly => _inner.IsReadOnly;
            public bool IsFixedSize => false;
        }
    }
}

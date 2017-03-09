using System;
using System.Collections;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// An infinite <see cref="IList{T}"/>, with a default value occupying all non-assigned cells
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <remarks>The list is implemented by a <see cref="List{T}"/> wrapped representing the first-most elements of the list. The class's memory usage will as large as the index of the last non-default element.</remarks>
    public class InfiniteList<T> : IList<T>
    {
        private readonly List<T> _data;
        /// <summary>
        /// The default value that populates the list's horizon.
        /// </summary>
        public T defaultValue { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultValue">The default value of the List's members.</param>
        /// <param name="capacity">The initial capacity for non-default members.</param>
        public InfiniteList(T defaultValue = default(T), int capacity = 4)
        {
            capacity.ThrowIfAbsurd(nameof(capacity));
            this.defaultValue = defaultValue;
            _data = new List<T>(capacity);
        }
        private void ExpandTo(int newsize)
        {
            if (_data.Count < newsize)
            {
                _data.Capacity = newsize;
                _data.AddRange(defaultValue.Enumerate(newsize - _data.Count));
            }
        }
        /// <inheritdoc />
        public int IndexOf(T item)
        {
            var ret = _data.IndexOf(item);
            if (ret == -1 && item.Equals(defaultValue))
                ret = this.Count;
            return ret;
        }
        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            index.ThrowIfAbsurd(nameof(index));
            if (index >= _data.Count)
                this[index] = item;
            else
                _data.Insert(index, item);
        }
        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            index.ThrowIfAbsurd(nameof(index));
            if (index >= _data.Count)
                return;
            _data.RemoveAt(index);
        }
        /// <inheritdoc />
        public T this[int ind]
        {
            get
            {
                ind.ThrowIfAbsurd(nameof(ind));
                return _data.Count <= ind ? defaultValue : _data[ind];
            }
            set
            {
                ind.ThrowIfAbsurd(nameof(ind));
                ExpandTo(ind + 1);
                _data[ind] = value;
            }
        }
        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _data.Concat(defaultValue.Enumerate().Cycle()).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_data).GetEnumerator();
        }
        /// <inheritdoc />
        public void Add(T item)
        {
            throw new NotSupportedException();
        }
        /// <inheritdoc />
        public void Clear()
        {
            throw new NotSupportedException();
        }
        /// <inheritdoc />
        public bool Contains(T item)
        {
            return item.Equals(defaultValue) || _data.Contains(item);
        }
        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }
        /// <inheritdoc />
        public bool Remove(T item)
        {
            return _data.Remove(item) || item.Equals(defaultValue);
        }
        /// <inheritdoc />
        public int Count => int.MaxValue;
        /// <inheritdoc />
        public bool IsReadOnly => false;
    }
}

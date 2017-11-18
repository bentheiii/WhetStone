using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// Represents a list array whose internal <see cref="T:System.Array" /> can be taken by reference, without need to copy it to a new <see cref="T:System.Array" />.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="T:System.Collections.Generic.IList`1" />.</typeparam>
    public class ResizingArray<T> : IList<T>, IReadOnlyList<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity">The initial capacity of the internal list array.</param>
        public ResizingArray(int capacity = 4)
        {
            capacity.ThrowIfAbsurd(nameof(capacity));
            _arr = new T[capacity];
        }
        /// <summary>
        /// Get the internal array reference.
        /// </summary>
        public T[] arr
        {
            get
            {
                minimize();
                return _arr;
            }
        }
        /// <inheritdoc />
        public bool Remove(T item)
        {
            var ind = IndexOf(item);
            if (ind == -1)
                return false;
            RemoveAt(ind);
            return true;
        }
        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count { get; private set; } = 0;
        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return _arr.IsReadOnly;
            }
        }
        private T[] _arr;
        /// <summary>
        /// Shrink the list array to minimal size (while) preserving all elements.
        /// </summary>
        public void minimize()
        {
            Array.Resize(ref _arr, Count);
        }
        /// <summary>
        /// Resizes the internal list array so that it could include a certain index.
        /// </summary>
        /// <param name="lastindex">The index to make valid.</param>
        /// <remarks>The array might become larger than necessary to accommodate the index.</remarks>
        public void ResizeTo(int lastindex)
        {
            lastindex.ThrowIfAbsurd(nameof(lastindex));
            while (!_arr.IsWithinBounds(lastindex))
                Array.Resize(ref _arr, Math.Max(arr.Length * 2, lastindex + 1));
        }
        /// <inheritdoc />
        public void Add(T x)
        {
            ResizeTo(Count);
            _arr[Count] = x;
            Count++;
        }
        /// <summary>
        /// Adds multiple elements at once.
        /// </summary>
        /// <param name="x">The elements to add.</param>
        public void AddRange(IEnumerable<T> x)
        {
            x.ThrowIfNull(nameof(x));
            int c = x.Count();
            ResizeTo(Count + c);
            foreach (var i in countUp.CountUp(Count).Zip(x))
            {
                _arr[i.Item1] = i.Item2;
            }
            Count += c;
        }
        /// <inheritdoc />
        public void Clear()
        {
            Count = 0;
        }
        /// <inheritdoc />
        public bool Contains(T item)
        {
            return _arr.Contains(item);
        }
        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            array.ThrowIfNull(nameof(array));
            arrayIndex.ThrowIfAbsurd(nameof(arrayIndex));

            _arr.Take(Count).CopyTo(array, arrayIndex);
        }
        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _arr.Take(Count).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return arr.CountBind().FirstOrDefault(a => a.Item1.Equals(item),(default(T),-1)).Item2;
        }
        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            index.ThrowIfAbsurd(nameof(index));

            if (index == Count)
            {
                Add(item);
                return;
            }
            Add(default(T));
            foreach (int i in range.Range(Count - 1, index))
            {
                _arr[i] = _arr[i - 1];
            }
            _arr[index] = item;
        }
        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            index.ThrowIfAbsurd(nameof(index));

            foreach (int i in range.Range(index,Count-1))
            {
                _arr[i] = _arr[i + 1];
            }
            Count--;
            if (_arr.Length > Count*2)
            {
                Array.Resize(ref _arr, _arr.Length/2);
            }
        }
        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get
            {
                index.ThrowIfAbsurd(nameof(index));

                if (index >= Count)
                    throw new ArgumentOutOfRangeException();
                return _arr[index];
            }
            set
            {
                index.ThrowIfAbsurd(nameof(index));

                if (index >= Count)
                    throw new ArgumentOutOfRangeException();
                _arr[index] = value;
            }
        }
    }
}

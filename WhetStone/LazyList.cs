using System;
using System.Collections;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    /// <summary>
    /// An infinite <see cref="IList{T}"/> of lazily computed and cached values.
    /// </summary>
    /// <typeparam name="T">The values of the <see cref="IList{T}"/>.</typeparam>
    public class LazyList<T> : IList<T>
    {
        private readonly Func<int, LazyList<T>, T> _generator;
        private readonly InfiniteList<T> _data;
        private readonly InfiniteList<bool> _initialized;
        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="generator">A function to generate the <see cref="LazyList{T}"/>'s elements by index.</param>
        public LazyList(Func<int, T> generator) : this((i, array) => generator(i)) { }
        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="generator">A function to generate the <see cref="LazyList{T}"/>'s elements by index and the <see cref="LazyList{T}"/> itself.</param>
        public LazyList(Func<int, LazyList<T>, T> generator)
        {
            _generator = generator;
            _data = new InfiniteList<T>();
            _initialized = new InfiniteList<bool>();
        }
        /// <summary>
        /// Get whether the element at an index has been initialized.
        /// </summary>
        /// <param name="index">The index of the lazy element.</param>
        /// <returns>Whether the element at an index has been initialized.</returns>
        public bool Initialized(int index)
        {
            return _initialized[index];
        }
        /// <summary>
        /// Invalidates an element, forcing it to be computed again if evaluated.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        public void Invalidate(int index)
        {
            _initialized[index] = false;
        }
        /// <inheritdoc />
        public int IndexOf(T item)
        {
            foreach (Tuple<T, int> tuple in this.CountBind())
            {
                if (tuple.Item1.Equals(item))
                    return tuple.Item2;
            }
            throw new Exception("impossible to get here, the list is endless");
        }
        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            _initialized.Insert(index,true);
            _data.Insert(index,item);
        }
        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            _initialized.RemoveAt(index);
            _data.RemoveAt(index);
        }
        /// <inheritdoc />
        public T this[int ind]
        {
            get
            {
                if (_initialized[ind])
                    return _data[ind];
                T ret = _data[ind] = _generator(ind, this);
                _initialized[ind] = true;
                return ret;
            }
            set
            {
                _data[ind] = value;
                _initialized[ind] = true;
            }
        }
        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return countUp.CountUp().Select(a => this[a]).GetEnumerator();
        }
        /// <inheritdoc />
        public bool Remove(T item)
        {
            var i = this.IndexOf(item);
            if (i < 0)
                return false;
            this.RemoveAt(i);
            return true;
        }
        /// <inheritdoc />
        public int Count => _data.Count;
        /// <inheritdoc />
        public bool IsReadOnly => false;
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
            return IndexOf(item) >= 0;
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A collection implemented by remembering the multiplicity of each element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiCollection<T> : ICollection<T>
    {
        private readonly IDictionary<T, int> _occurance;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comp">An <see cref="IEqualityComparer{T}"/> to group elements together.</param>
        public MultiCollection(IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            _occurance = new Dictionary<T, int>(comp);
        }
        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _occurance.SelectMany(a => a.Key.Enumerate(a.Value)).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Add an item to the <see cref="ICollection{T}"/>
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="amount">How many of the item to add.</param>
        public void Add(T item, int amount)
        {
            amount.ThrowIfAbsurd(nameof(amount));
            _occurance.EnsureValue(item);
            _occurance[item]+=amount;
            Count += amount;
        }
        /// <inheritdoc />
        public void Add(T item)
        {
            Add(item,1);
        }
        /// <inheritdoc />
        public void Clear()
        {
            _occurance.Clear();
            Count = 0;
        }
        /// <inheritdoc />
        public bool Contains(T item)
        {
            return _occurance.ContainsKey(item);
        }
        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var t in this)
            {
                array[arrayIndex++] = t;
            }
        }
        /// <inheritdoc />
        public bool Remove(T item)
        {
            return Remove(item, 1);
        }
        /// <summary>
        /// Removes a value from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <param name="amount">How many times to remove the item.</param>
        /// <returns>Whether the item existed in the first place.</returns>
        public bool Remove(T item, int amount)
        {
            amount.ThrowIfAbsurd(nameof(amount));
            if (!_occurance.TryGetValue(item, out int oldval))
                return false;
            if (oldval <= amount)
                _occurance.Remove(item);
            else
                _occurance[item]-=amount;
            Count -= amount;
            return true;
        }
        /// <inheritdoc />
        public int Count { get; private set; } = 0;
        /// <inheritdoc />
        public bool IsReadOnly => false;
        /// <summary>
        /// Get an indexed element from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="ind">The index of the element.</param>
        /// <returns>The element at index <paramref name="ind"/>.</returns>
        public T this[int ind]
        {
            get
            {
                foreach (KeyValuePair<T, int> i in _occurance)
                {
                    if (i.Value > ind)
                        return i.Key;
                    ind -= i.Value;
                }
                throw new IndexOutOfRangeException();
            }
        }
    }
}

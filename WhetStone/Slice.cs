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
    public static class slice
    {
        /// <overloads>Get a part of an enumerable.</overloads>
        /// <summary>
        /// Get a part from an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to slice.</param>
        /// <param name="start">The first index of the section to return.</param>
        /// <param name="max">The last index of the section to return. Exclusive. If this is set, <paramref name="length"/> must not be set.</param>
        /// <param name="steps">The step, in indices between the indices of the section.</param>
        /// <param name="length">The number of items in the section. If this is set, <paramref name="max"/> must not be set.</param>
        /// <returns>A mutability-passing slice of <paramref name="this"/>.</returns>
        public static IList<T> Slice<T>(this IList<T> @this, int start, int? max = null, int steps=1, int? length = null)
        {
            if (max.HasValue && length.HasValue)
                throw new ArgumentException("either max or length must be null");
            steps.ThrowIfAbsurd(nameof(steps),false);
            start.ThrowIfAbsurd();
            if (length.HasValue)
                max = length*steps + start;
            if (max == null)
                max = @this.Count;
            var s = @this as ListSlice<T>;
            var ret = s != null ? s.ReSlice(start, max.Value, steps) : new ListSlice<T>(@this, start, max.Value, steps);
            if (ret.Count < 0)
                throw new ArgumentOutOfRangeException();
            return ret;
        }
        private class ListSlice<T> : IList<T>
        {
            private readonly IList<T> _inner;
            private readonly int _start;
            private readonly int _step;
            private readonly IList<int> _indices;
            public ListSlice(IList<T> inner, int start, int max, int step)
            {
                _inner = inner;
                _start = start;
                _step = step;
                _indices = range.Range(start, max, step);
            }
            private IEnumerable<int> IndexRange()
            {
                return _indices;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return IndexRange().Select(i => _inner[i]).GetEnumerator();
            }
            public void Add(T item)
            {
                throw new NotSupportedException();
            }
            public void Clear()
            {
                throw new NotSupportedException();
            }
            public bool Contains(T item)
            {
                return this.Any(a=>a.Equals(item));
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
                throw new NotSupportedException();
            }
            public int Count => _indices.Count;
            public bool IsReadOnly => _inner.IsReadOnly;
            public int IndexOf(T item)
            {
                return this.CountBind().FirstOrDefault(a => a.Equals(item), Tuple.Create(default(T), -1)).Item2;
            }
            public void Insert(int index, T item)
            {
                throw new NotSupportedException();
            }
            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }
            public T this[int index]
            {
                get
                {
                    if (index > Count || index < 0)
                        throw new ArgumentOutOfRangeException();
                    return _inner[_indices[index]];
                }
                set
                {
                    if (index > Count || index < 0)
                        throw new ArgumentOutOfRangeException();
                    _inner[_indices[index]] = value;
                }
            }
            public ListSlice<T> ReSlice(int start, int max, int step)
            {
                return new ListSlice<T>(this._inner, this._start + start, max*step + _start,_step*step);
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        /// <summary>
        /// Get a part from an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to slice.</param>
        /// <param name="start">The first index of the section to return.</param>
        /// <param name="max">The last index of the section to return. Exclusive. If this is set, <paramref name="length"/> must not be set.</param>
        /// <param name="steps">The step, in indices between the indices of the section.</param>
        /// <param name="length">The number of items in the section. If this is set, <paramref name="max"/> must not be set.</param>
        /// <returns>A mutability-passing slice of <paramref name="this"/>.</returns>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> @this, int start = 0, int? max = null, int steps = 1, int? length = null)
        {
            steps.ThrowIfAbsurd(nameof(steps), false);
            start.ThrowIfAbsurd();
            if (max.HasValue && length.HasValue)
                throw new ArgumentException("either max or length must be null");
            if (length.HasValue)
                max = length * steps + start;
            var temp = @this.Skip(start).Step(steps);
            return max != null? temp.Take((max.Value-start)/steps) : temp;
        }
    }
}

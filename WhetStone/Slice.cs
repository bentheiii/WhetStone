﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class slice
    {
        /// <summary>
        /// max is exclusive
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static IList<T> Slice<T>(this IList<T> @this, int start, int max, int steps=1)
        {
            var s = @this as ListSlice<T>;
            return s != null ? s.ReSlice(start, max, steps) : new ListSlice<T>(@this, start, max, steps);
        }
        public static IList<T> Slice<T>(this IList<T> @this, int start)
        {
            return Slice(@this, start, @this.Count - start);
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
                var i = new Guard<int>();
                foreach (var t in this.CountBind(arrayIndex).Detach(i))
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
        
    }
}

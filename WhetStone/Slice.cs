using System;
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
        /// <param name="length"></param>
        /// <returns></returns>
        public static IList<T> Slice<T>(this IList<T> @this, int start, int? max = null, int steps=1, int? length = null)
        {
            if (max.HasValue && length.HasValue)
                throw new ArgumentException("either max or length must be null");
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
        public static IList<T> Skip<T>(this IList<T> @this, int skipCount)
        {
            return @this.Slice(skipCount);
        }
        public static IList<T> Take<T>(this IList<T> @this, int length)
        {
            return @this.Slice(0,length:length);
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

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> @this, int start = 0, int? max = null, int steps = 1, int? length = null)
        {
            var ts = @this.AsList(false);
            if (ts != null)
                return ts.Slice(start,max,steps,length);
            if (max.HasValue && length.HasValue)
                throw new ArgumentException("either max or length must be null");
            if (length.HasValue)
                max = length * steps + start;
            var temp = @this.Skip(start).Step(steps);
            return max != null? temp.Take((max.Value-start)/steps) : temp;
        }
    }
}

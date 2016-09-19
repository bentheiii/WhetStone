using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class reverse
    {
        public static IList<T> Reverse<T>(this IList<T> @this)
        {
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
                return _source.AsEnumerable().Reverse().GetEnumerator();
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
                var i = new Guard<int>();
                foreach (var t in this.CountBind(arrayIndex).Detach(i))
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

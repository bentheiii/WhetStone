using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.SystemExtensions
{
    public static class toGenList
    {
        public static IList ToGeneral<T>(this IList<T> @this)
        {
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
                foreach (var t in _inner.CountBind(index))
                {
                    array.SetValue(t.Item1,t.Item2);
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
                return value is T && _inner.Contains((T)value);
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

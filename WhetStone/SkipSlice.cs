using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class skipSlice
    {
        public static IEnumerable<T> SkipSlice<T>(this IEnumerable<T> @this, int start, int length = 1)
        {
            using (var tor = @this.GetEnumerator())
            {
                foreach (var _ in range.Range(start))
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return tor.Current;
                }
                foreach (var _ in range.Range(length))
                {
                    if (!tor.MoveNext())
                        yield break;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        private class SkipSliceList<T> : IList<T>
        {
            private readonly IList<T> _source;
            private int _start;
            private int _length;
            public SkipSliceList(IList<T> source, int start, int length)
            {
                _source = source;
                _start = start;
                _length = length;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _source.AsEnumerable().SkipSlice(_start, _length).GetEnumerator();
            }
            private bool hidden(int index)
            {
                return index >= _start && index < _start + _length;
            }
            public void Add(T item)
            {
                if (hidden(_source.Count))
                    _length = _source.Count - _start;
                _source.Add(item);
            }
            public void Clear()
            {
                _source.Clear();
                _length = 0;
                _start = 0;
            }
            public bool Contains(T item)
            {
                var i = _source.IndexOf(item);
                return i >= 0 && !hidden(i);
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var t in this)
                {
                    array[arrayIndex++] = t;
                }
            }
            public bool Remove(T item)
            {
                int i = IndexOf(item);
                if (i < 0)
                    return false;
                RemoveAt(i);
                return true;
            }
            public int Count
            {
                get
                {
                    if (_source.Count < _start + _length)
                        return _start;
                    return _source.Count - _length;
                }
            }
            public bool IsReadOnly => _source.IsReadOnly;
            public int IndexOf(T item)
            {
                int i = _source.IndexOf(item);
                if (i < 0 || hidden(i))
                    return -1;
                if (i > _start)
                    return i - _length;
                return i;
            }
            public void Insert(int index, T item)
            {
                if (index < _start)
                {
                    _source.Insert(index,item);
                    _start++;
                }
                else
                {
                    _source.Insert(index+_length,item);
                }
            }
            public void RemoveAt(int index)
            {
                if (index < _start)
                {
                    _source.RemoveAt(index);
                    _start--;
                }
                else
                {
                    _source.RemoveAt(index + _length);
                }
            }
            public T this[int index]
            {
                get
                {
                    if (index < _start)
                        return _source[index];
                    return _source[index + _length];
                }
                set
                {
                    if (index < _start)
                        _source[index] = value;
                    _source[index - _length] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static IList<T> SkipSlice<T>(this IList<T> @this, int start, int length = 1)
        {
            if (start == 0)
                return @this.Skip(length);
            if (length + start >= @this.Count)
                return @this.Take(start);
            return new SkipSliceList<T>(@this,start,length);
        }
    }
}

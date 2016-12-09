using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class splice
    {
        public static IEnumerable<T> Splice<T>(this IEnumerable<T> @this, T slice, int spliceStart)
        {
            return @this.Splice(slice.Enumerate(), spliceStart);
        }
        public static IEnumerable<T> Splice<T>(this IEnumerable<T> @this, IEnumerable<T> slice, int spliceStart)
        {
            using (var tor = @this.GetEnumerator())
            {
                foreach (int i in range.Range(spliceStart))
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return tor.Current;
                }
                foreach (var s in slice)
                {
                    yield return s;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        private class SpliceList<T> : IList<T>
        {
            private readonly IList<T> _source;
            private readonly IList<T> _slice;
            private int _spliceStart;
            public SpliceList(IList<T> source, IList<T> slice, int spliceStart)
            {
                _source = source;
                _slice = slice;
                _spliceStart = spliceStart;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _source.AsEnumerable().Splice(_slice, _spliceStart).GetEnumerator();
            }
            private bool isSpliced(int index)
            {
                return index >= _spliceStart && index < _slice.Count + _spliceStart;
            }
            public void Add(T item)
            {
                if (isSpliced(Count))
                    _slice.Add(item);
                _source.Add(item);
            }
            public void Clear()
            {
                _slice.Clear();
                _source.Clear();
            }
            public bool Contains(T item)
            {
                return _source.Contains(item) || _slice.Contains(item);
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var v in this)
                {
                    array[arrayIndex++] = v;
                }
            }
            public bool Remove(T item)
            {
                var i = IndexOf(item);
                if (i == -1)
                    return false;
                RemoveAt(i);
                return true;
            }
            public int Count => _source.Count + _slice.Count;
            public bool IsReadOnly => _source.IsReadOnly && _slice.IsReadOnly;
            public int IndexOf(T item)
            {
                var io = _source.IndexOf(item);
                if (io < _spliceStart)
                    return io;
                var si = _slice.IndexOf(item);
                return si != -1 ? si : io;
            }
            public void Insert(int index, T item)
            {
                if (index < _spliceStart)
                {
                    _source.Insert(index,item);
                    _spliceStart++;
                }
                else if (index < _slice.Count + _spliceStart)
                {
                    _slice.Insert(index-_spliceStart,item);
                }
                else
                {
                    _source.Insert(index - _slice.Count,item);
                }
            }
            public void RemoveAt(int index)
            {
                if (index < _spliceStart)
                {
                    _source.RemoveAt(index);
                    _spliceStart--;
                }
                else if (index < _slice.Count + _spliceStart)
                {
                    _slice.RemoveAt(index - _spliceStart);
                }
                else
                {
                    _source.RemoveAt(index - _slice.Count);
                }
            }
            public T this[int index]
            {
                get
                {
                    if (index < _spliceStart)
                        return _source[index];
                    if (index < _slice.Count + _spliceStart)
                        return _slice[index - _spliceStart];
                    return _source[index - _slice.Count];
                }
                set
                {
                    if (index < _spliceStart)
                        _source[index] = value;
                    if (index < _slice.Count + _spliceStart)
                        _slice[index - _spliceStart] = value;
                    _source[index - _slice.Count] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        public static IList<T> Splice<T>(this IList<T> @this, IList<T> slice, int spliceStart)
        {
            return new SpliceList<T>(@this,slice,spliceStart);
        }
        public static IList<T> Splice<T>(this IList<T> @this, T slice, int spliceStart)
        {
            return @this.Splice(slice.Enumerate(),spliceStart);
        }
    }
}

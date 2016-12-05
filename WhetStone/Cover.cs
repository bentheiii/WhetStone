using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class cover
    {
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, IEnumerable<T> cover, int start = 0)
        {
            using (var tor = @this.GetEnumerator())
            {
                foreach (var _ in range.Range(start))
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return tor.Current;
                }
                foreach (var c in cover)
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return c;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, IEnumerable<T> cover, IEnumerable<int> coverindices)
        {
            using (var ctor = cover.Cycle().GetEnumerator())
            {
                using (var itor = coverindices.GetEnumerator())
                {
                    int? nextCoverInd = null;
                    T nextCover = default(T);
                    if (!ctor.MoveNext())
                    {
                        if (itor.MoveNext())
                            throw new ArgumentException("empty cover");
                    }
                    else
                    {
                        itor.MoveNext();
                        nextCoverInd = itor.Current;
                        nextCover = ctor.Current;
                    }
                    foreach (var tuple in @this.CountBind())
                    {
                        if (nextCoverInd.HasValue && nextCoverInd.Value == tuple.Item2)
                        {
                            yield return nextCover;
                            if (!itor.MoveNext())
                            {
                                nextCoverInd = null;
                            }
                            else
                            {
                                ctor.MoveNext();
                                nextCoverInd = itor.Current;
                                nextCover = ctor.Current;
                            }
                        }
                        else
                        {
                            yield return tuple.Item1;
                        }
                    }
                }
            }
        }
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, params T[] cover)
        {
            return Cover(@this, cover, 0);
        }
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, T cover, int start = 0)
        {
            return @this.Cover(cover.Enumerate(),start);
        }
        
        private class CoverList<T> : IList<T>
        {
            private readonly IList<T> _source;
            private readonly IList<T> _cover;
            private int _start;
            public CoverList(IList<T> source, IList<T> cover, int start)
            {
                _source = source;
                _cover = cover;
                _start = start;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _source.AsEnumerable().Cover(_cover,_start).GetEnumerator();
            }
            public void Add(T item)
            {
                _source.Add(item);
                if (covered(this.Count))
                    _cover[this.Count - _start] = item;
            }
            public void Clear()
            {
                _cover.Clear();
                _source.Clear();
                _start = 0;
            }
            private bool covered(int index)
            {
                return index >= _start && index - _start < _cover.Count;
            }
            public bool Contains(T item)
            {
                return IndexOf(item) >= 0;
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
                var i = IndexOf(item);
                if (i == -1)
                    return false;
                RemoveAt(i);
                return true;
            }
            public int Count => _source.Count;
            public bool IsReadOnly => _source.IsReadOnly && _cover.IsReadOnly;
            public int IndexOf(T item)
            {
                var ic = _cover.IndexOf(item);
                if (ic >= 0 && ic + _start < _source.Count)
                    return ic + _start;
                var si = _source.IndexOf(item);
                if (si >= 0 && !covered(si))
                    return si;
                return -1;
            }
            public void Insert(int index, T item)
            {
                if (covered(index))
                {
                    _cover.Insert(index-_start,item);
                }
                _source.Insert(index, item);
            }
            public void RemoveAt(int index)
            {
                if (covered(index))
                {
                    _source.RemoveAt(index);
                    _cover.RemoveAt(index - _start);
                }
                else
                {
                    _source.RemoveAt(index);
                    if (index < _start)
                        _start--;
                }
            }
            public T this[int index]
            {
                get
                {
                    if (index >= _start && index - _start < _cover.Count)
                        return _cover[index - _start];
                    return _source[index];
                }
                set
                {
                    if (index >= _start && index - _start < _cover.Count)
                        _cover[index - _start] = value;
                    _source[index] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private class CoverListIndices<T> : IList<T>
        {
            private readonly IList<T> _source;
            private readonly IList<T> _cover;
            private readonly IList<int> _coverindices;
            public CoverListIndices(IList<T> source, IList<T> cover, IList<int> coverindices)
            {
                _source = source;
                _cover = cover.Count >= coverindices.Count ? cover : cover.Cycle();
                _coverindices = coverindices;
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _source.AsEnumerable().Cover(_cover, _coverindices).GetEnumerator();
            }
            public void Add(T item)
            {
                _source.Add(item);
                var ind = _coverindices.IndexOf(Count);
                if (ind >= 0)
                {
                    _cover[ind] = item;
                }
            }
            public void Clear()
            {
                _source.Clear();
                _cover.Clear();
                _coverindices.Clear();
            }
            public bool Contains(T item)
            {
                return IndexOf(item) >= 0;
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
                var i = IndexOf(item);
                if (i == -1)
                    return false;
                RemoveAt(i);
                return true;
            }
            public int Count => _source.Count;
            public bool IsReadOnly => _source.IsReadOnly && _cover.IsReadOnly && _coverindices.IsReadOnly;
            public int IndexOf(T item)
            {
                var ci = _cover.IndexOf(item);
                if (ci >= 0 && _coverindices[ci] < _source.Count)
                    return _coverindices[ci];
                var si = _source.IndexOf(item);
                if (si >= 0 && _coverindices.IndexOf(si) == -1)
                    return si;
                return -1;
            }
            public void Insert(int index, T item)
            {
                var minindex = _coverindices.BinarySearch(a => a >= index);
                foreach (var i in _coverindices.Indices().Slice(minindex))
                {
                    _coverindices[i]++;
                }
                _source.Insert(index, item);
            }
            public void RemoveAt(int index)
            {
                _source.RemoveAt(index);
                var i = _coverindices.IndexOf(index);
                if (i >= 0)
                {
                    _cover.RemoveAt(i);
                    _coverindices.RemoveAt(i);
                }
                var minindex = _coverindices.BinarySearch(a => a >= index);
                foreach (var c in _coverindices.Indices().Slice(minindex))
                {
                    _coverindices[c]--;
                }
            }
            public T this[int index]
            {
                get
                {
                    var i = _coverindices.IndexOf(index);
                    if (i >= 0)
                        return _cover[i];
                    return _source[index];
                }
                set
                {
                    var i = _coverindices.IndexOf(index);
                    if (i >= 0)
                        _cover[i] = value;
                    _source[index] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        public static IList<T> Cover<T>(this IList<T> @this, IList<T> cover, IList<int> coverindices)
        {
            return new CoverListIndices<T>(@this, cover, coverindices);
        }
        public static IList<T> Cover<T>(this IList<T> @this, IList<T> cover, int start = 0)
        {
            return new CoverList<T>(@this,cover,start);
        }
        public static IList<T> Cover<T>(this IList<T> @this, params T[] cover)
        {
            return @this.Cover(cover.AsList());
        }
        public static IList<T> Cover<T>(this IList<T> @this, T cover, int start)
        {
            return @this.Cover(cover.Enumerate(),start);
        }
    }
}

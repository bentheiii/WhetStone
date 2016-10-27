using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class concat
    {
        private class ConcatEnumerable<T> : LockedCollection<T>
        {
            private readonly IEnumerable<IEnumerable<T>> _source;
            public ConcatEnumerable(IEnumerable<IEnumerable<T>> source)
            {
                _source = source;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                foreach (var v in _source)
                {
                    foreach (var t in v)
                    {
                        yield return t;
                    }
                }
            }
            public override int Count
            {
                get
                {
                    return _source.Sum(a => a.Count());
                }
            }
        }
        private class ConcatList<T> : LockedList<T>
        {
            private readonly IList<IEnumerable<T>> _source;
            public ConcatList(IList<IEnumerable<T>> source)
            {
                _source = source;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                foreach (var v in _source)
                {
                    foreach (var t in v)
                    {
                        yield return t;
                    }
                }
            }
            public override int Count
            {
                get
                {
                    return _source.Sum(a => a.Count());
                }
            }
            public override T this[int index]
            {
                get
                {
                    foreach (var l in _source)
                    {
                        var c = l.Count();
                        if (index < c)
                            return l.ElementAt(index);
                        index -= c;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }
        private class ConcatListList<T> : IList<T>
        {
            private readonly IList<IList<T>> _source;
            public ConcatListList(IList<IList<T>> source)
            {
                _source = source;
            }
            public IEnumerator<T> GetEnumerator()
            {
                foreach (var v in _source)
                {
                    foreach (var t in v)
                    {
                        yield return t;
                    }
                }
            }
            public void Add(T item)
            {
                if (_source.Last().IsReadOnly)
                    _source.Add(new List<T>(1));
                _source.Last().Add(item);
            }
            public void Clear()
            {
                foreach (var l in _source)
                {
                    l.Clear();
                }
                _source.Clear();
            }
            public bool Contains(T item)
            {
                return _source.Any(x => x.Contains(item));
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var l in _source)
                {
                    l.CopyTo(array, arrayIndex);
                    arrayIndex += l.Count;
                }
            }
            public bool Remove(T item)
            {
                return _source.FirstOrDefault(x => x.Remove(item), null) != null;
            }
            public int Count
            {
                get
                {
                    return _source.Sum(a => a.Count);
                }
            }
            public bool IsReadOnly => false;
            private Tuple<IList<T>, int> ind(int index)
            {
                foreach (var l in _source)
                {
                    var c = l.Count;
                    if (index < c)
                        return Tuple.Create(l, index);
                    index -= c;
                }
                throw new IndexOutOfRangeException();
            }
            public int IndexOf(T item)
            {
                int offset = 0;
                foreach (var l in _source)
                {
                    var i = l.IndexOf(item);
                    if (i != -1)
                        return i + offset;
                    offset += l.Count;
                }
                return -1;
            }
            public void Insert(int index, T item)
            {
                var ind = this.ind(index);
                ind.Item1.Insert(ind.Item2, item);
            }
            public void RemoveAt(int index)
            {
                var ind = this.ind(index);
                ind.Item1.RemoveAt(ind.Item2);
            }
            public T this[int index]
            {
                get
                {
                    var i = ind(index);
                    return i.Item1[i.Item2];
                }
                set
                {
                    var i = ind(index);
                    i.Item1[i.Item2] = value;
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private class ConcatListListSameCount<T> : LockedList<T>
        {
            private readonly IList<IList<T>> _source;
            public ConcatListListSameCount(IList<IList<T>> source)
            {
                _source = source;
            }
            private int smallCount => _source[0].Count;
            public override IEnumerator<T> GetEnumerator()
            {
                foreach (var v in _source)
                {
                    foreach (var t in v)
                    {
                        yield return t;
                    }
                }
            }
            public override int Count
            {
                get
                {
                    return smallCount * _source.Count;
                }
            }
            public override T this[int index]
            {
                get
                {
                    return _source[index / smallCount][index % smallCount];
                }
            }
        }
        public static LockedList<T> Concat<T>(this IList<IEnumerable<T>> a)
        {
            return new ConcatList<T>(a);
        }
        public static IList<T> Concat<T>(this IList<IList<T>> a, bool? sameCount = null)
        {
            if (!a.Any())
                return new List<T>(0);
            sameCount = sameCount ?? a.Select(x => x.Count).AllEqual();
            if (sameCount.Value)
                return new ConcatListListSameCount<T>(a);
            return new ConcatListList<T>(a);
        }
        public static IList<T> Concat<T>(this IList<T> @this, IList<T> other)
        {
            return new ConcatListList<T>(new[] { @this, other });
        }
        public static LockedCollection<T> Concat<T>(this IEnumerable<IEnumerable<T>> a)
        {
            return new ConcatEnumerable<T>(a);
        }
    }
}

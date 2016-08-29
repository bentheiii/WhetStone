using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class concat
    {
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
        private class ConcatListList<T> : LockedList<T>
        {
            private readonly IList<IList<T>> _source;
            public ConcatListList(IList<IList<T>> source)
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
                    return _source.Sum(a => a.Count);
                }
            }
            public override T this[int index]
            {
                get
                {
                    foreach (var l in _source)
                    {
                        var c = l.Count;
                        if (index < c)
                            return l[index];
                        index -= c;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }
        public static LockedList<T> Concat<T>(this IList<IEnumerable<T>> a)
        {
            return new ConcatList<T>(a);
        }
        public static LockedList<T> Concat<T>(this IList<IList<T>> a)
        {
            return new ConcatListList<T>(a);
        }
        public static LockedList<T> Concat<T>(this IList<T> @this, IList<T> other)
        {
            return new ConcatListList<T>(new [] {@this,other});
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> a)
        {
            return a.SelectMany(i => i);
        }
    }
}

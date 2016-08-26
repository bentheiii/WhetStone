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
                return _source.SelectMany(l => l).GetEnumerator();
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
        public static LockedList<T> Concat<T>(this IList<IEnumerable<T>> a)
        {
            return new ConcatList<T>(a);
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> a)
        {
            return a.SelectMany(i => i);
        }
    }
}

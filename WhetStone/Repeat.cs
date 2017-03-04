using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class repeat
    {
        private class RepeatList<T> : LockedList<T>
        {
            private readonly IList<T> _source;
            private readonly int _count;
            public RepeatList(IList<T> source, int count)
            {
                _source = source;
                _count = count;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return range.Range(_count).SelectMany(i => _source).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _count * _source.Count;
                }
            }
            public override T this[int index]
            {
                get
                {
                    if (index >= Count)
                        throw new IndexOutOfRangeException();
                    return _source[index % _source.Count];
                }
            }
            public override bool Contains(T item)
            {
                return _source.Contains(item);
            }
            public override int IndexOf(T item)
            {
                return _source.IndexOf(item);
            }
        }
        private class RepeatCollection<T> : LockedCollection<T>
        {
            private readonly ICollection<T> _source;
            private readonly int _count;
            public RepeatCollection(ICollection<T> source, int count)
            {
                _source = source;
                _count = count;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return range.Range(_count).SelectMany(i => _source).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _count * _source.Count;
                }
            }
            public override bool Contains(T item)
            {
                return _source.Contains(item);
            }
        }
        public static IList<T> Repeat<T>(this IList<T> @this, int count)
        {
            return new RepeatList<T>(@this, count);
        }
        public static ICollection<T> Repeat<T>(this ICollection<T> @this, int count)
        {
            return new RepeatCollection<T>(@this, count);
        }
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> @this, int count)
        {
            return range.Range(count).SelectMany(i => @this);
        }
    }
}

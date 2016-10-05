using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class cycle
    {
        private class CycleList<T> : LockedList<T>
        {
            private readonly IList<T> _source;
            public CycleList(IList<T> source)
            {
                _source = source;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                while (true)
                {
                    foreach (T t in _source)
                    {
                        yield return t;
                    }
                }
            }
            public override int Count => int.MaxValue;
            public override T this[int index]
            {
                get
                {
                    return _source[index % _source.Count];
                }
            }
        }
        private class CycleEnumerable<T> : LockedList<T>
        {
            private readonly IEnumerable<T> _source;
            public CycleEnumerable(IEnumerable<T> source)
            {
                _source = source.CacheCount();
            }
            public override IEnumerator<T> GetEnumerator()
            {
                while (true)
                {
                    foreach (T t in _source)
                    {
                        yield return t;
                    }
                }
            }
            public override int Count => int.MaxValue;
            public override T this[int index]
            {
                get
                {
                    return _source.ElementAt(index % _source.Count());
                }
            }
        }
        public static LockedList<T> Cycle<T>(this IList<T> @this)
        {
            return new CycleList<T>(@this);
        }
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> @this)
        {
            return new CycleEnumerable<T>(@this);
        }
    }
}

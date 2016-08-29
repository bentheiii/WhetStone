using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class cacheCount
    {
        public static LockedCollection<T> CacheCount<T>(this IEnumerable<T> @this)
        {
            return new EnumerableCountCache<T>(@this);
        }
        private class EnumerableCountCache<T> : LockedCollection<T>
        {
            private readonly IEnumerable<T> _inner;
            private int? _count;
            public EnumerableCountCache(IEnumerable<T> inner)
            {
                _inner = inner;
                _count = _inner.RecommendSize();
            }
            public override IEnumerator<T> GetEnumerator()
            {
                int c = 0;
                foreach (var t in _inner)
                {
                    yield return t;
                    c++;
                }
                if (_count == null)
                {
                    _count = c;
                }

            }
            public override bool Contains(T item)
            {
                return _inner.Contains(item);
            }
            public override int Count
            {
                get
                {
                    if (!_count.HasValue)
                        _count = _inner.Count();
                    return _count.Value;
                }
            }
        }
    }
}
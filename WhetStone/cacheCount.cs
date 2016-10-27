using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class cacheCount
    {
        private class CountCache<T> : LockedCollection<T>
        {
            private int? _count;
            private readonly IEnumerable<T> _source;
            public CountCache(IEnumerable<T> source)
            {
                _source = source;
                _count = source.RecommendCount();
            }
            public override IEnumerator<T> GetEnumerator()
            {
                if (_count.HasValue)
                {
                    foreach (var t in _source)
                    {
                        yield return t;
                    }
                }
                else
                {
                    int c = 0;
                    foreach (T t in _source)
                    {
                        c++;
                        yield return t;
                    }
                    _count = c;
                }
            }
            public override int Count
            {
                get
                {
                    if (_count == null)
                        _count = _source.Count();
                    return _count.Value;
                }
            }
        }
        public static LockedCollection<T> CacheCount<T>(this IEnumerable<T> @this)
        {
            return new CountCache<T>(@this);
        }
    }
}

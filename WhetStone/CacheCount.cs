using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
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
        /// <summary>
        /// Caches only the count of an <see cref="IEnumerable{T}"/>, making only enumerate once at most in case of multiple <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> calls.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> whose count will be cached.</param>
        /// <returns>A new, read only <see cref="ICollection{T}"/> that remembers the count of <paramref name="this"/> hen it is calculated.</returns>
        public static ICollection<T> CacheCount<T>(this IEnumerable<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new CountCache<T>(@this);
        }
    }
}

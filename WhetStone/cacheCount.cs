using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class cacheCount
    {
        public static LockedCollection<T> CacheCount<T>(this IEnumerable<T> @this)
        {
            return new EnumerableCountCache<T>(@this);
        }
    }
}
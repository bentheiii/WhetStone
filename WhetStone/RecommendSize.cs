using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class recommendSize
    {
        public static int? RecommendCount<T>(this IEnumerable<T> @this)
        {
            return (@this as IReadOnlyCollection<T>)?.Count ?? (@this as ICollection<T>)?.Count;
        }
    }
}

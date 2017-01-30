using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class recommendSize
    {
        public static int? RecommendCount<T>(this IEnumerable<T> @this)
        {
            return @this.AsCollection(false)?.Count;
        }
    }
}

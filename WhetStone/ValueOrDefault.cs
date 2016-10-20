using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class valueOrDefault
    {
        public static G ValueOrDefault<T, G>(this IDictionary<T, G> @this, T key, G defaultval = default(G))
        {
            G val;
            return @this.TryGetValue(key, out val) ? val : defaultval;
        }
        public static T ValueOrDefault<T>(this IList<T> @this, int ind, T defaultval = default(T))
        {
            return ind < @this.Count ? @this[ind] : defaultval;
        }
    }
}

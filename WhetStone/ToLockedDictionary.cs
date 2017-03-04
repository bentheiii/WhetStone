using System.Collections.Generic;

namespace WhetStone.LockedStructures
{
    internal static class toLockedDictionary
    {
        public static IDictionary<T, G> ToLockedDictionary<T, G>(this IReadOnlyDictionary<T, G> @this)
        {
            return new LockedDictionaryReadOnlyAdaptor<T, G>(@this);
        }
    }
}
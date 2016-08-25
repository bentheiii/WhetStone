using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.LockedStructures
{
    internal static class toLockedDictionary
    {
        public static LockedDictionary<T, G> ToLockedDictionary<T, G>(this IReadOnlyDictionary<T, G> @this)
        {
            return new LockedDictionaryReadOnlyAdaptor<T, G>(@this);
        }
    }
}
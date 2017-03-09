using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.LockedStructures
{
    internal static class toLockedDictionary
    {
        public static IDictionary<T, G> ToLockedDictionary<T, G>(this IReadOnlyDictionary<T, G> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new LockedDictionaryReadOnlyAdaptor<T, G>(@this);
        }
    }
}
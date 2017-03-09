using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.LockedStructures
{
    
    internal static class toLockedCollection
    {
        public static ICollection<T> ToLockedCollection<T>(this IReadOnlyCollection<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new LockedCollectionReadOnlyAdaptor<T>(@this);
        }
    }
}

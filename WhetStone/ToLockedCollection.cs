using System.Collections.Generic;

namespace WhetStone.LockedStructures
{
    
    internal static class toLockedCollection
    {
        public static ICollection<T> ToLockedCollection<T>(this IReadOnlyCollection<T> @this)
        {
            return new LockedCollectionReadOnlyAdaptor<T>(@this);
        }
    }
}

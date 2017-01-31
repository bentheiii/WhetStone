using System.Collections.Generic;

namespace WhetStone.LockedStructures
{
    public static class toLockedCollection
    {
        public static LockedCollection<T> ToLockedCollection<T>(this IReadOnlyCollection<T> @this)
        {
            return new LockedCollectionReadOnlyAdaptor<T>(@this);
        }
    }
}

using System.Collections.Generic;
using WhetStone.LockedStructures;

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
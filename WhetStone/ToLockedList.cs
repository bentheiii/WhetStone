using System.Collections.Generic;

namespace WhetStone.LockedStructures
{
    internal static class toLockedList
    {
        public static IList<T> ToLockedList<T>(this IReadOnlyList<T> @this)
        {
            return new LockedListReadOnlyAdaptor<T>(@this);
        }
        public static IList<char> ToLockedList(this string @this)
        {
            return new LockedListStringAdaptor(@this);
        }
    }
}

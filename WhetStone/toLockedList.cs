using System.Collections.Generic;

namespace WhetStone.LockedStructures
{
    public static class toLockedList
    {
        public static LockedList<T> ToLockedList<T>(this IReadOnlyList<T> @this)
        {
            return new LockedListReadOnlyAdaptor<T>(@this);
        }
        public static LockedList<char> ToLockedList(this string @this)
        {
            return new LockedListStringAdaptor(@this);
        }
    }
}
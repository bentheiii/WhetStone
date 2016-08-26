using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class swap
    {
        public static void Swap<T>(this IList<T> toswap, int index1, int index2)
        {
            if (index1 == index2)
                return;
            T temp = toswap[index1];
            toswap[index1] = toswap[index2];
            toswap[index2] = temp;
        }
    }
}

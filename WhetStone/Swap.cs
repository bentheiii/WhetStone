using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Arrays
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

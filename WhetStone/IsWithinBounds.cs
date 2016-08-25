using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;
using System.Text;
using System.Threading.Tasks;
using WhetStone.NumbersMagic;

namespace WhetStone.Arrays
{
    public static class isWithinBounds
    {
        public static bool IsWithinBounds(this Array arr, params int[] ind)
        {
            if (arr.Rank != ind.Length)
                throw new ArgumentException("mismatch on indices");
            return arr.GetSize().Zip(ind).All(a => a.Item2.iswithinPartialExclusive(0, a.Item1));
        }
    }
}

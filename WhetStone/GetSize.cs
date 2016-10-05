using System;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class getSize
    {
        public static int[] GetSize(this Array mat)
        {
            return range.Range(mat.Rank).Select(mat.GetLength).ToArray();
        }
    }
}

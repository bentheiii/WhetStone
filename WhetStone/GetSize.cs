using System;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class getSize
    {
        public static LockedList<int> GetSize(this Array mat)
        {
            return range.Range(mat.Rank).Select(mat.GetLength);
        }
    }
}

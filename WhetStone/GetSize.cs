using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class getSize
    {
        public static IEnumerable<int> GetSize(this Array mat)
        {
            return range.Range(mat.Rank).Select(mat.GetLength);
        }
    }
}

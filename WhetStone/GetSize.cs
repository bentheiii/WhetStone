using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.Arrays
{
    public static class getSize
    {
        public static IEnumerable<int> GetSize(this Array mat)
        {
            return range.Range(mat.Rank).Select(mat.GetLength);
        }
    }
}

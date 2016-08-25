using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Comparison
{
    public class DynamicComparer<T> : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            dynamic d = x;
            if (d < y)
                return -1;
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (d > y)
                return 1;
            return 0;
        }
    }
}

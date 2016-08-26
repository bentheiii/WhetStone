using System.Collections.Generic;

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

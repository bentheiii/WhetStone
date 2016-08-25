using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Arrays
{
    public static class fill2D
    {
        public static T[,] Fill2D<T>(int rows, int cols, T tofill = default(T))
        {
            Func<int, int, T> tf = (n, m) => tofill;
            return Fill2D(rows, cols, tf);
        }
        public static T[,] Fill2D<T>(int rows, int cols, Func<int, int, T> tofill)
        {
            T[,] ret = new T[rows, cols];
            for (int i = 0; i < ret.GetLength(0); i++)
            {
                for (int j = 0; j < ret.GetLength(1); j++)
                    ret[i, j] = tofill(i, j);
            }
            return ret;
        }
    }
}

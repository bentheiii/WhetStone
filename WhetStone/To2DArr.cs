using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class to2DArr
    {
        public static T[,] To2DArr<T>(this IEnumerable<T> a, int dim0Length)
        {
            if (a.Count() % dim0Length != 0)
                throw new Exception("array length must divide row length evenly");
            int dim2Length = a.Count() / dim0Length;
            T[,] ret = new T[dim0Length, dim2Length];
            var tor = a.GetEnumerator();
            for (int i = 0; i < dim0Length; i++)
            {
                for (int j = 0; j < dim2Length; j++)
                {
                    tor.MoveNext();
                    ret[i, j] = tor.Current;
                }
            }
            return ret;
        }
    }
}

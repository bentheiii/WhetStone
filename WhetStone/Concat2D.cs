using System;
using System.Linq;
using WhetStone.Comparison;

namespace WhetStone.Looping
{
    public static class concat2D
    {
        public static T[,] Concat<T>(int dimen, params T[][,] a)
        {
            switch (dimen)
            {
                case 0:
                    {
                        if (!a.AllEqual(new EqualityFunctionComparer<T[,], int>(x => x.GetLength(1))) || a.Length == 0)
                            throw new ArgumentException("the arrays must be non-empty and of compatible sizes");
                        T[,] ret = new T[a.Sum(x => x.GetLength(0)), a[0].GetLength(1)];
                        int row = 0;
                        foreach (T[,] m in a)
                        {
                            foreach (int i in range.Range(m.GetLength(0)))
                            {
                                foreach (int j in range.Range(m.GetLength(1)))
                                    ret[row, j] = m[i, j];
                                row++;
                            }
                        }
                        return ret;
                    }
                case 1:
                    {
                        if (!a.AllEqual(new EqualityFunctionComparer<T[,], int>(x => x.GetLength(0))) || a.Length == 0)
                            throw new ArgumentException("the arrays must be non-empty and of compatible sizes");
                        T[,] ret = new T[a[0].GetLength(0), a.Sum(x => x.GetLength(1))];
                        int col = 0;
                        foreach (T[,] m in a)
                        {
                            foreach (int i in range.Range(m.GetLength(1)))
                            {
                                foreach (int j in range.Range(m.GetLength(0)))
                                    ret[j, col] = m[j, i];
                                col++;
                            }
                        }
                        return ret;
                    }
            }
            throw new ArgumentException($"{nameof(dimen)} must be either 1 or 0");
        }
        public static T[,] Concat<T>(this T[,] @this, T[,] other, int dimen)
        {
            return Concat(dimen, @this, other);
        }
    }
}

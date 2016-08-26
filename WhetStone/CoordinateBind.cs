using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class coordinateBind
    {
        public static IEnumerable<Tuple<T, int, int>> CoordinateBind<T>(this T[,] @this)
        {
            foreach (int row in range.Range(@this.GetLength(0)))
            {
                foreach (int col in range.Range(@this.GetLength(1)))
                {
                    yield return new Tuple<T, int, int>(@this[row, col], row, col);
                }
            }
        }
        public static IEnumerable<Tuple<T, int, int, int>> CoordinateBind<T>(this T[,,] @this)
        {
            foreach (int c0 in range.Range(@this.GetLength(0)))
            {
                foreach (int c1 in range.Range(@this.GetLength(1)))
                {
                    foreach (int c2 in range.Range(@this.GetLength(2)))
                    {
                        yield return new Tuple<T, int, int, int>(@this[c0, c1, c2], c0, c1, c2);
                    }
                }
            }
        }
        public static IEnumerable<Tuple<object, int[]>> CoordinateBind(this Array @this)
        {
            return @this.GetSize().Select(a => range.Range(a)).ToArray().Join().Select(a => Tuple.Create(@this.GetValue(a), a));
        }
        public static IEnumerable<Tuple<T, int[]>> CoordinateBind<T>(this Array @this)
        {
            return @this.GetSize().Select(a => range.Range(a)).ToArray().Join().Select(a => Tuple.Create((T)@this.GetValue(a), a));
        }
        public static IEnumerable<Tuple<T, int, int>> CoordinateBind<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            foreach (var t1 in @this.CountBind())
            {
                foreach (var t0 in t1.Item1.CountBind())
                {
                    yield return Tuple.Create(t0.Item1, t1.Item2, t0.Item2);
                }
            }
        }
        public static IEnumerable<Tuple<T, int, int, int>> CoordinateBind<T>(this IEnumerable<IEnumerable<IEnumerable<T>>> @this)
        {
            foreach (var t2 in @this.CountBind())
            {
                foreach (var t1 in t2.Item1.CountBind())
                {
                    foreach (var t0 in t1.Item1.CountBind())
                    {
                        yield return Tuple.Create(t0.Item1, t2.Item2, t1.Item2, t0.Item2);
                    }
                }
            }
        }
    }
}

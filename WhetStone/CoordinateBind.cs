using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class coordinateBind
    {
        /// <overloads>Attaches coordinates to nested enumerables.</overloads>
        /// <summary>
        /// Binds the elements in a 2D <see cref="Array"/> to their coordinates.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Array"/>.</typeparam>
        /// <param name="this">The <see cref="Array"/> whose elements to use.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3}"/>. The first element of each tuple is the element, the next are the coordinates.</returns>
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
        /// <summary>
        /// Binds the elements in a 3D <see cref="Array"/> to their coordinates.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Array"/>.</typeparam>
        /// <param name="this">The <see cref="Array"/> whose elements to use.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3,T4}"/>. The first element of each tuple is the element, the next are the coordinates.</returns>
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
        /// <summary>
        /// Binds the elements in an <see cref="Array"/> to their coordinates.
        /// </summary>
        /// <param name="this">The <see cref="Array"/> whose elements to use.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2}"/>. The first element of each tuple is the element, the second is the coordinates.</returns>
        /// <remarks>This is a non-generic overload, to use strong typing, see <see cref="CoordinateBind{T}(System.Array)"/>.</remarks>
        public static IEnumerable<Tuple<object, int[]>> CoordinateBind(this Array @this)
        {
            return @this.Indices().Select(a => Tuple.Create(@this.GetValue(a), a));
        }
        /// <summary>
        /// Binds the elements in an <see cref="Array"/> to their coordinates.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Array"/>.</typeparam>
        /// <param name="this">The <see cref="Array"/> whose elements to use.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2}"/>. The first element of each tuple is the element, the second is the coordinates.</returns>
        /// <remarks>This is a generic overload, to use weak typing, see <see cref="CoordinateBind{T}(System.Array)"/>.</remarks>
        public static IEnumerable<Tuple<T, int[]>> CoordinateBind<T>(this Array @this)
        {
            return @this.Indices().Select(a => Tuple.Create((T)@this.GetValue(a), a));
        }
        /// <summary>
        /// Binds the elements in an <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/>s to their coordinates.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/>s whose elements to use.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3}"/>. The first element of each tuple is the element, the next are the coordinates.</returns>
        public static IEnumerable<Tuple<T, Tuple<int, int>>> CoordinateBind<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            foreach (var t1 in @this.CountBind())
            {
                foreach (var t0 in t1.Item1.CountBind())
                {
                    yield return Tuple.Create(t0.Item1, Tuple.Create(t1.Item2, t0.Item2));
                }
            }
        }
        /// <summary>
        /// Binds the elements in an <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/>s to their coordinates.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/>s whose elements to use.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3,T4}"/>. The first element of each tuple is the element, the next are the coordinates.</returns>
        public static IEnumerable<Tuple<T, Tuple<int, int, int>>> CoordinateBind<T>(this IEnumerable<IEnumerable<IEnumerable<T>>> @this)
        {
            foreach (var t2 in @this.CountBind())
            {
                foreach (var t1 in t2.Item1.CountBind())
                {
                    foreach (var t0 in t1.Item1.CountBind())
                    {
                        yield return Tuple.Create(t0.Item1, Tuple.Create(t2.Item2, t1.Item2, t0.Item2));
                    }
                }
            }
        }
    }
}

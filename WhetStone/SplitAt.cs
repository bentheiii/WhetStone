using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class splitAt
    {
        /// <overloads>Split an enumerable by sub-enumerable lengths.</overloads>
        /// <summary>
        /// Split an <see cref="IEnumerable{T}"/> by sub-enumerable lengths.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to split.</param>
        /// <param name="lengths">The lengths of the sub-enumerables.</param>
        /// <returns><paramref name="this"/> split to <paramref name="lengths"/>-length sub-enumerables.</returns>
        public static IEnumerable<IEnumerable<T>> SplitAt<T>(this IEnumerable<T> @this, params int[] lengths)
        {
            using (var tor = @this.GetEnumerator())
            {
                foreach (int length in lengths)
                {
                    ResizingArray<T> ret = new ResizingArray<T>(length);
                    foreach (int _ in range.Range(length))
                    {
                        if (!tor.MoveNext())
                        {
                            yield return ret.arr;
                            yield break;
                        }
                        ret.Add(tor.Current);
                    }
                    yield return ret;
                }

                yield return infinite.Infinite().TakeWhile(a=>tor.MoveNext()).Select(a => tor.Current);
            }
        }
        /// <overloads>Split an enumerable by sub-enumerable lengths.</overloads>
        /// <summary>
        /// Split an <see cref="IList{T}"/> by sub-enumerable lengths.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to split.</param>
        /// <param name="lengths">The lengths of the sub-enumerables.</param>
        /// <returns><paramref name="this"/> split to <paramref name="lengths"/>-length sub-enumerables.</returns>
        public static IList<IList<T>> SplitAt<T>(this IList<T> @this, params int[] lengths)
        {
            return SplitAt(@this, lengths.AsList());
        }
        /// <overloads>Split an enumerable by sub-enumerable lengths.</overloads>
        /// <summary>
        /// Split an <see cref="IList{T}"/> by sub-enumerable lengths.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to split.</param>
        /// <param name="lengths">The lengths of the sub-enumerables.</param>
        /// <returns><paramref name="this"/> split to <paramref name="lengths"/>-length sub-enumerables.</returns>
        public static IList<IList<T>> SplitAt<T>(this IList<T> @this, IList<int> lengths)
        {
            lengths = lengths.PartialSums().ToList().Concat(@this.Count.Enumerate());
            var t = lengths.Trail(2).TakeWhile(a => a[0] <= @this.Count && a[1] <= @this.Count).ToArray();
            return t.Select(a => @this.Slice(a[0], a[1]));
        }
    }
}

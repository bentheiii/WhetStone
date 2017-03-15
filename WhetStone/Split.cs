using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class split
    {
        /// <overloads>Split an enumerable into sub-enumerables.</overloads>
        /// <summary>
        /// Split an <see cref="IEnumerable{T}"/> into sublists by capturing.
        /// </summary>
        /// <typeparam name="T">The type of the  <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to split.</param>
        /// <param name="capture">The capture function, that accepts an existing sub-list and an element, and returns whether the element is the next part of the sub-list.</param>
        /// <returns>A new <see cref="IList{T}"/>, splitting <paramref name="this"/> by the <paramref name="capture"/> function.</returns>
        public static IEnumerable<IList<T>> Split<T>(this IEnumerable<T> @this, Func<IList<T>, T, bool> capture)
        {
            @this.ThrowIfNull(nameof(@this));
            capture.ThrowIfNull(nameof(capture));
            ResizingArray<T> ret = new ResizingArray<T>();

            using (var tor = @this.GetEnumerator())
            {
                while (tor.MoveNext())
                {
                    if (capture(ret.arr, tor.Current))
                    {
                        ret.Add(tor.Current);
                    }
                    else
                    {
                        yield return ret.arr;
                        ret = new ResizingArray<T>{tor.Current};
                    }
                }
                if (ret.Count != 0)
                    yield return ret.arr;
            }
        }
        /// <overloads>Split an enumerable into sub-enumerables.</overloads>
        /// <summary>
        /// Split an <see cref="IList{T}"/> into sublists by capturing.
        /// </summary>
        /// <typeparam name="T">The type of the  <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to split.</param>
        /// <param name="capture">The capture function, that accepts an existing sub-list and an element, and returns whether the element is the next part of the sub-list.</param>
        /// <returns>A new <see cref="IList{T}"/>, splitting <paramref name="this"/> by the <paramref name="capture"/> function.</returns>
        public static IEnumerable<IList<T>> Split<T>(this IList<T> @this, Func<IList<T>, T,bool> capture)
        {
            @this.ThrowIfNull(nameof(@this));
            capture.ThrowIfNull(nameof(capture));
            int indstart = 0;
            int len = 0;
            while (true)
            {
                if (indstart + len >= @this.Count)
                {
                    if (len != 0)
                        yield return @this.Slice(indstart, length: len);
                    yield break;
                }
                if (capture(@this.Slice(indstart, length: len), @this[indstart + len]))
                {
                    len++;
                }
                else
                {
                    yield return @this.Slice(indstart, length: len);
                    indstart = indstart + len;
                    len = 0;
                }
            }
        }
        /// <summary>
        /// Split an <see cref="IEnumerable{T}"/> by a divisor element.
        /// </summary>
        /// <typeparam name="T">The element of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to split.</param>
        /// <param name="divisor">The divisor element.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to compare elements. <see langword="null"/> for default.</param>
        /// <returns><paramref name="this"/> split by <paramref name="divisor"/>.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> @this, T divisor, IEqualityComparer<T> comp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            comp = comp ?? EqualityComparer<T>.Default;
            return Split(@this, a => comp.Equals(divisor, a));
        }
        /// <summary>
        /// Split an <see cref="IEnumerable{T}"/> by a divisor element.
        /// </summary>
        /// <typeparam name="T">The element of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to split.</param>
        /// <param name="divisorDetector">A method to get whether an element is a divisor element.</param>
        /// <returns><paramref name="this"/> as split by divisors.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> @this, Func<T,bool> divisorDetector)
        {
            @this.ThrowIfNull(nameof(@this));
            divisorDetector.ThrowIfNull(nameof(divisorDetector));
            var ret = new ResizingArray<T>();
            foreach (var t in @this)
            {
                if (divisorDetector(t))
                {
                    yield return ret.arr;
                    ret = new ResizingArray<T>();
                    continue;
                }
                ret.Add(t);
            }
            yield return ret.arr;
        }
    }
}

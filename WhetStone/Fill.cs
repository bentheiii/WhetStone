using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class fill
    {
        /// <overloads>Fills an <see cref="IList{T}"/> with elements.</overloads>
        /// <summary>
        /// Fills an <see cref="IList{T}"/> with values.
        /// </summary>
        /// <typeparam name="T">The type of the values to fill.</typeparam>
        /// <param name="tofill">The <see cref="IList{T}"/> to fill.</param>
        /// <param name="v">The values to fill <paramref name="tofill"/> with, the values will repeat over it.</param>
        /// <example>
        /// <code>
        /// IList&lt;bool&gt;= new int[5];
        /// val.Fill(0,1); //val = {0,1,0,1,0}
        /// </code>
        /// </example>
        public static void Fill<T>(this IList<T> tofill, params T[] v)
        {
            tofill.ThrowIfNull(nameof(tofill));
            v.ThrowIfNull(nameof(v));
            tofill.Fill(v, 0);
        }
        /// <summary>
        /// Fills an <see cref="IList{T}"/> with values.
        /// </summary>
        /// <typeparam name="T">The type of the values to fill.</typeparam>
        /// <param name="tofill">The <see cref="IList{T}"/> to fill.</param>
        /// <param name="v">The values to fill <paramref name="tofill"/> with, the values will repeat over it.</param>
        /// <param name="start">The first index to be filled.</param>
        /// <param name="count">The number of indices to be filled, or <see langword="null" /> to continue filling to the end of the <see cref="IList{T}"/>.</param>
        /// <exception cref="ArgumentException">If <paramref name="v"/> is empty.</exception>
        public static void Fill<T>(this IList<T> tofill, T[] v, int start, int? count = null)
        {
            tofill.ThrowIfNull(nameof(tofill));
            if (v.Length == 0 && (count  ?? 1) != 0)
                throw new ArgumentException("cannot be empty", nameof(v));
            start.ThrowIfAbsurd(nameof(start));
            count.ThrowIfAbsurd(nameof(count));
            Fill(tofill, i => (v[i % v.Length]), start, count);
        }
        /// <summary>
        /// Fills an <see cref="IList{T}"/> with values.
        /// </summary>
        /// <typeparam name="T">The type of the values to fill.</typeparam>
        /// <param name="tofill">The <see cref="IList{T}"/> to fill.</param>
        /// <param name="filler">A function that retrieves the values to fill <paramref name="tofill"/> with.</param>
        /// <param name="start">The first index to be filled.</param>
        /// <param name="count">The number of indices to be filled, or <see langword="null" /> to continue filling to the end of the <see cref="IList{T}"/>.</param>
        public static void Fill<T>(this IList<T> tofill, Func<int, T> filler, int start = 0, int? count = null)
        {
            tofill.ThrowIfNull(nameof(tofill));
            filler.ThrowIfNull(nameof(filler));
            start.ThrowIfAbsurd(nameof(start));
            count.ThrowIfAbsurd(nameof(count));
            IList<int> indices = tofill.Indices().Skip(start);
            if (count.HasValue)
                indices = indices.Take(count.Value);
            foreach (int i in indices)
            {
                tofill[i] = filler(i);
            }
        }
        /// <summary>
        /// Fills an <see cref="IList{T}"/> with values.
        /// </summary>
        /// <typeparam name="T">The type of the values to fill.</typeparam>
        /// <param name="tofill">The <see cref="IList{T}"/> to fill.</param>
        /// <param name="filler">A function that retrieves the values to fill <paramref name="tofill"/> with.</param>
        /// <param name="start">The first index to be filled.</param>
        /// <param name="count">The number of indices to be filled, or <see langword="null" /> to continue filling to the end of the <see cref="IList{T}"/>.</param>
        public static void Fill<T>(this IList<T> tofill, Func<T> filler, int start = 0, int? count = null)
        {
            tofill.ThrowIfNull(nameof(tofill));
            filler.ThrowIfNull(nameof(filler));
            start.ThrowIfAbsurd(nameof(start));
            count.ThrowIfAbsurd(nameof(count));
            tofill.Fill(a => filler(), start, count);
        }
        /// <summary>
        /// Creates an <see cref="Array"/> and fills it with values.
        /// </summary>
        /// <typeparam name="T">The type of the created array.</typeparam>
        /// <param name="length">The length of the resultant array.</param>
        /// <param name="filler">The values to fill the array with, they will repeat.</param>
        /// <param name="start">The first index to be filled.</param>
        /// <param name="count">The number of indices to be filled, or <see langword="null" /> to continue filling to the end of the <see cref="IList{T}"/>.</param>
        /// <returns>A new <see cref="Array"/> of length <paramref name="length"/>, with values in the appropriated indices filled to <paramref name="filler"/></returns>
        public static T[] Fill<T>(int length, T[] filler, int start, int? count = null)
        {
            filler.ThrowIfNull(nameof(filler));
            length.ThrowIfAbsurd(nameof(length));
            start.ThrowIfAbsurd(nameof(start));
            count.ThrowIfAbsurd(nameof(count));
            T[] ret = new T[length];
            if (!filler.All(a => a.Equals(default(T))))
                ret.Fill(filler, start, count);
            return ret;
        }
        /// <summary>
        /// Creates an <see cref="Array"/> and fills it with values.
        /// </summary>
        /// <typeparam name="T">The type of the created array.</typeparam>
        /// <param name="length">The length of the resultant array.</param>
        /// <param name="filler">The values to fill the array with, they will repeat.</param>
        /// <returns>A new <see cref="Array"/> of length <paramref name="length"/>, with values in the filled to <paramref name="filler"/></returns>
        public static T[] Fill<T>(int length, params T[] filler)
        {
            length.ThrowIfAbsurd(nameof(length));
            filler.ThrowIfNull(nameof(filler));
            T[] ret = new T[length];
            if (!filler.All(a=>a.Equals(default(T))))
                ret.Fill(filler);
            return ret;
        }
        /// <summary>
        /// Creates an <see cref="Array"/> and fills it with values.
        /// </summary>
        /// <typeparam name="T">The type of the created array.</typeparam>
        /// <param name="length">The length of the resultant array.</param>
        /// <param name="filler">The function to generate filler elements.</param>
        /// <param name="start">The first index to be filled.</param>
        /// <param name="count">The number of indices to be filled, or <see langword="null" /> to continue filling to the end of the <see cref="IList{T}"/>.</param>
        /// <returns>A new <see cref="Array"/> of length <paramref name="length"/>, with values in the appropriated indices filled by <paramref name="filler"/></returns>
        public static T[] Fill<T>(int length, Func<int, T> filler, int start = 0, int? count = null)
        {
            filler.ThrowIfNull(nameof(filler));
            length.ThrowIfAbsurd(nameof(length));
            start.ThrowIfAbsurd(nameof(start));
            count.ThrowIfAbsurd(nameof(count));
            T[] ret = new T[length];
            ret.Fill(filler, start, count);
            return ret;
        }
        /// <summary>
        /// Creates an <see cref="Array"/> and fills it with values.
        /// </summary>
        /// <typeparam name="T">The type of the created array.</typeparam>
        /// <param name="length">The length of the resultant array.</param>
        /// <param name="filler">The function to generate filler elements.</param>
        /// <param name="start">The first index to be filled.</param>
        /// <param name="count">The number of indices to be filled, or <see langword="null" /> to continue filling to the end of the <see cref="IList{T}"/>.</param>
        /// <returns>A new <see cref="Array"/> of length <paramref name="length"/>, with values in the appropriated indices filled by <paramref name="filler"/></returns>
        public static T[] Fill<T>(int length, Func<T> filler, int start = 0, int? count = null)
        {
            filler.ThrowIfNull(nameof(filler));
            length.ThrowIfAbsurd(nameof(length));
            start.ThrowIfAbsurd(nameof(start));
            count.ThrowIfAbsurd(nameof(count));
            T[] ret = new T[length];
            ret.Fill(a => filler(), start, count);
            return ret;
        }
    }
}

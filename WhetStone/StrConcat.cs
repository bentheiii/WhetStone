using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class strConcat
    {
        /// <summary>
        /// Concatenates an <see cref="IEnumerable{T}"/>'s elements in a readable manner.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The <see cref="IEnumerable{T}"/> to use.</param>
        /// <param name="seperator">The <see cref="string"/> separator to place between elements.</param>
        /// <returns>All the elements in <paramref name="a"/> converted to <see cref="string"/> and concatenated with <paramref name="seperator"/>.</returns>
        public static string StrConcat<T>(this IEnumerable<T> a, string seperator = ", ")
        {
            a.ThrowIfNull(nameof(a));
            seperator.ThrowIfNull(nameof(seperator));
            StringBuilder b = new StringBuilder();
            using (var tor = a.GetEnumerator())
            {
                if (!tor.MoveNext())
                    return "";
                b.Append(tor.Current);
                while (tor.MoveNext())
                {
                    b.Append(seperator);
                    b.Append(tor.Current);
                }
            }
            return b.ToString();
        }
        /// <summary>
        /// Concatenates an <see cref="IEnumerable{T}"/>'s elements in a readable manner.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The <see cref="IEnumerable{T}"/> to use.</param>
        /// <param name="seperator">The <see cref="string"/> separator to place between elements.</param>
        /// <param name="format">The format of the elements when converted to string.</param>
        /// <param name="prov">The <see cref="IFormatProvider"/> to use when formatting. <see langword="null"/> for <see cref="CultureInfo.CurrentCulture"/>.</param>
        /// <returns>All the elements in <paramref name="a"/> converted to <see cref="string"/> and concatenated with <paramref name="seperator"/>.</returns>
        public static string StrConcat<T>(this IEnumerable<T> a, string seperator, string format, IFormatProvider prov = null) where T:IFormattable
        {
            a.ThrowIfNull(nameof(a));
            seperator.ThrowIfNull(nameof(seperator));
            format.ThrowIfNull(nameof(format));
            prov = prov ?? CultureInfo.CurrentCulture;
            return a.Select(x => x.ToString(format, prov)).StrConcat(seperator);
        }
        /// <summary>
        /// Concatenates an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey,TValue}"/>'s elements in a readable manner.
        /// </summary>
        /// <typeparam name="K">The type of the keys.</typeparam>
        /// <typeparam name="V">The type of the values</typeparam>
        /// <param name="a">The <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey,TValue}"/> to use.</param>
        /// <param name="definitionSeperator">The separator to place between keys and values.</param>
        /// <param name="seperator">The separator to place between key-value pairs.</param>
        /// <returns>All the key value pairs in <paramref name="a"/> converted to <see cref="string"/> and concatenated.</returns>
        public static string StrConcat<K, V>(this IEnumerable<KeyValuePair<K, V>> a, string definitionSeperator = ": ", string seperator = ", ")
        {
            a.ThrowIfNull(nameof(a));
            seperator.ThrowIfNull(nameof(seperator));
            definitionSeperator.ThrowIfNull(nameof(definitionSeperator));
            return a.Select(x => new[] {x.Key.ToString(), definitionSeperator, x.Value.ToString()}.StrConcat("")).StrConcat(seperator);
        }
    }
}

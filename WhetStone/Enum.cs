using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class @enum
    {
        /// <summary>
        /// Generates all the enum members of a particular <see langword="enum"/>.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>All the enum members.</returns>
        /// <remarks>
        /// <para>Uses reflection to generate types.</para>
        /// <para>See <see cref="Enum.GetValues"/> to get the order of values</para>
        /// </remarks>
        public static IEnumerable<T> Enum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }
        /// <summary>
        /// Generates all the enum members of a particular <see langword="enum"/>, filtering in only ones detected as distinct flags.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>All the enum members detected as flag members.</returns>
        /// <remarks>
        /// <para>Uses reflection to generate types.</para>
        /// <para>Detecting Flags is tricky, will only return members that are bitwise distinct from all of the previous ones. This means that the elements generated are dependent on the order they are generated.</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// [Flags] enum Enum1 {a=1,b=2,c=3 };
        /// @enum.EnumFlags&lt;Enum1&gt;();// a,b
        /// </code>
        /// </example>
        public static IEnumerable<T> EnumFlags<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type with [Flags] attribute");
            }
            long shown = 0;
            foreach (T t in Enum<T>())
            {
                var i = t.ToInt64(CultureInfo.CurrentCulture);
                if ((shown & i) == i)
                    continue;
                shown |= i;
                yield return t;
            }
        }
        /// <summary>
        /// Generates all the enum members of a particular <see langword="enum"/>, filtering in only ones detected as distinct flags that <paramref name="filter"/> contains.
        /// </summary>
        /// <typeparam name="T">The type of the <see langword="enum"/>.</typeparam>
        /// <param name="filter">Filters the results to only ones it contains.</param>
        /// <returns>Only flag elements that are contained in <paramref name="filter"/>.</returns>
        /// <remarks>Uses dynamics (once per call).</remarks>
        public static IEnumerable<T> EnumFlags<T>(this T filter) where T : struct, IConvertible
        {
            var f = (Enum)(dynamic)filter;
            return EnumFlags<T>().Cast<Enum>().Where(a => f.HasFlag(a)).Cast<T>();
        }
    }
}

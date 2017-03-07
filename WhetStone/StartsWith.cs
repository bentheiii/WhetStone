using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class startsWith
    {
        /// <summary>
        /// Get whether an <see cref="IEnumerable{T}"/> starts with another <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The complete <see cref="IEnumerable{T}"/>.</param>
        /// <param name="prefix">The prefix <see cref="IEnumerable{T}"/>.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to check for element equality. <see langword="null"/> for default.</param>
        /// <returns>Whether <paramref name="this"/> start with <paramref name="prefix"/>.</returns>
        public static bool StartsWith<T>(this IEnumerable<T> @this, IEnumerable<T> prefix, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;

            foreach (var p in @this.ZipUnBoundTuple(prefix))
            {
                if (p.Item2 == null)
                    return true;
                if (p.Item1 == null)
                    return false;
                if (!comp.Equals(p.Item1.Item1, p.Item2.Item1))
                    return false;
            }
            return true;
        }
    }
}

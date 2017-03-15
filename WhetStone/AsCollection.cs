using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class asCollection
    {
        /// <summary>
        /// Tries to cast or wrap <paramref name="this"/> in a collection adapter.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert to collection.</param>
        /// <param name="force">If this is set to <see langword="true"/>, and no suitable wrappings to <see cref="ICollection{T}"/> is found, a new <see cref="ICollection{T}"/> will be allocated and written to.</param>
        /// <returns>An <see cref="ICollection{T}"/> that contains <paramref name="this"/>'s elements, or <see langword="null"/> if the conversion was not successful.</returns>
        /// <remarks>
        /// <para>If <paramref name="force"/> is <see langword="true"/>, the return value will never be null.</para>
        /// <para>Without <paramref name="force"/> set to <see langword="true"/>, <paramref name="this"/> will only be wrapped if it is <see cref="ICollection{T}"/>, <see cref="IReadOnlyCollection{T}"/>, or <see cref="string"/></para>
        /// <para>If <paramref name="this"/> is <see cref="IReadOnlyCollection{T}"/> or <see cref="string"/>, the return value will be read-only.</para>
        /// </remarks>
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> @this, bool force = true)
        {
            @this.ThrowIfNull(nameof(@this));
            var l = @this as ICollection<T>;
            if (l != null)
                return l;
            var r = @this as IReadOnlyCollection<T>;
            if (r != null)
                return r.ToLockedCollection();
            var s = @this as string;
            if (s != null && typeof(T) == typeof(char))
                return (IList<T>)new LockedListStringAdaptor(s);
            return force ? @this.ToList() : null;
        }
    }
}

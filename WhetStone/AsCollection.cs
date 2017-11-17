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
        private class LockedCollectionWrapper<T> : LockedCollection<T>
        {
            private readonly ICollection<T> _int;
            public LockedCollectionWrapper(ICollection<T> i)
            {
                _int = i;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return _int.GetEnumerator();
            }
            public override int Count => _int.Count;
            public override bool Contains(T item)
            {
                return _int.Contains(item);
            }
            public override bool Equals(object obj)
            {
                return _int.Equals(obj);
            }
            public override int GetHashCode()
            {
                return _int.GetHashCode();
            }
            public override string ToString()
            {
                return _int.ToString();
            }
        }
        /// <summary>
        /// Tries to cast or wrap <paramref name="this"/> in a collection adapter.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert to collection.</param>
        /// <param name="force">If this is set to <see langword="true"/>, and no suitable wrappings to <see cref="ICollection{T}"/> is found, a new <see cref="ICollection{T}"/> will be allocated and written to.</param>
        /// <param name="ensureReadOnly">Whether to force the returned value to be immutable (or null, if <paramref name="force"/> is set to <see langword="false" />)</param>
        /// <returns>An <see cref="ICollection{T}"/> that contains <paramref name="this"/>'s elements, or <see langword="null"/> if the conversion was not successful.</returns>
        /// <remarks>
        /// <para>If <paramref name="force"/> is <see langword="true"/>, the return value will never be null.</para>
        /// <para>Without <paramref name="force"/> set to <see langword="true"/>, <paramref name="this"/> will only be wrapped if it is <see cref="ICollection{T}"/>, <see cref="IReadOnlyCollection{T}"/>, or <see cref="string"/></para>
        /// <para>If <paramref name="this"/> is <see cref="IReadOnlyCollection{T}"/> or <see cref="string"/>, the return value will be read-only.</para>
        /// </remarks>
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> @this, bool force = true, bool ensureReadOnly = false)
        {
            if (ensureReadOnly)
            {
                var ret = @this.AsCollection(force);
                if (ret != null && !ret.IsReadOnly)
                    ret = new LockedCollectionWrapper<T>(ret);
                return ret;
            }
            @this.ThrowIfNull(nameof(@this));
            switch (@this)
            {
                case ICollection<T> l:
                    return l;
                case IReadOnlyCollection<T> r:
                    return r.ToLockedCollection();
            }
#pragma warning disable IDE0019 // Use pattern matching
            var s = @this as string;
#pragma warning restore IDE0019 // Use pattern matching
            if (s != null && typeof(T) == typeof(char))
                return (IList<T>)new LockedListStringAdaptor(s);
            return force ? @this.ToList() : null;
        }
    }
}

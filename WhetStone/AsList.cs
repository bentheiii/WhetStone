using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class asList
    {
        private class LockedListWrapper<T> : LockedList<T>
        {
            private readonly IList<T> _int;
            public LockedListWrapper(IList<T> i)
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
            public override T this[int index]
            {
                get
                {
                    return _int[index];
                }
            }
            public override int IndexOf(T item)
            {
                return _int.IndexOf(item);
            }
        }

        /// <overloads>Attempt to adapt the input into an <see cref="IList{T}"/> with minimal memory overhead.</overloads>
        /// <summary>
        /// Tries to cast or wrap <paramref name="this"/> in a list adapter.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert to list.</param>
        /// <param name="force">If this is set to <see langword="true"/>, and no suitable wrappings to <see cref="IList{T}"/> is found, a new <see cref="IList{T}"/> will be allocated and written to.</param>
        /// <param name="ensureReadOnly">Whether to force the returned value to be immutable (or null, if <paramref name="force"/> is set to <see langword="false" />)</param>
        /// <returns>An <see cref="IList{T}"/> that contains <paramref name="this"/>'s elements, or <see langword="null"/> if the conversion was not successful.</returns>
        /// <remarks>
        /// <para>If <paramref name="force"/> is <see langword="true"/>, the return value will never be null.</para>
        /// <para>Without <paramref name="force"/> set to <see langword="true"/>, <paramref name="this"/> will only be wrapped if it is <see cref="IList{T}"/>, <see cref="IReadOnlyList{T}"/>, or <see cref="string"/></para>
        /// <para>If <paramref name="this"/> is <see cref="IReadOnlyList{T}"/> or <see cref="string"/>, the return value will be read-only.</para>
        /// </remarks>
        public static IList<T> AsList<T>(this IEnumerable<T> @this, bool force = true, bool ensureReadOnly = false)
        {
            if (ensureReadOnly)
            {
                var ret = @this.AsList(force);
                if (ret != null && !ret.IsReadOnly)
                    ret = new LockedListWrapper<T>(ret);
                return ret;
            }
            @this.ThrowIfNull(nameof(@this));
            switch (@this)
            {
                case IList<T> l:
                    return l;
                case IReadOnlyList<T> r:
                    return r.ToLockedList();
            }
#pragma warning disable IDE0019 // Use pattern matching
            var s = @this as string;
#pragma warning restore IDE0019 // Use pattern matching
            if (s != null && typeof(T) == typeof(char))
                return (IList<T>)new LockedListStringAdaptor(s);
            return force ? @this.ToList() : null;
        }
        /// <summary>
        /// Wraps the <see cref="BitArray"/> in a list wrapper.
        /// </summary>
        /// <param name="this">the <see cref="BitArray"/> to wrap.</param>
        /// <returns>A wrapping, partially read-only <see cref="IList{T}"/> of type <see cref="bool"/> (like a <see cref="Array"/> of type <see cref="bool"/>), with ability to add and remove from the end.(</returns>
        public static IList<bool> AsList(this BitArray @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new ListBitArrayAdaptor(@this);
        }
        /// <summary>
        /// Wraps the <see cref="BitVector32"/> in a list wrapper.
        /// </summary>
        /// <param name="this">the <see cref="BitVector32"/> to wrap.</param>
        /// <returns>A wrapping, partially read-only <see cref="IList{T}"/> of type <see cref="bool"/> (like a <see cref="Array"/> of type <see cref="bool"/>).(</returns>
        public static IList<bool> AsList(this BitVector32 @this)
        {
            return new ListBitVectorAdapter(@this);
        }
    }
}

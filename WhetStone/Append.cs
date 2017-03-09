using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class append
    {
        /// <overloads>Enlarges an array and adds elements to it.</overloads>
        /// <summary>
        /// Resizes an array and adds <paramref name="toAdd"/> to its end.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The array to append to.</param>
        /// <param name="toAdd">The <see cref="IEnumerable{T}"/> to copy to the <paramref name="this"/>.</param>
        /// <seealso cref="Append{T}(ref T[],IEnumerable{T})"/>
        public static void Append<T>(ref T[] @this, params T[] toAdd)
        {
            @this.ThrowIfNull(nameof(@this));
            toAdd.ThrowIfNull(nameof(toAdd));
            Append(ref @this, toAdd.AsEnumerable());
        }
        /// <summary>
        /// Resizes an array and adds <paramref name="toAdd"/> to its end.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The array to append to.</param>
        /// <param name="toAdd">The <see cref="IEnumerable{T}"/> to copy to the <paramref name="this"/>.</param>
        /// <remarks><para>The array will be mutated.</para><para>If <paramref name="toAdd"/> is a <see cref="ICollection{T}"/> or <see cref="IReadOnlyCollection{T}"/>, its <see cref="ICollection{T}.CopyTo"/> method will be called.</para>
        /// <para>If all the elements to add are <typeparamref name="T"/>'s default value, assigning can be skipped.</para></remarks>
        public static void Append<T>(ref T[] @this, IEnumerable<T> toAdd)
        {
            @this.ThrowIfNull(nameof(@this));
            toAdd.ThrowIfNull(nameof(toAdd));
            var oldlen = @this.Length;
            Array.Resize(ref @this, @this.Length + toAdd.Count());
            if (default(T).Enumerate().Concat(toAdd).AllEqual())
                return;
            var l = toAdd.AsCollection(false);
            if (l != null)
            {
                l.CopyTo(@this,oldlen);
            }
            else
            {
                foreach (var t in toAdd.CountBind(oldlen))
                {
                    @this[t.Item2] = t.Item1;
                }
            }
        }
    }
}

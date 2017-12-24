using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class toArray
    {
        /// <summary>
        /// Creates an <see cref="Array"/> and fills it with an <see cref="IEnumerable{T}"/>'s elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to take elements from.</param>
        /// <param name="capacity">The expected size of <paramref name="this"/>.</param>
        /// <param name="overflowPolicy">What to do when <paramref name="this"/> is larger than <paramref name="capacity"/> will allow.</param>
        /// <returns>An array with <paramref name="this"/>'s elements.</returns>
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, OverflowPolicy overflowPolicy = OverflowPolicy.Expand)
        {
            @this.ThrowIfNull(nameof(@this));
            capacity.ThrowIfAbsurd(nameof(capacity));
            if (@this is IList<T> l)
            {
                T[] ret;
                switch (overflowPolicy)
                {
                    case OverflowPolicy.Expand:
                        ret = new T[l.Count];
                        l.CopyTo(ret,0);
                        return ret;
                    case OverflowPolicy.End:
                        if (l.Count < capacity)
                        {
                            goto case OverflowPolicy.Expand;
                        }
                        else
                        {
                            ret = new T[capacity];
                            l.Take(capacity).CopyTo(ret, 0);
                            return ret;
                        }
                    case OverflowPolicy.Error:
                        if (l.Count > capacity)
                            throw new ArgumentException("capacity overflow");
                        goto case OverflowPolicy.Expand;
                    default:
                        throw new Exception();
                }
            }
            else
            {
                T[] ret = new T[capacity];
                int i = 0;
                foreach (T t in @this)
                {
                    if (ret.Length <= i)
                    {
                        switch (overflowPolicy)
                        {
                            case OverflowPolicy.End:
                                return ret;
                            case OverflowPolicy.Error:
                                throw new ArgumentException("capacity overflow");
                            case OverflowPolicy.Expand:
                                Array.Resize(ref ret, Math.Max(ret.Length * 2, 1));
                                break;
                        }
                    }
                    ret[i] = t;
                    i++;
                }
                Array.Resize(ref ret, i);
                return ret;
            }
        }
        /// <summary>
        /// Policy for when an <see cref="IEnumerable{T}"/> requires more space than alloted
        /// </summary>
        public enum OverflowPolicy
        {
            /// <summary>
            /// Throw an Exception.
            /// </summary>
            Error,
            /// <summary>
            /// Increase the alloted size.
            /// </summary>
            Expand,
            /// <summary>
            /// Return the <see cref="IEnumerable{T}"/> processed thus far, ignoring the rest.
            /// </summary>
            End
        }
    }
}

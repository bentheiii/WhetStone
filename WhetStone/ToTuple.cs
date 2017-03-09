using System;
using System.Collections;
using System.Collections.Generic;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace WhetStone.Tuples
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class toTuple
    {
        /// <summary>
        /// Convert an <see cref="IList{T}"/> into a tuple of 1 member.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1> ToTuple<T1>(this IList @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new Tuple<T1>((T1)@this[0]);
        }
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> into a tuple of 1 member.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1> ToTuple<T1>(this IEnumerable @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToObjArray().ToTuple<T1>();
        }
        /// <summary>
        /// Convert an <see cref="IList{T}"/> into a tuple of 2 members.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <typeparam name="T2">The type of the second tuple member.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1, T2> ToTuple<T1, T2>(this IList @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new Tuple<T1, T2>((T1)@this[0], (T2)@this[1]);
        }
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> into a tuple of 2 members.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <typeparam name="T2">The type of the second tuple member.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1, T2> ToTuple<T1, T2>(this IEnumerable @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToObjArray().ToTuple<T1, T2>();
        }
        /// <summary>
        /// Convert an <see cref="IList{T}"/> into a tuple of 3 members.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <typeparam name="T2">The type of the second tuple member.</typeparam>
        /// <typeparam name="T3">The type of the third tuple member.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(this IList @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new Tuple<T1, T2, T3>((T1)@this[0], (T2)@this[1], (T3)@this[2]);
        }
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> into a tuple of 3 members.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <typeparam name="T2">The type of the second tuple member.</typeparam>
        /// <typeparam name="T3">The type of the third tuple member.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(this IEnumerable @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToObjArray().ToTuple<T1, T2, T3>();
        }
        /// <summary>
        /// Convert an <see cref="IList{T}"/> into a tuple of 4 members.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <typeparam name="T2">The type of the second tuple member.</typeparam>
        /// <typeparam name="T3">The type of the third tuple member.</typeparam>
        /// <typeparam name="T4">The type of the fourth tuple member.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(this IList @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new Tuple<T1, T2, T3, T4>((T1)@this[0], (T2)@this[1], (T3)@this[2], (T4)@this[3]);
        }
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> into a tuple of 4 members.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <typeparam name="T2">The type of the second tuple member.</typeparam>
        /// <typeparam name="T3">The type of the third tuple member.</typeparam>
        /// <typeparam name="T4">The type of the fourth tuple member.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(this IEnumerable @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToObjArray().ToTuple<T1, T2, T3, T4>();
        }
        /// <summary>
        /// Convert an <see cref="IList{T}"/> into a tuple of 5 members.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <typeparam name="T2">The type of the second tuple member.</typeparam>
        /// <typeparam name="T3">The type of the third tuple member.</typeparam>
        /// <typeparam name="T4">The type of the fourth tuple member.</typeparam>
        /// <typeparam name="T5">The type of the fifth tuple member.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(this IList @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new Tuple<T1, T2, T3, T4, T5>((T1)@this[0], (T2)@this[1], (T3)@this[2], (T4)@this[3], (T5)@this[4]);
        }
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> into a tuple of 5 members.
        /// </summary>
        /// <typeparam name="T1">The type of the first tuple member.</typeparam>
        /// <typeparam name="T2">The type of the second tuple member.</typeparam>
        /// <typeparam name="T3">The type of the third tuple member.</typeparam>
        /// <typeparam name="T4">The type of the fourth tuple member.</typeparam>
        /// <typeparam name="T5">The type of the fifth tuple member.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to a tuple.</returns>
        public static Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(this IEnumerable @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToObjArray().ToTuple<T1, T2, T3, T4, T5>();
        }
    }
}

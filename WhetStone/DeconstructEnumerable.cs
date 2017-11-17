using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class deconstructEnumerable
    {
        /// <summary>
        /// Pop one element out of an <see cref="IEnumerator{T}"/> and insert it into out variable
        /// </summary>
        /// <param name="this">the <see cref="IEnumerator{T}"/> to deconstruct</param>
        /// <param name="mem1">the first element to pop into</param>
        /// <typeparam name="T">the type of the elements</typeparam>
        public static void Deconstruct<T>(this IEnumerator<T> @this, out T mem1)
        {
            @this.ThrowIfNull(nameof(@this));
            if (!@this.MoveNext())
                throw new ArgumentException("IEnumerator ended unexpectedly");
            mem1 = @this.Current;
        }
        /// <summary>
        /// Pop elements out of an <see cref="IEnumerator{T}"/> and insert it into out variable
        /// </summary>
        /// <param name="this">the <see cref="IEnumerator{T}"/> to deconstruct</param>
        /// <param name="mem1">the first element to pop into</param>
        /// <param name="mem2">the second element to pop into</param>
        /// <typeparam name="T">the type of the elements</typeparam>
        public static void Deconstruct<T>(this IEnumerator<T> @this, out T mem1, out T mem2)
        {
            @this.Deconstruct(out mem1);
            @this.Deconstruct(out mem2);
        }
        /// <summary>
        /// Pop elements out of an <see cref="IEnumerator{T}"/> and insert it into out variable
        /// </summary>
        /// <param name="this">the <see cref="IEnumerator{T}"/> to deconstruct</param>
        /// <param name="mem1">the first element to pop into</param>
        /// <param name="mem2">the second element to pop into</param>
        /// <param name="mem3">the third element to pop into</param>
        /// <typeparam name="T">the type of the elements</typeparam>
        public static void Deconstruct<T>(this IEnumerator<T> @this, out T mem1, out T mem2, out T mem3)
        {
            @this.Deconstruct(out mem1);
            (mem2, mem3) = @this;
        }
        /// <summary>
        /// Pop elements out of an <see cref="IEnumerator{T}"/> and insert it into out variable
        /// </summary>
        /// <param name="this">the <see cref="IEnumerator{T}"/> to deconstruct</param>
        /// <param name="mem1">the first element to pop into</param>
        /// <param name="mem2">the second element to pop into</param>
        /// <param name="mem3">the third element to pop into</param>
        /// <param name="mem4">the fourth element to pop into</param>
        /// <typeparam name="T">the type of the elements</typeparam>
        public static void Deconstruct<T>(this IEnumerator<T> @this, out T mem1, out T mem2, out T mem3,
            out T mem4)
        {
            @this.Deconstruct(out mem1);
            (mem2, mem3, mem4) = @this;
        }
        /// <summary>
        /// Pop elements out of an <see cref="IEnumerator{T}"/> and insert it into out variable
        /// </summary>
        /// <param name="this">the <see cref="IEnumerator{T}"/> to deconstruct</param>
        /// <param name="mem1">the first element to pop into</param>
        /// <param name="mem2">the second element to pop into</param>
        /// <param name="mem3">the third element to pop into</param>
        /// <param name="mem4">the fourth element to pop into</param>
        /// <param name="mem5">the fifth element to pop into</param>
        /// <typeparam name="T">the type of the elements</typeparam>
        public static void Deconstruct<T>(this IEnumerator<T> @this, out T mem1, out T mem2, out T mem3,
            out T mem4, out T mem5)
        {
            @this.Deconstruct(out mem1);
            (mem2, mem3, mem4, mem5) = @this;
        }
        /// <summary>
        /// Pop elements out of an <see cref="IEnumerator{T}"/> and insert it into out variable
        /// </summary>
        /// <param name="this">the <see cref="IEnumerator{T}"/> to deconstruct</param>
        /// <param name="mem1">the first element to pop into</param>
        /// <param name="mem2">the second element to pop into</param>
        /// <param name="mem3">the third element to pop into</param>
        /// <param name="mem4">the fourth element to pop into</param>
        /// <param name="mem5">the fifth element to pop into</param>
        /// <param name="mem6">the sixth element to pop into</param>
        /// <typeparam name="T">the type of the elements</typeparam>
        public static void Deconstruct<T>(this IEnumerator<T> @this, out T mem1, out T mem2, out T mem3,
            out T mem4, out T mem5, out T mem6)
        {
            @this.Deconstruct(out mem1);
            (mem2, mem3, mem4, mem5, mem6) = @this;
        }
        /// <summary>
        /// Pop elements out of an <see cref="IEnumerator{T}"/> and insert it into out variable
        /// </summary>
        /// <param name="this">the <see cref="IEnumerator{T}"/> to deconstruct</param>
        /// <param name="mem1">the first element to pop into</param>
        /// <param name="mem2">the second element to pop into</param>
        /// <param name="mem3">the third element to pop into</param>
        /// <param name="mem4">the fourth element to pop into</param>
        /// <param name="mem5">the fifth element to pop into</param>
        /// <param name="mem6">the sixth element to pop into</param>
        /// <param name="mem7">the seventh element to pop into</param>
        /// <typeparam name="T">the type of the elements</typeparam>
        public static void Deconstruct<T>(this IEnumerator<T> @this, out T mem1, out T mem2, out T mem3,
            out T mem4, out T mem5, out T mem6, out T mem7)
        {
            @this.Deconstruct(out mem1);
            (mem2, mem3, mem4, mem5, mem6, mem7) = @this;
        }

        /// <summary>
        /// deconstruct an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to deconstruct</param>
        /// <param name="mem1">the first memeber of the <paramref name="this"/></param>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <exception cref="ArgumentException">If <paramref name="this"/> is the wrong size</exception>
        public static void Deconstruct<T>(this IEnumerable<T> @this, out T mem1)
        {
            using (var tor = @this.GetEnumerator())
            {
                tor.Deconstruct(out mem1);
                if (tor.MoveNext())
                    throw new ArgumentException("too many elements to deconstruct");
            }
        }
        /// <summary>
        /// deconstruct an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to deconstruct</param>
        /// <param name="mem1">the first memeber of the <paramref name="this"/></param>
        /// <param name="mem2">the second memeber of the <paramref name="this"/></param>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <exception cref="ArgumentException">If <paramref name="this"/> is the wrong size</exception>
        public static void Deconstruct<T>(this IEnumerable<T> @this, out T mem1, out T mem2)
        {
            using (var tor = @this.GetEnumerator())
            {
                (mem1, mem2) = tor;
                if (tor.MoveNext())
                    throw new ArgumentException("too many elements to deconstruct");
            }
        }
        /// <summary>
        /// deconstruct an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to deconstruct</param>
        /// <param name="mem1">the first memeber of the <paramref name="this"/></param>
        /// <param name="mem2">the second memeber of the <paramref name="this"/></param>
        /// <param name="mem3">the third memeber of the <paramref name="this"/></param>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <exception cref="ArgumentException">If <paramref name="this"/> is the wrong size</exception>
        public static void Deconstruct<T>(this IEnumerable<T> @this, out T mem1, out T mem2, out T mem3)
        {
            using (var tor = @this.GetEnumerator())
            {
                (mem1, mem2, mem3) = tor;
                if (tor.MoveNext())
                    throw new ArgumentException("too many elements to deconstruct");
            }
        }
        /// <summary>
        /// deconstruct an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to deconstruct</param>
        /// <param name="mem1">the first memeber of the <paramref name="this"/></param>
        /// <param name="mem2">the second memeber of the <paramref name="this"/></param>
        /// <param name="mem3">the third memeber of the <paramref name="this"/></param>
        /// <param name="mem4">the fourth memeber of the <paramref name="this"/></param>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <exception cref="ArgumentException">If <paramref name="this"/> is the wrong size</exception>
        public static void Deconstruct<T>(this IEnumerable<T> @this, out T mem1, out T mem2, out T mem3,
            out T mem4)
        {
            using (var tor = @this.GetEnumerator())
            {
                (mem1, mem2, mem3, mem4) = tor;
                if (tor.MoveNext())
                    throw new ArgumentException("too many elements to deconstruct");
            }
        }
        /// <summary>
        /// deconstruct an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to deconstruct</param>
        /// <param name="mem1">the first memeber of the <paramref name="this"/></param>
        /// <param name="mem2">the second memeber of the <paramref name="this"/></param>
        /// <param name="mem3">the third memeber of the <paramref name="this"/></param>
        /// <param name="mem4">the fourth memeber of the <paramref name="this"/></param>
        /// <param name="mem5">the fifth memeber of the <paramref name="this"/></param>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <exception cref="ArgumentException">If <paramref name="this"/> is the wrong size</exception>
        public static void Deconstruct<T>(this IEnumerable<T> @this, out T mem1, out T mem2, out T mem3,
            out T mem4, out T mem5)
        {
            using (var tor = @this.GetEnumerator())
            {
                (mem1, mem2, mem3, mem4, mem5) = tor;
                if (tor.MoveNext())
                    throw new ArgumentException("too many elements to deconstruct");
            }
        }
        /// <summary>
        /// deconstruct an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to deconstruct</param>
        /// <param name="mem1">the first memeber of the <paramref name="this"/></param>
        /// <param name="mem2">the second memeber of the <paramref name="this"/></param>
        /// <param name="mem3">the third memeber of the <paramref name="this"/></param>
        /// <param name="mem4">the fourth memeber of the <paramref name="this"/></param>
        /// <param name="mem5">the fifth memeber of the <paramref name="this"/></param>
        /// <param name="mem6">the sixth memeber of the <paramref name="this"/></param>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <exception cref="ArgumentException">If <paramref name="this"/> is the wrong size</exception>
        public static void Deconstruct<T>(this IEnumerable<T> @this, out T mem1, out T mem2, out T mem3,
            out T mem4, out T mem5, out T mem6)
        {
            using (var tor = @this.GetEnumerator())
            {
                (mem1, mem2, mem3, mem4, mem5, mem6) = tor;
                if (tor.MoveNext())
                    throw new ArgumentException("too many elements to deconstruct");
            }
        }
        /// <summary>
        /// deconstruct an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to deconstruct</param>
        /// <param name="mem1">the first memeber of the <paramref name="this"/></param>
        /// <param name="mem2">the second memeber of the <paramref name="this"/></param>
        /// <param name="mem3">the third memeber of the <paramref name="this"/></param>
        /// <param name="mem4">the fourth memeber of the <paramref name="this"/></param>
        /// <param name="mem5">the fifth memeber of the <paramref name="this"/></param>
        /// <param name="mem6">the sixth memeber of the <paramref name="this"/></param>
        /// <param name="mem7">the seventh memeber of the <paramref name="this"/></param>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <exception cref="ArgumentException">If <paramref name="this"/> is the wrong size</exception>
        public static void Deconstruct<T>(this IEnumerable<T> @this, out T mem1, out T mem2, out T mem3,
            out T mem4, out T mem5, out T mem6, out T mem7)
        {
            using (var tor = @this.GetEnumerator())
            {
                (mem1, mem2, mem3, mem4, mem5, mem6, mem7) = tor;
                if (tor.MoveNext())
                    throw new ArgumentException("too many elements to deconstruct");
            }
        }
    }
}

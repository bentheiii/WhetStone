using System;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class countBind
    {
        /// <overloads>Attaches indices to elements of enumerables.</overloads>
        /// <summary>
        /// Attaches indices to elements of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="a">The <see cref="IEnumerable{T}"/> to attach to.</param>
        /// <param name="start">The initial index to count from.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2}"/>, the second element of which is the index.</returns>
        public static IEnumerable<Tuple<T, int>> CountBind<T>(this IEnumerable<T> a, int start = 0)
        {
            return a.Zip(countUp.CountUp(start));
        }
        /// <summary>
        /// Attaches indices to elements of an <see cref="IEnumerable{T}"/>. Indexes are of any type and use fielding to increment.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <typeparam name="C">The type of the index to attach.</typeparam>
        /// <param name="a">The <see cref="IEnumerable{T}"/> to attach to.</param>
        /// <param name="start">The initial index to count from.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2}"/>, the second element of which is the index.</returns>
        /// <remarks>This function uses fielding to increment the index.</remarks>
        public static IEnumerable<Tuple<T, C>> CountBind<T, C>(this IEnumerable<T> a, C start)
        {
            return a.Zip(countUp.CountUp(start));
        }
        private class CountBindCollection<T> : LockedCollection<Tuple<T,int>>
        {
            private readonly ICollection<T> _source;
            private readonly int _start;
            public CountBindCollection(ICollection<T> source, int start)
            {
                _source = source;
                _start = start;
            }
            public override IEnumerator<Tuple<T, int>> GetEnumerator()
            {
                return _source.Zip(countUp.CountUp(_start)).GetEnumerator();
            }
            public override int Count => _source.Count;
        }
        private class CountBindCollection<T,G> : LockedCollection<Tuple<T, G>>
        {
            private readonly ICollection<T> _source;
            private readonly G _start;
            public CountBindCollection(ICollection<T> source, G start)
            {
                _source = source;
                _start = start;
            }
            public override IEnumerator<Tuple<T, G>> GetEnumerator()
            {
                return _source.Zip(countUp.CountUp(_start)).GetEnumerator();
            }
            public override int Count => _source.Count;
        }
        /// <summary>
        /// Attaches indices to elements of an <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ICollection{T}"/></typeparam>
        /// <param name="a">The <see cref="ICollection{T}"/> to attach to.</param>
        /// <param name="start">The initial index to count from.</param>
        /// <returns>An <see cref="ICollection{T}"/> of <see cref="Tuple{T1,T2}"/>, the second element of which is the index.</returns>
        public static ICollection<Tuple<T, int>> CountBind<T>(this ICollection<T> a, int start = 0)
        {
            return new CountBindCollection<T>(a,start);
        }
        /// <summary>
        /// Attaches indices to elements of an <see cref="ICollection{T}"/>. Indexes are of any type and use fielding to increment.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ICollection{T}"/></typeparam>
        /// <typeparam name="C">The type of the index to attach.</typeparam>
        /// <param name="a">The <see cref="ICollection{T}"/> to attach to.</param>
        /// <param name="start">The initial index to count from.</param>
        /// <returns>An <see cref="ICollection{T}"/> of <see cref="Tuple{T1,T2}"/>, the second element of which is the index.</returns>
        /// <remarks>This function uses fielding to increment the index.</remarks>
        public static ICollection<Tuple<T, C>> CountBind<T, C>(this ICollection<T> a, C start)
        {
            return new CountBindCollection<T,C>(a, start);
        }
        /// <summary>
        /// Attaches indices to elements of an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/></typeparam>
        /// <param name="a">The <see cref="IList{T}"/> to attach to.</param>
        /// <param name="start">The initial index to count from.</param>
        /// <returns>An <see cref="IList{T}"/> of <see cref="Tuple{T1,T2}"/>, the second element of which is the index.</returns>
        public static IList<Tuple<T, int>> CountBind<T>(this IList<T> a, int start = 0)
        {
            return a.Zip(countUp.CountUp(start));
        }
        /// <summary>
        /// Attaches indices to elements of an <see cref="IList{T}"/>. Indexes are of any type and use fielding to increment.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/></typeparam>
        /// <typeparam name="C">The type of the index to attach.</typeparam>
        /// <param name="a">The <see cref="IList{T}"/> to attach to.</param>
        /// <param name="start">The initial index to count from.</param>
        /// <returns>An <see cref="IList{T}"/> of <see cref="Tuple{T1,T2}"/>, the second element of which is the index.</returns>
        /// <remarks>This function uses fielding to increment the index.</remarks>
        public static IList<Tuple<T, C>> CountBind<T, C>(this IList<T> a, C start)
        {
            return a.Zip(countUp.CountUp(start));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;
using WhetStone.Tuples;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class zip
    {
        private class ListZip<T> : LockedList<IList<T>>
        {
            private readonly IList<IList<T>> _sources;
            public ListZip(IList<IList<T>> sources)
            {
                _sources = sources;
            }
            public override IEnumerator<IList<T>> GetEnumerator()
            {
                var tor = _sources.Select(a => a.GetEnumerator()).ToArray();
                while (tor.All(a => a.MoveNext()))
                {
                    yield return tor.Select(a => a.Current);
                }
            }
            public override int Count
            {
                get
                {
                    return _sources.Min(a => a.Count);
                }
            }
            public override IList<T> this[int index]
            {
                get
                {
                    return _sources.Select(a => a[index]);
                }
            }
        }
        private class ListZip : LockedList<IList>
        {
            private readonly IList<IList> _sources;
            public ListZip(IList<IList> sources)
            {
                _sources = sources;
            }
            public override IEnumerator<IList> GetEnumerator()
            {
                var tor = _sources.Select(a => a.GetEnumerator()).ToArray();
                while (tor.All(a => a.MoveNext()))
                {
                    yield return tor.Select(a => a.Current).ToGeneral();
                }
            }
            public override int Count
            {
                get
                {
                    return _sources.Min(a => a.Count);
                }
            }
            public override IList this[int index]
            {
                get
                {
                    return _sources.Select(a => a[index]).ToGeneral();
                }
            }
        }
        /// <overloads>Transposes an enumerable of enumerables.</overloads>
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T">The types of the <see cref="IEnumerable{T}"/>s</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> of <see cref="IEnumerable{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IEnumerable<IEnumerable<T>> Zip<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            var tor = @this.Select(a => a.GetEnumerator()).ToArray();
            while (tor.All(a => a.MoveNext()))
            {
                yield return tor.Select(a => a.Current);
            }
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T">The types of the <see cref="IList{T}"/>s</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> of <see cref="IList{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IList<IList<T>> Zip<T>(this IList<IList<T>> @this)
        {
            return new ListZip<T>(@this);
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable"/>s spliced together.
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable"/> of <see cref="IEnumerable{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IEnumerable<IEnumerable> Zip(this IEnumerable<IEnumerable> @this)
        {
            var tor = @this.Select(a => a.GetEnumerator()).ToArray();
            while (tor.All(a => a.MoveNext()))
            {
                yield return tor.Select(a => a.Current);
            }
        }
        /// <summary>
        /// Get all the elements in <see cref="IList"/>s spliced together.
        /// </summary>
        /// <param name="this">The <see cref="IList"/> of <see cref="IList"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IList<IList> Zip(this IList<IList> @this)
        {
            return new ListZip(@this);
        }

        /// <summary>
        /// Get all the elements in two <see cref="IEnumerable{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="b">The second <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="IEnumerable{T}"/>s, spliced together.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IEnumerable<Tuple<T1, T2>> Zip<T1, T2>(this IEnumerable<T1> a, IEnumerable<T2> b)
        {
            return Zip(new IEnumerable[] { a, b }).Select(x => x.ToTuple<T1, T2>());
        }
        /// <summary>
        /// Get all the elements in three <see cref="IEnumerable{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="b">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="c">The third <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="IEnumerable{T}"/>s, spliced together.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IEnumerable<Tuple<T1, T2, T3>> Zip<T1, T2, T3>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c)
        {
            return Zip(new IEnumerable[] { a, b, c }).Select(x => x.ToTuple<T1, T2, T3>());
        }
        /// <summary>
        /// Get all the elements in four <see cref="IEnumerable{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="b">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="c">The third <see cref="IEnumerable{T}"/>.</param>
        /// <param name="d">The fourth <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="IEnumerable{T}"/>s, spliced together.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IEnumerable<Tuple<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c, IEnumerable<T4> d)
        {
            return Zip(new IEnumerable[] { a, b, c, d }).Select(x => x.ToTuple<T1, T2, T3, T4>());
        }
        /// <summary>
        /// Get all the elements in five <see cref="IEnumerable{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="b">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="c">The third <see cref="IEnumerable{T}"/>.</param>
        /// <param name="d">The fourth <see cref="IEnumerable{T}"/>.</param>
        /// <param name="e">The fifth <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The <see cref="IEnumerable{T}"/>s, spliced together.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c, IEnumerable<T4> d, IEnumerable<T5> e)
        {
            return Zip(new IEnumerable[] { a, b, c, d, e }).Select(x => x.ToTuple<T1, T2, T3, T4, T5>());
        }

        /// <summary>
        /// Get all the elements in two <see cref="IList{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IList{T}"/>.</param>
        /// <param name="b">The second <see cref="IList{T}"/>.</param>
        /// <returns>The <see cref="IList{T}"/>s, spliced together.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IList<Tuple<T1, T2>> Zip<T1, T2>(this IList<T1> a, IList<T2> b)
        {
            return Zip(new[] { a.ToGeneral(), b.ToGeneral() }).Select(x => x.ToTuple<T1, T2>());
        }
        /// <summary>
        /// Get all the elements in three <see cref="IList{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IList{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IList{T}"/>.</param>
        /// <param name="b">The second <see cref="IList{T}"/>.</param>
        /// <param name="c">The third <see cref="IList{T}"/>.</param>
        /// <returns>The <see cref="IList{T}"/>s, spliced together.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IList<Tuple<T1, T2,T3>> Zip<T1, T2, T3>(this IList<T1> a, IList<T2> b, IList<T3> c)
        {
            return Zip(new[] { a.ToGeneral(), b.ToGeneral(), c.ToGeneral()}).Select(x => x.ToTuple<T1, T2,T3>());
        }
        /// <summary>
        /// Get all the elements in four <see cref="IList{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth <see cref="IList{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IList{T}"/>.</param>
        /// <param name="b">The second <see cref="IList{T}"/>.</param>
        /// <param name="c">The third <see cref="IList{T}"/>.</param>
        /// <param name="d">The fourth <see cref="IList{T}"/>.</param>
        /// <returns>The <see cref="IList{T}"/>s, spliced together.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IList<Tuple<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(this IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d)
        {
            return Zip(new[] { a.ToGeneral(), b.ToGeneral(), c.ToGeneral(), d.ToGeneral() }).Select(x => x.ToTuple<T1, T2, T3, T4>());
        }
        /// <summary>
        /// Get all the elements in five <see cref="IList{T}"/>s spliced together.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth <see cref="IList{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IList{T}"/>.</param>
        /// <param name="b">The second <see cref="IList{T}"/>.</param>
        /// <param name="c">The third <see cref="IList{T}"/>.</param>
        /// <param name="d">The fourth <see cref="IList{T}"/>.</param>
        /// <param name="e">The fifth <see cref="IList{T}"/>.</param>
        /// <returns>The <see cref="IList{T}"/>s, spliced together.</returns>
        /// <remarks>The result is only as long as the shortest input</remarks>
        public static IList<Tuple<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(this IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e)
        {
            return Zip(new[] { a.ToGeneral(), b.ToGeneral(), c.ToGeneral(), d.ToGeneral(), e.ToGeneral() }).Select(x => x.ToTuple<T1, T2, T3, T4, T5>());
        }
    }
}

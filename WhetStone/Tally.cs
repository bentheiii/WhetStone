using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Tuples;

namespace WhetStone.Looping
{
    /// <summary>
    /// A Tallier, that can tally for a specific purpose.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> to tally.</typeparam>
    public interface ITallier<in T>
    {
        /// <summary>
        /// Get an instance of a tallier for a tallying run.
        /// </summary>
        /// <returns>A new <see cref="ITalliator{T}"/>.</returns>
        ITalliator<T> GetTalliaror();
    }
    /// <summary>
    /// A specific instance of a specific tallying job.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> to tally.</typeparam>
    public interface ITalliator<in T>
    {
        /// <summary>
        /// Introduces a new value into the tally.
        /// </summary>
        /// <param name="val">The value to tally.</param>
        /// <returns>Whether or not to end all tallies.</returns>
        bool next(T val);
        /// <summary>
        /// End the tally, and return the result.
        /// </summary>
        /// <returns>The result of the tally.</returns>
        object end();
    }
    //todo type specific
    //todo select and ignore tallies
    /// <summary>
    /// A container of talliers.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IEnumerable{T}"/> to tally.</typeparam>
    public class Tally<T>
    {
        private readonly IEnumerable<T> _src;
        private readonly List<ITallier<T>> _talliers = new List<ITallier<T>>();
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="src">The default <see cref="IEnumerable{T}"/> to tally in case none are provided. <see langword="null"/> for no default.</param>
        public Tally(IEnumerable<T> src = null)
        {
            _src = src;
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the tally.
        /// </summary>
        /// <param name="tallier">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The <see cref="Tally{T}"/>, to allow easy piping.</returns>
        public Tally<T> Add(ITallier<T> tallier)
        {
            _talliers.Add(tallier);
            return this;
        }
        /// <summary>
        /// Begins the tally, tallying all the <see cref="ITallier{T}"/>s added until the <see cref="IEnumerable{T}"/> is done or any of the <see cref="ITallier{T}"/>s have broken.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to tally, or <see langword="null"/> for default.</param>
        /// <returns>An array with all the results of the tallies.</returns>
        public object[] Do(IEnumerable<T> source = null)
        {
            source = source ?? _src;
            var tors = _talliers.Select(a=>a.GetTalliaror()).ToArray();
            foreach (T s in source)
            {
                if (tors.Any(t => !t.next(s)))
                    break;
            }
            return tors.Select(a => a.end()).ToArray();
        }
    }
    /// <summary>
    /// static container for extension methods.
    /// </summary>
    public static class TallierExtensions
    {
        /// <summary>
        /// Create a <see cref="Tally{T}"/> using an <see cref="IEnumerable{T}"/> as a default source.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The default source.</param>
        /// <returns>A new <see cref="Tally{T}"/> with <paramref name="this"/> as the default source.</returns>
        public static Tally<T> Tally<T>(this IEnumerable<T> @this)
        {
            return new Tally<T>(@this);
        }

        /// <summary>
        /// Add an aggregate to a <see cref="Tally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Tally{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the aggregated value.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static Tally<T> TallyAggregate<T,R>(this Tally<T> @this, Func<T, R, R> func, R seed = default(R), Func<R, bool> @break = null)
        {
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.Add(toadd);
        }
        /// <summary>
        /// Add a count to a <see cref="Tally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Tally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to add to.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static Tally<T> TallyCount<T>(this Tally<T> @this)
        {
            return @this.TallyAggregate((_, a) => a + 1, 0);
        }
        /// <summary>
        /// Add a count to a <see cref="Tally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Tally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static Tally<T> TallyCount<T>(this Tally<T> @this, Func<T, bool> cond)
        {
            return @this.TallyAggregate((v, a) => cond(v) ? a + 1 : a, 0);
        }
        /// <summary>
        /// Add an any to a <see cref="Tally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Tally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static Tally<T> TallyAny<T>(this Tally<T> @this, Func<T, bool> cond, bool @break = false)
        {
            return @this.TallyAggregate((v, a) => a || cond(v), false, @break ? a => a : (Func<bool, bool>)null);
        }
        /// <summary>
        /// Add an any to a <see cref="Tally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Tally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to add to.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static Tally<T> TallyAny<T>(this Tally<T> @this, bool @break = false)
        {
            return @this.TallyAggregate((v, a) => true, false, @break ? a => a : (Func<bool, bool>)null);
        }
        /// <summary>
        /// Add an all to a <see cref="Tally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Tally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found to return false.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static Tally<T> TallyAll<T>(this Tally<T> @this, Func<T, bool> cond, bool @break = false)
        {
            return @this.TallyAggregate((v, a) => a && cond(v), true, @break ? a => !a : (Func<bool, bool>)null);
        }

        /// <summary>
        /// Tally and convert the result to a type.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Tally{T}"/>.</typeparam>
        /// <typeparam name="T0">The type of the first result.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to tally.</param>
        /// <param name="source">The source to use, or <see langword="null"/> for <paramref name="this"/>'s default.</param>
        /// <returns>The first result of the tally.</returns>
        public static T0 Do<T, T0>(this Tally<T> @this, IEnumerable<T> source = null)
        {
            return (T0)@this.Do(source)[0];
        }
        /// <summary>
        /// Tally and convert the result to a tuple.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Tally{T}"/>.</typeparam>
        /// <typeparam name="T0">The type of the first result.</typeparam>
        /// <typeparam name="T1">The type of the second result.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to tally.</param>
        /// <param name="source">The source to use, or <see langword="null"/> for <paramref name="this"/>'s default.</param>
        /// <returns>The first results of the tally.</returns>
        public static Tuple<T0,T1> Do<T, T0, T1>(this Tally<T> @this, IEnumerable<T> source = null)
        {
            return @this.Do(source).ToTuple<T0,T1>();
        }
        /// <summary>
        /// Tally and convert the result to a tuple.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Tally{T}"/>.</typeparam>
        /// <typeparam name="T0">The type of the first result.</typeparam>
        /// <typeparam name="T1">The type of the second result.</typeparam>
        /// <typeparam name="T2">The type of the third result.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to tally.</param>
        /// <param name="source">The source to use, or <see langword="null"/> for <paramref name="this"/>'s default.</param>
        /// <returns>The first results of the tally.</returns>
        public static Tuple<T0, T1, T2> Do<T, T0, T1, T2>(this Tally<T> @this, IEnumerable<T> source = null)
        {
            return @this.Do(source).ToTuple<T0, T1, T2>();
        }
        /// <summary>
        /// Tally and convert the result to a tuple.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Tally{T}"/>.</typeparam>
        /// <typeparam name="T0">The type of the first result.</typeparam>
        /// <typeparam name="T1">The type of the second result.</typeparam>
        /// <typeparam name="T2">The type of the third result.</typeparam>
        /// <typeparam name="T3">The type of the fourth result.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to tally.</param>
        /// <param name="source">The source to use, or <see langword="null"/> for <paramref name="this"/>'s default.</param>
        /// <returns>The first results of the tally.</returns>
        public static Tuple<T0, T1, T2, T3> Do<T, T0, T1, T2, T3>(this Tally<T> @this, IEnumerable<T> source = null)
        {
            return @this.Do(source).ToTuple<T0, T1, T2, T3>();
        }
        /// <summary>
        /// Tally and convert the result to a tuple.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Tally{T}"/>.</typeparam>
        /// <typeparam name="T0">The type of the first result.</typeparam>
        /// <typeparam name="T1">The type of the second result.</typeparam>
        /// <typeparam name="T2">The type of the third result.</typeparam>
        /// <typeparam name="T3">The type of the fourth result.</typeparam>
        /// <typeparam name="T4">The type of the fifth result.</typeparam>
        /// <param name="this">The <see cref="Tally{T}"/> to tally.</param>
        /// <param name="source">The source to use, or <see langword="null"/> for <paramref name="this"/>'s default.</param>
        /// <returns>The first results of the tally.</returns>
        public static Tuple<T0, T1, T2, T3, T4> Do<T, T0, T1, T2, T3, T4>(this Tally<T> @this, IEnumerable<T> source = null)
        {
            return @this.Do(source).ToTuple<T0, T1, T2, T3, T4>();
        }
    }
    internal class TallierAggregateBreakable<T, R> : ITallier<T>
    {
        private class TallerAggBreakableTor : ITalliator<T>
        {
            private R _ret;
            private readonly TallierAggregateBreakable<T,R> _upper;
            public TallerAggBreakableTor(R ret, TallierAggregateBreakable<T, R> upper)
            {
                _ret = ret;
                _upper = upper;
            }
            public bool next(T val)
            {
                _ret = _upper._func(val, _ret);
                return !_upper._break(_ret);
            }
            public object end()
            {
                return _ret;
            }
        }
        private readonly R _seed;
        private readonly Func<T, R, R> _func;
        private readonly Func<R, bool> _break;
        public TallierAggregateBreakable(R seed, Func<T, R, R> func, Func<R, bool> @break)
        {
            _seed = seed;
            _func = func;
            _break = @break;
        }
        public ITalliator<T> GetTalliaror()
        {
            return new TallerAggBreakableTor(_seed,this);
        }
    }
    internal class TallierAggregate<T, R> : ITallier<T>
    {
        private class TallerAggTor : ITalliator<T>
        {
            private R _ret;
            private readonly TallierAggregate<T, R> _upper;
            public TallerAggTor(R ret, TallierAggregate<T, R> upper)
            {
                _ret = ret;
                _upper = upper;
            }
            public bool next(T val)
            {
                _ret = _upper._func(val, _ret);
                return true;
            }
            public object end()
            {
                return _ret;
            }
        }
        private readonly R _seed;
        private readonly Func<T, R, R> _func;
        public TallierAggregate(R seed, Func<T, R, R> func)
        {
            _seed = seed;
            _func = func;
        }
        public ITalliator<T> GetTalliaror()
        {
            return new TallerAggTor(_seed, this);
        }
    }
}

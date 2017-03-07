using System;
using System.Collections.Generic;
using System.Linq;

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
    /// <summary>
    /// A container of talliers.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IEnumerable{T}"/> to tally.</typeparam>
    public class GenericTally<T>
    {
        private readonly IEnumerable<T> _src;
        private readonly List<Tuple<ITallier<T>,bool>> _talliers = new List<Tuple<ITallier<T>, bool>>();
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="src">The default <see cref="IEnumerable{T}"/> to tally in case none are provided. <see langword="null"/> for no default.</param>
        public GenericTally(IEnumerable<T> src = null)
        {
            _src = src;
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the tally.
        /// </summary>
        /// <param name="tallier">The <see cref="ITallier{T}"/> to add.</param>
        /// <param name="append">Whether to add the aggregate result to the tally result.</param>
        /// <returns>The <see cref="GenericTally{T}"/>, to allow easy piping.</returns>
        public GenericTally<T> Add(ITallier<T> tallier, bool append = true)
        {
            _talliers.Add(Tuple.Create(tallier, append));
            return this;
        }
        /// <summary>
        /// Perform the tally, tallying all the <see cref="ITallier{T}"/>s added until the <see cref="IEnumerable{T}"/> is done or any of the <see cref="ITallier{T}"/>s have broken.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to tally, or <see langword="null"/> for default.</param>
        /// <returns>An array with all the results of the tallies.</returns>
        public object[] Do(IEnumerable<T> source = null)
        {
            source = source ?? _src;
            var tors = _talliers.Select(a=>a.Item1.GetTalliaror()).ToArray();
            foreach (T s in source)
            {
                if (tors.Any(t => !t.next(s)))
                    break;
            }
            return tors.Zip(_talliers).Where(a=>a.Item2.Item2).Select(a => a.Item1.end()).ToArray();
        }
    }
    /// <summary>
    /// static container for extension methods.
    /// </summary>
    public static partial class TallierExtensions
    {
        /// <summary>
        /// Add an aggregate to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the aggregated value.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <param name="append">Whether to add the aggregate result to the tally result.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAggregate<T,R>(this GenericTally<T> @this, Func<T, R, R> func, R seed = default(R), Func<R, bool> @break = null, bool append = true)
        {
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.Add(toadd, append);
        }
        /// <summary>
        /// Add an aggregate to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <typeparam name="A">The type of the aggregated value.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="select">The selector function to apply to the aggregate result and add to the tally result.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAggregateSelect<T, A, R>(this GenericTally<T> @this, Func<T, A, A> func, A seed , Func<A,R> select, Func<A, bool> @break = null)
        {
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregateSelect<T, A, R>(seed, func, select);
            else
                toadd = new TallierAggregateBreakableSelect<T, A, R>(seed, func, @break, select);
            return @this.Add(toadd);
        }
        /// <summary>
        /// Add a count to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyCount<T>(this GenericTally<T> @this)
        {
            return @this.TallyAggregate((_, a) => a + 1, 0);
        }
        /// <summary>
        /// Add a count to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyCount<T>(this GenericTally<T> @this, Func<T, bool> cond)
        {
            return @this.TallyAggregate((v, a) => cond(v) ? a + 1 : a, 0);
        }
        /// <summary>
        /// Add an any to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAny<T>(this GenericTally<T> @this, Func<T, bool> cond, bool @break = false)
        {
            return @this.TallyAggregate((v, a) => a || cond(v), false, @break ? a => a : (Func<bool, bool>)null);
        }
        /// <summary>
        /// Add an any to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAny<T>(this GenericTally<T> @this, bool @break = false)
        {
            return @this.TallyAggregate((v, a) => true, false, @break ? a => a : (Func<bool, bool>)null);
        }
        /// <summary>
        /// Add an all to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found to return false.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAll<T>(this GenericTally<T> @this, Func<T, bool> cond, bool @break = false)
        {
            return @this.TallyAggregate((v, a) => a && cond(v), true, @break ? a => !a : (Func<bool, bool>)null);
        }
        /// <summary>
        /// Add an ignored tally action to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAction<T>(this GenericTally<T> @this, Action<T> action)
        {
            return @this.Add(new TallierAction<T>(action),false);
        }
        /// <summary>
        /// Add an ignored tally action to a <see cref="GenericTally{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="GenericTally{T}"/>.</typeparam>
        /// <param name="this">The <see cref="GenericTally{T}"/> to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied. The action returns whether or not to continue the tallying.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAction<T>(this GenericTally<T> @this, Func<T,bool> action)
        {
            return @this.Add(new TallierActionBreakable<T>(action), false);
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
    internal class TallierAggregateSelect<T, A, R> : ITallier<T>
    {
        private class TallerAggSelTor : ITalliator<T>
        {
            private A _ret;
            private readonly TallierAggregateSelect<T, A, R> _upper;
            public TallerAggSelTor(A ret, TallierAggregateSelect<T, A, R> upper)
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
                return _upper._selector(_ret);
            }
        }
        private readonly A _seed;
        private readonly Func<T, A, A> _func;
        private readonly Func<A, R> _selector;
        public TallierAggregateSelect(A seed, Func<T, A, A> func, Func<A, R> selector)
        {
            _seed = seed;
            _func = func;
            _selector = selector;
        }
        public ITalliator<T> GetTalliaror()
        {
            return new TallerAggSelTor(_seed, this);
        }
    }
    internal class TallierAggregateBreakableSelect<T, A, R> : ITallier<T>
    {
        private class TallerAggSelBreakableTor : ITalliator<T>
        {
            private A _ret;
            private readonly TallierAggregateBreakableSelect<T, A, R> _upper;
            public TallerAggSelBreakableTor(A ret, TallierAggregateBreakableSelect<T, A, R> upper)
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
                return _upper._selector(_ret);
            }
        }
        private readonly A _seed;
        private readonly Func<T, A, A> _func;
        private readonly Func<A, bool> _break;
        private readonly Func<A, R> _selector;
        public TallierAggregateBreakableSelect(A seed, Func<T, A, A> func, Func<A, bool> @break, Func<A, R> selector)
        {
            _seed = seed;
            _func = func;
            _break = @break;
            _selector = selector;
        }
        public ITalliator<T> GetTalliaror()
        {
            return new TallerAggSelBreakableTor(_seed, this);
        }
    }
    internal class TallierAction<T> : ITallier<T>
    {
        private class TalierAcTor : ITalliator<T>
        {
            private readonly TallierAction<T> _upper;
            public TalierAcTor(TallierAction<T> upper)
            {
                _upper = upper;
            }
            public bool next(T val)
            {
                _upper._func(val);
                return true;
            }
            public object end()
            {
                return null;
            }
        }
        private readonly Action<T> _func;
        public TallierAction(Action<T> func)
        {
            _func = func;
        }
        public ITalliator<T> GetTalliaror()
        {
            return new TalierAcTor(this);
        }
    }
    internal class TallierActionBreakable<T> : ITallier<T>
    {
        private class TalierAcBrTor : ITalliator<T>
        {
            private readonly TallierActionBreakable<T> _upper;
            public TalierAcBrTor(TallierActionBreakable<T> upper)
            {
                _upper = upper;
            }
            public bool next(T val)
            {
                return _upper._func(val);
            }
            public object end()
            {
                return null;
            }
        }
        private readonly Func<T, bool> _func;
        public TallierActionBreakable(Func<T,bool> func)
        {
            _func = func;
        }
        public ITalliator<T> GetTalliaror()
        {
            return new TalierAcBrTor(this);
        }
    }
}

using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;
using WhetStone.Tuples;

namespace WhetStone.Looping
{
    #region Classes
    /// <summary>
    /// A tally for no output types.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> to tally.</typeparam>
    public class TypeTally<T>
    {
        private readonly GenericTally<T> _int;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="src">The default <see cref="IEnumerable{T}"/> to tally in case none are provided. <see langword="null"/> for no default.</param>
        public TypeTally(IEnumerable<T> src = null)
        {
            _int = new GenericTally<T>(src);
        }
        /// <summary>
        /// Perform the tally, tallying all the <see cref="ITallier{T}"/>s added until the <see cref="IEnumerable{T}"/> is done or any of the <see cref="ITallier{T}"/>s have broken.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to tally, or <see langword="null"/> for default.</param>
        public void Do(IEnumerable<T> source = null)
        {
            _int.Do(source);
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally.
        /// </summary>
        /// <typeparam name="T0">The <see cref="ITallier{T}"/>'s output type.</typeparam>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, with a new type wrapper.</returns>
        public TypeTally<T, T0> Add<T0>(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            return new TypeTally<T, T0>(_int.Add(tal));
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally, but without changing the output type.
        /// </summary>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, to allow piping.</returns>
        public TypeTally<T> AddHidden(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            _int.Add(tal,false);
            return this;
        }
    }
    /// <summary>
    /// A tally for a single output type.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> to tally.</typeparam>
    /// <typeparam name="T0">The first output type.</typeparam>
    public class TypeTally<T,T0>
    {
        private readonly GenericTally<T> _int;
        internal TypeTally(GenericTally<T> i)
        {
            _int = i;
        }
        /// <summary>
        /// Perform the tally, tallying all the <see cref="ITallier{T}"/>s added until the <see cref="IEnumerable{T}"/> is done or any of the <see cref="ITallier{T}"/>s have broken.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to tally, or <see langword="null"/> for default.</param>
        /// <returns>The Tallier's return value.</returns>
        public T0 Do(IEnumerable<T> source = null)
        {
            return (T0)_int.Do(source)[0];
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally.
        /// </summary>
        /// <typeparam name="T1">The <see cref="ITallier{T}"/>'s output type.</typeparam>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, with a new type wrapper.</returns>
        public TypeTally<T, T0, T1> Add<T1>(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            return new TypeTally<T, T0, T1>(_int.Add(tal));
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally, but without changing the output type.
        /// </summary>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, to allow piping.</returns>
        public TypeTally<T, T0> AddHidden(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            _int.Add(tal, false);
            return this;
        }
    }
    /// <summary>
    /// A tally for two output types.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> to tally.</typeparam>
    /// <typeparam name="T0">The first output type.</typeparam>
    /// <typeparam name="T1">The second output type.</typeparam>
    public class TypeTally<T, T0, T1>
    {
        private readonly GenericTally<T> _int;
        internal TypeTally(GenericTally<T> i)
        {
            _int = i;
        }
        /// <summary>
        /// Perform the tally, tallying all the <see cref="ITallier{T}"/>s added until the <see cref="IEnumerable{T}"/> is done or any of the <see cref="ITallier{T}"/>s have broken.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to tally, or <see langword="null"/> for default.</param>
        /// <returns>The Tallier's return values in a tuple.</returns>
        public (T0,T1) Do(IEnumerable<T> source = null)
        {
            return _int.Do(source).ToTuple<T0, T1>();
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally.
        /// </summary>
        /// <typeparam name="T2">The <see cref="ITallier{T}"/>'s output type.</typeparam>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, with a new type wrapper.</returns>
        public TypeTally<T, T0, T1, T2> Add<T2>(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            return new TypeTally<T, T0, T1, T2>(_int.Add(tal));
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally, but without changing the output type.
        /// </summary>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, to allow piping.</returns>
        public TypeTally<T, T0, T1> AddHidden(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            _int.Add(tal, false);
            return this;
        }
    }
    /// <summary>
    /// A tally for three output types.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> to tally.</typeparam>
    /// <typeparam name="T0">The first output type.</typeparam>
    /// <typeparam name="T1">The second output type.</typeparam>
    /// <typeparam name="T2">The third output type.</typeparam>
    public class TypeTally<T, T0, T1, T2>
    {
        private readonly GenericTally<T> _int;
        internal TypeTally(GenericTally<T> i)
        {
            _int = i;
        }
        /// <summary>
        /// Perform the tally, tallying all the <see cref="ITallier{T}"/>s added until the <see cref="IEnumerable{T}"/> is done or any of the <see cref="ITallier{T}"/>s have broken.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to tally, or <see langword="null"/> for default.</param>
        /// <returns>The Tallier's return values in a tuple.</returns>
        public (T0, T1, T2) Do(IEnumerable<T> source = null)
        {
            return _int.Do(source).ToTuple<T0, T1, T2>();
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally.
        /// </summary>
        /// <typeparam name="T3">The <see cref="ITallier{T}"/>'s output type.</typeparam>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, with a new type wrapper.</returns>
        public TypeTally<T, T0, T1, T2, T3> Add<T3>(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            return new TypeTally<T, T0, T1, T2, T3>(_int.Add(tal));
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally, but without changing the output type.
        /// </summary>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, to allow piping.</returns>
        public TypeTally<T, T0, T1, T2> AddHidden(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            _int.Add(tal, false);
            return this;
        }
    }
    /// <summary>
    /// A tally for three output types.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> to tally.</typeparam>
    /// <typeparam name="T0">The first output type.</typeparam>
    /// <typeparam name="T1">The second output type.</typeparam>
    /// <typeparam name="T2">The third output type.</typeparam>
    /// <typeparam name="T3">The fourth output type.</typeparam>
    public class TypeTally<T, T0, T1, T2, T3>
    {
        private readonly GenericTally<T> _int;
        internal TypeTally(GenericTally<T> i)
        {
            _int = i;
        }
        /// <summary>
        /// Perform the tally, tallying all the <see cref="ITallier{T}"/>s added until the <see cref="IEnumerable{T}"/> is done or any of the <see cref="ITallier{T}"/>s have broken.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to tally, or <see langword="null"/> for default.</param>
        /// <returns>The Tallier's return values in a tuple.</returns>
        public (T0, T1, T2, T3) Do(IEnumerable<T> source = null)
        {
            return _int.Do(source).ToTuple<T0, T1, T2, T3>();
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally.
        /// </summary>
        /// <typeparam name="T4">The <see cref="ITallier{T}"/>'s output type.</typeparam>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, with a new type wrapper.</returns>
        public TypeTally<T, T0, T1, T2, T3, T4> Add<T4>(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            return new TypeTally<T, T0, T1, T2, T3, T4>(_int.Add(tal));
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally, but without changing the output type.
        /// </summary>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, to allow piping.</returns>
        public TypeTally<T, T0, T1, T2, T3> AddHidden(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            _int.Add(tal, false);
            return this;
        }
    }
    /// <summary>
    /// A tally for five output types.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/> to tally.</typeparam>
    /// <typeparam name="T0">The first output type.</typeparam>
    /// <typeparam name="T1">The second output type.</typeparam>
    /// <typeparam name="T2">The third output type.</typeparam>
    /// <typeparam name="T3">The fourth output type.</typeparam>
    /// <typeparam name="T4">The fifth output type.</typeparam>
    public class TypeTally<T, T0, T1, T2, T3, T4>
    {
        private readonly GenericTally<T> _int;
        internal TypeTally(GenericTally<T> i)
        {
            _int = i;
        }
        /// <summary>
        /// Perform the tally, tallying all the <see cref="ITallier{T}"/>s added until the <see cref="IEnumerable{T}"/> is done or any of the <see cref="ITallier{T}"/>s have broken.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to tally, or <see langword="null"/> for default.</param>
        /// <returns>The Tallier's return values in a tuple.</returns>
        public (T0, T1, T2, T3, T4) Do(IEnumerable<T> source = null)
        {
            return _int.Do(source).ToTuple<T0, T1, T2, T3, T4>();
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally.
        /// </summary>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, without a type wrapper.</returns>
        public GenericTally<T> Add(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            return _int.Add(tal);
        }
        /// <summary>
        /// Add an <see cref="ITallier{T}"/> to the Tally, but without changing the output type.
        /// </summary>
        /// <param name="tal">The <see cref="ITallier{T}"/> to add.</param>
        /// <returns>The tally, to allow piping.</returns>
        public TypeTally<T, T0, T1, T2, T3, T4> AddHidden(ITallier<T> tal)
        {
            tal.ThrowIfNull(nameof(tal));
            _int.Add(tal, false);
            return this;
        }
    }
    #endregion
    public static partial class TallierExtensions
    {
        #region Aggregate
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T,R> TallyAggregate<T, R>(this TypeTally<T> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, R> TallyAggregate<T, T0, R>(this TypeTally<T, T0> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, R> TallyAggregate<T, T0, T1, R>(this TypeTally<T, T0, T1> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, R> TallyAggregate<T, T0, T1, T2, R>(this TypeTally<T, T0, T1, T2> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, R> TallyAggregate<T, T0, T1, T2, T3, R>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAggregate<T, T0, T1, T2, T3, T4, R>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.Add(toadd);
        }
        #endregion
        #region AggregateHidden
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T> TallyAggregatehidden<T, R>(this TypeTally<T> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.AddHidden(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0> TallyAggregatehidden<T, T0, R>(this TypeTally<T, T0> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.AddHidden(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1> TallyAggregatehidden<T, T0, T1, R>(this TypeTally<T, T0, T1> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.AddHidden(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2> TallyAggregatehidden<T, T0, T1, T2, R>(this TypeTally<T, T0, T1, T2> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.AddHidden(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3> TallyAggregatehidden<T, T0, T1, T2, T3, R>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.AddHidden(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, T4> TallyAggregatehidden<T, T0, T1, T2, T3, T4, R>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T, R, R> func, R seed = default(R), Func<R,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregate<T, R>(seed, func);
            else
                toadd = new TallierAggregateBreakable<T, R>(seed, func, @break);
            return @this.AddHidden(toadd);
        }
        #endregion
        #region AggregateSelect
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="A">The type of the aggregated value.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="select">The selector function to apply to the aggregate result and add to the tally result.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, R> TallyAggregateSelect<T, A, R>(this TypeTally<T> @this, Func<T, A, A> func, A seed, Func<A, R> select, Func<A,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            select.ThrowIfNull(nameof(select));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregateSelect<T, A, R>(seed, func, select);
            else
                toadd = new TallierAggregateBreakableSelect<T, A, R>(seed, func, @break, select);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="A">The type of the aggregated value.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="select">The selector function to apply to the aggregate result and add to the tally result.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, R> TallyAggregateSelect<T, T0, A, R>(this TypeTally<T, T0> @this, Func<T, A, A> func, A seed, Func<A, R> select, Func<A,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            select.ThrowIfNull(nameof(select));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregateSelect<T, A, R>(seed, func, select);
            else
                toadd = new TallierAggregateBreakableSelect<T, A, R>(seed, func, @break, select);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="A">The type of the aggregated value.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="select">The selector function to apply to the aggregate result and add to the tally result.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, R> TallyAggregateSelect<T, T0, T1, A, R>(this TypeTally<T, T0, T1> @this, Func<T, A, A> func, A seed, Func<A, R> select, Func<A,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            select.ThrowIfNull(nameof(select));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregateSelect<T, A, R>(seed, func, select);
            else
                toadd = new TallierAggregateBreakableSelect<T, A, R>(seed, func, @break, select);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="A">The type of the aggregated value.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="select">The selector function to apply to the aggregate result and add to the tally result.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, R> TallyAggregateSelect<T, T0, T1, T2, A, R>(this TypeTally<T, T0, T1, T2> @this, Func<T, A, A> func, A seed, Func<A, R> select, Func<A,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            select.ThrowIfNull(nameof(select));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregateSelect<T, A, R>(seed, func, select);
            else
                toadd = new TallierAggregateBreakableSelect<T, A, R>(seed, func, @break, select);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="A">The type of the aggregated value.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="select">The selector function to apply to the aggregate result and add to the tally result.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, R> TallyAggregateSelect<T, T0, T1, T2, T3, A, R>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T, A, A> func, A seed, Func<A, R> select, Func<A,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            select.ThrowIfNull(nameof(select));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregateSelect<T, A, R>(seed, func, select);
            else
                toadd = new TallierAggregateBreakableSelect<T, A, R>(seed, func, @break, select);
            return @this.Add<R>(toadd);
        }
        /// <summary>
        /// Add an aggregate to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="A">The type of the aggregated value.</typeparam>
        /// <typeparam name="R">The type of the value added to the tally result.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="func">The aggregate function.</param>
        /// <param name="seed">The seed of the aggregate function.</param>
        /// <param name="select">The selector function to apply to the aggregate result and add to the tally result.</param>
        /// <param name="break">The condition for the aggregated value on which to break, or <see langword="null"/> to never break.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAggregateSelect<T, T0, T1, T2, T3, T4, A, R>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T, A, A> func, A seed, Func<A, R> select, Func<A,bool> @break = null)
        {
            @this.ThrowIfNull(nameof(@this));
            func.ThrowIfNull(nameof(func));
            select.ThrowIfNull(nameof(select));
            ITallier<T> toadd;
            if (@break == null)
                toadd = new TallierAggregateSelect<T, A, R>(seed, func, select);
            else
                toadd = new TallierAggregateBreakableSelect<T, A, R>(seed, func, @break, select);
            return @this.Add(toadd);
        }
        #endregion
        #region CountAll
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, int> TallyCount<T>(this TypeTally<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((_, a) => a + 1, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, int> TallyCount<T, T0>(this TypeTally<T, T0> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((_, a) => a + 1, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <typeparam name="T1">The second output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, int> TallyCount<T, T0, T1>(this TypeTally<T, T0, T1> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((_, a) => a + 1, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <typeparam name="T1">The second output type of the tally.</typeparam>
        /// <typeparam name="T2">The third output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, int> TallyCount<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((_, a) => a + 1, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <typeparam name="T1">The second output type of the tally.</typeparam>
        /// <typeparam name="T2">The third output type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, int> TallyCount<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((_, a) => a + 1, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <typeparam name="T1">The second output type of the tally.</typeparam>
        /// <typeparam name="T2">The third output type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth output type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyCount<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((_, a) => a + 1, 0);
        }
        #endregion
        #region CountPred
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, int> TallyCount<T>(this TypeTally<T> @this, Func<T,bool> cond)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => cond(v) ? a + 1 : a, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, int> TallyCount<T, T0>(this TypeTally<T, T0> @this, Func<T,bool> cond)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => cond(v) ? a + 1 : a, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <typeparam name="T1">The second output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, int> TallyCount<T, T0, T1>(this TypeTally<T, T0, T1> @this, Func<T,bool> cond)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => cond(v) ? a + 1 : a, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <typeparam name="T1">The second output type of the tally.</typeparam>
        /// <typeparam name="T2">The third output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, int> TallyCount<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this, Func<T,bool> cond)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => cond(v) ? a + 1 : a, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <typeparam name="T1">The second output type of the tally.</typeparam>
        /// <typeparam name="T2">The third output type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, int> TallyCount<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T,bool> cond)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => cond(v) ? a + 1 : a, 0);
        }
        /// <summary>
        /// Add a count to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first output type of the tally.</typeparam>
        /// <typeparam name="T1">The second output type of the tally.</typeparam>
        /// <typeparam name="T2">The third output type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth output type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth output type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyCount<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T,bool> cond)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => cond(v) ? a + 1 : a, 0);
        }
        #endregion
        #region AnyAll
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, bool> TallyAny<T>(this TypeTally<T> @this, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((v, a) => true, false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, bool> TallyAny<T, T0>(this TypeTally<T, T0> @this, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((v, a) => true, false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, bool> TallyAny<T, T0, T1>(this TypeTally<T, T0, T1> @this, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((v, a) => true, false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, bool> TallyAny<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((v, a) => true, false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, bool> TallyAny<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((v, a) => true, false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAny<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.TallyAggregate((v, a) => true, false, @break ? a => a : (Func<bool,bool>)null);
        }
        #endregion
        #region AnyPred
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, bool> TallyAny<T>(this TypeTally<T> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a || cond(v), false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, bool> TallyAny<T, T0>(this TypeTally<T, T0> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a || cond(v), false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, bool> TallyAny<T, T0, T1>(this TypeTally<T, T0, T1> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a || cond(v), false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, bool> TallyAny<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a || cond(v), false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, bool> TallyAny<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a || cond(v), false, @break ? a => a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an any to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAny<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a || cond(v), false, @break ? a => a : (Func<bool,bool>)null);
        }
        #endregion
        #region AllPred
        /// <summary>
        /// Add an all to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found to return false.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, bool> TallyAll<T>(this TypeTally<T> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a && cond(v), true, @break ? a => !a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an all to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found to return false.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, bool> TallyAll<T, T0>(this TypeTally<T, T0> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a && cond(v), true, @break ? a => !a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an all to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found to return false.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, bool> TallyAll<T, T0, T1>(this TypeTally<T, T0, T1> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a && cond(v), true, @break ? a => !a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an all to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found to return false.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, bool> TallyAll<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a && cond(v), true, @break ? a => !a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an all to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found to return false.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, bool> TallyAll<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a && cond(v), true, @break ? a => !a : (Func<bool,bool>)null);
        }
        /// <summary>
        /// Add an all to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="break">Whether to break when an element is found to return false.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyAll<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T,bool> cond, bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.TallyAggregate((v, a) => a && cond(v), true, @break ? a => !a : (Func<bool,bool>)null);
        }
        #endregion
        #region ActionNoRet
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T> TallyAction<T>(this TypeTally<T> @this, Action<T> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierAction<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0> TallyAction<T, T0>(this TypeTally<T, T0> @this, Action<T> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierAction<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1> TallyAction<T, T0, T1>(this TypeTally<T, T0, T1> @this, Action<T> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierAction<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2> TallyAction<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this, Action<T> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierAction<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3> TallyAction<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this, Action<T> action)
        {
            return @this.AddHidden(new TallierAction<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, T4> TallyAction<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Action<T> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierAction<T>(action));
        }
        #endregion
        #region ActionRet
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied. The action returns whether or not to continue the tallying.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T> TallyAction<T>(this TypeTally<T> @this, Func<T,bool> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierActionBreakable<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied. The action returns whether or not to continue the tallying.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0> TallyAction<T, T0>(this TypeTally<T, T0> @this, Func<T,bool> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierActionBreakable<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied. The action returns whether or not to continue the tallying.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1> TallyAction<T, T0, T1>(this TypeTally<T, T0, T1> @this, Func<T,bool> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierActionBreakable<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied. The action returns whether or not to continue the tallying.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2> TallyAction<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this, Func<T,bool> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierActionBreakable<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied. The action returns whether or not to continue the tallying.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3> TallyAction<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T,bool> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierActionBreakable<T>(action));
        }
        /// <summary>
        /// Add an ignored tally action to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on every element tallied. The action returns whether or not to continue the tallying.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, T4> TallyAction<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T,bool> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return @this.AddHidden(new TallierActionBreakable<T>(action));
        }
        #endregion
        #region First
        /// <summary>
        /// Add a first to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <param name="break">Whether to break when an element has been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T> TallyFirst<T>(this TypeTally<T> @this, Func<T,bool> cond, T initial = default(T), bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            ITallier<T> toadd;
            if (@break)
                toadd = new TallierFirstBreak<T>(cond, initial);
            else
                toadd = new TallierFirst<T>(cond, initial);
            return @this.Add<T>(toadd);
        }
        /// <summary>
        /// Add a first to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <param name="break">Whether to break when an element has been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T> TallyFirst<T, T0>(this TypeTally<T, T0> @this, Func<T,bool> cond, T initial = default(T), bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            ITallier<T> toadd;
            if (@break)
                toadd = new TallierFirstBreak<T>(cond, initial);
            else
                toadd = new TallierFirst<T>(cond, initial);
            return @this.Add<T>(toadd);
        }
        /// <summary>
        /// Add a first to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <param name="break">Whether to break when an element has been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T> TallyFirst<T, T0, T1>(this TypeTally<T, T0, T1> @this, Func<T,bool> cond, T initial = default(T), bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            ITallier<T> toadd;
            if (@break)
                toadd = new TallierFirstBreak<T>(cond, initial);
            else
                toadd = new TallierFirst<T>(cond, initial);
            return @this.Add<T>(toadd);
        }
        /// <summary>
        /// Add a first to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <param name="break">Whether to break when an element has been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T> TallyFirst<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this, Func<T,bool> cond, T initial = default(T), bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            ITallier<T> toadd;
            if (@break)
                toadd = new TallierFirstBreak<T>(cond, initial);
            else
                toadd = new TallierFirst<T>(cond, initial);
            return @this.Add<T>(toadd);
        }
        /// <summary>
        /// Add a first to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <param name="break">Whether to break when an element has been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, T> TallyFirst<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T,bool> cond, T initial = default(T), bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            ITallier<T> toadd;
            if (@break)
                toadd = new TallierFirstBreak<T>(cond, initial);
            else
                toadd = new TallierFirst<T>(cond, initial);
            return @this.Add<T>(toadd);
        }
        /// <summary>
        /// Add a first to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <param name="break">Whether to break when an element has been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyFirst<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T,bool> cond, T initial = default(T), bool @break = false)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            ITallier<T> toadd;
            if (@break)
                toadd = new TallierFirstBreak<T>(cond, initial);
            else
                toadd = new TallierFirst<T>(cond, initial);
            return @this.Add(toadd);
        }
        #endregion
        #region Last
        /// <summary>
        /// Add a last to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T> TallyLast<T>(this TypeTally<T> @this, Func<T,bool> cond, T initial = default(T))
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.Add<T>(new TallierLast<T>(cond, initial));
        }
        /// <summary>
        /// Add a last to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T> TallyLast<T, T0>(this TypeTally<T, T0> @this, Func<T,bool> cond, T initial = default(T))
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.Add<T>(new TallierLast<T>(cond, initial));
        }
        /// <summary>
        /// Add a last to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T> TallyLast<T, T0, T1>(this TypeTally<T, T0, T1> @this, Func<T,bool> cond, T initial = default(T))
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.Add<T>(new TallierLast<T>(cond, initial));
        }
        /// <summary>
        /// Add a last to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T> TallyLast<T, T0, T1, T2>(this TypeTally<T, T0, T1, T2> @this, Func<T,bool> cond, T initial = default(T))
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.Add<T>(new TallierLast<T>(cond, initial));
        }
        /// <summary>
        /// Add a last to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static TypeTally<T, T0, T1, T2, T3, T> TallyLast<T, T0, T1, T2, T3>(this TypeTally<T, T0, T1, T2, T3> @this, Func<T,bool> cond, T initial = default(T))
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.Add<T>(new TallierLast<T>(cond, initial));
        }
        /// <summary>
        /// Add a last to a tally.
        /// </summary>
        /// <typeparam name="T">The type of the tally.</typeparam>
        /// <typeparam name="T0">The first return type of the tally.</typeparam>
        /// <typeparam name="T1">The second return type of the tally.</typeparam>
        /// <typeparam name="T2">The third return type of the tally.</typeparam>
        /// <typeparam name="T3">The fourth return type of the tally.</typeparam>
        /// <typeparam name="T4">The fifth return type of the tally.</typeparam>
        /// <param name="this">The tally to add to.</param>
        /// <param name="cond">The condition for which to count an element.</param>
        /// <param name="initial">The initial result for when an element has not been found.</param>
        /// <returns><paramref name="this"/>, to allow piping.</returns>
        public static GenericTally<T> TallyLast<T, T0, T1, T2, T3, T4>(this TypeTally<T, T0, T1, T2, T3, T4> @this, Func<T,bool> cond, T initial = default(T))
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            return @this.Add(new TallierLast<T>(cond, initial));
        }
        #endregion
        /// <summary>
        /// Create a tally using an <see cref="IEnumerable{T}"/> as a default source.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The default source.</param>
        /// <returns>A new tally with <paramref name="this"/> as the default source.</returns>
        public static TypeTally<T> Tally<T>(this IEnumerable<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new TypeTally<T>(@this);
        }
    }
}

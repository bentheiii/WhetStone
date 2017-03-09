using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class asyncDo
    {
        #region enumerables
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="maxParallelism">The maximum number of threads that can enumerate <paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T, ParallelLoopState, long},int)"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x), maxParallelism);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="maxParallelism">The maximum number of threads that can enumerate <paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T, ParallelLoopState, long},int)"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state), maxParallelism);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="maxParallelism">The maximum number of threads that can enumerate <paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T, ParallelLoopState, long},int)"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, long> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind), maxParallelism);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="maxParallelism">The maximum number of threads that can enumerate <paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState, long> action, int maxParallelism)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            var options = new ParallelOptions {MaxDegreeOfParallelism = maxParallelism};
            return Parallel.ForEach(@this, options, action);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="options">The <see cref="ParallelOptions"/> for the parallel loop.<paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x), options);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="options">The <see cref="ParallelOptions"/> for the parallel loop.<paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state), options);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="options">The <see cref="ParallelOptions"/> for the parallel loop.<paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, long> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind), options);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="options">The <see cref="ParallelOptions"/> for the parallel loop.<paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState, long> action, ParallelOptions options)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            options.ThrowIfNull(nameof(options));
            return Parallel.ForEach(@this, options, action);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x));
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state));
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, long> action)
        {
            return @this.AsyncDo((x,state,ind)=>action(x,ind));
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>This simply decorates <see cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>.</remarks>
        /// <seealso cref="Parallel.ForEach{TSource}(IEnumerable{TSource},Action{TSource, ParallelLoopState, long})"/>
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState, long> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return Parallel.ForEach(@this,action);
        }
        #endregion
        #region lists
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="maxParallelism">The maximum number of threads that can enumerate <paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        /// <seealso cref="Parallel.For(int,int,System.Action{int})"/>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x), maxParallelism);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="maxParallelism">The maximum number of threads that can enumerate <paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state), maxParallelism);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="maxParallelism">The maximum number of threads that can enumerate <paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, long> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind), maxParallelism);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="maxParallelism">The maximum number of threads that can enumerate <paramref name="this"/> simultaneously.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState, long> action, int maxParallelism)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            var options = new ParallelOptions { MaxDegreeOfParallelism = maxParallelism };
            return Parallel.For(0, @this.Count, (l, state) => action(@this[l], state, l));
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="options">The options for the parallel loop.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x), options);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="options">The options for the parallel loop.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state), options);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="options">The options for the parallel loop.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, long> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind), options);
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <param name="options">The options for the parallel loop.</param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState, long> action, ParallelOptions options)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            options.ThrowIfNull(nameof(options));
            return Parallel.For(0, @this.Count, options, (l, state) => action(@this[l], state, l));
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x));
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state));
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, long> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind));
        }
        /// <summary>
        /// Enumerates and applies <paramref name="action" /> on an <see cref="IList{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to enumerate.</param>
        /// <param name="action">The action to apply to each element of <paramref name="this"/></param>
        /// <returns>The result of the parallel loop</returns>
        /// <remarks>
        /// <para>This simply decorates <see cref="Parallel.For(int,int,System.Action{int})"/>.</para>
        /// <para>Each thread is started to independently run over a slice of <paramref name="this"/>. This accesses each member of <paramref name="this"/> via the <see cref="IList{T}.this"/> operator. If the <see cref="IList{T}.this"/> operator is particularly slow, consider the <see cref="AsyncDo{T}(IEnumerable{T},System.Action{T},int)"> Enumerating overloads</see>.</para></remarks>
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState, long> action)
        {
            @this.ThrowIfNull(nameof(@this));
            action.ThrowIfNull(nameof(action));
            return Parallel.For(0,@this.Count,(l, state) => action(@this[l],state,l));
        }
        #endregion
    }
}

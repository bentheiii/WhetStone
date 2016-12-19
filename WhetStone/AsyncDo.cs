using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class asyncDo
    {
        #region enumerables
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x), maxParallelism);
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state), maxParallelism);
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, long> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind), maxParallelism);
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState, long> action, int maxParallelism, TaskScheduler scheduler = null)
        {
            var options = new ParallelOptions {MaxDegreeOfParallelism = maxParallelism, TaskScheduler = scheduler};
            return Parallel.ForEach(@this, options, action);
        }

        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x), options);
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state), options);
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, long> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind), options);
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState, long> action, ParallelOptions options)
        {
            return Parallel.ForEach(@this, options, action);
        }

        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x));
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state));
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, long> action)
        {
            return @this.AsyncDo((x,state,ind)=>action(x,ind));
        }
        public static ParallelLoopResult AsyncDo<T>(this IEnumerable<T> @this, Action<T, ParallelLoopState, long> action)
        {
            return Parallel.ForEach(@this,action);
        }
        #endregion
        #region lists
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x), maxParallelism);
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state), maxParallelism);
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, long> action, int maxParallelism)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind), maxParallelism);
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState, long> action, int maxParallelism, TaskScheduler scheduler = null)
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = maxParallelism, TaskScheduler = scheduler };
            return Parallel.For(0, @this.Count, (l, state) => action(@this[l], state, l));
        }

        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x), options);
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state), options);
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, long> action, ParallelOptions options)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind), options);
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState, long> action, ParallelOptions options)
        {
            return Parallel.For(0, @this.Count, (l, state) => action(@this[l], state, l));
        }

        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x));
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x, state));
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, long> action)
        {
            return @this.AsyncDo((x, state, ind) => action(x, ind));
        }
        public static ParallelLoopResult AsyncDo<T>(this IList<T> @this, Action<T, ParallelLoopState, long> action)
        {
            return Parallel.For(0,@this.Count,(l, state) => action(@this[l],state,l));
        }
        #endregion
    }
}

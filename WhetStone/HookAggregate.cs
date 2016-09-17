using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookAggregate
    {
        public static IEnumerable<T> HookAggregate<T, R>(this IEnumerable<T> @this, IGuard<R> sink, Func<R, T, R> aggregator, R seed)
        {
            sink.value = seed;
            foreach (T t in @this)
            {
                sink.value = aggregator(sink.value, t);
                yield return t;
            }
        }
        public static IEnumerable<T> HookAggregate<T>(this IEnumerable<T> @this, IGuard<T> sink, Func<T, T, T> aggregator)
        {
            IGuard<positionBind.Position> pos = new Guard<positionBind.Position>();
            foreach (T t in @this.PositionBind().Detach(pos))
            {
                sink.value = pos.value.HasFlag(positionBind.Position.First) ? t : aggregator(sink.value, t);
                yield return t;
            }
        }
    }
}

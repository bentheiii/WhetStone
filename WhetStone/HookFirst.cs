﻿using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookFirst
    {
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink)
        {
            sink.value = null;
            var tor = @this.GetEnumerator();
            if (!tor.MoveNext())
                yield break;
            sink.value = Tuple.Create(tor.Current);
            yield return tor.Current;
            while (tor.MoveNext())
            {
                yield return tor.Current;
            }
        }
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink, Func<T, bool> critiria)
        {
            sink.value = null;
            var tor = @this.GetEnumerator();
            while (true)
            {
                if (!tor.MoveNext())
                    yield break;
                var yes = critiria(tor.Current);
                if (yes)
                    sink.value = Tuple.Create(tor.Current);
                yield return tor.Current;
                if (yes)
                    break;
            }
            while (tor.MoveNext())
            {
                yield return tor.Current;
            }
            //return @this.HookAggregate(sink, (t0, t1) => (t0 == null && critiria(t1)) ? Tuple.Create(t1) : t0, null);
        }

        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<T> sink)
        {
            var tor = @this.GetEnumerator();
            if (!tor.MoveNext())
                yield break;
            sink.value = tor.Current;
            yield return tor.Current;
            while (tor.MoveNext())
            {
                yield return tor.Current;
            }
        }
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<T> sink, Func<T, bool> critiria)
        {
            var tor = @this.GetEnumerator();
            while (true)
            {
                if (!tor.MoveNext())
                    yield break;
                var yes = critiria(tor.Current);
                if (yes)
                    sink.value = tor.Current;
                yield return tor.Current;
                if (yes)
                    break;
            }
            while (tor.MoveNext())
            {
                yield return tor.Current;
            }
            //return @this.HookAggregate(sink, (t0, t1) => (t0 == null && critiria(t1)) ? Tuple.Create(t1) : t0, null);
        }
    }
}

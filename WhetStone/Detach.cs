using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class detach
    {
        public static IEnumerable<T1> Detach<T1, T2>(this IEnumerable<Tuple<T1, T2>> @this, IGuard<T2> informer1 = null)
        {
            foreach (var t in @this)
            {
                informer1.CondSet(t.Item2);
                yield return t.Item1;
            }
        }
        public static IEnumerable<T1> Detach<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> @this, IGuard<T2> informer1, IGuard<T3> informer2)
        {
            foreach (var t in @this)
            {
                informer1.value = t.Item2;
                informer2.value = t.Item3;
                yield return t.Item1;
            }
        }
        public static IEnumerable<Tuple<T1, T2>> Detach<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> @this, IGuard<T3> informer1 = null)
        {
            foreach (var t in @this)
            {
                informer1.CondSet(t.Item3);
                yield return Tuple.Create(t.Item1, t.Item2);
            }
        }
        public static IEnumerable<T1> Detach<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this, IGuard<T2> informer1, IGuard<T3> informer2, IGuard<T4> informer3)
        {
            foreach (var t in @this)
            {
                informer1.value = t.Item2;
                informer2.value = t.Item3;
                informer3.value = t.Item4;
                yield return t.Item1;
            }
        }
        public static IEnumerable<Tuple<T1, T2>> Detach<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this, IGuard<T3> informer2, IGuard<T4> informer3)
        {
            foreach (var t in @this)
            {
                informer2.value = t.Item3;
                informer3.value = t.Item4;
                yield return Tuple.Create(t.Item1, t.Item2);
            }
        }
        public static IEnumerable<Tuple<T1, T2, T3>> Detach<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this, IGuard<T4> informer3 = null)
        {
            foreach (var t in @this)
            {
                informer3.CondSet(t.Item4);
                yield return Tuple.Create(t.Item1, t.Item2, t.Item3);
            }
        }
    }
}

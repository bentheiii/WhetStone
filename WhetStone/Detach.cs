using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;
using WhetStone.LockedStructures;

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

        private class DetachList<T1,T2> : LockedList<T1>
        {
            private readonly IList<Tuple<T1, T2>> _source;
            private readonly IGuard<T2> _informer;
            public DetachList(IList<Tuple<T1, T2>> source, IGuard<T2> informer)
            {
                _source = source;
                _informer = informer;
            }
            public override IEnumerator<T1> GetEnumerator()
            {
                return _source.AsEnumerable().Detach(_informer).GetEnumerator();
            }
            public override int Count => _source.Count;
            public override T1 this[int index]
            {
                get
                {
                    var val = _source[index];
                    _informer.CondSet(val.Item2);
                    return val.Item1;
                }
            }
        }
        public static IList<T1> Detach<T1, T2>(this IList<Tuple<T1, T2>> @this, IGuard<T2> informer1 = null)
        {
            return new DetachList<T1,T2>(@this,informer1);
        }
        public static IList<T1> Detach<T1, T2, T3>(this IList<Tuple<T1, T2, T3>> @this, IGuard<T2> informer1, IGuard<T3> informer2)
        {
            return @this.Detach(informer2).Detach(informer1);
        }
        public static IList<Tuple<T1,T2>> Detach<T1, T2, T3>(this IList<Tuple<T1, T2, T3>> @this, IGuard<T3> informer1=null)
        {
            return @this.Select(a => Tuple.Create(Tuple.Create(a.Item1, a.Item2), a.Item3)).Detach(informer1);
        }
    }
}

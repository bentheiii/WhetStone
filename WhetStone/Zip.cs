using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;
using WhetStone.Tuples;

namespace WhetStone.Looping
{
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
        public static IEnumerable<IEnumerable<T>> Zip<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            var tor = @this.Select(a => a.GetEnumerator()).ToArray();
            while (tor.All(a => a.MoveNext()))
            {
                yield return tor.Select(a => a.Current);
            }
        }
        public static IList<IList<T>> Zip<T>(this IList<IList<T>> @this)
        {
            return new ListZip<T>(@this);
        }
        public static IEnumerable<IEnumerable> Zip(this IEnumerable<IEnumerable> @this)
        {
            var tor = @this.Select(a => a.GetEnumerator()).ToArray();
            while (tor.All(a => a.MoveNext()))
            {
                yield return tor.Select(a => a.Current);
            }
        }
        public static IList<IList> Zip(this IList<IList> @this)
        {
            return new ListZip(@this);
        }

        public static IEnumerable<Tuple<T1, T2>> Zip<T1, T2>(this IEnumerable<T1> a, IEnumerable<T2> b)
        {
            return Zip(new IEnumerable[] { a, b }).Select(x => x.ToTuple<T1, T2>());
        }
        public static IEnumerable<Tuple<T1, T2, T3>> Zip<T1, T2, T3>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c)
        {
            return Zip(new IEnumerable[] { a, b, c }).Select(x => x.ToTuple<T1, T2, T3>());
        }
        public static IEnumerable<Tuple<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c, IEnumerable<T4> d)
        {
            return Zip(new IEnumerable[] { a, b, c, d }).Select(x => x.ToTuple<T1, T2, T3, T4>());
        }
        public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c, IEnumerable<T4> d, IEnumerable<T5> e)
        {
            return Zip(new IEnumerable[] { a, b, c, d, e }).Select(x => x.ToTuple<T1, T2, T3, T4, T5>());
        }

        public static IList<Tuple<T1, T2>> Zip<T1, T2>(this IList<T1> a, IList<T2> b)
        {
            return Zip(new[] { a.ToGeneral(), b.ToGeneral() }).Select(x => x.ToTuple<T1, T2>());
        }
        public static IList<Tuple<T1, T2,T3>> Zip<T1, T2, T3>(this IList<T1> a, IList<T2> b, IList<T3> c)
        {
            return Zip(new[] { a.ToGeneral(), b.ToGeneral(), c.ToGeneral()}).Select(x => x.ToTuple<T1, T2,T3>());
        }
        public static IList<Tuple<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(this IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d)
        {
            return Zip(new[] { a.ToGeneral(), b.ToGeneral(), c.ToGeneral(), d.ToGeneral() }).Select(x => x.ToTuple<T1, T2, T3, T4>());
        }
        public static IList<Tuple<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(this IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e)
        {
            return Zip(new[] { a.ToGeneral(), b.ToGeneral(), c.ToGeneral(), d.ToGeneral(), e.ToGeneral() }).Select(x => x.ToTuple<T1, T2, T3, T4, T5>());
        }
    }
}

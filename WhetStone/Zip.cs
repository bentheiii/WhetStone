using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.Tuples;

namespace WhetStone.Looping
{
    public static class zip
    {
        public class ListZip<T> : LockedList<IEnumerable<T>>
        {
            private readonly IEnumerable<IList<T>> _sources;
            public ListZip(IEnumerable<IList<T>> sources)
            {
                _sources = sources;
            }
            public override IEnumerator<IEnumerable<T>> GetEnumerator()
            {
                return range.Range(Count).Select(i => _sources.Select(a => a[i])).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _sources.Min(a => a.Count);
                }
            }
            public override IEnumerable<T> this[int index]
            {
                get
                {
                    return _sources.Select(a => a[index]);
                }
            }
        }
        public class ListZip : LockedList<IEnumerable>
        {
            private readonly IEnumerable<IList> _sources;
            public ListZip(IEnumerable<IList> sources)
            {
                _sources = sources;
            }
            public override IEnumerator<IEnumerable> GetEnumerator()
            {
                return range.Range(Count).Select(i => _sources.Select(a => a[i])).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _sources.Min(a => a.Count);
                }
            }
            public override IEnumerable this[int index]
            {
                get
                {
                    return _sources.Select(a => a[index]);
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
        public static LockedList<IEnumerable<T>> Zip<T>(this IEnumerable<IList<T>> @this)
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
        public static LockedList<IEnumerable> Zip(this IEnumerable<IList> @this)
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

        public static LockedList<Tuple<T1, T2>> Zip<T1, T2>(this IList<T1> a, IList<T2> b)
        {
            return Zip(new[] { (IList)a, (IList)b }).Select(x => x.ToTuple<T1, T2>());
        }
        public static LockedList<Tuple<T1, T2,T3>> Zip<T1, T2, T3>(this IList<T1> a, IList<T2> b, IList<T3> c)
        {
            return Zip(new[] { (IList)a, (IList)b, (IList) c}).Select(x => x.ToTuple<T1, T2,T3>());
        }
        public static LockedList<Tuple<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(this IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d)
        {
            return Zip(new[] { (IList)a, (IList)b, (IList)c, (IList)d }).Select(x => x.ToTuple<T1, T2, T3, T4>());
        }
        public static LockedList<Tuple<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(this IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e)
        {
            return Zip(new[] { (IList)a, (IList)b, (IList)c, (IList)d, (IList)e }).Select(x => x.ToTuple<T1, T2, T3, T4, T5>());
        }
    }
}

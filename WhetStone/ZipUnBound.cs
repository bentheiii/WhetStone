using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.LockedStructures;
using WhetStone.Tuples;

namespace WhetStone.Looping
{
    public static class zipUnBound
    {
        private class ListZipUnbound<T> : LockedList<IEnumerable<T>>
        {
            private readonly IEnumerable<IList<T>> _sources;
            private readonly IList<T> _defaultmembers;
            public ListZipUnbound(IEnumerable<IList<T>> sources, IList<T> defaultmember)
            {
                _sources = sources;
                _defaultmembers = defaultmember;
            }
            public override IEnumerator<IEnumerable<T>> GetEnumerator()
            {
                return range.Range(Count).Select(i => _sources.Select(a => a.Count <= i? a[i] : _defaultmembers[i])).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _sources.Max(a => a.Count);
                }
            }
            public override IEnumerable<T> this[int index]
            {
                get
                {
                    return _sources.Select(a => a.Count <= index ? a[index] : _defaultmembers[index]);
                }
            }
        }
        private class ListZipUnbound : LockedList<IEnumerable>
        {
            private readonly IEnumerable<IList> _sources;
            private readonly IList _defaultmembers;
            public ListZipUnbound(IEnumerable<IList> sources, IList defaultmember)
            {
                _sources = sources;
                _defaultmembers = defaultmember;
            }
            public override IEnumerator<IEnumerable> GetEnumerator()
            {
                return range.Range(Count).Select(i => _sources.Select(a => a.Count <= i ? a[i] : _defaultmembers[i])).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _sources.Max(a => a.Count);
                }
            }
            public override IEnumerable this[int index]
            {
                get
                {
                    return _sources.Select(a => a.Count <= index ? a[index] : _defaultmembers[index]);
                }
            }
        }

        public static IEnumerable<IEnumerable> ZipUnbound(IEnumerable<IEnumerable> @this, params object[] defvals)
        {
            IEnumerator[] tor = @this.Select(a => a.GetEnumerator()).ToArray();
            while (true)
            {
                bool cont = false;
                for (int i = 0; i < tor.Length; i++)
                {
                    if (tor[i] == null)
                        continue;
                    if (tor[i].MoveNext())
                        cont = true;
                    else
                        tor[i] = null;
                }
                if (!cont)
                    yield break;
                yield return tor.CountBind().Select(a => a.Item1 == null ? (defvals.Length > a.Item2 ? defvals[a.Item2] : null) : a.Item1.Current);
            }
        }
        public static IEnumerable<IEnumerable<T>> ZipUnbound<T>(IEnumerable<IEnumerable<T>> @this, params T[] defvals)
        {
            IEnumerator<T>[] tor = @this.Select(a => a.GetEnumerator()).ToArray();
            while (true)
            {
                bool cont = false;
                for (int i = 0; i < tor.Length; i++)
                {
                    if (tor[i] == null)
                        continue;
                    if (tor[i].MoveNext())
                        cont = true;
                    else
                        tor[i] = null;
                }
                if (!cont)
                    yield break;
                yield return tor.CountBind().Select(a => a.Item1 == null ? (defvals.Length > a.Item2 ? defvals[a.Item2] : default(T)) : a.Item1.Current);
            }
        }
        public static LockedList<IEnumerable> ZipUnbound(IEnumerable<IList> @this, params object[] defvals)
        {
            return new ListZipUnbound(@this,defvals);
        }
        public static LockedList<IEnumerable<T>> ZipUnbound<T>(IEnumerable<IList<T>> @this, params T[] defvals)
        {
            return new ListZipUnbound<T>(@this, defvals);
        }

        public static IEnumerable<Tuple<T1, T2>> ZipUnbound<T1, T2>(this IEnumerable<T1> a, IEnumerable<T2> b, T1 defa = default(T1), T2 defb = default(T2))
        {
            return ZipUnbound(new IEnumerable[] { a, b }, defa, defb).Select(x => x.ToTuple<T1, T2>());
        }
        public static IEnumerable<Tuple<T1, T2, T3>> ZipUnbound<T1, T2, T3>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c, T1 defa = default(T1), T2 defb = default(T2), T3 defc = default(T3))
        {
            return ZipUnbound(new IEnumerable[] { a, b, c }, defa, defb, defc).Select(x => x.ToTuple<T1, T2, T3>());
        }
        public static IEnumerable<Tuple<T1, T2, T3, T4>> ZipUnbound<T1, T2, T3, T4>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c, IEnumerable<T4> d, T1 defa = default(T1), T2 defb = default(T2), T3 defc = default(T3), T4 defd = default(T4))
        {
            return ZipUnbound(new IEnumerable[] { a, b, c, d }, defa, defb, defc, defd).Select(x => x.ToTuple<T1, T2, T3, T4>());
        }
        public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> ZipUnbound<T1, T2, T3, T4, T5>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c, IEnumerable<T4> d, IEnumerable<T5> e, T1 defa = default(T1), T2 defb = default(T2), T3 defc = default(T3), T4 defd = default(T4), T5 defe = default(T5))
        {
            return ZipUnbound(new IEnumerable[] { a, b, c, d, e }, defa, defb, defc, defd, defe).Select(x => x.ToTuple<T1, T2, T3, T4, T5>());
        }

        public static IEnumerable<Tuple<T1, T2>> ZipUnbound<T1, T2>(this IList<T1> a, IList<T2> b, T1 defa = default(T1), T2 defb = default(T2))
        {
            return ZipUnbound(new [] { (IList)a, (IList)b }, defa, defb).Select(x => x.ToTuple<T1, T2>());
        }
        public static IEnumerable<Tuple<T1, T2, T3>> ZipUnbound<T1, T2, T3>(this IList<T1> a, IList<T2> b, IList<T3> c, T1 defa = default(T1), T2 defb = default(T2), T3 defc = default (T3))
        {
            return ZipUnbound(new[] { (IList)a, (IList)b, (IList)c }, defa, defb, defc).Select(x => x.ToTuple<T1, T2, T3>());
        }
        public static IEnumerable<Tuple<T1, T2, T3, T4>> ZipUnbound<T1, T2, T3, T4>(this IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d , T1 defa = default(T1), T2 defb = default(T2), T3 defc = default(T3), T4 defd = default(T4))
        {
            return ZipUnbound(new[] { (IList)a, (IList)b, (IList)c, (IList)d }, defa, defb, defc, defd).Select(x => x.ToTuple<T1, T2, T3, T4>());
        }
        public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> ZipUnbound<T1, T2, T3, T4, T5>(this IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e, T1 defa = default(T1), T2 defb = default(T2), T3 defc = default(T3), T4 defd = default(T4), T5 defe = default(T5))
        {
            return ZipUnbound(new[] { (IList)a, (IList)b, (IList)c, (IList)d, (IList)e }, defa, defb, defc, defd, defe).Select(x => x.ToTuple<T1, T2, T3, T4, T5>());
        }
    }
}

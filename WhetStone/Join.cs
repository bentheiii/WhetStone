using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;

namespace WhetStone.Looping
{
    public static class join
    {
        [Flags]
        public enum CartesianType { AllPairs = 0, NoSymmatry = 1, NoReflexive = 2 }
        public static IEnumerable<Tuple<T, T>> Join<T>(this IEnumerable<T> a, CartesianType t = CartesianType.AllPairs)
        {
            foreach (var v0 in a.CountBind())
            {
                var iEnumerable = a.CountBind();
                if (t.HasFlag(CartesianType.NoSymmatry))
                    iEnumerable = iEnumerable.Take(v0.Item2 + 1);
                foreach (var v1 in iEnumerable)
                {
                    if (t.HasFlag(CartesianType.NoReflexive) && v0.Item2 == v1.Item2)
                        continue;
                    yield return new Tuple<T, T>(v0.Item1, v1.Item1);
                }
            }
        }
        public static IEnumerable<Tuple<T1, T2>> Join<T1, T2>(this IEnumerable<T1> a, IEnumerable<T2> b)
        {
            foreach (T1 v0 in a)
            {
                foreach (T2 v1 in b)
                {
                    yield return new Tuple<T1, T2>(v0, v1);
                }
            }
        }
        public static IEnumerable<Tuple<T1, T2, T3>> Join<T1, T2, T3>(this IEnumerable<T1> a, IEnumerable<T2> b, IEnumerable<T3> c)
        {
            foreach (T1 v0 in a)
            {
                foreach (T2 v1 in b)
                {
                    foreach (T3 v2 in c)
                    {
                        yield return Tuple.Create(v0, v1, v2);
                    }
                }
            }
        }
        public static IEnumerable<T[]> Join<T>(this IEnumerable<T> @this, int cartesLength, CartesianType t = CartesianType.AllPairs)
        {
            var tors = @this.Enumerate().Repeat(cartesLength).Select(a => a.CountBind().GetEnumerator()).ToArray();
            //initialization
            if (t.HasFlag(CartesianType.NoReflexive))
            {
                foreach (var enumerator in tors.CountBind())
                {
                    foreach (var i in range.Range(tors.Length - enumerator.Item2))
                    {
                        if (!enumerator.Item1.MoveNext())
                            yield break;
                    }
                }
            }
            else
            {
                if (tors.Any(a => !a.MoveNext()))
                    yield break;
            }
            //yield initial
            yield return tors.Select(a => a.Current.Item1).ToArray();
            while (true)
            {
                int nexttorind = 0;
                while (true)
                {
                    if (!tors.IsWithinBounds(nexttorind))
                        yield break;
                    if (!tors[nexttorind].MoveNext())
                    {
                        tors[nexttorind] = @this.CountBind().GetEnumerator();
                        tors[nexttorind].MoveNext();
                        nexttorind++;
                    }
                    else
                    {
                        if (t.HasFlag(CartesianType.NoSymmatry) && nexttorind > 0)
                        {
                            bool retry = false;
                            foreach (var i in range.Range(0, nexttorind))
                            {
                                foreach (int i1 in range.Range(tors[nexttorind].Current.Item2 + (t.HasFlag(CartesianType.NoReflexive) ? i + 1 : 0)))
                                {
                                    if (!tors[nexttorind - i - 1].MoveNext())
                                    {
                                        retry = true;
                                        break;
                                    }
                                }
                                if (retry)
                                    break;
                            }
                            if (retry)
                            {
                                foreach (int i in range.IRange(nexttorind))
                                {
                                    tors[i] = @this.CountBind().GetEnumerator();
                                    tors[i].MoveNext();
                                }
                                nexttorind++;
                                continue;
                            }
                        }
                        break;
                    }
                }
                if (t.HasFlag(CartesianType.NoReflexive) && !t.HasFlag(CartesianType.NoSymmatry) &&
                        tors.Join(CartesianType.NoReflexive | CartesianType.NoSymmatry).Any(
                            a => a.Item1.Current.Item2 == a.Item2.Current.Item2))
                {
                    continue;
                }
                yield return tors.Select(a => a.Current.Item1).ToArray();
            }
        }
        public static IEnumerable<T[]> Join<T>(this IList<IEnumerable<T>> @this)
        {
            var tors = @this.Select(a => a.GetEnumerator()).ToArray();
            //initialization
            if (tors.Any(a => !a.MoveNext()))
                yield break;
            //yield initial
            yield return tors.Select(a => a.Current).ToArray();
            while (true)
            {
                int nexttorind = 0;
                while (true)
                {
                    if (!tors.IsWithinBounds(nexttorind))
                        yield break;
                    if (!tors[nexttorind].MoveNext())
                    {
                        tors[nexttorind] = @this[nexttorind].GetEnumerator();
                        tors[nexttorind].MoveNext();
                        nexttorind++;
                    }
                    else
                    {
                        break;
                    }
                }
                yield return tors.Select(a => a.Current).ToArray();
            }
        }
    }
}

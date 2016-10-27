using System;
using System.Collections.Generic;
using System.Linq;
using NumberStone;
using WhetStone.LockedStructures;

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
        private static IEnumerable<T[]> JoinAllPairs<T>(this IEnumerable<T> @this, int cartesLength)
        {
            var tors = @this.Enumerate(cartesLength).Select(a => a.GetEnumerator()).ToArray();
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
                        tors[nexttorind] = @this.GetEnumerator();
                        tors[nexttorind].MoveNext();
                        nexttorind++;
                    }
                    else
                        break;
                }
                yield return tors.Select(a => a.Current).ToArray();
            }
        }
        private static IEnumerable<T[]> JoinMonoDescendingPairs<T>(this IEnumerable<T> @this, int cartesLength)
        {
            var tors = @this.Enumerate().Repeat(cartesLength).Select(a => a.CountBind().GetEnumerator()).ToArray();
            //initialization
                if (tors.Any(a => !a.MoveNext()))
                    yield break;
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
                        if (nexttorind > 0)
                        {
                            bool retry = false;
                            foreach (var i in range.Range(0, nexttorind))
                            {
                                foreach (int i1 in range.Range(tors[nexttorind].Current.Item2))
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
                yield return tors.Select(a => a.Current.Item1).ToArray();
            }
        }
        private static IEnumerable<T[]> JoinDescendingPairs<T>(this IEnumerable<T> @this, int cartesLength)
        {
            var tors = @this.Enumerate().Repeat(cartesLength).Select(a => a.CountBind().GetEnumerator()).ToArray();
            //initialization
            foreach (var enumerator in tors.CountBind())
            {
                foreach (var i in range.Range(tors.Length - enumerator.Item2))
                {
                    if (!enumerator.Item1.MoveNext())
                        yield break;
                }
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
                        if (nexttorind > 0)
                        {
                            bool retry = false;
                            foreach (var i in range.Range(0, nexttorind))
                            {
                                foreach (int i1 in range.Range(tors[nexttorind].Current.Item2 + i + 1))
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
                yield return tors.Select(a => a.Current.Item1).ToArray();
            }
        }
        private static IEnumerable<T[]> JoinNoReflexive<T>(this IEnumerable<T> @this, int cartesLength)
        {
            var tors = @this.Enumerate().Repeat(cartesLength).Select(a => a.CountBind().GetEnumerator()).ToArray();
            //initialization
                foreach (var enumerator in tors.CountBind())
                {
                    foreach (var i in range.Range(tors.Length - enumerator.Item2))
                    {
                        if (!enumerator.Item1.MoveNext())
                            yield break;
                    }
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
                        break;
                    }
                }
                if (tors.Join(CartesianType.NoReflexive | CartesianType.NoSymmatry).Any(
                            a => a.Item1.Current.Item2 == a.Item2.Current.Item2))
                {
                    continue;
                }
                yield return tors.Select(a => a.Current.Item1).ToArray();
            }
        }
        public static IEnumerable<T[]> Join<T>(this IEnumerable<T> @this, int cartesLength, CartesianType t = CartesianType.AllPairs)
        {
            switch (t)
            {
                case CartesianType.AllPairs:
                    return JoinAllPairs(@this,cartesLength);
                case CartesianType.NoReflexive:
                    return JoinNoReflexive(@this,cartesLength);
                case CartesianType.NoSymmatry:
                    return JoinMonoDescendingPairs(@this,cartesLength);
                case CartesianType.NoReflexive | CartesianType.NoSymmatry:
                    return JoinDescendingPairs(@this,cartesLength);
                default:
                    throw new NotSupportedException();
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

        private class JointList<T1,T2> : LockedList<Tuple<T1,T2>>
        {
            private readonly IList<T1> _source1;
            private readonly IList<T2> _source2;
            public JointList(IList<T1> source1, IList<T2> source2)
            {
                _source1 = source1;
                _source2 = source2;
            }
            public override IEnumerator<Tuple<T1, T2>> GetEnumerator()
            {
                foreach (T2 t2 in _source2)
                {
                    foreach (T1 t1 in _source1)
                    {
                        yield return Tuple.Create(t1, t2);
                    }
                }
            }
            public override int Count => _source1.Count*_source2.Count;
            public override Tuple<T1, T2> this[int index]
            {
                get
                {
                    return Tuple.Create(_source1[index%_source1.Count], _source2[index/_source1.Count]);
                }
            }
        }
        private class JointList<T> : LockedList<T[]>
        {
            private readonly IList<IList<T>> _sources;
            public JointList(IList<IList<T>> sources)
            {
                _sources = sources;
            }
            public override IEnumerator<T[]> GetEnumerator()
            {
                int[] indices = new int[_sources.Count];
                while (true)
                {
                    yield return indices.Select((a, i) => _sources[i][a]).ToArray();
                    int incindex = 0;
                    while (indices[incindex]+1 >= _sources[incindex].Count)
                    {
                        indices[incindex] = 0;
                        incindex++;
                        if (incindex >= indices.Length)
                            yield break;
                    }
                    indices[incindex]++;
                }
            }
            public override int Count => _sources.Aggregate(1, (i, ts) => i*ts.Count);
            public override T[] this[int index]
            {
                get
                {
                    var ret = new ResizingArray<T>(_sources.Count);
                    int divisor = 1;
                    foreach (IList<T> source in _sources)
                    {
                        ret.Add(source[(index/divisor)%source.Count]);
                        divisor *= source.Count;
                    }
                    return ret.arr;
                }
            }
        }
        private class JointMetaListAllPairs<T> : LockedList<T[]>
        {
            private readonly IList<T> _source;
            private readonly int _length;
            public JointMetaListAllPairs(IList<T> source, int length)
            {
                _source = source;
                _length = length;
            }
            public override IEnumerator<T[]> GetEnumerator()
            {
                int[] indices = new int[_length];
                while (true)
                {
                    yield return indices.Select((a, i) => _source[a]).ToArray();
                    int incindex = 0;
                    while (indices[incindex] + 1 >= _source.Count)
                    {
                        indices[incindex] = 0;
                        incindex++;
                        if (incindex >= indices.Length)
                            yield break;
                    }
                    indices[incindex]++;
                }
            }
            public override int Count
            {
                get
                {
                    return (int)Math.Pow(_source.Count, _length);
                }
            }
            public override T[] this[int index]
            {
                get
                {
                    var ret = new T[_length];
                    foreach (var i in range.Range(_length))
                    {
                        ret[i] = _source[index%_source.Count];
                        index /= _source.Count;
                    }
                    return ret;
                }
            }
        }
        private class JointMetaListMonoDescendingPairs<T> : LockedList<T[]>
        {
            private readonly IList<T> _source;
            private readonly int _length;
            public JointMetaListMonoDescendingPairs(IList<T> source, int length)
            {
                _source = source;
                _length = length;
            }
            public override IEnumerator<T[]> GetEnumerator()
            {
                if (_length == 0)
                {
                    yield return new T[0];
                    yield break;
                }
                foreach (int firstind in range.Range(_source.Count))
                {
                    foreach (var decendingPair in new JointMetaListMonoDescendingPairs<T>(_source.Slice(firstind), _length - 1))
                    {
                        yield return decendingPair.Concat(_source[firstind].Enumerate()).ToArray();
                    }
                }
            }
            public override int Count
            {
                get
                {
                    return (int)choose.Choose(_source.Count + _length - 1, _length);
                }
            }
            private static IEnumerable<int> iterationIndices(int length, int maxnum, int index)
            {
                if (maxnum == 0 || length == 0)
                    return new int[0];
                int cutoff = (int)choose.Choose(maxnum + length - 2, length - 1);
                int prevcutoff = 0;
                int first = 0;
                while (index >= cutoff)
                {
                    prevcutoff = cutoff;
                    first++;
                    cutoff += (int)choose.Choose(maxnum + length - first - 2, length - 1);
                }
                return iterationIndices(length - 1, maxnum - first, index - prevcutoff).Select(a => a + first).Concat((first).Enumerate());
            }
            public override T[] this[int index]
            {
                get
                {
                    return iterationIndices(_length, _source.Count, index).Select(a => _source[a]).ToArray();
                }
            }
        }
        private class JointMetaListDescendingPairs<T> : LockedList<T[]>
        {
            private readonly IList<T> _source;
            private readonly int _length;
            public JointMetaListDescendingPairs(IList<T> source, int length)
            {
                _source = source;
                _length = length;
            }
            public override IEnumerator<T[]> GetEnumerator()
            {
                if (_source.Count < _length)
                    yield break;
                if (_length == 0)
                {
                    yield return new T[0];
                    yield break;
                }
                foreach (T[] descendingPair in new JointMetaListDescendingPairs<T>(_source.Slice(1), _length - 1))
                {
                    yield return (descendingPair).Concat(_source[0].Enumerate()).ToArray();
                }
                foreach (T[] descendingPair in new JointMetaListDescendingPairs<T>(_source.Slice(1), _length))
                {
                    yield return descendingPair;
                }
            }
            public override int Count
            {
                get
                {
                    return (int)choose.Choose(_source.Count, _length);
                }
            }
            private static IEnumerable<int> iterationIndices(int length, int maxnum, int index)
            {
                if (maxnum == 0 || length == 0)
                    return new int[0];
                int cutoff = (int)choose.Choose(maxnum - 1, length - 1);
                int prevcutoff = 0;
                int first = 0;
                while (index >= cutoff)
                {
                    prevcutoff = cutoff;
                    first++;
                    cutoff += (int)choose.Choose(maxnum - first - 1, length - 1);
                }
                return (iterationIndices(length - 1, maxnum - first - 1, index - prevcutoff).Select(a => a + first + 1)).Concat((first).Enumerate());
            }
            public override T[] this[int index]
            {
                get
                {
                    return iterationIndices(_length,_source.Count,index).Select(a=>_source[a]).ToArray();
                }
            }
        }
        private class JointMetaListNoReflexive<T> : LockedList<T[]>
        {
            private readonly IList<T> _source;
            private readonly int _length;
            public JointMetaListNoReflexive(IList<T> source, int length)
            {
                _source = source;
                _length = length;
            }
            public override IEnumerator<T[]> GetEnumerator()
            {
                if (_source.Count < _length)
                    yield break;
                if (_length == 0)
                {
                    yield return new T[0];
                    yield break;
                }
                var remainder = new List<T>(_source);
                int ind = 0;
                foreach (var first in _source)
                {
                    remainder.Remove(first);
                    var cont = new JointMetaListNoReflexive<T>(remainder, _length - 1);
                    foreach (var c in cont)
                    {
                        yield return c.Concat(first.Enumerate()).ToArray();
                    }
                    remainder.Insert(ind++, first);
                }
            }
            public override int Count
            {
                get
                {
                    BigProduct p = new BigProduct();
                    p.MultiplyFactorial(_source.Count);
                    p.DivideFactorial(_source.Count - _length);
                    return (int)p.toNum();
                }
            }
            public override T[] this[int index]
            {
                get
                {
                    if (_length == 1)
                        return new[] { _source[index] };
                    BigProduct gap = new BigProduct();
                    gap.MultiplyFactorial(_source.Count - 1);
                    gap.DivideFactorial(_source.Count - _length);
                    int g = (int)gap.toNum();
                    var first = _source[index / g];
                    return new JointMetaListNoReflexive<T>(_source.Except(first).ToArray(), _length - 1)[index % g].Concat(first.Enumerate()).ToArray();
                }
            }
        }

        public static LockedList<Tuple<T1, T2>> Join<T1, T2>(this IList<T1> @this, IList<T2> other)
        {
            return new JointList<T1, T2>(@this,other);
        }
        public static LockedList<T[]> Join<T>(this IList<IList<T>> @this)
        {
            return new JointList<T>(@this);
        }
        public static LockedList<T[]> Join<T>(this IList<T>[] @this)
        {
            return new JointList<T>(@this);
        }
        public static LockedList<T[]> Join<T>(this IList<T> @this, int length, CartesianType t = CartesianType.AllPairs)
        {
            switch (t)
            {
                case CartesianType.AllPairs:
                    return new JointMetaListAllPairs<T>(@this, length);
                case CartesianType.NoReflexive:
                    return new JointMetaListNoReflexive<T>(@this, length);
                case CartesianType.NoSymmatry:
                    return new JointMetaListMonoDescendingPairs<T>(@this, length);
                case CartesianType.NoReflexive | CartesianType.NoSymmatry:
                    return new JointMetaListDescendingPairs<T>(@this, length);
                default:
                    throw new NotSupportedException();
            }
        }

    }
}

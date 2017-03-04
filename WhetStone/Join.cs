using System;
using System.Collections.Generic;
using System.Linq;
using NumberStone;
using WhetStone.LockedStructures;
using WhetStone.Tuples;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class join
    {
        /// <summary>
        /// A type for Cartesian self-multiplication.
        /// </summary>
        [Flags] public enum CartesianType
        {
            /// <summary>
            /// All combinations will be included.
            /// </summary>
            /// <remarks>
            /// As an example:{0,1,2}x{0,1,2} = (0,0),(0,1),(0,2),(1,0),(1,1),(1,2),(2,0),(2,1),(2,2)
            /// </remarks>
            AllPairs = 0,
            /// <summary>
            /// All combinations will be in descending order, in relation to the source index.
            /// </summary>
            /// <remarks>
            /// As an example:{0,1,2}x{0,1,2} = (0,0),(1,0),(1,1),(2,0),(2,1),(2,2)
            /// </remarks>
            NoSymmatry = 1,
            /// <summary>
            /// All combinations that have the same element more than once will be omitted.
            /// </summary>
            /// <remarks>
            /// As an example:{0,1,2}x{0,1,2} = (0,1),(0,2),(1,0),(1,2),(2,0),(2,1)
            /// </remarks>
            NoReflexive = 2
        }
        /// <overloads>Performs a Cartesian multiplication on enumerables.</overloads>
        /// <summary>
        /// Get the Cartesian multiple of an <see cref="IEnumerable{T}"/> by itself.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The <see cref="IEnumerable{T}"/> to multiply.</param>
        /// <param name="t">The <see cref="CartesianType"/> of the multiplication.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with the Cartesian multiple of <paramref name="a"/> by itself.</returns>
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
        /// <summary>
        /// Get the Cartesian multiple of two <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="b">The second <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The Cartesian multiple of <paramref name="a"/> and <paramref name="b"/>.</returns>
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
        /// <summary>
        /// Get the Cartesian multiple of two <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="a">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="b">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="c">The third <see cref="IEnumerable{T}"/></param>
        /// <returns>The Cartesian multiple of <paramref name="a"/>, <paramref name="b"/> and <paramref name="c"/>.</returns>
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
                                if (range.Range(tors[nexttorind].Current.Item2).Any(i1 => !tors[nexttorind - i - 1].MoveNext()))
                                {
                                    retry = true;
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
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> multiplied by itself multiple times.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to exponentiate.</param>
        /// <param name="cartesLength">The exponential power.</param>
        /// <param name="t">The <see cref="CartesianType"/>.</param>
        /// <returns>The Cartesian exponential of <paramref name="this"/> by <paramref name="cartesLength"/>.</returns>
        public static IEnumerable<T[]> Join<T>(this IEnumerable<T> @this, int cartesLength, CartesianType t = CartesianType.AllPairs)
        {
            if (cartesLength == 0)
                return new[] {new T[0]};
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
        //todo @this could be an ienumerable?
        /// <summary>
        /// Get the Cartesian product of multiple <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> of <see cref="IEnumerable{T}"/></param>
        /// <returns>The Cartesian product of <paramref name="this"/>.</returns>
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

        private class JointMetaListAllPairs : LockedList<int[]>
        {
            private readonly int _maxind;
            private readonly int _length;
            public JointMetaListAllPairs(int maxind, int length)
            {
                _maxind = maxind;
                _length = length;
            }
            public override IEnumerator<int[]> GetEnumerator()
            {
                if (_length == 0)
                {
                    yield return new int[0];
                    yield break;
                }
                var ret = new int[_length];
                yield return ret.ToArray(_length);
                while (true)
                {
                    int incInd = 0;
                    while (incInd != -1)
                    {
                        ret[incInd]++;
                        if (ret[incInd] == _maxind)
                        {
                            ret[incInd] = 0;
                            incInd++;
                            if (incInd == _length)
                                yield break;
                        }
                        else
                        {
                            incInd = -1;
                        }
                    }
                    yield return ret.ToArray(_length);
                }
            }
            public override int Count
            {
                get
                {
                    return (int)Math.Pow(_maxind, _length);
                }
            }
            public override int[] this[int index]
            {
                get
                {
                    var ret = new int[_length];
                    foreach (var i in range.Range(_length))
                    {
                        ret[i] = index % _maxind;
                        index /= _maxind;
                    }
                    return ret;
                }
            }
        }
        private class JointMetaListDescendingPairs : LockedList<int[]>
        {
            private readonly int _maxind;
            private readonly int _length;
            public JointMetaListDescendingPairs(int maxind, int length)
            {
                _maxind = maxind;
                _length = length;
            }
            public override IEnumerator<int[]> GetEnumerator()
            {
                if (_length > _maxind)
                {
                    yield break;
                }
                if (_length == 0)
                {
                    yield return new int[0];
                    yield break;
                }
                var ret = range.IRange(_length-1,0,-1).ToArray();
                yield return ret.ToArray(_length);
                while (true)
                {
                    int incInd = 0;
                    int cascadelength = 0;
                    while (incInd != -1)
                    {
                        ret[incInd]++;
                        if (ret[incInd] == (incInd==0 ? _maxind : ret[incInd-1]-1))
                        {
                            cascadelength++;
                            incInd++;
                            if (cascadelength == _length)
                                yield break;
                        }
                        else
                        {
                            incInd = -1;
                        }
                    }
                    if (cascadelength != 0)
                    {
                        int retval = ret[cascadelength]+cascadelength;
                        foreach (int i in range.Range(0,cascadelength))
                        {
                            ret[i] = retval--;
                        }
                    }
                    yield return ret.ToArray(_length);
                }
            }
            public override int Count
            {
                get
                {
                    return (int)choose.Choose(_maxind, _length);
                }
            }
            public override int[] this[int index]
            {
                get
                {
                    int max = _maxind, len = _length;
                    int[] ret = new int[len];
                    int offset = 0;
                    foreach (int i in ret.Indices().Reverse())
                    {
                        var choose = new BinomialCoefficient(max-1, len-1);
                        int cutoff = choose.value;
                        int prevcutoff = 0;
                        int first = 0;
                        while (index >= cutoff)
                        {
                            prevcutoff = cutoff;
                            first++;
                            choose.DecreaseSuper();
                            cutoff += choose.value;
                        }
                        ret[i] = first + offset;
                        offset += (first + 1);
                        index = index - prevcutoff;
                        max = max - first - 1;
                        len--;
                    }
                    return ret;
                }
            }
        }
        private class JointMetaListMonoDescendingPairs : LockedList<int[]>
        {
            private readonly int _maxind;
            private readonly int _length;
            public JointMetaListMonoDescendingPairs(int maxind, int length)
            {
                _maxind = maxind;
                _length = length;
            }
            public override IEnumerator<int[]> GetEnumerator()
            {
                if (_length == 0)
                {
                    yield return new int[0];
                    yield break;
                }
                var ret = new int[_length];
                yield return ret.ToArray(_length);
                while (true)
                {
                    int incInd = 0;
                    int cascadelength = 0;
                    while (incInd != -1)
                    {
                        ret[incInd]++;
                        if (ret[incInd] == (incInd == 0 ? _maxind : ret[incInd - 1]))
                        {
                            cascadelength++;
                            incInd++;
                            if (cascadelength == _length)
                                yield break;
                        }
                        else
                        {
                            incInd = -1;
                        }
                    }
                    if (cascadelength != 0)
                    {
                        int retval = ret[cascadelength];
                        foreach (int i in range.Range(0, cascadelength))
                        {
                            ret[i] = retval;
                        }
                    }
                    yield return ret.ToArray(_length);
                }
            }
            public override int Count
            {
                get
                {
                    return (int)choose.Choose(_maxind + _length - 1, _length);
                }
            }
            public override int[] this[int index]
            {
                get
                {
                    int max = _maxind, len = _length;
                    int[] ret = new int[len];
                    int offset = 0;
                    foreach (int i in ret.Indices().Reverse())
                    {
                        var choose = new BinomialCoefficient(max+len-2,len-1);
                        int cutoff = choose.value;
                        int prevcutoff = 0;
                        int first = 0;
                        while (index >= cutoff)
                        {
                            prevcutoff = cutoff;
                            first++;
                            choose.DecreaseSuper();
                            cutoff += choose.value;
                        }
                        ret[i] = first + offset;
                        offset += first;
                        index -= prevcutoff;
                        max -= first;
                        len--;
                    }
                    return ret;
                }
            }
        }
        private class JointMetaListNoReflexivePairs : LockedList<int[]>
        {
            private readonly int _maxind;
            private readonly int _length;
            public JointMetaListNoReflexivePairs(int maxind, int length)
            {
                _maxind = maxind;
                _length = length;
            }
            public override IEnumerator<int[]> GetEnumerator()
            {
                return GetEnumerator(new SortedSet<int>(range.Range(_maxind)), _length).Select(a => a.ToArray()).GetEnumerator();
            }
            private static IEnumerable<IEnumerable<int>> GetEnumerator(ICollection<int> source, int length)
            {
                if (length > source.Count)
                {
                    yield break;
                }
                if (length == 0)
                {
                    yield return new int[0];
                    yield break;
                }
                foreach (int i in source.ToArray())
                {
                    source.Remove(i);
                    foreach (var v in GetEnumerator(source,length-1))
                    {
                        yield return v.Concat(i.Enumerate());
                    }
                    source.Add(i);
                }
            }
            public override int Count
            {
                get
                {
                    BigProduct p = new BigProduct();
                    p.MultiplyFactorial(_maxind);
                    p.DivideFactorial(_maxind - _length);
                    return p.toNum();
                }
            }
            private static IList<int> indices(ICollection<int> source, int length, int index)
            {
                if (length == 1)
                    return new[] { source.ElementAt(index) };
                BigProduct gap = new BigProduct();
                gap.MultiplyFactorial(source.Count - 1);
                gap.DivideFactorial(source.Count - length);
                int g = gap.toNum();
                var first = source.ElementAt(index / g);
                source.Remove(first);
                return indices(source, length - 1,index % g).Concat(first.Enumerate()).ToArray();
            }
            public override int[] this[int index]
            {
                get
                {
                    return indices(new SortedSet<int>(range.Range(_maxind)), _length, index).ToArray();
                }
            }
        }

        /// <summary>
        /// Get the Cartesian multiple of two <see cref="IList{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IList{T}"/>.</param>
        /// <param name="other">The second <see cref="IList{T}"/>.</param>
        /// <returns>The Cartesian multiple of <paramref name="this"/> and <paramref name="other"/>.</returns>
        public static IList<Tuple<T1, T2>> Join<T1, T2>(this IList<T1> @this, IList<T2> other)
        {
            return new JointList<T1, T2>(@this,other);
        }
        /// <summary>
        /// Get the Cartesian product of multiple <see cref="IList{T}"/>s.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> of <see cref="IList{T}"/></param>
        /// <returns>The Cartesian product of <paramref name="this"/>.</returns>
        public static IList<T[]> Join<T>(this IList<IList<T>> @this)
        {
            return new JointList<T>(@this);
        }
        /// <summary>
        /// Get the Cartesian product of multiple <see cref="IList{T}"/>s.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> of <see cref="IList{T}"/></param>
        /// <returns>The Cartesian product of <paramref name="this"/>.</returns>
        public static IList<T[]> Join<T>(this IList<T>[] @this)
        {
            return new JointList<T>(@this);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> multiplied by itself multiple times.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to exponentiate.</param>
        /// <param name="length">The exponential power.</param>
        /// <param name="t">The <see cref="CartesianType"/>.</param>
        /// <returns>The Cartesian exponential of <paramref name="this"/> by <paramref name="length"/>.</returns>
        public static IList<IList<T>> Join<T>(this IList<T> @this, int length, CartesianType t = CartesianType.AllPairs)
        {
            IList<int[]> inter;
            switch (t)
            {
                case CartesianType.AllPairs:
                    inter =  new JointMetaListAllPairs(@this.Count, length);
                    break;
                case CartesianType.NoReflexive:
                    inter = new JointMetaListNoReflexivePairs(@this.Count, length);
                    break;
                case CartesianType.NoSymmatry:
                    inter = new JointMetaListMonoDescendingPairs(@this.Count, length);
                    break;
                case CartesianType.NoReflexive | CartesianType.NoSymmatry:
                    inter = new JointMetaListDescendingPairs(@this.Count, length);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return inter.Select(a => (IList<T>)a.Select(x => @this[x]));
        }
        /// <summary>
        /// Get the Cartesian multiple of an <see cref="IList{T}"/> by itself.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to multiply.</param>
        /// <param name="t">The <see cref="CartesianType"/> of the multiplication.</param>
        /// <returns>A new <see cref="IList{T}"/> with the Cartesian multiple of <paramref name="this"/> by itself.</returns>
        public static IList<Tuple<T,T>> Join<T>(this IList<T> @this, CartesianType t = CartesianType.AllPairs)
        {
            return @this.Join(2, t).Select(a => Tuple.Create(a[0],a[1]));
        }

    }
}

using System;
using System.Collections.Generic;
using WhetStone.Fielding;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class countBind
    {
        public static IEnumerable<Tuple<T, int>> CountBind<T>(this IEnumerable<T> a, int start = 0)
        {
            return a.Zip(countUp.CountUp(start));
        }
        public static IEnumerable<Tuple<T, C>> CountBind<T, C>(this IEnumerable<T> a, C start)
        {
            return a.Zip(countUp.CountUp(start));
        }
        private class CountBindCollection<T> : LockedCollection<Tuple<T,int>>
        {
            private readonly ICollection<T> _source;
            private readonly int _start;
            public CountBindCollection(ICollection<T> source, int start)
            {
                _source = source;
                _start = start;
            }
            public override IEnumerator<Tuple<T, int>> GetEnumerator()
            {
                return _source.Zip(countUp.CountUp(_start)).GetEnumerator();
            }
            public override int Count => _source.Count;
        }
        private class CountBindCollection<T,G> : LockedCollection<Tuple<T, G>>
        {
            private readonly ICollection<T> _source;
            private readonly G _start;
            public CountBindCollection(ICollection<T> source, G start)
            {
                _source = source;
                _start = start;
            }
            public override IEnumerator<Tuple<T, G>> GetEnumerator()
            {
                return _source.Zip(countUp.CountUp(_start)).GetEnumerator();
            }
            public override int Count => _source.Count;
        }
        public static LockedCollection<Tuple<T, int>> CountBind<T>(this ICollection<T> a, int start = 0)
        {
            return new CountBindCollection<T>(a,start);
        }
        public static LockedCollection<Tuple<T, C>> CountBind<T, C>(this ICollection<T> a, C start)
        {
            return new CountBindCollection<T,C>(a, start);
        }
        private class CountBindList<T> : LockedList<Tuple<T,int>>
        {
            private readonly IList<T> _source;
            private readonly int _start;
            public CountBindList(IList<T> source, int start)
            {
                _source = source;
                _start = start;
            }
            public override IEnumerator<Tuple<T, int>> GetEnumerator()
            {
                return _source.Zip(countUp.CountUp(_start)).GetEnumerator();
            }
            public override int Count => _source.Count;
            public override Tuple<T, int> this[int index]
            {
                get
                {
                    return Tuple.Create(_source[index],index+_start);
                }
            }
        }
        private class CountBindList<T,G> : LockedList<Tuple<T, G>>
        {
            private readonly IList<T> _source;
            private readonly G _start;
            public CountBindList(IList<T> source, G start)
            {
                _source = source;
                _start = start;
            }
            public override IEnumerator<Tuple<T, G>> GetEnumerator()
            {
                return _source.Zip(countUp.CountUp(_start)).GetEnumerator();
            }
            public override int Count => _source.Count;
            public override Tuple<T, G> this[int index]
            {
                get
                {
                    Field<G> f = Fields.getField<G>();
                    return Tuple.Create(_source[index], f.add(f.fromInt(index),_start));
                }
            }
        }
        public static LockedList<Tuple<T, int>> CountBind<T>(this IList<T> a, int start = 0)
        {
            return new CountBindList<T>(a, start);
        }
        public static LockedList<Tuple<T, C>> CountBind<T, C>(this IList<T> a, C start)
        {
            return new CountBindList<T, C>(a, start);
        }
    }
}

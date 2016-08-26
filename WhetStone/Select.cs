using System;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class select
    {
        private class SelectList<T, R> : LockedList<R>
        {
            private readonly Func<T, R> _mapper;
            private readonly IList<T> _source;
            public SelectList(IList<T> source, Func<T, R> mapper)
            {
                _source = source;
                _mapper = mapper;
            }
            public override IEnumerator<R> GetEnumerator()
            {
                foreach (var v in _source)
                {
                    yield return _mapper(v);
                }
            }
            public override int Count
            {
                get
                {
                    return _source.Count;
                }
            }
            public override R this[int index]
            {
                get
                {
                    return _mapper(_source[index]);
                }
            }
        }
        private class SelectCollection<T, R> : LockedCollection<R>
        {
            private readonly Func<T, R> _mapper;
            private readonly ICollection<T> _source;
            public SelectCollection(ICollection<T> source, Func<T, R> mapper)
            {
                _source = source;
                _mapper = mapper;
            }
            public override IEnumerator<R> GetEnumerator()
            {
                foreach (var v in _source)
                {
                    yield return _mapper(v);
                }
            }
            public override int Count
            {
                get
                {
                    return _source.Count;
                }
            }
        }
        public static LockedList<R> Select<T, R>(this IList<T> @this, Func<T, R> selector)
        {
            return new SelectList<T, R>(@this, selector);
        }
        public static LockedCollection<R> Select<T, R>(this ICollection<T> @this, Func<T, R> selector)
        {
            return new SelectCollection<T, R>(@this, selector);
        }
    }
}

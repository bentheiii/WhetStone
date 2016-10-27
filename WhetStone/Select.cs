using System;
using System.Collections;
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
        private class SelectInverseList<T, R> : IList<R>
        {
            private readonly Func<T, R> _mapper;
            private readonly Func<R, T> _invers;
            private readonly IList<T> _source;
            public SelectInverseList(IList<T> source, Func<T, R> mapper, Func<R, T> invers)
            {
                _source = source;
                _mapper = mapper;
                _invers = invers;
            }
            public  IEnumerator<R> GetEnumerator()
            {
                foreach (var v in _source)
                {
                    yield return _mapper(v);
                }
            }
            public void Add(R item)
            {
                _source.Add(_invers(item));
            }
            public void Clear()
            {
                _source.Clear();
            }
            public bool Contains(R item)
            {
                return _source.Contains(_invers(item));
            }
            public void CopyTo(R[] array, int arrayIndex)
            {
                foreach (var t in this)
                {
                    array[arrayIndex++] = t;
                }
            }
            public bool Remove(R item)
            {
                return _source.Remove(_invers(item));
            }
            public  int Count
            {
                get
                {
                    return _source.Count;
                }
            }
            public bool IsReadOnly => false;
            public int IndexOf(R item)
            {
                return _source.IndexOf(_invers(item));
            }
            public void Insert(int index, R item)
            {
                _source.Insert(index,_invers(item));
            }
            public void RemoveAt(int index)
            {
                _source.RemoveAt(index);
            }
            public R this[int index]
            {
                get
                {
                    return _mapper(_source[index]);
                }
                set
                {
                    _source[index] = _invers(value);
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
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
        private class SelectInverseCollection<T, R> : ICollection<R>
        {
            private readonly Func<T, R> _mapper;
            private readonly Func<R, T> _invers;
            private readonly ICollection<T> _source;
            public SelectInverseCollection(ICollection<T> source, Func<T, R> mapper, Func<R, T> invers)
            {
                _source = source;
                _mapper = mapper;
                _invers = invers;
            }
            public IEnumerator<R> GetEnumerator()
            {
                foreach (var v in _source)
                {
                    yield return _mapper(v);
                }
            }
            public void Add(R item)
            {
                _source.Add(_invers(item));
            }
            public void Clear()
            {
                _source.Clear();
            }
            public bool Contains(R item)
            {
                return _source.Contains(_invers(item));
            }
            public void CopyTo(R[] array, int arrayIndex)
            {
                foreach (var t in this)
                {
                    array[arrayIndex++] = t;
                }
            }
            public bool Remove(R item)
            {
                return _source.Remove(_invers(item));
            }
            public int Count
            {
                get
                {
                    return _source.Count;
                }
            }
            public bool IsReadOnly => false;
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        public static LockedList<R> Select<T, R>(this IList<T> @this, Func<T, R> selector)
        {
            return new SelectList<T, R>(@this, selector);
        }
        public static IList<R> Select<T, R>(this IList<T> @this, Func<T, R> selector, Func<R, T> inverse)
        {
            return new SelectInverseList<T, R>(@this, selector, inverse);
        }
        public static LockedCollection<R> Select<T, R>(this ICollection<T> @this, Func<T, R> selector)
        {
            return new SelectCollection<T, R>(@this, selector);
        }
        public static ICollection<R> Select<T, R>(this ICollection<T> @this, Func<T, R> selector, Func<R, T> inverse)
        {
            return new SelectInverseCollection<T, R>(@this, selector, inverse);
        }
    }
}

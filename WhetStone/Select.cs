using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private class SelectValueDictionary<K,V0,V1> : LockedDictionary<K,V1>
        {
            private readonly Func<V0, V1> _mapper;
            private readonly IDictionary<K,V0> _source;
            public SelectValueDictionary(IDictionary<K, V0> source, Func<V0, V1> mapper)
            {
                _source = source;
                _mapper = mapper;
            }
            public override IEnumerator<KeyValuePair<K, V1>> GetEnumerator()
            {
                return _source.AsEnumerable().Select(a => new KeyValuePair<K, V1>(a.Key, _mapper(a.Value))).GetEnumerator();
            }
            public override int Count => _source.Count;
            public override bool TryGetValue(K key, out V1 value)
            {
                V0 s;
                bool ret = _source.TryGetValue(key, out s);
                value = _mapper(s);
                return ret;
            }
        }
        private class SelectDictionary<K0,K1,V0,V1> : IDictionary<K1,V1>
        {
            private readonly IDictionary<K0, V0> _inner;
            private readonly Func<K0, K1> _keysel;
            private readonly Func<K1, K0> _keyinv;
            private readonly Func<V0, V1> _valsel;
            private readonly Func<V1, V0> _valinv;
            private KeyValuePair<K1, V1> Sel(KeyValuePair<K0,V0> a)
            {
                    return new KeyValuePair<K1, V1>(_keysel(a.Key), _valsel(a.Value));
            }
            private KeyValuePair<K0, V0> Inv(KeyValuePair<K1, V1> a)
            {
                return new KeyValuePair<K0, V0>(_keyinv(a.Key), _valinv(a.Value));
            }
            public SelectDictionary(IDictionary<K0, V0> inner, Func<K0, K1> keysel, Func<K1, K0> keyinv, Func<V0, V1> valsel, Func<V1, V0> valinv)
            {
                _inner = inner;
                _keysel = keysel;
                _keyinv = keyinv;
                _valsel = valsel;
                _valinv = valinv;
            }
            public IEnumerator<KeyValuePair<K1, V1>> GetEnumerator()
            {
                return _inner.AsEnumerable().Select(Sel).GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            public void Add(KeyValuePair<K1, V1> item)
            {
                _inner.Add(Inv(item));
            }
            public void Clear()
            {
                _inner.Clear();
            }
            public bool Contains(KeyValuePair<K1, V1> item)
            {
                V1 val;
                return TryGetValue(item.Key, out val) && item.Value.Equals(val);
            }
            public void CopyTo(KeyValuePair<K1, V1>[] array, int arrayIndex)
            {
                _inner.AsCollection().Select(Sel).CopyTo(array, arrayIndex);
            }
            public bool Remove(KeyValuePair<K1, V1> item)
            {
                return Contains(item) && this.Remove(item.Key);
            }
            public int Count => _inner.Count;
            public bool IsReadOnly => false;
            public bool ContainsKey(K1 key)
            {
                return _inner.ContainsKey(_keyinv(key));
            }
            public void Add(K1 key, V1 value)
            {
                _inner[_keyinv(key)] = _valinv(value);
            }
            public bool Remove(K1 key)
            {
                return _inner.Remove(_keyinv(key));
            }
            public bool TryGetValue(K1 key, out V1 value)
            {
                V0 val;
                var ret = _inner.TryGetValue(_keyinv(key), out val);
                value = _valsel(val);
                return ret;
            }
            public V1 this[K1 key]
            {
                get
                {
                    return _valsel(_inner[_keyinv(key)]);
                }
                set
                {
                    _inner[_keyinv(key)] = _valinv(value);
                }
            }
            public ICollection<K1> Keys => _inner.Keys.Select(_keysel);
            public ICollection<V1> Values => _inner.Values.Select(_valsel);
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
        public static LockedDictionary<T,R1> Select<T, R0, R1>(this IDictionary<T,R0> @this, Func<R0, R1> selector)
        {
            return new SelectValueDictionary<T,R0,R1>(@this,selector);
        }
        public static IDictionary<K1, R1> Select<K0, K1, R0, R1>(this IDictionary<K0, R0> @this, Func<K0, K1> keyMapper, Func<K1, K0> keyInverse, Func<R0, R1> valueMapper, Func<R1,R0> valueInverse=null)
        {
            return new SelectDictionary<K0, K1, R0, R1>(@this, keyMapper, keyInverse, valueMapper, valueInverse);
        }
    }
}

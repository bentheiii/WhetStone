using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
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
                return _source.AsEnumerable().Select(v => _mapper(v)).GetEnumerator();
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
                return _source.AsEnumerable().Select(v => _mapper(v)).GetEnumerator();
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
                return _source.AsEnumerable().Select(v => _mapper(v)).GetEnumerator();
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
                return _source.AsEnumerable().Select(v => _mapper(v)).GetEnumerator();
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
                bool ret = _source.TryGetValue(key, out var s);
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
                if (_valinv == null)
                    throw new InvalidOperationException("The dictionary cannot be assigned new values");
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
                return TryGetValue(item.Key, out var val) && item.Value.Equals(val);
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
                if (_valinv == null)
                    throw new InvalidOperationException("The dictionary cannot be assigned new values");
                _inner[_keyinv(key)] = _valinv(value);
            }
            public bool Remove(K1 key)
            {
                return _inner.Remove(_keyinv(key));
            }
            public bool TryGetValue(K1 key, out V1 value)
            {
                var ret = _inner.TryGetValue(_keyinv(key), out var val);
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
                    if (_valinv == null)
                        throw new InvalidOperationException("The dictionary cannot be assigned new values");
                    _inner[_keyinv(key)] = _valinv(value);
                }
            }
            public ICollection<K1> Keys => _inner.Keys.Select(_keysel);
            public ICollection<V1> Values => _inner.Values.Select(_valsel);
        }
        /// <overloads>Get a 1-1 mapping of enumerables.</overloads>
        /// <summary>
        /// Get a 1-1 mapping of an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the original <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the returned <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to map.</param>
        /// <param name="selector">The mapping function.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with <paramref name="selector"/> applied on <paramref name="this"/>'s elements.</returns>
        public static IList<R> Select<T, R>(this IList<T> @this, Func<T, R> selector)
        {
            @this.ThrowIfNull(nameof(@this));
            selector.ThrowIfNull(nameof(selector));
            return new SelectList<T, R>(@this, selector);
        }
        /// <summary>
        /// Get a 1-1 invertible mapping of an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the original <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the returned <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to map.</param>
        /// <param name="selector">The mapping function.</param>
        /// <param name="inverse">The inverse of <paramref name="selector"/>.</param>
        /// <returns>A mutability passing <see cref="IList{T}"/> with <paramref name="selector"/> applied on <paramref name="this"/>'s elements.</returns>
        /// <remarks>Alongside allowing mutating <paramref name="this"/>, the return value can optimize some methods:
        /// <example>
        /// <code>
        /// var arr = range.IRange(-850,5000,9) //immutable, but searching is an O(1) operation.
        /// var range+1 = arr.Select(a=>a+1,b=>b-1)
        /// negPrimes.Contains(-53) //will perform O(1) operation, searching for -54 in an IRange.
        /// </code>
        /// </example>
        /// </remarks>
        public static IList<R> Select<T, R>(this IList<T> @this, Func<T, R> selector, Func<R, T> inverse)
        {
            @this.ThrowIfNull(nameof(@this));
            selector.ThrowIfNull(nameof(selector));
            inverse.ThrowIfNull(nameof(inverse));
            return new SelectInverseList<T, R>(@this, selector, inverse);
        }
        /// <overloads>Get a 1-1 mapping of enumerables.</overloads>
        /// <summary>
        /// Get a 1-1 mapping of an <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the original <see cref="ICollection{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the returned <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="this">The <see cref="ICollection{T}"/> to map.</param>
        /// <param name="selector">The mapping function.</param>
        /// <returns>A read-only <see cref="ICollection{T}"/> with <paramref name="selector"/> applied on <paramref name="this"/>'s elements.</returns>
        public static ICollection<R> Select<T, R>(this ICollection<T> @this, Func<T, R> selector)
        {
            @this.ThrowIfNull(nameof(@this));
            selector.ThrowIfNull(nameof(selector));
            return new SelectCollection<T, R>(@this, selector);
        }
        /// <summary>
        /// Get a 1-1 invertible mapping of an <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the original <see cref="ICollection{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the returned <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="this">The <see cref="ICollection{T}"/> to map.</param>
        /// <param name="selector">The mapping function.</param>
        /// <param name="inverse">The inverse of <paramref name="selector"/>.</param>
        /// <returns>A mutability passing <see cref="ICollection{T}"/> with <paramref name="selector"/> applied on <paramref name="this"/>'s elements.</returns>
        /// <remarks>Alongside allowing mutating <paramref name="this"/>, the return value can optimize some methods:
        /// <example>
        /// <code>
        /// var arr = range.IRange(-850,5000,9) //immutable, but searching is an O(1) operation.
        /// var range+1 = arr.Select(a=>a+1,b=>b-1)
        /// negPrimes.Contains(-53) //will perform O(1) operation, searching for -54 in an IRange.
        /// </code>
        /// </example>
        /// </remarks>
        public static ICollection<R> Select<T, R>(this ICollection<T> @this, Func<T, R> selector, Func<R, T> inverse)
        {
            @this.ThrowIfNull(nameof(@this));
            selector.ThrowIfNull(nameof(selector));
            inverse.ThrowIfNull(nameof(inverse));
            return new SelectInverseCollection<T, R>(@this, selector, inverse);
        }
        /// <summary>
        /// Get a 1-1 mapping of an <see cref="IDictionary{T,V}"/>, mapping only the values of the <see cref="KeyValuePair{T,V}"/>.'s
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="R0">The original value type.</typeparam>
        /// <typeparam name="R1">The new value type.</typeparam>
        /// <param name="this">The <see cref="IDictionary{T,V}"/> to map.</param>
        /// <param name="selector">The value selector.</param>
        /// <returns>A read-only <see cref="IDictionary{T,V}"/> with <paramref name="selector"/> applied on <paramref name="this"/>'s values.</returns>
        public static IDictionary<T,R1> Select<T, R0, R1>(this IDictionary<T,R0> @this, Func<R0, R1> selector)
        {
            @this.ThrowIfNull(nameof(@this));
            selector.ThrowIfNull(nameof(selector));
            return new SelectValueDictionary<T,R0,R1>(@this,selector);
        }
        /// <summary>
        /// Get a 1-1 invertible mapping of an <see cref="IDictionary{T,V}"/>, mapping the keys and values of the <see cref="KeyValuePair{T,V}"/>.'s
        /// </summary>
        /// <typeparam name="K0">The original key type.</typeparam>
        /// <typeparam name="K1">The new key type.</typeparam>
        /// <typeparam name="R0">The original value type.</typeparam>
        /// <typeparam name="R1">The new value type.</typeparam>
        /// <param name="this">The <see cref="IDictionary{T,V}"/> to map.</param>
        /// <param name="keyMapper">The original->new key mapper.</param>
        /// <param name="keyInverse">The new->original key mapper.</param>
        /// <param name="valueMapper">The original->new value mapper.</param>
        /// <param name="valueInverse">The new->original value mapper. If <see langword="null"/>, the resultant <see cref="IDictionary{T,V}"/> cannot be assigned new values.</param>
        /// <returns>A mutability-passing <see cref="IDictionary{T,V}"/> with <paramref name="keyMapper"/> and <paramref name="valueMapper"/> applied to <paramref name="this"/>'s elements.</returns>
        public static IDictionary<K1, R1> Select<K0, K1, R0, R1>(this IDictionary<K0, R0> @this, Func<K0, K1> keyMapper, Func<K1, K0> keyInverse, Func<R0, R1> valueMapper, Func<R1,R0> valueInverse=null)
        {
            @this.ThrowIfNull(nameof(@this));
            keyMapper.ThrowIfNull(nameof(keyMapper));
            keyInverse.ThrowIfNull(nameof(keyInverse));
            valueMapper.ThrowIfNull(nameof(valueMapper));
            return new SelectDictionary<K0, K1, R0, R1>(@this, keyMapper, keyInverse, valueMapper, valueInverse);
        }
    }
}

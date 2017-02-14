using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    //todo test this, it might very well not be working
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class cache
    {
        /// <overloads><summary>Stores the values of the enumerable in an cached container.</summary></overloads>
        /// <summary>
        /// Caches the <see cref="IEnumerable{T}"/>, causing it to enumerate once at most.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to be cached.</param>
        /// <param name="bound">If set to an integer, only that number of elements will be cached.</param>
        /// <returns>A new structure, wrapping <paramref name="this"/> and storing its elements as they are enumerated.</returns>
        public static LockedList<T> Cache<T>(this IEnumerable<T> @this, int? bound = null)
        {
            if (bound == null)
                return new EnumerableCache<T>(@this);
            return new EnumerableCacheBound<T>(@this, bound.Value);
        }
        /// <summary>
        /// Caches the <see cref="IList{T}"/>, causing it to enumerate once at most.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to be cached.</param>
        /// <param name="bound">If set to an integer, only that number of elements will be cached.</param>
        /// <returns>A new structure, wrapping <paramref name="this"/> and storing its elements as they are enumerated.</returns>
        /// <remarks>Because of the way caching works, and that lists may be partially cached, there are two ways enumerating a cache can be done. See <see cref="IListCache{T}"/>.</remarks>
        public static IListCache<T> Cache<T>(this IList<T> @this, int? bound = null)
        {
            if (bound == null)
                return new ListCache<T>(@this);
            return new ListCacheBound<T>(@this,bound.Value);
        }
        private class EnumerableCacheBound<T> : LockedList<T>
        {
            private readonly IEnumerator<Tuple<T,int>> _tor;
            private readonly IEnumerable<T> _source;
            private readonly T[] _cache;
            private int _initcount = 0;
            public EnumerableCacheBound(IEnumerable<T> tor, int bound)
            {
                _source = tor.CountBind().Select(a =>
                {
                    if (a.Item2 >= _initcount)
                        _initcount = a.Item2+1;
                    if (a.Item2 < bound)
                        _cache[a.Item2] = a.Item1;
                    return a.Item1;
                });
                _tor = tor.CountBind().Select(a =>
                {
                    if(a.Item2 >= _initcount)
                        _initcount = a.Item2 + 1;
                    if (a.Item2 < bound)
                        _cache[a.Item2] = a.Item1;
                    return a;
                }).GetEnumerator();
                _cache = new T[bound];
            }
            public override T this[int ind]
            {
                get
                {
                    if (ind < 0)
                        throw new ArgumentOutOfRangeException("ind cannot be negative");
                    return ind < _initcount ? _cache[ind] : _source.ElementAt(ind);
                }
            }
            public override IEnumerator<T> GetEnumerator()
            {
                int cachesize = Math.Min(_initcount, _cache.Length);
                foreach (var t in _cache.Take(cachesize))
                {
                    yield return t;
                }
                if (_tor.Current == null || _tor.Current.Item2 <= cachesize)
                {
                    while (_tor.Current == null || _tor.Current.Item2 < cachesize)
                    {
                        _tor.MoveNext();
                    }
                    bool cont = true;
                    while (cont)
                    {
                        yield return _tor.Current.Item1;
                        cont = _tor.MoveNext();
                    }
                }
                else
                {
                    foreach (var t in _source.Skip(cachesize))
                    {
                        yield return t;
                    }
                }
            }
            public override int Count
            {
                get
                {
                    return _source.Count();
                }
            }
        }
        private class EnumerableCache<T> : LockedList<T>
        {
            private readonly IEnumerator<T> _tor;
            private readonly int? _sourceSize;
            private readonly IList<T> _cache;
            private bool _formed = false;
            public EnumerableCache(IEnumerable<T> tor)
            {
                _sourceSize = tor.RecommendCount();
                _tor = tor.GetEnumerator();
                _cache = new List<T>();
            }
            private bool InflateToIndex(int? index)
            {
                while (!index.HasValue || _cache.Count <= index)
                {
                    if (_formed || !_tor.MoveNext())
                    {
                        _formed = true;
                        return false;
                    }
                    _cache.Add(_tor.Current);
                }
                return true;
            }
            public override T this[int ind]
            {
                get
                {
                    if (ind < 0)
                        throw new ArgumentOutOfRangeException("ind cannot be negative");
                    if (!InflateToIndex(ind))
                        throw new ArgumentOutOfRangeException("IEnumerator ended unexpectedly");
                    return _cache[ind];
                }
            }
            public override IEnumerator<T> GetEnumerator()
            {
                int i = 0;
                while (InflateToIndex(i))
                {
                    yield return _cache[i];
                    i++;
                }
            }
            public override int Count
            {
                get
                {
                    if (_sourceSize.HasValue)
                        return _sourceSize.Value;
                    if (!_formed)
                        InflateToIndex(null);
                    return _cache.Count;
                }
            }
        }
        /// <summary>
        /// An abstract class for Cached lists, allowing two ways to enumerate.
        /// </summary>
        /// <typeparam name="T">The type of the cached list.</typeparam>
        public abstract class IListCache<T> : IList<T>
        {
            /// <summary>
            /// Returns an enumerable, that returns its elements through <see cref="IList{T}.this"/> operator. This means that only elements not cached will be accessed.
            /// </summary>
            /// <returns>An <see cref="IEnumerable{T}"/> with specialized caching</returns>
            public IEnumerable<T> GetEnumeratorRandAccess()
            {
                return this.Indices().Select(index => this[index]);
            }
            /// <inheritdoc />
            /// <remarks>This uses the cache until it needs to access an element it has not cached. When this happens it will run an <see cref="IEnumerator{T}"/> up to that element and continue.</remarks>
            public abstract IEnumerator<T> GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            /// <inheritdoc />
            public abstract void Add(T item);
            /// <inheritdoc />
            public abstract void Clear();
            /// <inheritdoc />
            public abstract bool Contains(T item);
            /// <inheritdoc />
            public abstract void CopyTo(T[] array, int arrayIndex);
            /// <inheritdoc />
            public abstract bool Remove(T item);
            /// <inheritdoc />
            public abstract int Count { get; }
            /// <inheritdoc />
            public abstract bool IsReadOnly { get; }
            /// <inheritdoc />
            public abstract int IndexOf(T item);
            /// <inheritdoc />
            public abstract void Insert(int index, T item);
            /// <inheritdoc />
            public abstract void RemoveAt(int index);
            /// <inheritdoc />
            public abstract T this[int index] { get; set; }
        }
        private class ListCache<T> : IListCache<T>
        {
            private readonly IList<T> _source;
            private readonly IList<T> _cache;
            private readonly BitList _initialized;
            public ListCache(IList<T> source)
            {
                _source = source;
                int count = _source.Count;
                _cache = new List<T>(range.Range(count).Select(a=>default(T)));
                _initialized = new BitList(count);
            }
            private void initialize(int ind)
            {
                _cache[ind] = _source[ind];
                _initialized[ind] = true;
            }
            public override T this[int ind]
            {
                get
                {
                    if (ind < 0)
                        throw new ArgumentOutOfRangeException("ind cannot be negative");
                    if (!_initialized[ind])
                        initialize(ind);
                    return _cache[ind];
                }
                set
                {
                    _initialized[ind] = true;
                    _cache[ind] = _source[ind] = value;
                }
            }
            public override IEnumerator<T> GetEnumerator()
            {
                using (var tor = _source.GetEnumerator())
                {
                    int torind = -1;
                    foreach (var toYieldInd in range.Range(this.Count))
                    {
                        if (_initialized[toYieldInd])
                        {
                            yield return _cache[toYieldInd];
                        }
                        else
                        {
                            foreach(var _ in range.Range(torind,toYieldInd))
                            {
                                if (!tor.MoveNext())
                                    yield break;
                            }
                            _cache[toYieldInd] = tor.Current;
                            _initialized[toYieldInd] = true;
                            yield return tor.Current;
                            torind = toYieldInd;
                        }
                    }
                }
            }
            public override void Add(T item)
            {
                _source.Add(item);
                _cache.Add(item);
                _initialized.Add(true);
            }
            public override bool Remove(T item)
            {
                var i = IndexOf(item);
                if (i == -1)
                    return false;
                RemoveAt(i);
                return true;
            }
            public override int Count => _source.Count;
            public override bool IsReadOnly => _source.IsReadOnly;
            public override int IndexOf(T item)
            {
                int ret = 0;
                foreach (var v in this)
                {
                    if (v.Equals(item))
                        return ret;
                    ret++;
                }
                return -1;
            }
            public override void Insert(int index, T item)
            {
                _source.Insert(index,item);
                _cache.Insert(index,item);
                _initialized.Insert(index,true);
            }
            public override void Clear()
            {
                _source.Clear();
                _initialized.Clear();
                _cache.Clear();
            }
            public override bool Contains(T item)
            {
                return IndexOf(item) > 0;
            }
            public override void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var t in this)
                {
                    array[arrayIndex++] = t;
                }
            }
            public override void RemoveAt(int index)
            {
                _source.RemoveAt(index);
                _initialized.RemoveAt(index);
                _cache.RemoveAt(index);
            }
        }
        private class ListCacheBound<T> : IListCache<T>
        {
            private readonly IList<T> _source;
            private readonly T[] _cache;
            private readonly BitArray _initialized;
            public ListCacheBound(IList<T> source, int bound)
            {
                _source = source;
                int count = _source.Count;
                if (bound > count)
                    bound = count;
                _cache = new T[bound];
                _initialized = new BitArray(bound);
            }
            private void initialize(int ind)
            {
                _cache[ind] = _source[ind];
                _initialized[ind] = true;
            }
            public override void RemoveAt(int index)
            {
                _source.RemoveAt(index);
                if (index < _cache.Length)
                {
                    if (index > 0)
                        _initialized.And(new BitArray(index - 1, true));
                    else
                        _initialized.SetAll(false);
                }
            }
            public override T this[int ind]
            {
                get
                {
                    if (ind < 0)
                        throw new ArgumentOutOfRangeException("ind cannot be negative");
                    if (ind < _cache.Length)
                    {
                        if (!_initialized[ind])
                            initialize(ind);
                        return _cache[ind];
                    }
                    return _source[ind];
                }
                set
                {
                    _initialized[ind] = true;
                    _cache[ind] = _source[ind] = value;
                }
            }
            public override IEnumerator<T> GetEnumerator()
            {
                using (var tor = _source.GetEnumerator())
                {
                    int torind = -1;
                    foreach (var toYieldInd in range.Range(this.Count))
                    {
                        if (_initialized[toYieldInd])
                        {
                            yield return _cache[toYieldInd];
                        }
                        else
                        {
                            foreach (var _ in range.Range(torind, toYieldInd))
                            {
                                if (!tor.MoveNext())
                                    yield break;
                            }
                            _cache[toYieldInd] = tor.Current;
                            _initialized[toYieldInd] = true;
                            yield return tor.Current;
                            torind = toYieldInd;
                        }
                    }
                }
            }
            public override void Add(T item)
            {
                _source.Add(item);
            }
            public override void Clear()
            {
                _source.Clear();
                _initialized.SetAll(false);
            }
            public override bool Contains(T item)
            {
                return IndexOf(item) >= 0;
            }
            public override void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var t in this)
                {
                    array[arrayIndex++] = t;
                }
            }
            public override bool Remove(T item)
            {
                var i = IndexOf(item);
                if (i == -1)
                    return false;
                RemoveAt(i);
                return true;
            }
            public override int Count => _source.Count;
            public override bool IsReadOnly => _source.IsReadOnly;
            public override int IndexOf(T item)
            {
                return _source.Indices().FirstOrDefault(a => this[a].Equals(item), -1);
            }
            public override void Insert(int index, T item)
            {
                _source.Insert(index,item);
                if (index < _cache.Length)
                {
                    if (index > 0)
                        _initialized.And(new BitArray(index - 1, true));
                    else
                        _initialized.SetAll(false);
                }
            }
        }
    }
}

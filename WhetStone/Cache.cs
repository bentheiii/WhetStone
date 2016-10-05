﻿using System;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class cache
    {
        public static LockedList<T> Cache<T>(this IEnumerable<T> @this)
        {
            return new EnumerableCache<T>(@this);
        }
        public static IList<T> Cache<T>(this IList<T> @this)
        {
            return new LazyArray<T>(i => @this[i]);
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
    }
}

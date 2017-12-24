﻿using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class trail
    {
        private class TrailList<T> : LockedList<IList<T>>
        {
            private readonly IList<T> _source;
            private readonly int _trailLength;
            public TrailList(IList<T> source, int trailLength)
            {
                _source = source;
                _trailLength = trailLength;
            }
            public override IEnumerator<IList<T>> GetEnumerator()
            {
                return range.Range(Count)
                    .Select(a => this[a])
                    .GetEnumerator();
            }
            public override int Count => _source.Count - _trailLength + 1;
            public override IList<T> this[int index]
            {
                get
                {
                    return _source.Slice(index, length: _trailLength);
                }
            }
        }
        /// <summary>
        /// Get all the sub-lists of an <see cref="IList{T}"/> of a specific length.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/>to use.</param>
        /// <param name="trailLength">The length of the sub-lists.</param>
        /// <param name="wrap">Whether to wrap the list for the sake of the last elements.</param>
        /// <returns>a read-only <see cref="IList{T}"/> of all the sub-lists of <paramref name="this"/> of length <paramref name="trailLength"/>.</returns>
        public static IList<IList<T>> Trail<T>(this IList<T> @this, int trailLength, bool wrap = false)
        {
            @this.ThrowIfNull(nameof(@this));
            trailLength.ThrowIfAbsurd(nameof(trailLength),false);
            if (wrap)
            {
                @this = @this.Concat(@this.Slice(0,trailLength - 1));
            }
            return new TrailList<T>(@this,trailLength);
        }
        /// <summary>
        /// Get all the sub-lists of an <see cref="IEnumerable{T}"/> of a specific length.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/>to use.</param>
        /// <param name="trailLength">The length of the sub-lists.</param>
        /// <param name="wrap">Whether to wrap the list for the sake of the last elements.</param>
        /// <returns>All the sub-lists of <paramref name="this"/> of length <paramref name="trailLength"/>.</returns>
        public static IEnumerable<T[]> Trail<T>(this IEnumerable<T> @this, int trailLength, bool wrap = false)
        {
            @this.ThrowIfNull(nameof(@this));
            trailLength.ThrowIfAbsurd(nameof(trailLength), false);
            var buffer = new LinkedList<T>();
            if (wrap)
            {
                @this = @this.Concat(@this.Take(trailLength - 1));
            }
            foreach (T t in @this)
            {
                buffer.AddLast(t);
                while (buffer.Count > trailLength)
                {
                    buffer.RemoveFirst();
                }
                if (buffer.Count == trailLength)
                {
                    yield return buffer.ToArray(trailLength);
                }
            }
        }
    }
}

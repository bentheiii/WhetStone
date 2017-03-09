using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class compareCount
    {
        /// <summary>
        /// Compares the lengths of two <see cref="IEnumerable{T}"/>s, avoiding enumerating more than needed.
        /// </summary>
        /// <typeparam name="T0">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T1">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/> to compare.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/> to compare.</param>
        /// <returns>-1 if <paramref name="this"/> is shorter, 1 if <paramref name="other"/> is shorter, 0 if they have the same length.</returns>
        /// <remarks>If either <see cref="IEnumerable{T}"/>s are <see cref="asCollection.AsCollection{T}"/> compatible, they will not be enumerated at all.</remarks>
        public static int CompareCount<T0, T1>(this IEnumerable<T0> @this, IEnumerable<T1> other)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));

            int? rect = @this.RecommendCount();
            int? reco = other.RecommendCount();

            if (reco.HasValue && rect.HasValue)
                return rect.Value.CompareTo(reco.Value);
            if (reco.HasValue) //rect is null
            {
                int c = 0;
                foreach (var t0 in @this)
                {
                    c++;
                    if (c >= reco.Value + 1)
                        return 1;
                }
                return c == reco.Value ? 0 : -1;
            }
            if (rect.HasValue) //reco is null
            {
                int c = 0;
                foreach (var t0 in other)
                {
                    c++;
                    if (c >= rect.Value + 1)
                        return -1;
                }
                return c == rect.Value ? 0 : 1;
            }


            var tor = new IEnumerator[] {@this.GetEnumerator(), other.GetEnumerator()}.AsEnumerable();
            var next = tor.Select(a => a.MoveNext()).ToArray();
            while (next.All())
            {
                next = tor.Select(a => a.MoveNext()).ToArray();
            }
            if (next[0] == next[1])
                return 0;
            return next[0] ? 1 : -1;
        }
    }
}

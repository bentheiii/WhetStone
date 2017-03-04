using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class occurances
    {
        /// <summary>
        /// Transforms a sorted <see cref="IEnumerable{T}"/> into an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2}"/> of members and their multiplicity.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to transform.</param>
        /// <param name="c">The <see cref="IEqualityComparer{T}"/> to check for equality. <see langword="null"/> means default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2}"/> of members and their multiplicity.</returns>
        /// <remarks>
        /// <para><paramref name="this"/> doesn't have to be sorted, it just has to have all elements equal to each other adjacent.</para>
        /// <para>Alternately, all non-adjacent equal elements will be treated as non-equal.</para>
        /// </remarks>
        public static IEnumerable<Tuple<T,int>> ToOccurancesSorted<T>(this IEnumerable<T> @this, IEqualityComparer<T> c = null)
        {
            c = c ?? EqualityComparer<T>.Default;
            using (var tor = @this.GetEnumerator())
            {
                int ret = 0;
                T mem = default(T);
                foreach (var t in @this)
                {
                    if (ret == 0)
                    {
                        ret = 1;
                        mem = t;
                        continue;
                    }
                    if (c.Equals(mem, t))
                    {
                        ret++;
                    }
                    else
                    {
                        yield return Tuple.Create(mem, ret);
                        mem = t;
                        ret = 1;
                    }
                }
                if (ret != 0)
                    yield return Tuple.Create(mem, ret);
            }
        }
        /// <summary>
        /// Transforms an <see cref="IEnumerable{T}"/> into an <see cref="IDictionary{T,G}"/> of <see cref="Tuple{T1,T2}"/> of members and their multiplicity.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="arr">The <see cref="IEnumerable{T}"/> to transform.</param>
        /// <param name="c">The <see cref="IEqualityComparer{T}"/> to check for equality. <see langword="null"/> means default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>An <see cref="IDictionary{T,G}"/> of <see cref="Tuple{T1,T2}"/> of members and their multiplicity.</returns>
        public static IDictionary<T, int> ToOccurances<T>(this IEnumerable<T> arr, IEqualityComparer<T> c = null)
        {
            c = c ?? EqualityComparer<T>.Default;
            Dictionary<T, int> oc = new Dictionary<T, int>(c);
            foreach (T v in arr)
            {
                if (oc.ContainsKey(v))
                    oc[v]++;
                else
                    oc[v] = 1;
            }
            return oc;
        }
    }
}

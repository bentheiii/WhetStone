using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class balanced
    {
        /// <summary>
        /// Checks whether an <see cref="IEnumerable{T}"/> is balanced, in terms of openers and closers.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check for balance.</param>
        /// <param name="opener">The <see cref="IEnumerable{T}"/> that represents a parenthesis opening.</param>
        /// <param name="closer">The <see cref="IEnumerable{T}"/> that represents a parenthesis closing.</param>
        /// <param name="maxdepth">The maximum depths of parenthesis allowed, on <see langword="null"/> for no depth limit. <paramref name="maxdepth"/> is inclusive</param>
        /// <returns>Whether <paramref name="this"/> is balanced, and it's maximum depth is no more than <paramref name="maxdepth"/>, if one is stated.</returns>
        public static bool Balanced<T>(this IEnumerable<T> @this, IEnumerable<T> opener, IEnumerable<T> closer, int? maxdepth = null)
        {
            @this.ThrowIfNull(nameof(@this));
            opener.ThrowIfNull(nameof(opener));
            closer.ThrowIfNull(nameof(closer));
            if ((maxdepth ?? 1) < 0)
                return false;
            if (opener.Equals(closer))
            {
                int c = @this.Count(opener);
                return c % 2 == 0 && (!maxdepth.HasValue || c == 0 || maxdepth >= 1);
            }
            var openerindicies = @this.Trail(opener.Count()).CountBind().Where(a => a.Item1.SequenceEqual(opener)).Select(a => a.Item2);
            var closerindicies = @this.Trail(closer.Count()).CountBind().Where(a => a.Item1.SequenceEqual(closer)).Select(a => a.Item2);
            var parenonly = openerindicies.Attach(a => 0).Concat(closerindicies.Attach(a => 1)).OrderBy(a => a.Item1).Select(a => a.Item2);
            return parenonly.Balanced(0, 1, maxdepth);
        }
        /// <summary>
        /// Checks whether an <see cref="IEnumerable{T}"/> is balanced, in terms of openers and closers.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check for balance.</param>
        /// <param name="opener">The element that represents a parenthesis opening.</param>
        /// <param name="closer">The element that represents a parenthesis closing.</param>
        /// <param name="maxdepth">The maximum depths of parenthesis allowed, on <see langword="null"/> for no depth limit. <paramref name="maxdepth"/> is inclusive</param>
        /// <returns>Whether <paramref name="this"/> is balanced, and it's maximum depth is no more than <paramref name="maxdepth"/>, if one is stated.</returns>
        /// <remarks>In case of an <see cref="ICollection{T}"/> or <see cref="asCollection.AsCollection{T}"/>-compatible <paramref name="this"/> type, the algorithm might break the enumeration if it detects that balance is impossible. 
        /// For example: <c>"((((((the next parentheses will not be enumerated)))"</c></remarks>
        public static bool Balanced<T>(this IEnumerable<T> @this, T opener, T closer, int? maxdepth = null)
        {
            @this.ThrowIfNull(nameof(@this));
            if ((maxdepth ?? 1) < 0)
                return false;
            if (opener.Equals(closer))
            {
                int c = @this.Count(opener);
                return c % 2 == 0 && (!maxdepth.HasValue || c == 0 || maxdepth >= 1);
            }
            var count = @this.RecommendCount();
            int ret = 0;
            foreach ((var t, var index) in @this.CountBind())
            {
                if (t.Equals(opener))
                {
                    ret++;
                    if (count.HasValue && count.Value - index < ret)
                        return false;
                    if (maxdepth.HasValue && maxdepth.Value < ret)
                        return false;
                }
                else if (t.Equals(closer))
                {
                    ret--;
                    if (ret < 0)
                        return false;
                }
            }
            return ret == 0;
        }
        /// <summary>
        /// Checks whether an <see cref="IEnumerable{T}"/> is balanced, in terms of openers and closers.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check for balance.</param>
        /// <param name="couples">An <see cref="IEnumerable{T}"/> of openers and closers of many types, each one closing and opening only itself.</param>
        /// <param name="maxdepth">The maximum depths of parenthesis allowed, on <see langword="null"/> for no depth limit. <paramref name="maxdepth"/> is inclusive</param>
        /// <returns>Whether <paramref name="this"/> is balanced, and it's maximum depth is no more than <paramref name="maxdepth"/>, if one is stated.</returns>
        /// <remarks>In case of an <see cref="ICollection{T}"/> or <see cref="asCollection.AsCollection{T}"/>-compatible <paramref name="this"/> type, the algorithm might break the enumeration if it detects that balance is impossible. 
        /// For example: <c>"((((((the next parentheses will not be enumerated)))"</c></remarks>
        public static bool Balanced<T>(this IEnumerable<T> @this, IEnumerable<Tuple<T, T>> couples, int? maxdepth = null)
        {
            @this.ThrowIfNull(nameof(@this));
            couples.ThrowIfNull(nameof(couples));
            if ((maxdepth ?? 1) < 0)
                return false;
            Stack<T> layers = new Stack<T>(maxdepth ?? 0);
            var dic = couples.ToDictionary();
            var count = @this.RecommendCount();
            Guard<int> index = new Guard<int>();
            foreach (T t in @this)
            {
                if (dic.ContainsKey(t))
                {
                    if (maxdepth.HasValue && layers.Count >= maxdepth.Value)
                        return false;
                    layers.Push(t);
                }
                else if (dic.Values.Contains(t))
                {
                    if (layers.Count == 0 || !dic[layers.Pop()].Equals(t))
                        return false;
                }
                if (count.HasValue && count.Value - index < layers.Count)
                    return false;
            }
            return layers.Count == 0;
        }
    }
}

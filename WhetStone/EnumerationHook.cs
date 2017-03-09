using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class enumerationHook
    {
        /// <overloads>Binds an action to an enumerable, to be executed upon enumeration.</overloads>
        /// <summary>
        /// Binds actions to an <see cref="IEnumerable{T}"/>, causing the action to call whenever the <see cref="IEnumerable{T}"/> is enumerated.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="preYield">An <see cref="Action{T}"/> to call before the element is returned to the enumeration caller.</param>
        /// <param name="postYield">An <see cref="Action{T}"/> to call before the element is returned to the enumeration caller (if the next item is requested).</param>
        /// <param name="begin">An <see cref="Action"/> to call before any element is enumerated.</param>
        /// <param name="end">An <see cref="Action"/> to call after all elements are enumerated.</param>
        /// <returns>An enumerable with the same elements, but whenever enumerated, will trigger a hooked method.</returns>
        /// <example>
        /// Say you want to find the sum and count of an <see cref="IEnumerable{T}"/> <c>val</c> while only enumerating it once.
        /// <code>
        /// int count = 0;
        /// int sum = val.EnumerationHook(a=>count++).Sum();
        /// </code>
        /// </example>
        public static IEnumerable<T> EnumerationHook<T>(this IEnumerable<T> @this, Action<T> preYield = null, Action<T> postYield = null, Action begin = null, Action end = null)
        {
            @this.ThrowIfNull(nameof(@this));
            begin?.Invoke();
            foreach (var t in @this)
            {
                preYield?.Invoke(t);
                yield return t;
                postYield?.Invoke(t);
            }
            end?.Invoke();
        }
        //if a function returns false, that function will not be called again
        /// <summary>
        /// Binds actions to an <see cref="IEnumerable{T}"/>, causing the action to call whenever the <see cref="IEnumerable{T}"/> is enumerated.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="preYield">An <see cref="Action{T}"/> to call before the element is returned to the enumeration caller. If the function returns <see langword="false"/>, it will not be called again (but the <see cref="IEnumerable{T}"/> will still be enumerated).</param>
        /// <param name="postYield">An <see cref="Action{T}"/> to call before the element is returned to the enumeration caller (if the next item is requested). If the function returns <see langword="false"/>, it will not be called again (but the <see cref="IEnumerable{T}"/> will still be enumerated).</param>
        /// <returns>An enumerable with the same elements, but whenever enumerated, will trigger a hooked method.</returns>
        public static IEnumerable<T> EnumerationHook<T>(this IEnumerable<T> @this, Func<T,bool> preYield = null, Func<T,bool> postYield = null)
        {
            @this.ThrowIfNull(nameof(@this));
            using (var tor = @this.GetEnumerator())
            {
                //phase 0, both active
                if (preYield != null && postYield != null)
                {
                    while (tor.MoveNext())
                    {
                        var t = tor.Current;
                        var anybreak = false;
                        if (!preYield.Invoke(t))
                        {
                            anybreak = true;
                            preYield = null;
                        }
                        yield return t;
                        if (!postYield.Invoke(t))
                        {
                            anybreak = true;
                            postYield = null;
                        }
                        if (anybreak)
                            break;
                    }
                }
                //phase 1, pre-active
                if (preYield != null)
                {
                    while (tor.MoveNext())
                    {
                        var t = tor.Current;
                        bool anybreak = !preYield.Invoke(t);
                        yield return t;
                        if (anybreak)
                            break;
                    }
                }
                //phase 2, post-active (will not be accessed if phase 1 was accessed)
                else if (postYield != null)
                {
                    while (tor.MoveNext())
                    {
                        var t = tor.Current;
                        yield return t;
                        if (!postYield.Invoke(t))
                            break;
                    }
                }
                //phase 3, none active
                while (tor.MoveNext())
                {
                    var t = tor.Current;
                    yield return t;
                }
            }
        }

    }
}

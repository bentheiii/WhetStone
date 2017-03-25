using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class detach
    {
        /// <overloads>
        /// <summary>Transform an enumerable of tuples into only the tuple's first-most elements, assigning the leftover elements to guard objects upon enumeration.</summary>
        /// <remarks>This method is intended to be removed upon entry to C# 7 with tuple unpacking.</remarks>
        /// </overloads>
        /// <summary>
        /// Transform an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2}"/>s into only the <see cref="Tuple{T1,T2}"/>'s first element, assigning the second element to an <see cref="IGuard{T2}"/> upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2}"/> members.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to transform</param>
        /// <param name="informer1">The <see cref="IGuard{T}"/> to which put the second values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with only the first member of each <see cref="Tuple{T1,T2}"/> member. When an element is enumerated, the second element of the original enumerated member is set to be <paramref name="informer1"/>s value.</returns>
        /// <example>
        /// <code>
        /// var names = new [] {"Alice","Bob","Clara","Danny","Emily"};
        /// var index = new Guard&lt;int&gt;();
        /// foreach (var name in names.CountBind(start:1).Detach(index)){
        ///     Console.WriteLine($"name #{index} is {name}");
        /// }
        /// </code>
        /// </example>
        public static IEnumerable<T1> Detach<T1, T2>(this IEnumerable<Tuple<T1, T2>> @this, IGuard<T2> informer1 = null)
        {
            @this.ThrowIfNull(nameof(@this));
            foreach (var t in @this)
            {
                informer1.CondSet(t.Item2);
                yield return t.Item1;
            }
        }
        /// <summary>
        /// Transform an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3}"/>s into only the <see cref="Tuple{T1,T2,T3}"/>'s first element, assigning the second and third elements to <see cref="IGuard{T}"/>s upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to transform</param>
        /// <param name="informer1">The <see cref="IGuard{T}"/> to which put the second values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <param name="informer2">The <see cref="IGuard{T}"/> to which put the third values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with only the first member of each <see cref="Tuple{T1,T2}"/> member. When an element is enumerated, the second element of the original enumerated member is set to be <paramref name="informer1"/>s value.</returns>
        public static IEnumerable<T1> Detach<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> @this, IGuard<T2> informer1, IGuard<T3> informer2)
        {
            @this.ThrowIfNull(nameof(@this));
            foreach (var t in @this)
            {
                informer1.CondSet(t.Item2);
                informer2.CondSet(t.Item3);
                yield return t.Item1;
            }
        }
        /// <summary>
        /// Transform an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3}"/>s into only the <see cref="Tuple{T1,T2,T3}"/>'s first and second elements, assigning the third element to an <see cref="IGuard{T}"/> upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to transform</param>
        /// <param name="informer1">The <see cref="IGuard{T}"/> to which put the third values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with only the first member of each tuple member. When an element is enumerated, the remaining elements of the original enumerated member is set to be the informer's values.</returns>
        public static IEnumerable<Tuple<T1, T2>> Detach<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> @this, IGuard<T3> informer1 = null)
        {
            @this.ThrowIfNull(nameof(@this));
            foreach (var t in @this)
            {
                informer1.CondSet(t.Item3);
                yield return Tuple.Create(t.Item1, t.Item2);
            }
        }
        /// <summary>
        /// Transform an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3,T4}"/>s into only the <see cref="Tuple{T1,T2,T3,T4}"/>'s first element, assigning the remaining elements to <see cref="IGuard{T}"/>s upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to transform</param>
        /// <param name="informer1">The <see cref="IGuard{T}"/> to which put the second values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <param name="informer2">The <see cref="IGuard{T}"/> to which put the third values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <param name="informer3">The <see cref="IGuard{T}"/> to which put the fourth values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with only the first member of each tuple member. When an element is enumerated, the remaining elements of the original enumerated member is set to be the informer's values.</returns>
        public static IEnumerable<T1> Detach<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this, IGuard<T2> informer1, IGuard<T3> informer2, IGuard<T4> informer3)
        {
            @this.ThrowIfNull(nameof(@this));
            foreach (var t in @this)
            {
                informer1.CondSet(t.Item2);
                informer2.CondSet(t.Item3);
                informer3.CondSet(t.Item4);
                yield return t.Item1;
            }
        }
        /// <summary>
        /// Transform an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3,T4}"/>s into only the <see cref="Tuple{T1,T2,T3,T4}"/>'s first and second elements, assigning the remaining elements to <see cref="IGuard{T}"/>s upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to transform</param>
        /// <param name="informer2">The <see cref="IGuard{T}"/> to which put the third values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <param name="informer3">The <see cref="IGuard{T}"/> to which put the fourth values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with only the first member of each tuple member. When an element is enumerated, the remaining elements of the original enumerated member is set to be the informer's values.</returns>
        public static IEnumerable<Tuple<T1, T2>> Detach<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this, IGuard<T3> informer2, IGuard<T4> informer3)
        {
            @this.ThrowIfNull(nameof(@this));
            foreach (var t in @this)
            {
                informer2.CondSet(t.Item3);
                informer3.CondSet(t.Item4);
                yield return Tuple.Create(t.Item1, t.Item2);
            }
        }
        /// <summary>
        /// Transform an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2,T3,T4}"/>s into only the <see cref="Tuple{T1,T2,T3,T4}"/>'s first, second, and third elements, assigning the remaining elements to <see cref="IGuard{T}"/>s upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to transform</param>
        /// <param name="informer3">The <see cref="IGuard{T}"/> to which put the fourth values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with only the first member of each tuple member. When an element is enumerated, the remaining elements of the original enumerated member is set to be the informer's values.</returns>
        public static IEnumerable<Tuple<T1, T2, T3>> Detach<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this, IGuard<T4> informer3 = null)
        {
            @this.ThrowIfNull(nameof(@this));
            foreach (var t in @this)
            {
                informer3.CondSet(t.Item4);
                yield return Tuple.Create(t.Item1, t.Item2, t.Item3);
            }
        }

        private class DetachList<T1,T2> : LockedList<T1>
        {
            private readonly IList<Tuple<T1, T2>> _source;
            private readonly IGuard<T2> _informer;
            public DetachList(IList<Tuple<T1, T2>> source, IGuard<T2> informer)
            {
                _source = source;
                _informer = informer;
            }
            public override IEnumerator<T1> GetEnumerator()
            {
                return _source.AsEnumerable().Detach(_informer).GetEnumerator();
            }
            public override int Count => _source.Count;
            public override T1 this[int index]
            {
                get
                {
                    var val = _source[index];
                    _informer.CondSet(val.Item2);
                    return val.Item1;
                }
            }
        }
        /// <summary>
        /// Transform an <see cref="IList{T}"/> of <see cref="Tuple{T1,T2}"/>s into only the <see cref="Tuple{T1,T2}"/>'s first element, assigning the second element to an <see cref="IGuard{T2}"/> upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2}"/> members.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to transform</param>
        /// <param name="informer1">The <see cref="IGuard{T}"/> to which put the second values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IList{T}"/> with only the first member of each <see cref="Tuple{T1,T2}"/> member. When an element is enumerated, the second element of the original enumerated member is set to be <paramref name="informer1"/>s value.</returns>
        public static IList<T1> Detach<T1, T2>(this IList<Tuple<T1, T2>> @this, IGuard<T2> informer1 = null)
        {
            @this.ThrowIfNull(nameof(@this));
            return new DetachList<T1,T2>(@this,informer1);
        }
        /// <summary>
        /// Transform an <see cref="IList{T}"/> of <see cref="Tuple{T1,T2,T3}"/>s into only the <see cref="Tuple{T1,T2,T3}"/>'s first element, assigning the second and third elements to <see cref="IGuard{T}"/>s upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to transform</param>
        /// <param name="informer1">The <see cref="IGuard{T}"/> to which put the second values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <param name="informer2">The <see cref="IGuard{T}"/> to which put the third values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IList{T}"/> with only the first member of each <see cref="Tuple{T1,T2}"/> member. When an element is enumerated, the second element of the original enumerated member is set to be <paramref name="informer1"/>s value.</returns>
        public static IList<T1> Detach<T1, T2, T3>(this IList<Tuple<T1, T2, T3>> @this, IGuard<T2> informer1, IGuard<T3> informer2)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Detach(informer2).Detach(informer1);
        }
        /// <summary>
        /// Transform an <see cref="IList{T}"/> of <see cref="Tuple{T1,T2,T3}"/>s into only the <see cref="Tuple{T1,T2,T3}"/>'s first and second elements, assigning the third element to an <see cref="IGuard{T}"/> upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3}"/> members.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to transform</param>
        /// <param name="informer1">The <see cref="IGuard{T}"/> to which put the third values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IList{T}"/> with only the first member of each tuple member. When an element is enumerated, the remaining elements of the original enumerated member is set to be the informer's values.</returns>
        public static IList<Tuple<T1,T2>> Detach<T1, T2, T3>(this IList<Tuple<T1, T2, T3>> @this, IGuard<T3> informer1=null)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Select(a => Tuple.Create(Tuple.Create(a.Item1, a.Item2), a.Item3)).Detach(informer1);
        }
        /// <summary>
        /// Transform an <see cref="IList{T}"/> of <see cref="Tuple{T1,T2,T3,T4}"/>s into only the <see cref="Tuple{T1,T2,T3,T4}"/>'s first element, assigning the remaining elements to <see cref="IGuard{T}"/>s upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to transform</param>
        /// <param name="informer1">The <see cref="IGuard{T}"/> to which put the second values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <param name="informer2">The <see cref="IGuard{T}"/> to which put the third values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <param name="informer3">The <see cref="IGuard{T}"/> to which put the fourth values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IList{T}"/> with only the first member of each tuple member. When an element is enumerated, the remaining elements of the original enumerated member is set to be the informer's values.</returns>
        public static IList<T1> Detach<T1, T2, T3, T4>(this IList<Tuple<T1, T2, T3, T4>> @this, IGuard<T2> informer1, IGuard<T3> informer2, IGuard<T4> informer3)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Detach(informer2, informer3).Detach(informer1);
        }
        /// <summary>
        /// Transform an <see cref="IList{T}"/> of <see cref="Tuple{T1,T2,T3,T4}"/>s into only the <see cref="Tuple{T1,T2,T3,T4}"/>'s first and second elements, assigning the remaining elements to <see cref="IGuard{T}"/>s upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to transform</param>
        /// <param name="informer2">The <see cref="IGuard{T}"/> to which put the third values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <param name="informer3">The <see cref="IGuard{T}"/> to which put the fourth values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IList{T}"/> with only the first member of each tuple member. When an element is enumerated, the remaining elements of the original enumerated member is set to be the informer's values.</returns>
        public static IList<Tuple<T1, T2>> Detach<T1, T2, T3, T4>(this IList<Tuple<T1, T2, T3, T4>> @this, IGuard<T3> informer2, IGuard<T4> informer3)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Detach(informer3).Detach(informer2);
        }
        /// <summary>
        /// Transform an <see cref="IList{T}"/> of <see cref="Tuple{T1,T2,T3,T4}"/>s into only the <see cref="Tuple{T1,T2,T3,T4}"/>'s first, second, and third elements, assigning the remaining elements to <see cref="IGuard{T}"/>s upon enumeration.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple{T1,T2,T3,T4}"/> members.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to transform</param>
        /// <param name="informer3">The <see cref="IGuard{T}"/> to which put the fourth values of the members. Setting to <see langword="null"/> will put the values nowhere.</param>
        /// <returns>A new <see cref="IList{T}"/> with only the first member of each tuple member. When an element is enumerated, the remaining elements of the original enumerated member is set to be the informer's values.</returns>
        public static IList<Tuple<T1, T2, T3>> Detach<T1, T2, T3, T4>(this IList<Tuple<T1, T2, T3, T4>> @this, IGuard<T4> informer3 = null)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Select(a => Tuple.Create(Tuple.Create(a.Item1, a.Item2, a.Item3), a.Item4)).Detach(informer3);
        }
    }
}

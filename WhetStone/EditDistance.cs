using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class editDistance
    {
        /// <summary>
        /// An interface for an edit step.
        /// </summary>
        /// <typeparam name="T">The type of the element the step affects.</typeparam>
        public interface IEditStep<T>
        {
            /// <summary>
            /// Get a new <see cref="IEnumerable{T}"/> with the step applied to the original
            /// </summary>
            /// <param name="en">The enumerable to apply to.</param>
            /// <returns>A new enumerable with <paramref name="en"/>'s member except the step is applied to it.</returns>
            IEnumerable<T> apply(IEnumerable<T> en);
            /// <summary>
            /// Mutates an <see cref="IList{T}"/> to reflect the edit step.
            /// </summary>
            /// <param name="li">The <see cref="IList{T}"/> to mutate.</param>
            /// <remarks>This will mutate <paramref name="li"/>.</remarks>
            void apply(IList<T> li);
        }
        /// <summary>
        /// Represents an element being added to an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the element to add.</typeparam>
        public class Insert<T> : IEditStep<T>
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="newVal">The element to add.</param>
            /// <param name="newInd">The index to add the element in.</param>
            public Insert(T newVal, int newInd)
            {
                this.newVal = newVal;
                this.newInd = newInd;
            }
            /// <summary>
            /// The element that is added during the step.
            /// </summary>
            public T newVal { get; }
            /// <summary>
            /// The destination index of the added element.
            /// </summary>
            public int newInd { get; }
            /// <inheritdoc />
            public IEnumerable<T> apply(IEnumerable<T> en)
            {
                return en.Splice(newVal, newInd);
            }
            /// <inheritdoc />
            public void apply(IList<T> li)
            {
                li.Insert(newInd, newVal);
            }
            /// <inheritdoc />
            public override string ToString()
            {
                return $"Add {newVal} at index {newInd}";
            }
        }
        /// <summary>
        /// Represents an element being removed from an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the element to remove.</typeparam>
        public class Delete<T> : IEditStep<T>
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="deletedInd">The index of the element to remove.</param>
            public Delete(int deletedInd)
            {
                this.deletedInd = deletedInd;
            }
            /// <summary>
            /// The index of the element to remove.
            /// </summary>
            public int deletedInd { get; }
            /// <inheritdoc />
            public IEnumerable<T> apply(IEnumerable<T> en)
            {
                return en.SkipSlice(deletedInd);
            }
            /// <inheritdoc />
            public void apply(IList<T> li)
            {
                li.RemoveAt(deletedInd);
            }
            /// <inheritdoc />
            public override string ToString()
            {
                return $"Delete index {deletedInd}";
            }
        }
        /// <summary>
        /// Represents an element being replaced in an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the element to rep-lace.</typeparam>
        public class Substitute<T> : IEditStep<T>
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="newVal">The value to replace the element with.</param>
            /// <param name="ind">The index of the element to replace.</param>
            public Substitute(T newVal, int ind)
            {
                this.newVal = newVal;
                this.ind = ind;
            }
            /// <summary>
            /// The value to replace the element with.
            /// </summary>
            public T newVal { get; }
            /// <summary>
            /// The index of the element to replace.
            /// </summary>
            public int ind { get; }
            /// <inheritdoc />
            public IEnumerable<T> apply(IEnumerable<T> en)
            {
                return en.Cover(newVal, ind);
            }
            /// <inheritdoc />
            public void apply(IList<T> li)
            {
                li[ind] = newVal;
            }
            /// <inheritdoc />
            public override string ToString()
            {
                return $"Set index {ind} with {newVal}";
            }
        }
        
        // todo step weights
        // todo make other not necessarily a list, and remark it
        /// <summary>
        /// Get the edit steps in the shortest edit path from <paramref name="this"/> to <paramref name="other"/>.
        /// </summary>
        /// <param name="this">The starting <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The destination <see cref="IList{T}"/>.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to compare elements. Setting to <see langword="null"/>.</param>
        /// <param name="allowIns">Whether to allow insertions.</param>
        /// <param name="allowDel">Whether to allow deletions.</param>
        /// <param name="allowSub">Whether to allow substations.</param>
        /// <typeparam name="T">The type of <paramref name="this"/> and <paramref name="other"/>'s elements.</typeparam>
        /// <returns>An enumerable with all the edit steps necessary to turn <paramref name="this"/> into <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentException">If no edit paths are found.</exception>
        /// <remarks>
        /// <para>Using dynamic programming, space and time complexity O(n^2).</para>
        /// <para>Because <paramref name="other"/> is enumerated so many times, it is recommended to be more efficient than <paramref name="this"/>, if possible.</para>
        /// </remarks>
        public static IEnumerable<IEditStep<T>> EditSteps<T>(this IEnumerable<T> @this, IList<T> other, IEqualityComparer<T> comp = null, bool allowIns = true, bool allowDel = true, bool allowSub = true)
        {
            comp = comp ?? EqualityComparer<T>.Default;

            if (!allowIns && !allowDel && !allowSub)
            {
                if (@this.SequenceEqual(other))
                    yield break;
                throw new ArgumentException("No edit path found.");
            }
            //todo more special cases

            const int ins = 0;
            const int del = 1;
            const int sub = 2;
            const int non = 3;
            const int err = 4;
            IList<Tuple<int, int>>[] v = new IList<Tuple<int, int>>[@this.Count()+1];

            v[0] =
                Tuple.Create(non, 0).Enumerate().Concat(allowIns
                    ? range.IRange(1, other.Count, 1).Select(a => Tuple.Create(ins, a))
                    : Tuple.Create(err, -2).Enumerate(other.Count));

            foreach (var t in @this.CountBind(1))
            {
                int i = t.Item2;
                var tv = t.Item1;
                var newarr = new Tuple<int,int>[other.Count+1];
                newarr[0] = allowDel ? Tuple.Create(del, i) : Tuple.Create(err, -2);
                foreach (var z in other.CountBind(1))
                {
                    int j = z.Item2;
                    var ov = z.Item1;
                    int cost = 1;
                    if (comp.Equals(tv, ov))
                        cost = 0;

                    int inscost = -1;
                    int delcost = -1;
                    int subcost = -1;
                    if (allowIns)
                        inscost = newarr[j - 1].Item2 + 1;
                    if (allowDel)
                        delcost = v[i - 1][j].Item2 + 1;
                    if (allowSub || cost == 0)
                        subcost = v[i - 1][j - 1].Item2 + cost;
                    var costs = new[] {inscost, delcost, subcost}.Where(a => a >= 0);
                    var mincost = costs.Any() ? costs.Min() : -2;
                    if (mincost == inscost)
                    {
                        newarr[j] = Tuple.Create(ins, inscost);
                    }
                    else if (mincost == delcost)
                    {
                        newarr[j] = Tuple.Create(del, delcost);
                    }
                    else if (mincost == subcost)
                    {
                        if (cost == 1)
                            newarr[j] = Tuple.Create(sub, subcost);
                        else
                            newarr[j] = Tuple.Create(non, subcost);
                    }
                    else
                    {
                        newarr[j] = Tuple.Create(err, -2);
                    }
                }
                v[i] = newarr;
            }

            {   
                int i = v.Length - 1;
                int j = v.First().Count - 1;
                while (i != 0 || j != 0)
                {
                    switch (v[i][j].Item1)
                    {
                        case ins:
                            yield return new Insert<T>(other[j-1], i);
                            j--;
                            continue;
                        case del:
                            yield return new Delete<T>(i-1);
                            i--;
                            continue;
                        case sub:
                            yield return new Substitute<T>(other[j-1], i - 1);
                            i--;
                            j--;
                            continue;
                        case non:
                            i--;
                            j--;
                            continue;
                        default:
                            throw new ArgumentException("No edit path found.");
                    }
                }
            }
        }
        //todo step weights
        /// <summary>
        /// Gets the minimum number of edit steps to transform <paramref name="this"/> to <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/> and <paramref name="other"/>'s elements.</typeparam>
        /// <param name="this">The starting <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The destination <see cref="IList{T}"/>.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to compare elements. Setting to <see langword="null"/>.</param>
        /// <param name="allowIns">Whether to allow insertions.</param>
        /// <param name="allowDel">Whether to allow deletions.</param>
        /// <param name="allowSub">Whether to allow substations.</param>
        /// <returns>The number of steps in the minimum edit path from <paramref name="this"/> to <paramref name="other"/>.</returns>
        /// <remarks>
        /// <para>Using forgetful dynamic programming, time complexity O(n^2), space complexity O(n).</para>
        /// </remarks>
        public static int EditDistance<T>(this IEnumerable<T> @this, IEnumerable<T> other, IEqualityComparer<T> comp = null, bool allowIns = true, bool allowDel = true, bool allowSub = true)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            IList<int> v = allowIns ? range.IRange(other.Count()) : (-2).Enumerate(other.Count()+1);

            //todo special cases

            foreach (var t in @this)
            {
                var temp = new int[v.Count];
                temp[0] = allowDel ? v[0] + 1 : -2;
                Guard<int> i = new Guard<int>();
                foreach (var o in other.CountBind(1).Detach(i))
                {
                    int cost = 1;
                    if (comp.Equals(t, o))
                        cost = 0;

                    int inscost = -1;
                    int delcost = -1;
                    int subcost = -1;
                    if (allowIns)
                        inscost = temp[i - 1] + 1;
                    if (allowDel)
                        delcost = v[i] + 1;
                    if (allowSub || cost == 0)
                        subcost = v[i - 1] + cost;

                    var costs = new[] { inscost, delcost, subcost }.Where(a => a >= 0);
                    temp[i] = costs.Any() ? costs.Min() : -2;
                }
                v = temp;
            }
            if (v.Last() == -2)
                throw new Exception("No Edit Path Found!");
            return v.Last();
        }
    }
}
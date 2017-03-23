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
                newInd.ThrowIfAbsurd(nameof(newInd));
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
                en.ThrowIfNull(nameof(en));
                return en.Splice(newVal, newInd);
            }
            /// <inheritdoc />
            public void apply(IList<T> li)
            {
                li.ThrowIfNull(nameof(li));
                if (li.IsReadOnly)
                    throw new ArgumentException(nameof(li)+" is read-only");
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
                deletedInd.ThrowIfAbsurd(nameof(deletedInd));
                this.deletedInd = deletedInd;
            }
            /// <summary>
            /// The index of the element to remove.
            /// </summary>
            public int deletedInd { get; }
            /// <inheritdoc />
            public IEnumerable<T> apply(IEnumerable<T> en)
            {
                en.ThrowIfNull(nameof(en));
                return en.SkipSlice(deletedInd);
            }
            /// <inheritdoc />
            public void apply(IList<T> li)
            {
                li.ThrowIfNull(nameof(li));
                if (li.IsReadOnly)
                    throw new ArgumentException(nameof(li) + " is read-only");
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
                ind.ThrowIfAbsurd(nameof(ind));
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
                en.ThrowIfNull(nameof(en));
                return en.Cover(newVal, ind);
            }
            /// <inheritdoc />
            public void apply(IList<T> li)
            {
                li.ThrowIfNull(nameof(li));
                if (li.IsReadOnly)
                    throw new ArgumentException(nameof(li) + " is read-only");
                li[ind] = newVal;
            }
            /// <inheritdoc />
            public override string ToString()
            {
                return $"Set index {ind} with {newVal}";
            }
        }
        /// <summary>
        /// Get the edit steps in the shortest edit path from <paramref name="this"/> to <paramref name="other"/>.
        /// </summary>
        /// <param name="this">The starting <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The destination <see cref="IEnumerable{T}"/>.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to compare elements. Setting to <see langword="null"/>.</param>
        /// <param name="allowIns">Whether to allow insertions.</param>
        /// <param name="allowDel">Whether to allow deletions.</param>
        /// <param name="allowSub">Whether to allow substations.</param>
        /// <param name="insertWeight">weight of an insert step.</param>
        /// <param name="removeWeight">weight of a remove step.</param>
        /// <param name="subWeight">weight of a substitution step.</param>
        /// <typeparam name="T">The type of <paramref name="this"/> and <paramref name="other"/>'s elements.</typeparam>
        /// <returns>An enumerable with all the edit steps necessary to turn <paramref name="this"/> into <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentException">If no edit paths are found.</exception>
        /// <exception cref="ArgumentException">If any of the weights are non-positive.</exception>
        /// <remarks>
        /// <para>Using dynamic programming, space and time complexity O(n^2).</para>
        /// <para>Because <paramref name="other"/> is enumerated so many times, it is recommended to be more efficient than <paramref name="this"/>, if possible.</para>
        /// </remarks>
        public static IEnumerable<IEditStep<T>> EditSteps<T>(this IEnumerable<T> @this, IEnumerable<T> other, IEqualityComparer<T> comp = null,
            bool allowIns = true, bool allowDel = true, bool allowSub = true, 
            double insertWeight = 1.0, double removeWeight = 1.0, double subWeight = 1.0)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            comp = comp ?? EqualityComparer<T>.Default;

            if (!allowIns && !allowDel && !allowSub)
            {
                if (@this.SequenceEqual(other))
                    yield break;
                throw new ArgumentException("No edit path found.");
            }
            if (!allowIns && !allowDel)
            {
                if (@this.CompareCount(other) != 0)
                    throw new ArgumentException("No edit path found.");
                foreach (var t in @this.Zip(other).CountBind())
                {
                    if (comp.Equals(t.Item1.Item1,t.Item1.Item2))
                        continue;
                    yield return new Substitute<T>(t.Item1.Item2,t.Item2);
                }
                yield break;
            }
            if (insertWeight < 0 || removeWeight < 0 || subWeight < 0)
            {
                throw new ArgumentException("Cannot handle non-positive weights.");
            }
            if (double.IsPositiveInfinity(insertWeight))
            {
                allowIns = false;
                insertWeight = 1;
            }
            if (double.IsPositiveInfinity(removeWeight))
            {
                allowDel = false;
                removeWeight = 1;
            }
            if (double.IsPositiveInfinity(subWeight))
            {
                allowSub = false;
                subWeight = 1;
            }
            insertWeight.ThrowIfAbsurd(nameof(insertWeight));
            removeWeight.ThrowIfAbsurd(nameof(removeWeight));
            subWeight.ThrowIfAbsurd(nameof(subWeight));

            const int ins = 0;
            const int del = 1;
            const int sub = 2;
            const int non = 3;
            const int err = 4;
            IList<Tuple<int, double>>[] v = new IList<Tuple<int, double>>[@this.Count()+1];

            v[0] =
                Tuple.Create(non, 0.0).Enumerate().Concat(allowIns
                    ? range.IRange(1.0, other.Count(), 1).Select(a => Tuple.Create(ins, a))
                    : Tuple.Create(err, -2.0).Enumerate(other.Count()));

            foreach (var t in @this.CountBind(1))
            {
                int i = t.Item2;
                var tv = t.Item1;
                var newarr = new Tuple<int, double>[other.Count()+1];
                newarr[0] = allowDel ? Tuple.Create(del, (double)i) : Tuple.Create(err, -2.0);
                foreach (var z in other.CountBind(1))
                {
                    int j = z.Item2;
                    var ov = z.Item1;
                    double cost = subWeight;
                    if (comp.Equals(tv, ov))
                        cost = 0;

                    double inscost = -1;
                    double delcost = -1;
                    double subcost = -1;
                    if (allowIns)
                        inscost = newarr[j - 1].Item2 + insertWeight;
                    if (allowDel)
                        delcost = v[i - 1][j].Item2 + removeWeight;
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
                        newarr[j] = Tuple.Create(err, -2.0);
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
                            yield return new Insert<T>(other.ElementAt(j-1), i);
                            j--;
                            continue;
                        case del:
                            yield return new Delete<T>(i-1);
                            i--;
                            continue;
                        case sub:
                            yield return new Substitute<T>(other.ElementAt(j - 1), i - 1);
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

            if (!allowIns && !allowDel && !allowSub)
            {
                if (@this.SequenceEqual(other))
                    return 0;
                throw new ArgumentException("No edit path found.");
            }
            if (!allowIns && !allowDel)
            {
                if (@this.CompareCount(other) != 0)
                    throw new ArgumentException("No edit path found.");
                return @this.Zip(other).Count(a => !comp.Equals(a.Item1, a.Item2));
            }

            foreach (var t in @this)
            {
                var temp = new int[v.Count];
                temp[0] = allowDel ? v[0] + 1 : -2;
                foreach ((var o, var i) in other.CountBind(1))
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
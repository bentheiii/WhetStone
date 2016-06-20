using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WhetStone.Arrays.Arr2D;
using WhetStone.Comparison;
using WhetStone.Fielding;
using WhetStone.Guard;
using WhetStone.Looping;
using WhetStone.NumbersMagic;
using WhetStone.SpecialNumerics;
using WhetStone.Random;
using WhetStone.Structures.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Arrays
{
    // ReSharper disable once InconsistentNaming
    public static class ArrayExtensions
    {
        //maximum is exclusive
        public static int binSearch(Func<int, int> searcher, int min, int max, int failvalue = -1)
        {
            if (failvalue.iswithin(min,max))
                throw new ArgumentException("failval cannot be within searched values");
            while (min < max)
            {
                int i = (min + max) / 2;
                int res = searcher(i);
                if (res == 0)
                    return i;
                if (i == min)
                    break;
                if (res > 0)
                    min = i;
                else
                    max = i;
            }
            return failvalue;
        }
        /// <summary>
        /// returns the last index at which searcher returns positive
        /// </summary>
        /// <returns></returns>
        public static int binSearch(Func<int, bool> searcher, int min, int max, int failvalue = -1)
        {
            if (failvalue.iswithin(min, max))
                throw new ArgumentException("failval cannot be within searched values");
            int rangemax = max;
            while (min < max)
            {
                int i = (min + max) / 2;
                var res = searcher(i);
                if (res == false)
                    max = i;
                else
                {
                    if (i + 1 >= rangemax || !searcher(i + 1))
                        return i;
                    min = i + 1;
                }
            }
            return failvalue;
        }
        /// <summary>
        /// returns the last index at which searcher returns positive
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedarr"></param>
        /// <param name="searcher"></param>
        /// <returns></returns>
        public static int binSearch<T>(this IList<T> sortedarr, Func<T, bool> searcher)
        {
            return binSearch(i => searcher(sortedarr[i]), 0, sortedarr.Count);
        }
        /// <summary>
        /// searches an array with a binary search
        /// </summary>
        /// <typeparam name="T" />
        /// <param name="sortedarr">the <b>sorted</b> array</param>
        /// <param name="searcher">a negative result means the target is before this element, a positive result means after, and zero means this is the target</param>
        /// <returns>the index of the sought element</returns>
        public static int binSearch<T>(this IList<T> sortedarr, Func<T, int> searcher)
        {
            return binSearch(i => searcher(sortedarr[i]), 0, sortedarr.Count);
        }
        /// <summary>
        /// searches an array with a binary search
        /// </summary>
        /// <typeparam name="T" />
        /// <param name="sortedarr">the <b>sorted</b> array</param>
        /// <param name="tofind">the target to find</param>
        /// <param name="comp">the comparer the array was sorted by</param>
        /// <returns>the index of the sought element</returns>
        public static int binSearch<T>(this IList<T> sortedarr, T tofind, IComparer<T> comp)
        {
            return binSearch(sortedarr, a => comp.Compare(tofind, a));
        }
        /// <summary>
        /// searches an array with a binary search
        /// </summary>
        /// <typeparam name="T" />
        /// <param name="sortedarr">the <b>sorted</b> array</param>
        /// <param name="tofind">the target to find</param>
        /// <returns>the index of the sought element</returns>
        public static int binSearch<T>(this IList<T> sortedarr, T tofind)
        {
            return binSearch(sortedarr, tofind, Comparer<T>.Default);
        }
        /// <summary>
        /// fill an array with values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tofill">array to fill</param>
        /// <param name="v">elements to fill the array with, they will repeat through the array</param>
        public static void Fill<T>(this IList<T> tofill, params T[] v)
        {
            tofill.Fill(v, 0);
        }
        public static void Fill<T>(this IList<T> tofill, T[] v, int start, int count = int.MaxValue)
        {
            if (v.Length == 0)
                throw new ArgumentException("cannot be empty", nameof(v));
            Fill(tofill, i => (v[i % v.Length]), start, count);
        }
        public static void Fill<T>(this IList<T> tofill, Func<int, T> filler, int start = 0, int count = int.MaxValue)
        {
            for (int i = 0; i < tofill.Count - start && i < count; i++)
            {
                if (i + start < 0)
                    continue;
                tofill[i + start] = filler(i + start);
            }
        }
        public static void Fill<T>(this IList<T> tofill, Func<T> filler, int start = 0, int count = int.MaxValue)
        {
            tofill.Fill(a => filler(), start, count);
        }
        public static T[] Fill<T>(int length, T[] filler, int start, int count = int.MaxValue)
        {
            T[] ret = new T[length];
            ret.Fill(filler, start, count);
            return ret;
        }
        public static T[] Fill<T>(int length, params T[] filler)
        {
            T[] ret = new T[length];
            ret.Fill(filler);
            return ret;
        }
        public static T[] Fill<T>(int length, Func<int, T> filler, int start = 0, int count = int.MaxValue)
        {
            T[] ret = new T[length];
            ret.Fill(filler, start, count);
            return ret;
        }
        public static T[] Fill<T>(int length, Func<T> filler, int start = 0, int count = int.MaxValue)
        {
            T[] ret = new T[length];
            ret.Fill(a => filler(), start, count);
            return ret;
        }
        /// <summary>
        /// translates an IEnumerable to a converted array
        /// </summary>
        /// <typeparam name="T" />
        /// <typeparam name="T1" />
        /// <param name="filler1">the <b>finite</b> enumerable to convert</param>
        /// <param name="function">the converter for the array</param>
        /// <returns>an array converted with the function</returns>
        public static T[] SelectToArray<T, T1>(this IEnumerable<T1> filler1, Func<T1, T> function)
        {
            return filler1.Select(function).ToArray();
        }
        private static int? RecommendSize<T>(this IEnumerable<T> @this)
        {
            return (@this as IReadOnlyCollection<T>)?.Count ?? ((@this as ICollection<T>)?.Count);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, Action<int> reporter, bool limitToCapacity = false)
        {
            return ToArray(@this, @this.RecommendSize() ?? 0, (arg1, i) => reporter?.Invoke(i),limitToCapacity);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, Action<T, int> reporter, bool limitToCapacity = false)
        {
            return ToArray(@this, @this.RecommendSize() ?? 0, reporter, limitToCapacity);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, bool limitToCapacity = false)
        {
            return ToArray(@this, capacity, (Action<T, int>)null, limitToCapacity);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, Action<int> reporter, bool limitToCapacity = false)
        {
            return ToArray(@this, capacity, (arg1, i) => reporter?.Invoke(i), limitToCapacity);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, Action<T, int> reporter,bool limitToCapacity = false)
        {
            T[] ret = new T[capacity <= 0 ? 1 : capacity];
            int i = 0;
            foreach (T t in @this)
            {
                if (ret.Length <= i)
                {
                    if (limitToCapacity)
                        return ret;
                    Array.Resize(ref ret, ret.Length*2);
                }
                ret[i] = t;
                reporter?.Invoke(t, i);
                i++;
            }
            Array.Resize(ref ret, i);
            return ret;
        }
        public static bool isSymmetrical<T>(this IList<T> @this)
        {
            return isSymmetrical(@this, EqualityComparer<T>.Default);
        }
        public static bool isSymmetrical<T>(this IList<T> @this, IEqualityComparer<T> c)
        {
            foreach (int i in Loops.Range(@this.Count / 2))
            {
                if (!c.Equals(@this[i], @this[(-i - 1).TrueMod(@this.Count)]))
                    return false;
            }
            return true;
        }
        public static bool isSymmetrical<T>(this IEnumerable<T> @this, IEqualityComparer<T> c)
        {
            int? count = @this.RecommendSize();
            int len = count / 2 ?? int.MaxValue;
            return @this.Zip(@this.Reverse()).Take(len).All(a => c.Equals(a.Item1, a.Item2));
        }
        public static bool isSymmetrical<T>(this IEnumerable<T> @this)
        {
            return isSymmetrical(@this, EqualityComparer<T>.Default);
        }
        public static IDictionary<T, ulong> ToOccurances<T>(this IEnumerable<T> arr)
        {
            return ToOccurances(arr, EqualityComparer<T>.Default);
        }
        public static IDictionary<T, ulong> ToOccurances<T>(this IEnumerable<T> arr, IEqualityComparer<T> c)
        {
            Dictionary<T, ulong> oc = new Dictionary<T, ulong>(c);
            foreach (T v in arr)
            {
                if (oc.ContainsKey(v))
                    oc[v]++;
                else
                    oc[v] = 1;
            }
            return oc;
        }
        /// <summary>
        /// converts a dictionary to an array where each key appears a number of times as its value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d">the dictionary</param>
        /// <returns>an array formed from the dictionary</returns>
        public static IEnumerable<T> FromOccurances<T>(this IEnumerable<KeyValuePair<T, int>> d)
        {
            return d.Select(a => a.Key.Enumerate().Repeat(a.Value)).Concat();
        }
        public static bool ContainsAll<T>(this IEnumerable<T> @this, IEnumerable<T> vals)
        {
            return vals.All(@this.Contains);
        }
        public static bool ContainsAny<T>(this IEnumerable<T> @this, IEnumerable<T> vals)
        {
            return vals.Any(@this.Contains);
        }
        public static void WriteTo<T>(this IEnumerable<T> @this, TextWriter w, string seperator = ", ", string opening = "[", string closing = "]")
        {
            w.Write(opening);
            bool first = true;
            foreach (T temp in @this)
            {
                if (!first)
                    w.Write(seperator);
                first = false;
                w.Write(temp);
            }
            w.Write(closing);
        }
        public static string ToPrintable<T>(this IEnumerable<T> a, string seperator = ", ", string opening = "[", string closing = "]")
        {
            using (StringWriter w = new StringWriter())
            {
                a.WriteTo(w, seperator, opening, closing);
                return w.ToString();
            }
        }
        public static string ToPrintable<K,V>(this IDictionary<K,V> a, string definitionSeperator = ":", string seperator = ", ", string opening = "{", string closing = "}")
        {
            return ToPrintable(a, x=>x.ToString(), x=>x.ToString(), definitionSeperator, seperator, opening, closing);
        }
        public static string ToPrintable<K,V>(this IDictionary<K,V> a,Func<K,string> kPrinter, Func<V,string> vPrinter, string definitionSeperator = ":", string seperator = ", ", string opening = "{", string closing = "}")
        {
            return a.Select(x => kPrinter?.Invoke(x.Key) + definitionSeperator + vPrinter?.Invoke(x.Value)).ToPrintable(seperator,opening, closing);
        }
        public static void Swap<T>(this IList<T> toswap, int index1, int index2)
        {
            if (index1 != index2)
            {
                T temp = toswap[index1];
                toswap[index1] = toswap[index2];
                toswap[index2] = temp;
            }
        }
        public static T[] Sort<T>(this T[] tosort, IComparer<T> comparer = null)
        {
            T[] ret = tosort.Copy();
            Array.Sort(ret, comparer ?? Comparer<T>.Default);
            return ret;
        }
        public static T[] Sort<T>(this T[] tosort,int startindex, int length, IComparer<T> comparer = null)
        {
            T[] ret = tosort.Copy();
            Array.Sort(ret, startindex, length, comparer ?? Comparer<T>.Default);
            return ret;
        }
        public static T getMedianNoAlloc<T>(this IEnumerable<T> @this, IComparer<T> comparer, out int index)
        {
            if (!@this.Any())
                throw new ArgumentException("cannot be empty", nameof(@this));
            int c = @this.Count();
            var res = @this.CountBind().OrderBy(Comparer<Tuple<T,int>>.Create((a,b)=>comparer.Compare(a.Item1,b.Item1))).Skip(c / 2).First();
            index = res.Item2;
            return res.Item1;
        }
        public static T getMedian<T>(this IEnumerable<T> tosearch, IComparer<T> comparer, out int index)
        {
            if (!tosearch.Any())
                throw new ArgumentException("cannot be empty", nameof(tosearch));
            Tuple<T, int>[] arr = tosearch.CountBind().ToArray();
            Array.Sort(arr, Comparer<Tuple<T, int>>.Create((a, b) => comparer.Compare(a.Item1, b.Item1)));
            var ret = arr[arr.Length / 2];
            index = ret.Item2;
            return ret.Item1;
        }
        public static T getMedian<T>(this IEnumerable<T> tosearch, IComparer<T> comparer)
        {
            int prox;
            return tosearch.getMedian(comparer, out prox);
        }
        public static T getMedian<T>(this IEnumerable<T> tosearch, out int index) where T : IComparable<T>
        {
            return tosearch.getMedian(Comparer<T>.Default, out index);
        }
        public static T getMedian<T>(this IEnumerable<T> tosearch) where T : IComparable<T>
        {
            int prox;
            return tosearch.getMedian(Comparer<T>.Default, out prox);
        }
        public static T getMin<T>(this IEnumerable<T> tosearch, IComparer<T> compare, out int index)
        {
            if (!tosearch.Any())
                throw new ArgumentException("cannot be empty", nameof(tosearch));
            T ret = tosearch.First();
            index = 0;
            int i = 1;
            foreach (T var in tosearch.Skip(1))
            {
                if (compare.Compare(var, ret) < 0)
                {
                    ret = var;
                    index = i;
                }
                i++;
            }
            return ret;
        }
        public static T getMin<T>(this IEnumerable<T> tosearch, IComparer<T> compare)
        {
            int prox;
            return tosearch.getMin(compare, out prox);
        }
        public static T getMin<T>(this IEnumerable<T> tosearch, out int index)
        {
            return tosearch.getMin(Comparer<T>.Default, out index);
        }
        public static T getMin<T>(this IEnumerable<T> tosearch)
        {
            return tosearch.getMin(Comparer<T>.Default);
        }
        public static T getMax<T>(this IEnumerable<T> tosearch, IComparer<T> compare, out int index)
        {
            return tosearch.getMin(compare.Reverse(), out index);
        }
        public static T getMax<T>(this IEnumerable<T> tosearch, IComparer<T> compare)
        {
            int prox;
            return tosearch.getMax(compare, out prox);
        }
        public static T getMax<T>(this IEnumerable<T> tosearch, out int index)
        {
            return tosearch.getMax(Comparer<T>.Default, out index);
        }
        public static T getMax<T>(this IEnumerable<T> tosearch)
        {
            return tosearch.getMax(Comparer<T>.Default);
        }
        public static T getAverage<T>(this IEnumerable<T> tosearch)
        {
            Field<T> f = Fields.getField<T>();
            return tosearch.getSum().ToFieldWrapper() / tosearch.Count();
        }
        public static T getGeometricAverage<T>(this IEnumerable<T> tosearch)
        {
            var f = Fields.getField<T>();
            return f.pow(tosearch.getProduct(), f.Invert(f.fromInt(tosearch.Count())));
        }
        public static T getMode<T>(this IEnumerable<T> tosearch, IEqualityComparer<T> comparer, out int index)
        {
            if (!tosearch.Any())
                throw new ArgumentException("cannot be empty", nameof(tosearch));
            var oc = tosearch.CountBind().ToOccurances(new EqualityFunctionComparer<Tuple<T, int>,T>(a => a.Item1, comparer));
            KeyValuePair<Tuple<T, int>, ulong> max = oc.getMax(new FunctionComparer<KeyValuePair<Tuple<T, int>, ulong>>(a => a.Value));
            index = max.Key.Item2;
            return max.Key.Item1;
        }
        public static T getMode<T>(this IEnumerable<T> tosearch, out int index)
        {
            return getMode(tosearch, EqualityComparer<T>.Default, out index);
        }
        public static T getMode<T>(this IEnumerable<T> tosearch)
        {
            int prox;
            return tosearch.getMode(out prox);
        }
        public static T getSum<T>(this IEnumerable<T> toAdd)
        {
            Field<T> f = Fields.getField<T>();
            return toAdd.getSum(f.add);
        }
        public static T getSum<T>(this IEnumerable<T> toAdd, Func<T, T, T> adder)
        {
            Field<T> f = Fields.getField<T>();
            return toAdd.Aggregate(f.zero, adder);
        }
        public static T getProduct<T>(this IEnumerable<T> toAdd)
        {
            return getProduct(toAdd, Fields.getField<T>().multiply);
        }
        public static T getProduct<T>(this IEnumerable<T> toAdd, Func<T, T, T> multiplier)
        {
            Field<T> f = Fields.getField<T>();
            return toAdd.Aggregate(f.one, multiplier);
        }
        public static T[] Shuffle<T>(this IList<T> arr)
        {
            return Shuffle(arr, new GlobalRandomGenerator());
        }
        public static T[] Shuffle<T>(this IList<T> arr, RandomGenerator gen)
        {
            T[] x = arr.ToArray(arr.Count);
            for (int i = 0; i < x.Length; i++)
            {
                int j = gen.Int(i, x.Length);
                x.Swap(i, j);
            }
            return x;
        }
        public static T Pick<T>(this IEnumerable<T> @this, RandomGenerator gen = null)
        {
            gen = gen ?? new GlobalRandomGenerator();
            return @this.Pick(1,gen).First();
        }
        public static IEnumerable<T> Pick<T>(this IEnumerable<T> @this, int count, RandomGenerator gen = null)
        {
            gen = gen ?? new GlobalRandomGenerator();
            int nom = count;
            int denom = @this.Count();
            if (nom > denom || nom < 0)
                throw new ArgumentException();
            foreach (var t in @this)
            {
                if (nom == 0)
                    yield break;
                if (gen.success(nom / (double)denom))
                {
                    yield return t;
                    nom--;
                }
                denom--;
            }
        }
        public static void Append<T>(ref T[] @this, params T[] toAdd)
        {
            Array.Resize(ref @this, @this.Length+toAdd.Length);
            foreach (var t in toAdd.CountBind(@this.Length))
            {
                @this[t.Item2] = t.Item1;
            }
        }
        public static bool AllEqual<T>(this IEnumerable<T> @this)
        {
            return AllEqual(@this, EqualityComparer<T>.Default);
        }
        public static bool AllEqual<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp)
        {
            return @this.All(a => comp.Equals(a, @this.First()));
        }
        public static object[] toObjArray(this IEnumerable @this)
        {
            return @this.Cast<object>().ToArray();
        }
        public static int HammingDistance<T>(this IEnumerable<T> @this, IEnumerable<T> other)
        {
            return HammingDistance(@this, other, EqualityComparer<T>.Default);
        }
        public static int HammingDistance<T>(this IEnumerable<T> @this, IEnumerable<T> other, IEqualityComparer<T> comp)
        {
            return @this.Zip(other).Count(a => !comp.Equals(a.Item1, a.Item2));
        }
        public static int CompareCount<T0,T1>(this IEnumerable<T0> @this, IEnumerable<T1> other)
        {
            int? rect = @this.RecommendSize();
            if (rect.HasValue)
            {
                int? reco = other.RecommendSize();
                if (reco.HasValue)
                    return rect.Value.CompareTo(reco.Value);
            }
            var tor = new IEnumerator[] { @this.GetEnumerator(), other.GetEnumerator()};
            var next = tor.SelectToArray(a => a.MoveNext());
            while (next.All(a=>a))
            {
                next = tor.SelectToArray(a => a.MoveNext());
            }
            if (next[0] == next[1])
                return 0;
            return next[0] ? 1 : -1;
        }
        public static bool CountAtLeast<T>(this IEnumerable<T> @this, int minCount, Func<T, bool> predicate = null)
        {
            var rec = @this.RecommendSize();
            if (rec.HasValue)
                return rec.Value >= minCount;
            return predicate == null ? @this.Skip(minCount - 1).Any() : @this.Skip(minCount - 1).Any(predicate);
        }
        public static Tuple<T[], T[]> SplitAt<T>(this IEnumerable<T> @this, int item1Length)
        {
            var ret1 = new ResizingArray<T>(item1Length);
            var ret2 = new ResizingArray<T>((@this.RecommendSize() ?? item1Length) - item1Length);
            var tor = @this.GetEnumerator();
            while (ret1.Count < item1Length)
            {
                if (!tor.MoveNext())
                    return Tuple.Create(ret1.arr,ret2.arr);
                ret1.Add(tor.Current);
            }
            while (!tor.MoveNext())
            {
                ret2.Add(tor.Current);
            }
            return Tuple.Create(ret1.arr, ret2.arr);
        }
        public static Tuple<T[], T[]> SplitBy<T>(this IEnumerable<T> @this, Func<T,bool> takeWhile)
        {
            var rec = (@this.RecommendSize() ?? 0);
            var ret1 = new ResizingArray<T>(rec/2);
            var ret2 = new ResizingArray<T>(rec/2);
            var tor = @this.GetEnumerator();
            while (true)
            {
                if (!tor.MoveNext())
                    return Tuple.Create<T[],T[]>(ret1, ret2);
                if (takeWhile(tor.Current))
                    ret1.Add(tor.Current);
                else
                {
                    ret2.Add(tor.Current);
                    break;
                }
            }
            while (!tor.MoveNext())
            {
                ret2.Add(tor.Current);
            }
            return Tuple.Create<T[], T[]>(ret1, ret2);
        }
        public static int Count<T>(this IEnumerable<T> @this, T query)
        {
            return Count(@this, query, EqualityComparer<T>.Default);
        }
        public static int Count<T>(this IEnumerable<T> @this, T query, IEqualityComparer<T> comp)
        {
            return @this.Count(a=>comp.Equals(a,query));
        }
        public static int Count<T>(this IEnumerable<T> @this, IEnumerable<T> query)
        {
            return Count(@this, query, EqualityComparer<T>.Default);
        }
        public static int Count<T>(this IEnumerable<T> @this, IEnumerable<T> query, IEqualityComparer<T> comp)
        {
            return @this.Trail(query.Count()).Count(a => a.SequenceEqual(query, comp));
        }
        public static bool Balanced<T>(this IEnumerable<T> @this, IEnumerable<T> opener, IEnumerable<T> closer, int? maxdepth = null)
        {
            if (opener.Equals(closer))
            {
                int c = @this.Count(opener);
                return c % 2 == 0 && (!maxdepth.HasValue || (c == 0 || maxdepth >= 1));
            }
            var openerindicies = @this.Trail(opener.Count()).CountBind().Where(a => a.Item1.SequenceEqual(opener)).Select(a => a.Item2);
            var closerindicies = @this.Trail(closer.Count()).CountBind().Where(a => a.Item1.SequenceEqual(closer)).Select(a => a.Item2);
            var parenonly = openerindicies.Attach(a => 0).Concat(closerindicies.Attach(a => 1)).OrderBy(a=>a.Item1).Select(a=>a.Item2);
            return parenonly.Balanced(0, 1, maxdepth);
        }
        public static bool Balanced<T>(this IEnumerable<T> @this, T opener, T closer, int? maxdepth = null)
        {
            if (opener.Equals(closer))
            {
                int c = @this.Count(opener);
                return c % 2 == 0 && (!maxdepth.HasValue || (c == 0 || maxdepth >= 1));
            }
            var count = @this.RecommendSize();
            int ret = 0;
            Guard<int> index = new Guard<int>();
            foreach (var t in @this.CountBind().Detach(index))
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
        public static bool Balanced<T>(this IEnumerable<T> @this, IEnumerable<Tuple<T, T>> couples, int? maxdepth = null)
        {
            Stack<T> layers = new Stack<T>(maxdepth ?? 0);
            var dic = couples.ToDictionary();
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
            }
            return layers.Count == 0;
        }
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, T def)
        {
            return !@this.Any() ? def : @this.First();
        }
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, Func<T,bool> cond, T def)
        {
            return @this.Any(cond) ? @this.First(cond) : def;
        }
        public static T First<T>(this IEnumerable<T> @this, Func<T, bool> cond, out bool any)
        {
            foreach (T t in @this)
            {
                if (cond(t))
                {
                    any = true;
                    return t;
                }
            }
            any = false;
            return default(T);
        }
        public static T Last<T>(this IEnumerable<T> @this, Func<T, bool> cond, out bool any)
        {
            T ret = default(T);
            any = false;
            foreach (T t in @this)
            {
                if (cond(t))
                {
                    any = true;
                    ret = t;
                }
            }
            return ret;
        }
        public static T LastOrDefault<T>(this IEnumerable<T> @this, T def)
        {
            return !@this.Any() ? def : @this.Last();
        }
        public static T LastOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, T def)
        {
            return @this.Any(cond) ? @this.Last(cond) : def;
        }
        public static bool AnyAndAll<T>(this IEnumerable<T> @this, Func<T, bool> cond)
        {
            bool any = false;
            foreach (T t in @this)
            {
                var v = cond(t);
                if (v == false)
                    return false;
                any = true;
            }
            return any;
        }
        public static T Bracket<T>(this IEnumerable<T> contestants, int matchsize = 2, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            while (true)
            {
                var len = contestants.Count();
                if (len == 1)
                    return contestants.First();
                contestants = contestants.GroupUnbound(matchsize).SelectToArray(a => a.getMin(comp));
            }
        }
        public static IEnumerable<IEnumerable<T>> BracketRank<T>(this IEnumerable<T> contestants, int matchsize = 2, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            while (true)
            {
                var len = contestants.Count();
                if (len == 1)
                {
                    yield return contestants.First().Enumerate();
                    yield break;
                }
                List<T> winners = new List<T>();
                List<T> losers = new List<T>();
                foreach (var match in contestants.GroupUnbound(matchsize))
                {
                    int winindex;
                    var winner = match.getMin(comp, out winindex);
                    winners.Add(winner);
                    losers.AddRange(match.CountBind().Where(a => a.Item2 != winindex).Select(a => a.Item1));
                }
                yield return losers;
                contestants = winners;
            }
        }
        public static ListSlice<T> Slice<T>(this IList<T> @this, int start, int length)
        {
            var s = @this as ListSlice<T>;
            if (s != null)
                return s.ReSlice(start, length);
            return new ListSlice<T>(@this,start,length);
        }
        public static IList<T> Slice<T>(this IList<T> @this, int start)
        {
            return Slice(@this, start, @this.Count - start);
        }
        public static bool StartsWith<T>(this IEnumerable<T> @this, IEnumerable<T> prefix, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.CompareCount(prefix) >= 0 && @this.Zip(prefix, comp.Equals).All();
        }
        public static bool All(this IEnumerable<bool> @this)
        {
            foreach (var t in @this)
            {
                if (!t)
                    return false;
            }
            return true;
        }
        public static IList<T> AsList<T>(this IEnumerable<T> @this)
        {
            var l = @this as IList<T>;
            if (l != null)
                return l;
            var r = @this as IReadOnlyList<T>;
            if (r != null)
                return r.ToLockedList();
            var s = @this as string;
            if (s != null)
                return (IList<T>)new LockedListStringAdaptor(s);
            return @this.ToArray();
        }
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> @this)
        {
            var l = @this as ICollection<T>;
            if (l != null)
                return l;
            var r = @this as IReadOnlyCollection<T>;
            if (r != null)
                return r.ToLockedCollection();
            return @this.ToArray();
        }
    }
    public static class DictionaryExtensions
    {
        public static bool SumDefinition<T, G>(this IDictionary<T, G> @this, T key, G val)
        {
            return AggregateDefinition(@this, key, val, Fields.getField<G>().add);
        }
        public static bool ProductDefinition<T, G>(this IDictionary<T, G> @this, T key, G val)
        {
            return AggregateDefinition(@this, key, val, Fields.getField<G>().multiply);
        }
        public static bool AggregateDefinition<T, G>(this IDictionary<T, G> @this, T key, G val, Func<G, G, G> aggfunc)
        {
            bool ret = @this.ContainsKey(key);
            @this[key] = ret ? aggfunc(val, @this[key]) : val;
            return ret;
        }
        public static bool EnsureDefinition<T, G>(this IDictionary<T, G> @this, T key, G defaultval = default(G))
        {
            if (@this.ContainsKey(key))
                return true;
            @this[key] = defaultval;
            return false;
        }
        public static G DefinitionOrDefault<T, G>(this IDictionary<T, G> @this, T key, G defaultval = default(G))
        {
            G val;
            return @this.TryGetValue(key, out val) ? val : defaultval;
        }
    }
    public sealed class RotatorArray<T> : IList<T>
    {
        private readonly T[] _items;
        private RollerNum<int> _mIndex;
        public int Index
        {
            get
            {
                return _mIndex.value;
            }
        }
        public int IndexOf(T item)
        {
            return ((IList)_items).IndexOf(item);
        }
        void IList<T>.Insert(int index, T item)
        {
            ((IList)_items).Insert(index, item);
        }
        void IList<T>.RemoveAt(int index)
        {
            ((IList)_items).RemoveAt(index);
        }
        public T this[int i]
        {
            get
            {
                return _items[(_mIndex + i).value];
            }
            set
            {
                _items[(_mIndex + i).value] = value;
            }
        }
        public void Rotate(int val = 1)
        {
            _mIndex += val;
        }
        public static RotatorArray<T> operator ++(RotatorArray<T> a)
        {
            a.Rotate();
            return a;
        }
        public static RotatorArray<T> operator --(RotatorArray<T> a)
        {
            a.Rotate(-1);
            return a;
        }
        public RotatorArray(params T[] switchvalues)
        {
            if (switchvalues?.Length == 0)
                throw new ArgumentException("cannot be empty",nameof(switchvalues));
            this._items = switchvalues.Copy();
            this._mIndex = new RollerNum<int>(0, _items.Length, 0);
        }
        public RotatorArray(int length)
        {
            this._items = new T[length];
            this._mIndex = new RollerNum<int>(0, _items.Length, 0);
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        void ICollection<T>.Add(T item)
        {
            ((IList)_items).Add(item);
        }
        void ICollection<T>.Clear()
        {
            ((IList)_items).Clear();
        }
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }
        bool ICollection<T>.Remove(T item)
        {
            return ((IList<T>)_items).Remove(item);
        }
        public int Count
        {
            get
            {
                return _items.Length;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return _items.IsReadOnly;
            }
        }
    }
    public class SparseArray<T>
    {
        private struct Coordinate : IComparable<Coordinate>
        {
            private readonly int[] _cors;
            public Coordinate(params int[] cors)
            {
                this._cors = cors;
            }
            public override int GetHashCode()
            {
                return _cors.GetHashCode() ^ _cors.Length;
            }
            public override bool Equals(object obj)
            {
                Coordinate? c = obj as Coordinate?;
                return c != null && this._cors.SequenceEqual(c.Value._cors);
            }
            public override string ToString()
            {
                return _cors.ToPrintable();
            }
            public int CompareTo(Coordinate other)
            {
                if (other._cors.Length != _cors.Length)
                    return _cors.Length.CompareTo(other._cors.Length);
                int ret = 0;
                var cors = _cors.Zip(other._cors).GetEnumerator();
                while (ret == 0 && cors.MoveNext())
                    ret = cors.Current.Item1.CompareTo(cors.Current.Item2);
                return ret;
            }
        }
        private readonly IDictionary<Coordinate, T> _values;
        private readonly int[] _dim;
        public SparseArray(int dims, T def = default(T))
        {
            if (dims <= 0)
                throw new ArgumentException("must be positive", nameof(dims));
            this._dim = new int[dims];
            this.defaultValue = def;
            this._values = new SortedDictionary<Coordinate, T>();
        }
        public T defaultValue { get; }
        public T this[params int[] query]
        {
            get
            {
                Coordinate co = new Coordinate(query);
                return _values.ContainsKey(co) ? _values[co] : defaultValue;
            }
            set
            {
                Coordinate co = new Coordinate(query);
                if (query.Length != _dim.Length)
                    throw new ArgumentException("incorrect number of arguments for " + _dim.Length + " rank array");
                for (int i = 0; i < query.Length; i++)
                {
                    if (query[i] > _dim[i])
                        _dim[i] = query[i] + 1;
                }
                this._values[co] = value;
            }
        }
        public int GetLength(int i)
        {
            return _dim[i];
        }
    }
    public class ExpandingArray<T> : IEnumerable<T>
    {
        private readonly List<T> _data;
        public T defaultValue { get; }
        public ExpandingArray(T defaultValue = default(T), int capacity = 4)
        {
            this.defaultValue = defaultValue;
            _data = new List<T>(capacity);
        }
        public void ExpandTo(int newsize)
        {
            if (_data.Count < newsize)
            {
                _data.Capacity = newsize;
                _data.AddRange(defaultValue.Enumerate().Repeat(newsize -_data.Count));
            }
        }
        public T this[int ind]
        {
            get
            {
                return _data.Count <= ind ? defaultValue : _data[ind];
            }
            set
            {
                ExpandTo(ind+1);
                _data[ind] = value;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _data.Concat(defaultValue.Enumerate().Cycle()).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_data).GetEnumerator();
        }
        public void Add(T item)
        {
            if (item.Equals(defaultValue))
                return;
            this[_data.Count] = item;
        }
        public void Clear()
        {
            _data.Clear();
        }
        public bool Contains(T item)
        {
            return item.Equals(defaultValue) || _data.Contains(item);
        }
    }
    public class LazyArray<T> : IEnumerable<T>
    {
        private readonly Func<int, LazyArray<T>, T> _generator;
        private readonly ExpandingArray<T> _data;
        private readonly ExpandingArray<bool> _initialized;
        public LazyArray(Func<int, T> generator) : this((i, array) => generator(i)) {}
        public LazyArray(Func<int, LazyArray<T>, T> generator)
        {
            _generator = generator;
            _data = new ExpandingArray<T>();
            _initialized = new ExpandingArray<bool>();
        }
        public bool Initialized(int index)
        {
            return _initialized[index];
        }
        public void RemoveAt(int index)
        {
            _initialized[index] = false;
        }
        public T this[int ind]
        {
            get
            {
                if (_initialized[ind])
                    return _data[ind];
                T ret = _data[ind] = _generator(ind,this);
                _initialized[ind] = true;
                return ret;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Loops.Count().Select(a=>this[a]).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Clear()
        {
            _data.Clear();
            _initialized.Clear();
        }
    }
    public class LazyDictionary<K,V>
    {
        private readonly IDictionary<K, V> _dic;
        private readonly Func<K, V> _gen;
        public LazyDictionary(Func<K, V> gen, int capacity=0)
        {
            _gen = gen;
            _dic = new Dictionary<K, V>(capacity);
        }
        public V this[K key]
        {
            get
            {
                if (!_dic.ContainsKey(key))
                    _dic[key] = _gen(key);
                return _dic[key];
            }
        }
        public bool Contains(K key)
        {
            return _dic.ContainsKey(key);
        }
    }
    public class BoundLazyDictionary<K, V> : IReadOnlyDictionary<K,V>
    {
        private readonly LazyDictionary<K, V> _dic;
        private readonly ICollection<K> _keys;
        public BoundLazyDictionary(Func<K, V> gen, IEnumerable<K> keys)
        {
            _dic = new LazyDictionary<K, V>(gen,keys.Count());
            _keys = keys.ToLockedCollection();
        }
        public bool TryGetValue(K key, out V value)
        {
            value = default(V);
            if (!ContainsKey(key))
                return false;
            value = this[key];
            return true;
        }
        public V this[K key]
        {
            get
            {
                if (!_keys.Contains(key))
                    throw new ArgumentOutOfRangeException("key does not exist");
                return _dic[key];
            }
        }
        public IEnumerable<K> Keys => _keys;
        public IEnumerable<V> Values => _keys.Select(a => _dic[a]);
        public bool ContainsKey(K key)
        {
            return _keys.Contains(key);
        }
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return _keys.Select(a => new KeyValuePair<K,V>(a, _dic[a])).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public int Count => _keys.Count;
    }
    public class EnumerableCache<T> : IReadOnlyList<T>
    {
        private readonly IEnumerator<T> _tor;
        private readonly IList<T> _cache;
        private bool _formed = false;
        public EnumerableCache(IEnumerable<T> tor)
        {
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
        public T this[int ind]
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
        public IEnumerator<T> GetEnumerator()
        {
            int i = 0;
            while (InflateToIndex(i))
            {
                yield return _cache[i];
                i++;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_cache).GetEnumerator();
        }
        public int Count
        {
            get
            {
                if (!_formed)
                    InflateToIndex(null);
                return _cache.Count;
            }
        }
    }
    public class EnumerationStream : Stream
    {
        private IEnumerator<byte> _tor;
        private readonly Lazy<int> _lcount; 
        public EnumerationStream(IEnumerable<byte> tor)
        {
            _tor = tor.GetEnumerator();
            _lcount = new Lazy<int>(tor.Count);
        }
        public override void Flush()
        {
            throw new NotSupportedException();
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_tor == null)
                return 0;
            int ret = 0;
            foreach (int i in Loops.Range(offset,count))
            {
                if (!_tor.MoveNext())
                {
                    _tor = null;
                    break;
                }
                buffer[i] = _tor.Current;
                ret++;
            }
            return ret;
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length
        {
            get
            {
                return _lcount.Value;
            }
        }
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
    public class ResizingArray<T> : ICollection<T>, IReadOnlyList<T>
    {
        public ResizingArray(int capacity = 0)
        {
            _arr = new T[capacity];
        } 
        public T[] arr
        {
            get
            {
                minimize();
                return _arr;
            }
        }
        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }
        public int Count { get; private set; }= 0;
        public bool IsReadOnly
        {
            get
            {
                return _arr.IsReadOnly;
            }
        }
        private T[] _arr;
        public void minimize()
        {
            Array.Resize(ref _arr,Count);
        }
        public void ResizeTo(int lastindex)
        {
            if (lastindex == 0)
            {
                _arr = new T[0];
                return;
            }
            while (!_arr.isWithinBounds(lastindex))
                Array.Resize(ref _arr, arr.Length == 0 ? lastindex+1 : Math.Max(arr.Length * 2, lastindex+1));
        }
        public void Add(T x)
        {
            ResizeTo(Count+1);
            _arr[Count++] = x;
        }
        public void AddRange(IEnumerable<T> x)
        {
            int c = x.Count();
            ResizeTo(Count+c - 1);
            foreach (var i in Loops.Count(Count).Zip(x))
            {
                _arr[i.Item1] = i.Item2;
            }
            Count += c;
        }
        public void Clear()
        {
            _arr = new T[0];
        }
        public bool Contains(T item)
        {
            return _arr.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _arr.CopyTo(array, arrayIndex);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _arr.Take(Count).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public T this[int index]
        {
            get
            {
                if (index >= Count)
                    throw new ArgumentOutOfRangeException();
                return _arr[index];
            }
        }
        public static implicit operator T[](ResizingArray<T> @this)
        {
            Array.Resize(ref @this._arr,@this.Count);
            return @this.arr;
        }
    }
    public class ListSlice<T> : IList<T>
    {
        private readonly IList<T> _inner;
        private readonly int _start;
        public ListSlice(IList<T> inner, int start, int length)
        {
            _inner = inner;
            _start = start;
            Count = length;
        }
        private IEnumerable<int> IndexRange()
        {
            return Loops.Range(Count).Select(a => a + _start);
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (int i in IndexRange())
            {
                yield return _inner[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(T item)
        {
            _inner.Insert(_start+Count,item);
            Count++;
        }
        public void Clear()
        {
            while (Count > 0)
            {
                Count--;
                _inner.RemoveAt(_start);
            }
        }
        public bool Contains(T item)
        {
            return this.Any(a => a.Equals(item));
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var t in this.CountBind(arrayIndex))
            {
                array[t.Item2] = t.Item1;
            }
        }
        public bool Remove(T item)
        {
            foreach (var t in this.CountBind(_start))
            {
                if (item.Equals(t.Item1))
                {
                    Count--;
                    _inner.RemoveAt(t.Item2);
                    return true;
                }
            }
            return false;
        }
        public int Count { get; private set; }
        public bool IsReadOnly
        {
            get
            {
                return _inner.IsReadOnly;
            }
        }
        public int IndexOf(T item)
        {
            foreach (var t in this.CountBind())
            {
                if (item.Equals(t.Item1))
                {
                    return t.Item2;
                }
            }
            return -1;
        }
        public void Insert(int index, T item)
        {
            _inner.Insert(_start + index, item);
            Count++;
        }
        public void RemoveAt(int index)
        {
            _inner.RemoveAt(_start + index);
            Count--;
        }
        public T this[int index]
        {
            get
            {
                if (index > Count || index < 0)
                    throw new ArgumentOutOfRangeException();
                return _inner[index+_start];
            }
            set
            {
                if (index > Count || index < 0)
                    throw new ArgumentOutOfRangeException();
                _inner[index + _start] = value;
            }
        }
        public ListSlice<T> ReSlice(int start, int length)
        {
            return new ListSlice<T>(this._inner,this._start+start, length);
        } 
    }
    namespace Arr2D
    {
        public static class Array2D
        {
            public static T[,] Fill<T>(int rows, int cols, T tofill = default(T))
            {
                Func<int, int, T> tf = (n, m) => tofill;
                return Fill(rows, cols, tf);
            }
            public static T[,] Fill<T>(int rows, int cols, Func<int, int, T> tofill)
            {
                T[,] ret = new T[rows, cols];
                for (int i = 0; i < ret.GetLength(0); i++)
                {
                    for (int j = 0; j < ret.GetLength(1); j++)
                        ret[i, j] = tofill(i, j);
                }
                return ret;
            }
            public static IEnumerable<int> getSize(this Array mat)
            {
                return Loops.Range(mat.Rank).Select(mat.GetLength);
            }
            public static T[,] to2DArr<T>(this T[] a, int dim0Length)
            {
                if (a.Length % dim0Length != 0)
                    throw new Exception("array length must divide row length evenly");
                int dim2Length = a.Length / dim0Length;
                T[,] ret = new T[dim0Length, dim2Length];
                for (int i = 0; i < dim0Length; i++)
                {
                    for (int j = 0; j < dim2Length; j++)
                        ret[i, j] = a[i * ret.GetLength(1) + j];
                }
                return ret;
            }
            public static bool isWithinBounds(this Array arr, params int[] ind)
            {
                if (arr.Rank != ind.Length)
                    throw new ArgumentException("mismatch on indices");
                return arr.getSize().Zip(ind).All(a => a.Item2.iswithinPartialExclusive(0, a.Item1));
            }
            public static IEnumerable<IEnumerable<T>> Rows<T>(this T[,] @this)
            {
                return Loops.Range(@this.GetLength(0)).Select(a => Loops.Range(@this.GetLength(1)).Select(x => @this[a, x]));
            }
            public static IEnumerable<IEnumerable<T>> Cols<T>(this T[,] @this)
            {
                return Loops.Range(@this.GetLength(1)).Select(a => Loops.Range(@this.GetLength(0)).Select(x => @this[x, a]));
            }
            public static string ToTablePrintable<T>(this T[,] arr, string openerfirst = "/", string openermid = "|", string openerlast = @"\",
                                                     string closerfirst = @"\", string closermid = "|", string closerlast = "/", string divider = " ",
                                                     string linediv = null)
            {
                linediv = linediv ?? Environment.NewLine;
                var cols = arr.Cols();
                int[] collengths = cols.SelectToArray(a => a.Max(x => x.ToString().Length));
                StringBuilder ret = new StringBuilder();
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    string opener = openermid;
                    if (i == 0)
                        opener = openerfirst;
                    if (i == arr.GetLength(0) - 1)
                        opener = openerlast;
                    ret.Append(opener);
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (j > 0)
                            ret.Append(divider);
                        ret.Append(arr[i, j].ToString().PadLeft(collengths[j]));
                    }
                    string closer = closermid;
                    if (i == 0)
                        closer = closerfirst;
                    if (i == arr.GetLength(0) - 1)
                        closer = closerlast;
                    ret.Append(closer + linediv);
                }
                return ret.ToString();
            }
            public static string ToTablePrintable<T>(this IEnumerable<IEnumerable<T>> @this, string opener = "{", string closer = "}",
                                                     string instanceopener = "[", string instancecloser = "]", string instancediv = ", ",
                                                     string linediv = null)
            {
                linediv = linediv ?? Environment.NewLine;
                return @this.Select(a => a.ToPrintable(instancediv, instanceopener, instancecloser)).ToPrintable(linediv, opener, closer);
            }
            public static T[,] Concat<T>(int dimen, params T[][,] a)
            {
                switch (dimen)
                {
                    case 0:
                        {
                            if (!a.AllEqual(new EqualityFunctionComparer<T[,], int>(x => x.GetLength(1))) || a.Length == 0)
                                throw new ArgumentException("the arrays must be non-empty and of compatible sizes");
                            T[,] ret = new T[a.Sum(x => x.GetLength(0)), a[0].GetLength(1)];
                            int row = 0;
                            foreach (T[,] m in a)
                            {
                                foreach (int i in Loops.Range(m.GetLength(0)))
                                {
                                    foreach (int j in Loops.Range(m.GetLength(1)))
                                        ret[row, j] = m[i, j];
                                    row++;
                                }
                            }
                            return ret;
                        }
                    case 1:
                        {
                            if (!a.AllEqual(new EqualityFunctionComparer<T[,], int>(x => x.GetLength(0))) || a.Length == 0)
                                throw new ArgumentException("the arrays must be non-empty and of compatible sizes");
                            T[,] ret = new T[a[0].GetLength(0), a.Sum(x => x.GetLength(1))];
                            int col = 0;
                            foreach (T[,] m in a)
                            {
                                foreach (int i in Loops.Range(m.GetLength(1)))
                                {
                                    foreach (int j in Loops.Range(m.GetLength(0)))
                                        ret[j, col] = m[j, i];
                                    col++;
                                }
                            }
                            return ret;
                        }
                }
                throw new ArgumentException($"{nameof(dimen)} must be either 1 or 0");
            }
            public static T[,] Concat<T>(this T[,] @this, T[,] other, int dimen)
            {
                return Concat(dimen, @this, other);
            }
        }
        public class SymmetricMatrix<T>
        {
            private readonly T[] _data;
            private int Size { get; }
            public bool Reflexive { get; }
            public SymmetricMatrix(int size, bool reflexive = true)
            {
                this.Size = size;
                if (Size < 0)
                    throw new ArgumentException("must be non-negative",nameof(size));
                Reflexive = reflexive;
                _data = new T[(this.Size * (this.Size + (reflexive ? 1 : -1))) / 2];
            }
            private int getindex(int r, int c)
            {
                if (r >= this.Size || c >= this.Size)
                    throw new IndexOutOfRangeException();
                if (r > c)
                    return getindex(c, r);
                if (this.Reflexive)
                    return (int)(r * (this.Size - (r + 1) / 2.0) + c);
                if (r == c)
                    throw new IndexOutOfRangeException("this matrix is non-reflexive");
                return (int)(r * (this.Size - (r + 3) / 2.0) + c - 1);
            }
            public int length
            {
                get
                {
                    return _data.Length;
                }
            }
            public T this[int row, int col]
            {
                get
                {
                    if (row >= Size || col >= Size)
                        throw new IndexOutOfRangeException();
                    return _data[this.getindex(row, col)];
                }
                set
                {
                    if (row >= Size || col >= Size)
                        throw new IndexOutOfRangeException();
                    _data[this.getindex(row, col)] = value;
                }
            }
            public int GetLength(int i)
            {
                switch (i)
                {
                    case 1:
                    case 0:
                        return this.Size;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            public T[,] toArr()
            {
                T[,] ret = new T[this.Size, this.Size];
                for (int i = 0; i < ret.GetLength(0); i++)
                {
                    for (int j = 0; j < ret.GetLength(1); j++)
                        ret[i, j] = ((i == j && !this.Reflexive) ? default(T) : this[i, j]);
                }
                return ret;
            }
        }
    }
} 
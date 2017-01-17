using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class editDistance
    {
        public interface IEditStep<T>
        {
            IEnumerable<T> apply(IEnumerable<T> en);
            void apply(IList<T> li);
        }

        public class Insert<T> : IEditStep<T>
        {
            public Insert(T newVal, int newInd)
            {
                this.newVal = newVal;
                this.newInd = newInd;
            }
            public T newVal { get; }
            public int newInd { get; }
            public IEnumerable<T> apply(IEnumerable<T> en)
            {
                return en.Splice(newVal, newInd);
            }
            public void apply(IList<T> li)
            {
                li.Insert(newInd, newVal);
            }
            public override string ToString()
            {
                return $"Add {newVal} at index {newInd}";
            }
        }
        public class Delete<T> : IEditStep<T>
        {
            public Delete(int deletedInd)
            {
                this.deletedInd = deletedInd;
            }
            public int deletedInd { get; }
            public IEnumerable<T> apply(IEnumerable<T> en)
            {
                return en.SkipSlice(deletedInd);
            }
            public void apply(IList<T> li)
            {
                li.RemoveAt(deletedInd);
            }
            public override string ToString()
            {
                return $"Delete index {deletedInd}";
            }
        }
        public class Substitute<T> : IEditStep<T>
        {
            public Substitute(T newVal, int ind)
            {
                this.newVal = newVal;
                this.ind = ind;
            }
            public T newVal { get; }
            public int ind { get; }
            public IEnumerable<T> apply(IEnumerable<T> en)
            {
                return en.Cover(newVal, ind);
            }
            public void apply(IList<T> li)
            {
                li[ind] = newVal;
            }
            public override string ToString()
            {
                return $"Set index {ind} with {newVal}";
            }
        }
        
        public static IEnumerable<IEditStep<T>> EditSteps<T>(this IEnumerable<T> @this, IList<T> other, IEqualityComparer<T> comp = null, bool allowIns = true, bool allowDel = true, bool allowSub = true)
        {
            comp = comp ?? EqualityComparer<T>.Default;
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
                            throw new Exception("No Edit Path Found!");
                    }
                }
            }
        }
        public static int EditDistance<T>(this IEnumerable<T> @this, IEnumerable<T> other, IEqualityComparer<T> comp = null, bool allowIns = true, bool allowDel = true, bool allowSub = true)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            IList<int> v = allowIns ? range.IRange(other.Count()) : (-2).Enumerate(other.Count()+1);

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
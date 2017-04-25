using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;

namespace WhetStone.Comparison
{
    public class SequenceIndexEquator<T> : IEqualityComparer<IList<T>>
    {
        private readonly IEqualityComparer<T> _int;
        public SequenceIndexEquator(IEqualityComparer<T> i)
        {
            _int = i ?? EqualityComparer<T>.Default;
        }
        public bool Equals(IList<T> @this, IList<T> other)
        {
            if (@this.Count != other.Count)
                return false;
            foreach (Tuple<Tuple<T>,Tuple<T>,Tuple<int>> t in @this.ZipUnBoundTuple(other,@this.Indices()))
            {
                if (t.Item1 == null || t.Item2 == null || t.Item3 == null)
                    return false;
                int ind = t.Item3.Item1;

                var tind = @this[ind];
                var oind = other[ind];
                var tnum = t.Item1.Item1;
                var onum = t.Item2.Item1;

                if (!new [] { tind,oind,tnum,onum }.AllEqual(_int))
                    return false;
            }
            return true;
        }
        public int GetHashCode(IList<T> obj)
        {
            return obj.Take(5).Select(_int.GetHashCode).Aggregate(0, (i, j) => i ^ (3*j));
        }
    }
}

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
            return @this.Select(Tuple.Create).ZipUnbound(other.Select(Tuple.Create), @this.Indices().Select(Tuple.Create))
                .All(a =>
                {
                    if (a.Item1 == null || a.Item2 == null || a.Item3 == null)
                        return false;
                    var arr = new[] {a.Item1.Item1, a.Item2.Item1, @this[a.Item3.Item1], other[a.Item3.Item1]};
                    return arr.AllEqual(_int);
                }); 
        }
        public int GetHashCode(IList<T> obj)
        {
            return obj.Take(5).Select(_int.GetHashCode).Aggregate(0, (i, j) => i ^ (3*j));
        }
    }
}

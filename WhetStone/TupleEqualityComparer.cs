using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Comparison
{
    public class TupleEqualityComparer<T1, T2> : IEqualityComparer<Tuple<T1, T2>>
    {
        private readonly IEqualityComparer<T1> _c1;
        private readonly IEqualityComparer<T2> _c2;
        public TupleEqualityComparer(IEqualityComparer<T1> c1 = null, IEqualityComparer<T2> c2 = null)
        {
            _c1 = c1 ?? EqualityComparer<T1>.Default;
            _c2 = c2 ?? EqualityComparer<T2>.Default;
        }
        public bool Equals(Tuple<T1, T2> x, Tuple<T1, T2> y)
        {
            return _c1.Equals(x.Item1, y.Item1) && _c2.Equals(x.Item2, x.Item2);
        }
        public int GetHashCode(Tuple<T1, T2> obj)
        {
            return _c1.GetHashCode(obj.Item1) ^ _c2.GetHashCode(obj.Item2);
        }
    }
    public class TupleEqualityComparer<T1, T2, T3> : IEqualityComparer<Tuple<T1, T2, T3>>
    {
        private readonly IEqualityComparer<T1> _c1;
        private readonly IEqualityComparer<T2> _c2;
        private readonly IEqualityComparer<T3> _c3;
        public TupleEqualityComparer(IEqualityComparer<T1> c1 = null, IEqualityComparer<T2> c2 = null, IEqualityComparer<T3> c3 = null)
        {
            _c1 = c1 ?? EqualityComparer<T1>.Default;
            _c2 = c2 ?? EqualityComparer<T2>.Default;
            _c3 = c3 ?? EqualityComparer<T3>.Default;
        }
        public bool Equals(Tuple<T1, T2, T3> x, Tuple<T1, T2, T3> y)
        {
            return _c1.Equals(x.Item1, y.Item1) && _c2.Equals(x.Item2, x.Item2) && _c3.Equals(x.Item3, y.Item3);
        }
        public int GetHashCode(Tuple<T1, T2, T3> obj)
        {
            return _c1.GetHashCode(obj.Item1) ^ _c2.GetHashCode(obj.Item2) ^ _c3.GetHashCode(obj.Item3);
        }
    }
}

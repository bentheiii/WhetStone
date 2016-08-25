using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Comparison
{
    public class EnumerableEqualityCompararer<T> : IEqualityComparer<IEnumerable<T>>
    {
        private readonly IEqualityComparer<T> _int;
        private readonly int _maxhashlength;
        public EnumerableEqualityCompararer(int maxhashlength = 5) : this(EqualityComparer<T>.Default, maxhashlength) { }
        public EnumerableEqualityCompararer(IEqualityComparer<T> i, int maxhashlength = 5)
        {
            this._int = i;
            _maxhashlength = maxhashlength;
        }
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            return x.SequenceEqual(y, _int);
        }
        public int GetHashCode(IEnumerable<T> obj)
        {
            int ret = 0;
            int i = 1;
            foreach (var v in obj.Take(_maxhashlength))
            {
                ret ^= _int.GetHashCode(v) * i;
                i++;
            }
            return ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Comparison
{
    public class PriorityComparer<T> : IComparer<T>
    {
        private readonly IEnumerable<IComparer<T>> _comps;
        private PriorityComparer(IEnumerable<IComparer<T>> c)
        {
            this._comps = c;
        }
        public PriorityComparer(params IComparer<T>[] c)
        {
            this._comps = c.ToArray();
        }
        public PriorityComparer(params Func<T, T, int>[] c) : this(c.Select(a => ((IComparer<T>)new FunctionComparer<T>(a))))
        {
        }
        public PriorityComparer(params Func<T, IComparable>[] c)
            : this(c.Select(a => new FunctionComparer<T>(a)))
        {
        }
        public int Compare(T x, T y)
        {
            return _comps.Select(c => c.Compare(x, y)).FirstOrDefault(ret => ret != 0);
        }
    }
}

using System.Collections.Generic;
using WhetStone.Looping;

namespace WhetStone.Comparison
{
    public class EnumerableCompararer<T> : IComparer<IEnumerable<T>>
    {
        private readonly IComparer<T> _int;
        public EnumerableCompararer(IComparer<T> i = null)
        {
            this._int = i ?? Comparer<T>.Default;
        }
        public int Compare(IEnumerable<T> x, IEnumerable<T> y)
        {
            int ret = 0;
            foreach (var z in x.Zip(y))
            {
                ret = _int.Compare(z.Item1, z.Item2);
                if (ret != 0)
                    break;
            }
            return ret;
        }
    }
}

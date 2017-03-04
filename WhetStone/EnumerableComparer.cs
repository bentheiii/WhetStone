using System.Collections.Generic;
using WhetStone.Looping;

namespace WhetStone.Comparison
{
    //todo delete?
    //todo handle unequal lengths
    //todo tack on iequalitycomparer
    /// <summary>
    /// A comparer that compares <see cref="IEnumerable{T}"/>s, element-wise, then length-wise.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s to compare.</typeparam>
    public class EnumerableCompararer<T> : IComparer<IEnumerable<T>>
    {
        private readonly IComparer<T> _int;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="i">The inner <see cref="IComparer{T}"/> to compare individual elements.</param>
        public EnumerableCompararer(IComparer<T> i = null)
        {
            this._int = i ?? Comparer<T>.Default;
        }
        //todo doesn't sort by length
        /// <inheritdoc />
        public int Compare(IEnumerable<T> x, IEnumerable<T> y)
        {
            int ret = 0;
            foreach (var z in x.Zip(y))
            {
                ret = _int.Compare(z.Item1, z.Item2);
                if (ret != 0)
                    break;
            }
            //todo improve this
            if (ret == 0)
                ret = x.CompareCount(y);
            return ret;
        }
    }
}

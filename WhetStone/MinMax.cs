using System.Collections.Generic;
using System.Numerics;

namespace WhetStone.Comparison
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class minmax
    {
        /// <summary>
        /// Reorganizes two values to have strict order.
        /// </summary>
        /// <param name="min">The smaller value.</param>
        /// <param name="max">The larger value.</param>
        /// <returns>Whether the two values were switched.</returns>
        public static bool MinMax(ref int min, ref int max)
        {
            if (min < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
        /// <summary>
        /// Reorganizes two values to have strict order.
        /// </summary>
        /// <param name="min">The smaller value.</param>
        /// <param name="max">The larger value.</param>
        /// <returns>Whether the two values were switched.</returns>
        public static bool MinMax(ref BigInteger min, ref BigInteger max)
        {
            if (min < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
        /// <summary>
        /// Reorganizes two values to have strict order.
        /// </summary>
        /// <param name="min">The smaller value.</param>
        /// <param name="max">The larger value.</param>
        /// <returns>Whether the two values were switched.</returns>
        public static bool MinMax(ref double min, ref double max)
        {
            if (min < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
        /// <summary>
        /// Reorganizes two values to have strict order.
        /// </summary>
        /// <typeparam name="T">The type of the values</typeparam>
        /// <param name="min">The smaller value.</param>
        /// <param name="max">The larger value.</param>
        /// <param name="comp">The <see cref="IComparer{T}"/> to compare the values.</param>
        /// <returns>Whether the two values were switched.</returns>
        public static bool MinMax<T>(ref T min, ref T max, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            if (comp.Compare(min,max) <= 0)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
    }
}

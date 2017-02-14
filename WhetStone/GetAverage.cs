using System.Collections.Generic;
using System.Linq;
using WhetStone.Fielding;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class getAverage
    {
        /// <summary>
        /// Get arithmetic average of an <see cref="IEnumerable{T}"/> using fielding.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="tosearch">The <see cref="IEnumerable{T}"/> to find the average of.</param>
        /// <returns>The arithmetic average of the elements in <paramref name="tosearch"/></returns>
        /// <remarks>This function uses fielding, use <see cref="Enumerable.Average(System.Collections.Generic.IEnumerable{int})"/> for non-generic types.</remarks>
        public static T GetAverage<T>(this IEnumerable<T> tosearch)
        {
            int? reccount = tosearch.RecommendCount();
            int count;
            FieldWrapper<T> sum;
            if (reccount.HasValue)
            {
                count = reccount.Value;
                sum = tosearch.GetSum();
            }
            else
            {
                IGuard<int> cg = new Guard<int>();
                sum = tosearch.HookCount(cg).GetSum();
                count = cg.value;
            }
            return sum/count;
        }
        /// <summary>
        /// Get geometric average of an <see cref="IEnumerable{T}"/> using fielding.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="tosearch">The <see cref="IEnumerable{T}"/> to find the average of.</param>
        /// <returns>The geometric average of the elements in <paramref name="tosearch"/></returns>
        /// <remarks>This function uses fielding.</remarks>
        public static T GetGeometricAverage<T>(this IEnumerable<T> tosearch)
        {
            var f = Fields.getField<T>();
            int? reccount = tosearch.RecommendCount();
            int count;
            T product;
            if (reccount.HasValue)
            {
                count = reccount.Value;
                product = tosearch.Aggregate(f.one, f.multiply);
            }
            else
            {
                IGuard<int> cg = new Guard<int>();
                product = tosearch.HookCount(cg).Aggregate(f.one,f.multiply);
                count = cg.value;
            }
            return f.pow(product, f.Invert(f.fromInt(count)));
        }
    }
}

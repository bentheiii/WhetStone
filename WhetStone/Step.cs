using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class step
    {
        /// <summary>
        /// Strides an <see cref="IEnumerable{T}"/> in steps.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to stride.</param>
        /// <param name="step">The distance between indices stridden.</param>
        /// <param name="start">The offset of the indices.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> stridden in <paramref name="step"/> steps and <paramref name="start"/> offset.</returns>
        public static IEnumerable<T> Step<T>(this IEnumerable<T> @this, int step, int start = 0)
        {
            int c = start;
            foreach (var t in @this)
            {
                if (c == 0)
                    yield return t;
                c++;
                if (c == step)
                    c = 0;
            }
        }
    }
}

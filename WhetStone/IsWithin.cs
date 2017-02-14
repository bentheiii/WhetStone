using System.Collections.Generic;
using WhetStone.Comparison;

namespace NumberStone
{
    /// <summary>
    /// A static function container for comparing an element to two boundaries.
    /// </summary>
    public static class isWithin
    {
        /// <overloads>Get whether an element is between two boundaries or equal to either of them.</overloads>
        /// <summary>
        /// Get whether an element is between two boundaries or equal to either of them.
        /// </summary>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary.</param>
        /// <param name="border2">The second boundary.</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/> or equal to either of them.</returns>
        public static bool iswithin(this int x, int border1, int border2)
        {
            return iswithinexclusive(x, border1, border2) || x == border1 || x == border2;
        }
        /// <summary>
        /// Get whether an element is between two boundaries or equal to either of them.
        /// </summary>
        /// <typeparam name="T">The type of elements to compare.</typeparam>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary.</param>
        /// <param name="border2">The second boundary.</param>
        /// <param name="comp">The <see cref="IComparer{T}"/> to check for equality and inequality</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/> or equal to either of them.</returns>
        public static bool iswithin<T>(this T x, T border1, T border2, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            return iswithinexclusive(x, border1, border2, comp) || comp.Compare(x,border1) == 0 || comp.Compare(x,border2) == 0;
        }
        /// <summary>
        /// Get whether an element is between two boundaries or equal to either of them.
        /// </summary>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary.</param>
        /// <param name="border2">The second boundary.</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/> or equal to either of them.</returns>
        public static bool iswithin(this double x, double border1, double border2)
        {
            return iswithinexclusive(x, border1, border2) || x == border1 || x == border2;
        }
        /// <overloads>Get whether an element is between two boundaries or equal to the first of them.</overloads>
        /// <summary>
        /// Get whether an element is between two boundaries or equal to the first.
        /// </summary>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary. Inclusive.</param>
        /// <param name="border2">The second boundary. Exclusive.</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/> or equal to <paramref name="border1"/>.</returns>
        /// <remarks>
        /// <para>If <paramref name="border1"/> is equal to <paramref name="border2"/>, nothing will be between them. </para>
        /// <para><paramref name="border1"/> need not be smaller than <paramref name="border2"/>.</para>
        /// </remarks>
        public static bool iswithinPartialExclusive(this int x, int border1, int border2)
        {
            return iswithinexclusive(x, border1, border2) || (x == border1 && x != border2);
        }
        /// <summary>
        /// Get whether an element is between two boundaries or equal to the first.
        /// </summary>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary. Inclusive.</param>
        /// <param name="border2">The second boundary. Exclusive.</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/> or equal to <paramref name="border1"/>.</returns>
        /// <remarks>
        /// <para>If <paramref name="border1"/> is equal to <paramref name="border2"/>, nothing will be between them. </para>
        /// <para><paramref name="border1"/> need not be smaller than <paramref name="border2"/>.</para>
        /// </remarks>
        public static bool iswithinPartialExclusive(this double x, double border1, double border2)
        {
            return iswithinexclusive(x, border1, border2) || (x == border1 && x != border2);
        }
        /// <summary>
        /// Get whether an element is between two boundaries or equal to the first.
        /// </summary>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary. Inclusive.</param>
        /// <param name="border2">The second boundary. Exclusive.</param>
        /// <param name="comp">The <see cref="IComparer{T}"/> to check for equality and inequality</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/> or equal to <paramref name="border1"/>.</returns>
        /// <remarks>
        /// <para>If <paramref name="border1"/> is equal to <paramref name="border2"/>, nothing will be between them. </para>
        /// <para><paramref name="border1"/> need not be smaller than <paramref name="border2"/>.</para>
        /// </remarks>
        public static bool iswithinPartialExclusive<T>(this T x, T border1, T border2, IComparer<T> comp = null)
        {
            return iswithinexclusive(x, border1, border2, comp) || (comp.Compare(x, border1)  == 0 && comp.Compare(x, border2) != 0);
        }
        /// <overloads>Get whether an element is between two boundaries.</overloads>
        /// <summary>
        /// Get whether an element is between two boundaries or equal to either of them.
        /// </summary>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary.</param>
        /// <param name="border2">The second boundary.</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/>.</returns>
        public static bool iswithinexclusive(this int x, int border1, int border2)
        {
            double min;
            double max;
            if (border1 < border2)
            {
                min = border1;
                max = border2;
            }
            else
            {
                min = border2;
                max = border1;
            }
            return x > min && x < max;
        }
        /// <summary>
        /// Get whether an element is between two boundaries or equal to either of them.
        /// </summary>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary.</param>
        /// <param name="border2">The second boundary.</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/>.</returns>
        public static bool iswithinexclusive(this double x, double border1, double border2)
        {
            minmax.MinMax(ref border1, ref border2);
            return x > border1 && x < border2;
        }
        /// <summary>
        /// Get whether an element is between two boundaries or equal to either of them.
        /// </summary>
        /// <param name="x">The element to check</param>
        /// <param name="border1">The first boundary.</param>
        /// <param name="border2">The second boundary.</param>
        /// <param name="comp">The <see cref="IComparer{T}"/> to check for equality and inequality</param>
        /// <returns>Whether <paramref name="x"/> is between <paramref name="border1"/> an <paramref name="border2"/>.</returns>
        public static bool iswithinexclusive<T>(this T x, T border1, T border2, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            minmax.MinMax(ref border1, ref border2, comp);
            return comp.Compare(x,border1) > 0 && comp.Compare(x, border2) < 0;
        }
    }
}

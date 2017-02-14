using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    //todo indiscrete binary search
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class binarySearch
    {
        private static int BinarySearchBoundsUnknown(Func<int, int> searcher, int failval = -1)
        {
            var s = searcher(0);
            if (s == 0)
                return 0;
            if (s < 0)
                return BinarySearchMaxUnknown(searcher, 0, failval);
            return BinarySearchMinUnknown(searcher, 0, failval);
        }
        private static int BinarySearchMaxUnknown(Func<int, int> searcher, int min, int failval = -1)
        {
            if (searcher(min) == 0)
                return min;
            int range = 1;
            int practmin = min;
            while (true)
            {
                int max = min + range;
                var s = searcher(max);
                if (s == 0)
                    return max;
                if (s > 0)
                    return BinarySearch(searcher, practmin, max, failval);
                practmin = max;
                range *= 2;
            }
        }
        private static int BinarySearchMinUnknown(Func<int, int> searcher, int max, int failval = -1)
        {
            if (searcher(max) == 0)
                return max;
            int range = 1;
            int practmax = max;
            while (true)
            {
                int min = max - range;
                var s = searcher(min);
                if (s == 0)
                    return max;
                if (s < 0)
                    return BinarySearch(searcher, min, practmax, failval);
                practmax = min;
                range *= 2;
            }
        }
        private static int BinarySearchAllKnown(Func<int, int> searcher, int min, int max, int failvalue = -1)
        {
            while (min < max)
            {
                int i = (min + max)/2;
                int res = searcher(i);
                if (res == 0)
                    return i;
                if (i == min)
                    break;
                if (res < 0)
                    min = i;
                else
                    max = i;
            }
            return failvalue;
        }
        /// <overloads>Performs a binary search.</overloads>
        /// <summary>
        /// Performs a binary search on a discrete range from <paramref name="min"/> to <paramref name="max"/> (exclusive). Returns the number for which <paramref name="searcher"/> returns 0.
        /// </summary>
        /// <param name="searcher">The searcher function, a positive number means the input is too high, a negative number means the input is too low, zero means the input has been found.</param>
        /// <param name="min">The minimum of the range to search in (inclusive). Setting this to <see langword="null"/> will perform an exponential search.</param>
        /// <param name="max">The maximum of the range to search in (exclusive). Setting this to <see langword="null"/> will perform an exponential search.</param>
        /// <param name="failvalue">The value to return if no correct index is found.</param>
        /// <returns>The index on which <paramref name="searcher"/> returned zero, or <paramref name="failvalue"/> if such is not found.</returns>
        /// <remarks>In case both <paramref name="min"/> and <paramref name="max"/> are set to <see langword="null"/>, index 0 will be tested, and an exponential search will be done with zero being either an upper or lower bound.</remarks>
        public static int BinarySearch(Func<int, int> searcher, int? min = null, int? max = null, int failvalue = -1)
        {
            if (min == null)
            {
                if (max == null)
                    return BinarySearchBoundsUnknown(searcher, failvalue);
                return BinarySearchMinUnknown(searcher, max.Value, failvalue);
            }
            if (max == null)
                return BinarySearchMaxUnknown(searcher, min.Value, failvalue);
            return BinarySearchAllKnown(searcher, min.Value, max.Value, failvalue);
        }
        /// <summary>
        /// A style for boolean binary search.
        /// </summary>
        public enum BooleanBinSearchStyle
        {
            /// <summary>
            /// A boolean binary search with this style will attempt to return the index of the last input to return <see langword="true"/>.
            /// </summary>
            /// <remarks>searcher(min) should return <see langword="true"/></remarks>
            GetLastTrue,
            /// <summary>
            /// A boolean binary search with this style will attempt to return the index of the first input to return <see langword="true"/>.
            /// </summary>
            /// <remarks>searcher(max) should return <see langword="true"/></remarks>
            GetFirstTrue
        }
        /// <summary>
        /// Performs a binary search on a discrete range from <paramref name="min"/> to <paramref name="max"/> (exclusive). Returns the number for which <paramref name="searcher"/> returns either the first or last <see langword="true"/> in the range, depending on the <paramref name="style"/>.
        /// </summary>
        /// <param name="searcher">A searcher function.</param>
        /// <param name="min">The minimum of the range to search in (inclusive). Setting this to <see langword="null"/> will perform an exponential search.</param>
        /// <param name="max">The maximum of the range to search in (exclusive). Setting this to <see langword="null"/> will perform an exponential search.</param>
        /// <param name="failvalue">The value to return if no correct index is found.</param>
        /// <param name="style">The boolean search style, whether to return the index of the first or last <see langword="true"/>.</param>
        /// <returns>The index on which <paramref name="searcher"/> returned the first or last <see langword="true"/>, or <paramref name="failvalue"/> if such is not found.</returns>
        /// <remarks><para>For every "normal" binary search call, the <paramref name="searcher"/> will be called twice.</para><para>Any exception thrown by the <paramref name="searcher"/> will be treated as a <see langword="false"/> output.</para></remarks>
        public static int BinarySearch(Func<int, bool> searcher, int? min = null, int? max = null, int failvalue = -1, BooleanBinSearchStyle style = BooleanBinSearchStyle.GetLastTrue)
        {
            Func<int, int> s;
            if (style == BooleanBinSearchStyle.GetLastTrue)
                s = i =>
                {
                    if (searcher(i))
                    {
                        bool ks;
                        try
                        {
                            ks = searcher(i + 1);
                        }
                        catch (Exception)
                        {
                            ks = false;
                        }
                        return !ks ? 0 : -1;
                    }
                    return 1;
                };
            else
                s = i =>
                {
                    if (searcher(i))
                    {
                        bool ks;
                        try
                        {
                            ks = searcher(i - 1);
                        }
                        catch (Exception)
                        {
                            ks = false;
                        }
                        return !ks ? 0 : 1;
                    }
                    return -1;
                };
            return BinarySearch(s, min, max, failvalue);
        }
        /// <summary>
        /// Performs a boolean binary search over an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to search.</param>
        /// <param name="searcher">The binary searcher function.</param>
        /// <param name="style">The search style of the boolean binary search.</param>
        /// <returns>The index that the binary search returns, or -1 if none found.</returns>
        /// <remarks>unlike <see cref="BinarySearch(Func{int,bool},int?,int?,int,BooleanBinSearchStyle)"/>, exception thrown by <paramref name="searcher"/> will not be caught.</remarks>
        public static int BinarySearch<T>(this IList<T> @this, Func<T, bool> searcher, BooleanBinSearchStyle style = BooleanBinSearchStyle.GetLastTrue)
        {
            Func<int, int> s;
            if (style == BooleanBinSearchStyle.GetLastTrue)
                s = i =>
                {
                    if (searcher(@this[i]))
                    {
                        return (i + 1 != @this.Count) && searcher(@this[i + 1]) ? -1 :0;
                    }
                    return 1;
                };
            else
                s = i =>
                {
                    if (searcher(@this[i]))
                    {
                        return i != 0 && searcher(@this[i - 1]) ? 1 : 0;
                    }
                    return -1;
                };
            return BinarySearch(s, 0, @this.Count);
        }
        /// <summary>
        /// Performs a binary search over an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to search.</param>
        /// <param name="searcher">The binary searcher function.</param>
        /// <returns>The index that the binary search returns, or -1 if none found.</returns>
        public static int BinarySearch<T>(this IList<T> @this, Func<T, int> searcher)
        {
            return BinarySearch(i => searcher(@this[i]), 0, @this.Count);
        }
        /// <summary>
        /// Performs a binary search for an element in an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The sorted <see cref="IList{T}"/> to perform the search on.</param>
        /// <param name="tofind">The element to search for.</param>
        /// <param name="comp">The comparer by which <paramref name="this"/> is sorted. If set to null, the default comparer will be used.</param>
        /// <returns>The index of <paramref name="tofind"/> in <paramref name="this"/>, or -1 if <paramref name="tofind"/> is not in <paramref name="this"/>.</returns>
        public static int BinarySearch<T>(this IList<T> @this, T tofind, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            return BinarySearch(@this, a => comp.Compare(a, tofind));
        }
        /// <overloads>Performs a exponential search.</overloads>
        /// <summary>
        /// Performs a exponential binary search over an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to search.</param>
        /// <param name="searcher">The binary searcher function.</param>
        /// <param name="style">The search style of the boolean binary search.</param>
        /// <returns>The index that the binary search returns, or -1 if none found.</returns>
        /// <remarks>unlike <see cref="BinarySearch(System.Func{int,bool},System.Nullable{int},System.Nullable{int},int,BooleanBinSearchStyle)"/>, exception thrown by <paramref name="searcher"/> will not be caught.</remarks>
        public static int BinarySearchStartBias<T>(this IList<T> @this, Func<T, bool> searcher, BooleanBinSearchStyle style = BooleanBinSearchStyle.GetLastTrue)
        {
            Func<int, int> s;
            if (style == BooleanBinSearchStyle.GetLastTrue)
                s = i =>
                {
                    if (searcher(@this[i]))
                    {
                        return (i + 1 != @this.Count) && searcher(@this[i + 1]) ? -1 : 0;
                    }
                    return 1;
                };
            else
                s = i =>
                {
                    if (searcher(@this[i]))
                    {
                        return i != 0 && searcher(@this[i - 1]) ? 1 : 0;
                    }
                    return -1;
                };
            return BinarySearch(s, 0);
        }
        /// <summary>
        /// Performs a exponential search over an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to search.</param>
        /// <param name="searcher">The binary searcher function.</param>
        /// <returns>The index that the binary search returns, or -1 if none found.</returns>
        public static int BinarySearchStartBias<T>(this IList<T> @this, Func<T, int> searcher)
        {
            return BinarySearch(i => i >= @this.Count ? 1 : searcher(@this[i]), 0);
        }
        /// <summary>
        /// Performs an exponential search for an element in an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The sorted <see cref="IList{T}"/> to perform the search on.</param>
        /// <param name="tofind">The element to search for.</param>
        /// <param name="comp">The comparer by which <paramref name="this"/> is sorted. If set to null, the default comparer will be used.</param>
        /// <returns>The index of <paramref name="tofind"/> in <paramref name="this"/>, or -1 if <paramref name="tofind"/> is not in <paramref name="this"/>.</returns>
        public static int BinarySearchStartBias<T>(this IList<T> @this, T tofind, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            return BinarySearchStartBias(@this, a => comp.Compare(a, tofind));
        }
    }
}

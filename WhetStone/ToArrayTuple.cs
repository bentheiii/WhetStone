using System;

namespace WhetStone.Tuples
{
    public static class toArrayTuple
    {
        public static T[] ToArray<T>(this Tuple<T> @this)
        {
            return new[] { @this.Item1 };
        }
        public static T[] ToArray<T>(this Tuple<T, T> @this)
        {
            return new[] { @this.Item1, @this.Item2 };
        }
        public static T[] ToArray<T>(this Tuple<T, T, T> @this)
        {
            return new[] { @this.Item1, @this.Item2, @this.Item3 };
        }
        public static T[] ToArray<T>(this Tuple<T, T, T, T> @this)
        {
            return new[] { @this.Item1, @this.Item2, @this.Item3, @this.Item4 };
        }
        public static T[] ToArray<T>(this Tuple<T, T, T, T, T> @this)
        {
            return new[] { @this.Item1, @this.Item2, @this.Item3, @this.Item4, @this.Item5 };
        }
    }
}

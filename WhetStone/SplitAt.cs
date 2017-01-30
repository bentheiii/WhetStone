using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class splitAt
    {
        public static IEnumerable<IEnumerable<T>> SplitAt<T>(this IEnumerable<T> @this, params int[] lengths)
        {
            using (var tor = @this.GetEnumerator())
            {
                foreach (int length in lengths)
                {
                    ResizingArray<T> ret = new ResizingArray<T>(length);
                    foreach (int _ in range.Range(length))
                    {
                        if (!tor.MoveNext())
                        {
                            yield return ret.arr;
                            yield break;
                        }
                        ret.Add(tor.Current);
                    }
                    yield return ret;
                }

                yield return infinite.Infinite().TakeWhile(a=>tor.MoveNext()).Select(a => tor.Current);
            }
        }
        public static IList<IList<T>> SplitAt<T>(this IList<T> @this, params int[] lengths)
        {
            return SplitAt(@this, lengths.AsList());
        }
        public static IList<IList<T>> SplitAt<T>(this IList<T> @this, IList<int> lengths)
        {
            lengths = lengths.PartialSums().ToList().Concat(@this.Count.Enumerate());
            var t = lengths.Trail(2);
            return t.Select(a => @this.Slice(a[0], a[1]));
        }
    }
}

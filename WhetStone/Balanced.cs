using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class balanced
    {
        public static bool Balanced<T>(this IEnumerable<T> @this, IEnumerable<T> opener, IEnumerable<T> closer, int? maxdepth = null)
        {
            if (opener.Equals(closer))
            {
                int c = @this.Count(opener);
                return c % 2 == 0 && (!maxdepth.HasValue || (c == 0 || maxdepth >= 1));
            }
            var openerindicies = @this.Trail(opener.Count()).CountBind().Where(a => a.Item1.SequenceEqual(opener)).Select(a => a.Item2);
            var closerindicies = @this.Trail(closer.Count()).CountBind().Where(a => a.Item1.SequenceEqual(closer)).Select(a => a.Item2);
            var parenonly = openerindicies.Attach(a => 0).Concat(closerindicies.Attach(a => 1)).OrderBy(a => a.Item1).Select(a => a.Item2);
            return parenonly.Balanced(0, 1, maxdepth);
        }
        public static bool Balanced<T>(this IEnumerable<T> @this, T opener, T closer, int? maxdepth = null)
        {
            if (opener.Equals(closer))
            {
                int c = @this.Count(opener);
                return c % 2 == 0 && (!maxdepth.HasValue || (c == 0 || maxdepth >= 1));
            }
            var count = @this.RecommendCount();
            int ret = 0;
            Guard<int> index = new Guard<int>();
            foreach (var t in @this.CountBind().Detach(index))
            {
                if (t.Equals(opener))
                {
                    ret++;
                    if (count.HasValue && count.Value - index < ret)
                        return false;
                    if (maxdepth.HasValue && maxdepth.Value < ret)
                        return false;
                }
                else if (t.Equals(closer))
                {
                    ret--;
                    if (ret < 0)
                        return false;
                }
            }
            return ret == 0;
        }
        public static bool Balanced<T>(this IEnumerable<T> @this, IEnumerable<Tuple<T, T>> couples, int? maxdepth = null)
        {
            Stack<T> layers = new Stack<T>(maxdepth ?? 0);
            var dic = couples.ToDictionary();
            foreach (T t in @this)
            {
                if (dic.ContainsKey(t))
                {
                    if (maxdepth.HasValue && layers.Count >= maxdepth.Value)
                        return false;
                    layers.Push(t);
                }
                else if (dic.Values.Contains(t))
                {
                    if (layers.Count == 0 || !dic[layers.Pop()].Equals(t))
                        return false;
                }
            }
            return layers.Count == 0;
        }
    }
}

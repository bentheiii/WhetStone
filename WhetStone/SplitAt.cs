﻿using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;
using WhetStone.Looping;

namespace WhetStone.Arrays
{
    public static class splitAt
    {
        public static T[][] SplitAt<T>(this IEnumerable<T> @this, params int[] lengths)
        {
            ResizingArray<T[]> ret = new ResizingArray<T[]>(lengths.Length+1);
            IGuard<positionBind.Position> pos = new Guard<positionBind.Position>();
            var tor = @this.GetEnumerator();
            foreach (int i in lengths.PositionBind().Detach(pos))
            {
                var toAdd = new ResizingArray<T>(i);
                foreach (int _ in range.Range(i))
                {
                    if (!tor.MoveNext())
                    {
                        ret.Add(toAdd.arr);
                        return ret;
                    }
                    toAdd.Add(tor.Current);
                }
            }
            var recsize = 0;
            int? recl = @this.RecommendSize();
            if (recl.HasValue)
                recsize = recl.Value - lengths.Sum();
            var final = new ResizingArray<T>(recsize);
            while (tor.MoveNext())
            {
                final.Add(tor.Current);
            }
            ret.Add(final.arr);
            return ret.arr;
        }
    }
}

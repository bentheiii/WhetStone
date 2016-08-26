using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Fielding;

namespace WhetStone.Looping
{
    public static class getSum
    {
        public static T GetSum<T>(this IEnumerable<T> toAdd, Func<T, T, T> adder = null)
        {
            var f = Fields.getField<T>();
            return GetSum(toAdd, f.zero, f.add);
        }
        public static T GetSum<T>(this IEnumerable<T> toAdd, T initial, Func<T, T, T> adder = null)
        {
            adder = adder ?? Fields.getField<T>().add;
            return toAdd.Aggregate(initial, adder);
        }
    }
}

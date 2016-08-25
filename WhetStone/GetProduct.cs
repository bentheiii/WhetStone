using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Fielding;

namespace WhetStone.Arrays
{
    public static class getProduct
    {
        public static T GetProduct<T>(this IEnumerable<T> toAdd, Func<T, T, T> multiplier = null)
        {
            var f = Fields.getField<T>();
            return GetProduct(toAdd, f.one, f.multiply);
        }
        public static T GetProduct<T>(this IEnumerable<T> toAdd, T initial, Func<T, T, T> multiplier = null)
        {
            multiplier = multiplier ?? Fields.getField<T>().add;
            return toAdd.Aggregate(initial, multiplier);
        }
    }
}

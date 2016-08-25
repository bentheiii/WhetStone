using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Fielding;

namespace WhetStone.Arrays
{
    public static class getAverage
    {
        public static T GetAverage<T>(this IEnumerable<T> tosearch)
        {
            Field<T> f = Fields.getField<T>();
            return tosearch.GetSum().ToFieldWrapper() / tosearch.Count();
        }
        public static T GetGeometricAverage<T>(this IEnumerable<T> tosearch)
        {
            var f = Fields.getField<T>();
            return f.pow(tosearch.GetProduct(), f.Invert(f.fromInt(tosearch.Count())));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using WhetStone.Fielding;

namespace WhetStone.Looping
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

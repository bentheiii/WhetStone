using System.Collections;
using System.Linq;

namespace WhetStone.Arrays
{
    // ReSharper disable once InconsistentNaming
    public static class toObjArray
    {
        public static object[] ToObjArray(this IEnumerable @this)
        {
            return @this.Cast<object>().ToArray();
        }
    }
} 
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class all
    {
        public static bool All(this IEnumerable<bool> @this)
        {
            return @this.All(t => t);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Arrays
{
    public static class all
    {
        public static bool All(this IEnumerable<bool> @this)
        {
            return @this.All(t => t);
        }
    }
}

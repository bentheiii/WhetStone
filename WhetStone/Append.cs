using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.Arrays
{
    public static class append
    {
        public static void Append<T>(ref T[] @this, params T[] toAdd)
        {
            Array.Resize(ref @this, @this.Length + toAdd.Length);
            foreach (var t in toAdd.CountBind(@this.Length))
            {
                @this[t.Item2] = t.Item1;
            }
        }
    }
}

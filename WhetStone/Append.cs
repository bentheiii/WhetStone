using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class append
    {
        public static void Append<T>(ref T[] @this, params T[] toAdd)
        {
            Append(ref @this, toAdd.AsEnumerable());
        }
        public static void Append<T>(ref T[] @this, IEnumerable<T> toAdd)
        {
            Array.Resize(ref @this, @this.Length + toAdd.Count());
            foreach (var t in toAdd.CountBind(@this.Length))
            {
                @this[t.Item2] = t.Item1;
            }
        }
    }
}

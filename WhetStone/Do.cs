using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class @do
    {
        public static void Do<T>(this IEnumerable<T> @this, Action<T> action = null)
        {
            foreach (T t in @this)
            {
                action?.Invoke(t);
            }
        }
    }
}

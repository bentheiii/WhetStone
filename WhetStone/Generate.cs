using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class generate
    {
        public static IEnumerable<T> Generate<T>(Func<T> gen)
        {
            while (true)
            {
                yield return gen();
            }
        }
    }
}

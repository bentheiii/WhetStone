using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Dictionaries
{
    public static class valueOrDefault
    {
        public static G ValueOrDefault<T, G>(this IDictionary<T, G> @this, T key, G defaultval = default(G))
        {
            G val;
            return @this.TryGetValue(key, out val) ? val : defaultval;
        }
    }
}

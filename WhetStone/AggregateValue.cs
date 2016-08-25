using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;

namespace WhetStone.Dictionaries
{
    //todo hooking
    public static class aggregateValue
    {
        public static void AggregateValue<T, G, V>(this IDictionary<T, G> @this, T key, V val, Func<G, V, G> aggfunc, G defaultseed = default(G))
        {
            @this[key] = aggfunc(@this.ValueOrDefault(key,defaultseed),val);
        }
    }
}

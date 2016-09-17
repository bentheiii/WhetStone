using System;
using System.Collections.Generic;
namespace WhetStone.Looping
{
    public static class aggregateValue
    {
        public static void AggregateValue<T, G, V>(this IDictionary<T, G> @this, T key, V val, Func<G, V, G> aggfunc, G defaultseed = default(G))
        {
            @this[key] = aggfunc(@this.ValueOrDefault(key,defaultseed),val);
        }
    }
}

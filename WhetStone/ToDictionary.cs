using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class toDictionary
    {
        public static IDictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> @this)
        {
            return @this.ToDictionary(a => a.Key, a => a.Value);
        }
        public static IDictionary<K, V> ToDictionary<K, V>(this IEnumerable<Tuple<K, V>> @this)
        {
            return @this.ToDictionary(a => a.Item1, a => a.Item2);
        }
    }
}

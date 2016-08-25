using System;
using System.Collections.Generic;
using WhetStone.Arrays;

namespace WhetStone.Dictionaries
{
    public static class insertSuffixes
    {
        public static void InsertSuffixes<T>(this IDictionary<IEnumerable<T>, Tuple<IEnumerable<T>, int>> @this, IEnumerable<T> masterkey)
        {
            InsertSuffixes(@this, masterkey, Tuple.Create);
        }
        public static void InsertSuffixes<T, V>(this IDictionary<IEnumerable<T>, V> @this, IEnumerable<T> masterkey, Func<IEnumerable<T>, int, V> value)
        {
            var toadd = masterkey.AsList();
            int i = 0;
            while (toadd.Count > 0)
            {
                @this[toadd] = value(masterkey, i);
                toadd = toadd.Slice(1);
                i++;
            }
        }
    }
}

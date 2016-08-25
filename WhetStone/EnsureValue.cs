﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Dictionaries
{
    public static class ensureValue
    {
        public static bool EnsureValue<T, G>(this IDictionary<T, G> @this, T key, G defaultval = default(G))
        {
            if (@this.ContainsKey(key))
                return true;
            @this[key] = defaultval;
            return false;
        }
    }
}

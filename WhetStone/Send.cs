using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Serializations;

namespace WhetStone.Ports
{
    public static class send
    {
        public static int Send<T>(this IConnection @this, T message, Func<T, byte[]> converter = null)
        {
            converter = converter ?? (serialize.Serialize);
            return @this.SendBytes(converter(message));
        }
    }
}

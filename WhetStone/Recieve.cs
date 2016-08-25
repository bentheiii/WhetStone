using System.Net;
using WhetStone.Ports.AutoCommands;
using WhetStone.Serializations;

namespace WhetStone.Ports
{
    public static class recieve
    {
        public static object Recieve(this IConnection c, int buffersize = 1024)
        {
            EndPoint p;
            return c.Recieve(out p, buffersize);
        }
        public static T Recieve<T>(this IConnection c, int buffersize = 1024)
        {
            EndPoint from;
            return Recieve<T>(c, out from, buffersize);
        }
        public static T Recieve<T>(this IConnection c, out EndPoint from, int buffersize = 1024)
        {
            return (T)Recieve(c, out from, buffersize);
        }
        public static object Recieve(this IConnection c, out EndPoint from, int buffersize = 1024)
        {
            object r = serialize.Deserialize(c.RecieveBytes(out from, buffersize));
            IConnectionAutoCommand a = r as IConnectionAutoCommand;
            if (a != null && c.enabledAutoCommands != null && c.AcceptsAutoCommand(a))
            {
                a.OnRecieve(c);
            }
            return r;
        }
    }
}

using WhetStone.Ports.AutoCommands;

namespace WhetStone.Ports
{
    public static class ping
    {
        public static bool Ping(this IConnection c)
        {
            const string pingstring = "ping0112358";
            ConnectionSendCommand p = new ConnectionSendCommand(pingstring);
            c.Send(p);
            object reply = c.Recieve();
            return pingstring.Equals(reply as string);
        }
    }
}

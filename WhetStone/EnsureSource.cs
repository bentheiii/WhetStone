using System.Net;

namespace WhetStone.Ports
{
    public static class ensureSource
    {
        public static EndPoint EnsureSource(this IPortBound c, localEndPoint.SourceStyle style = localEndPoint.SourceStyle.Private)
        {
            if (c.source == null)
            {
                c.setLocalSource(0, style);
            }
            return c.source;
        }
    }
}

using System.Runtime.Serialization;

namespace WhetStone.SystemExtensions
{
    public static class IdDistributer
    {
        private static readonly ObjectIDGenerator _g = new ObjectIDGenerator();
        public static long getId<T>(T o)
        {
            bool proxy;
            return _g.GetId(o, out proxy);
        }
    }
}
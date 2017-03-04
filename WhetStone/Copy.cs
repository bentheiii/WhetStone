using System;

namespace WhetStone.SystemExtensions
{
    public static class copy
    {
        public static T Copy<T>(this T @this) where T : ICloneable
        {
            return (T)@this.Clone();
        }
    }
}
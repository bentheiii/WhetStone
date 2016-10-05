namespace WhetStone.NumbersMagic
{
    public static class unsignedDiff
    {
        public static uint UnsignedDiff(this uint @this, uint other)
        {
            if (@this > other)
                return @this - other;
            return other - @this;
        }
        public static ushort UnsignedDiff(this ushort @this, ushort other)
        {
            if (@this > other)
                return (ushort)(@this - other);
            return (ushort)(other - @this);
        }
        public static byte UnsignedDiff(this byte @this, byte other)
        {
            if (@this > other)
                return (byte)(@this - other);
            return (byte)(other - @this);
        }
        public static ulong UnsignedDiff(this ulong @this, ulong other)
        {
            if (@this > other)
                return @this - other;
            return other - @this;
        }
    }
}

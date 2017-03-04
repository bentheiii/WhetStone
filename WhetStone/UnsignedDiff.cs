namespace NumberStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class unsignedDiff
    {
        /// <summary>
        /// Get the absolute difference between two numbers.
        /// </summary>
        /// <param name="this">The first number.</param>
        /// <param name="other">The second number.</param>
        /// <returns>The absolute difference between <paramref name="this"/> and <paramref name="other"/>.</returns>
        public static uint UnsignedDiff(this uint @this, uint other)
        {
            if (@this > other)
                return @this - other;
            return other - @this;
        }
        /// <summary>
        /// Get the absolute difference between two numbers.
        /// </summary>
        /// <param name="this">The first number.</param>
        /// <param name="other">The second number.</param>
        /// <returns>The absolute difference between <paramref name="this"/> and <paramref name="other"/>.</returns>
        public static ushort UnsignedDiff(this ushort @this, ushort other)
        {
            if (@this > other)
                return (ushort)(@this - other);
            return (ushort)(other - @this);
        }
        /// <summary>
        /// Get the absolute difference between two numbers.
        /// </summary>
        /// <param name="this">The first number.</param>
        /// <param name="other">The second number.</param>
        /// <returns>The absolute difference between <paramref name="this"/> and <paramref name="other"/>.</returns>
        public static byte UnsignedDiff(this byte @this, byte other)
        {
            if (@this > other)
                return (byte)(@this - other);
            return (byte)(other - @this);
        }
        /// <summary>
        /// Get the absolute difference between two numbers.
        /// </summary>
        /// <param name="this">The first number.</param>
        /// <param name="other">The second number.</param>
        /// <returns>The absolute difference between <paramref name="this"/> and <paramref name="other"/>.</returns>
        public static ulong UnsignedDiff(this ulong @this, ulong other)
        {
            if (@this > other)
                return @this - other;
            return other - @this;
        }
    }
}

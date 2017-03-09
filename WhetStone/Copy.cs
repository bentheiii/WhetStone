using System;

namespace WhetStone.SystemExtensions
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class copy
    {
        /// <summary>
        /// Copy the <see cref="ICloneable"/> while still conserving the type.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ICloneable"/>.</typeparam>
        /// <param name="this">The <see cref="ICloneable"/> to copy.</param>
        /// <returns><paramref name="this"/>'s clone converted to a <typeparamref name="T"/>.</returns>
        public static T Copy<T>(this T @this) where T : ICloneable
        {
            @this.ThrowIfNull(nameof(@this));
            return (T)@this.Clone();
        }
    }
}
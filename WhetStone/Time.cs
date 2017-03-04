using System;

namespace WhetStone.Units.Time
{
    /// <summary>
    /// A static class for handling <see cref="TimeSpan"/>s.
    /// </summary>
    public static class TimeExtentions
    {
        /// <summary>
        /// Get a <see cref="TimeSpan"/> divided by a factor.
        /// </summary>
        /// <param name="t">The <see cref="TimeSpan"/> to divide.</param>
        /// <param name="divisor">The factor to divide by.</param>
        /// <returns><paramref name="t"/> divided by <paramref name="divisor"/>.</returns>
        public static TimeSpan Divide(this TimeSpan t, double divisor)
        {
            return Multiply(t, 1.0 / divisor);
        }
        /// <summary>
        /// Get the ratio between two <see cref="TimeSpan"/>s.
        /// </summary>
        /// <param name="t">The <see cref="TimeSpan"/> to divide.</param>
        /// <param name="divisor">The <see cref="TimeSpan"/> to divide by.</param>
        /// <returns>The ratio between <paramref name="t"/> and <paramref name="divisor"/></returns>
        public static double Divide(this TimeSpan t, TimeSpan divisor)
        {
            return (t.Ticks / (double)divisor.Ticks);
        }
        /// <summary>
        /// Get a <see cref="TimeSpan"/> multiplied by a factor.
        /// </summary>
        /// <param name="t">The <see cref="TimeSpan"/> to multiply.</param>
        /// <param name="factor">The factor to multiply by.</param>
        /// <returns><paramref name="t"/> multiplied by <paramref name="factor"/>.</returns>
        public static TimeSpan Multiply(this TimeSpan t, double factor)
        {
            return new TimeSpan((long)(t.Ticks * factor));
        }
    }
}

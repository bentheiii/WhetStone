using System;
#if NOPARAMCHECK
using System.Diagnostics;
#endif
#if RESHARPER
using JetBrains.Annotations;
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
// ReSharper disable UnusedParameter.Global
#endif

namespace WhetStone.SystemExtensions
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class throwIf
    {
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if an object is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="this">The object to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
#if NOPARAMCHECK
        [Conditional("FALSE")]
#endif
        public static void ThrowIfNull<T>(this T @this, string paramName = "parameter")
        {
            if (@this == null)
                throw new ArgumentNullException(paramName);
        }
#if RESHARPER
        /// <summary>
        /// Resharper Template for Null checking.
        /// </summary>
        /// <param name="x">The value to check.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        [SourceTemplate] public static void throwIfNull<T>(this T x)
        {
            x.ThrowIfNull(nameof(x));/*$$END$*/
        }
        /// <summary>
        /// Resharper Template for Absurdity checking.
        /// </summary>
        /// <param name="x">The value to check.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        [SourceTemplate] public static void throwIfAbsurd<T>(this T x)
        {
            //$ x.ThrowIfAbsurd(nameof(x));$END$
        }
#endif
#if UNSAFE
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if an pointer is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="this">The object to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
#if NOPARAMCHECK
        [Conditional("FALSE")]
#endif
        public static unsafe void ThrowIfPtrNull(void* @this, [InvokerParameterName] string paramName = "parameter")
        {
            if (@this == null)
                throw new ArgumentNullException(paramName);
        }
#endif
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if a double is NAN or infinity.
        /// </summary>
        /// <param name="this">The double to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
        /// <param name="allowPosInfinity">Whether to allow positive infinity.</param>
        /// <param name="allowNegInfity">Whether to allow negative infinity.</param>
        /// <param name="allowNan">Whether to allow NAN.</param>
        /// <param name="allowZero">Whether to allow 0.</param>
        /// <param name="allowNegative">Whether to allow negative values.</param>
#if NOPARAMCHECK
        [Conditional("FALSE")]
#endif
        public static void ThrowIfAbsurd(this double @this, string paramName = "parameter", bool allowPosInfinity = false, bool allowNegInfity = false, bool allowNan = false, bool allowZero = true, bool allowNegative = true)
        {
            if (!allowNan && double.IsNaN(@this))
                throw new ArgumentException(paramName + " cannot be NAN");
            if (!allowPosInfinity && double.IsPositiveInfinity(@this))
                throw new ArgumentException(paramName + " cannot be infinity");
            if (!allowNegInfity && double.IsNegativeInfinity(@this))
                throw new ArgumentException(paramName + " cannot be negative infinity");
            if (!allowZero && @this == 0.0)
                throw new ArgumentException(paramName + " cannot be zero");
            if (!allowNegative && @this  < 0)
                throw new ArgumentException(paramName + " cannot be negative");
        }
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if an int is not positive.
        /// </summary>
        /// <param name="this">The double to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
        /// <param name="allowZero">Whether to allow 0.</param>
        /// <param name="allowOne">Whether to allow 1.</param>
        /// <param name="allowNeg">Whether to allow negative values.</param>
#if NOPARAMCHECK
        [Conditional("FALSE")]
#endif
        public static void ThrowIfAbsurd(this int @this, string paramName = "parameter", bool allowZero = true, bool allowOne = true, bool allowNeg = false)
        {
            if ((!allowZero && @this == 0) || (!allowNeg && @this < 0) || (!allowOne && @this == 1))
                throw new ArgumentOutOfRangeException(paramName, @this,"value is invalid");
        }
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if a long is not positive.
        /// </summary>
        /// <param name="this">The double to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
        /// <param name="allowZero">Whether to allow 0.</param>
        /// <param name="allowOne">Whether to allow 1.</param>
        /// <param name="allowNeg">Whether to allow negative values.</param>
#if NOPARAMCHECK
        [Conditional("FALSE")]
#endif
        public static void ThrowIfAbsurd(this long @this, string paramName = "parameter", bool allowZero = true, bool allowOne = true, bool allowNeg = false)
        {
            if ((!allowZero && @this == 0) || (!allowNeg && @this < 0))
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
        }
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if an int? is not positive or null.
        /// </summary>
        /// <param name="this">The double to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
        /// <param name="allowZero">Whether to allow 0.</param>
        /// <param name="allowOne">Whether to allow 1.</param>
        /// <param name="allowNeg">Whether to allow negative values.</param>
        /// <param name="allowNull">Whether to allow null</param>
#if NOPARAMCHECK
        [Conditional("FALSE")]
#endif
        public static void ThrowIfAbsurd(this int? @this, string paramName = "parameter", bool allowZero = true, bool allowOne = true, bool allowNeg = false, bool allowNull = true)
        {
            if (@this.HasValue)
            {
                if (allowNull)
                    return;
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
            }
            if ((!allowZero && @this == 0) || (!allowNeg && @this < 0))
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
        }
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if an long? is not positive or null.
        /// </summary>
        /// <param name="this">The double to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
        /// <param name="allowZero">Whether to allow 0.</param>
        /// <param name="allowOne">Whether to allow 1.</param>
        /// <param name="allowNeg">Whether to allow negative values.</param>
        /// <param name="allowNull">Whether to allow null</param>
#if NOPARAMCHECK
        [Conditional("FALSE")]
#endif
        public static void ThrowIfAbsurd(this long? @this, string paramName = "parameter", bool allowZero = true, bool allowOne = true, bool allowNeg = false, bool allowNull = true)
        {
            if (@this.HasValue)
            {
                if (allowNull)
                    return;
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
            }
            if ((!allowZero && @this == 0) || (!allowNeg && @this < 0))
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
        }
    }
}

using System;
using JetBrains.Annotations;

// ReSharper disable UnusedParameter.Global

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
        public static void ThrowIfNull<T>(this T @this, [InvokerParameterName] string paramName = "")
        {
#if !NOPARAMCHECK
            paramName = paramName ?? "parameter";
            if (@this == null)
                throw new ArgumentNullException(paramName);
#endif
        }
#if UNSAFE
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if an pointer is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="this">The object to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
        public static unsafe void ThrowIfPtrNull(void* @this, [InvokerParameterName] string paramName = "")
        {
#if !NOPARAMCHECK
            paramName = paramName ?? "parameter";
            if (@this == null)
                throw new ArgumentNullException(paramName);
#endif
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
        public static void ThrowIfAbsurd(this double @this, [InvokerParameterName] string paramName = null, bool allowPosInfinity = false, bool allowNegInfity = false, bool allowNan = false, bool allowZero = true)
        {
#if !NOPARAMCHECK
            paramName = paramName ?? "parameter";
            if (!allowNan && double.IsNaN(@this))
                throw new ArgumentException(paramName+" cannot be NAN");
            if (!allowPosInfinity && double.IsPositiveInfinity(@this))
                throw new ArgumentException(paramName + " cannot be infinity");
            if (!allowNegInfity && double.IsNegativeInfinity(@this))
                throw new ArgumentException(paramName + " cannot be negative infinity");
            if (!allowZero && @this == 0.0)
                throw new ArgumentException(paramName + " cannot be zero");
#endif
        }
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if an int is not positive.
        /// </summary>
        /// <param name="this">The double to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
        /// <param name="allowZero">Whether to allow 0.</param>
        /// <param name="allowOne">Whether to allow 1.</param>
        /// <param name="allowNeg">Whether to allow negative values.</param>
        public static void ThrowIfAbsurd(this int @this, [InvokerParameterName] string paramName = null, bool allowZero = true, bool allowOne = true, bool allowNeg = false)
        {
#if !NOPARAMCHECK
            paramName = paramName ?? "parameter";
            if ((!allowZero && @this == 0) || (!allowNeg && @this < 0) || (!allowOne && @this == 1))
                throw new ArgumentOutOfRangeException(paramName, @this,"value is invalid");
#endif
        }
        /// <summary>
        /// Throw a <see cref="ArgumentNullException"/> if a long is not positive.
        /// </summary>
        /// <param name="this">The double to check.</param>
        /// <param name="paramName">The string of the exception to throw. <see langword="null"/> for generic parameter name.</param>
        /// <param name="allowZero">Whether to allow 0.</param>
        /// <param name="allowOne">Whether to allow 1.</param>
        /// <param name="allowNeg">Whether to allow negative values.</param>
        public static void ThrowIfAbsurd(this long @this, [InvokerParameterName] string paramName = null, bool allowZero = true, bool allowOne = true, bool allowNeg = false)
        {
#if !NOPARAMCHECK
            paramName = paramName ?? "parameter";
            if ((!allowZero && @this == 0) || (!allowNeg && @this < 0))
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
#endif
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
        public static void ThrowIfAbsurd(this int? @this, [InvokerParameterName] string paramName = null, bool allowZero = true, bool allowOne = true, bool allowNeg = false, bool allowNull = true)
        {
#if !NOPARAMCHECK
            paramName = paramName ?? "parameter";
            if (@this.HasValue)
            {
                if (allowNull)
                    return;
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
            }
            if ((!allowZero && @this == 0) || (!allowNeg && @this < 0))
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
#endif
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
        public static void ThrowIfAbsurd(this long? @this, [InvokerParameterName] string paramName = null, bool allowZero = true, bool allowOne = true, bool allowNeg = false, bool allowNull = true)
        {
#if !NOPARAMCHECK
            paramName = paramName ?? "parameter";
            if (@this.HasValue)
            {
                if (allowNull)
                    return;
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
            }
            if ((!allowZero && @this == 0) || (!allowNeg && @this < 0))
                throw new ArgumentOutOfRangeException(paramName, @this, "value is invalid");
#endif
        }
    }
}

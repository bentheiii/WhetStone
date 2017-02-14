using System;

namespace WhetStone.Fielding
{
    /// <summary>
    /// A wrapper for an element, allowing for easy <see cref="Field{T}"/> functionality.
    /// </summary>
    /// <typeparam name="T">The type of the original wrapped element.</typeparam>
    public class FieldWrapper<T> : IComparable<T>, IComparable<FieldWrapper<T>>, IEquatable<T>, IEquatable<FieldWrapper<T>>
    {
        private static readonly Field<T> _field;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="val">The element to wrap.</param>
        public FieldWrapper(T val)
        {
            this.val = val;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="i">The <see cref="int"/> equivalent element to wrap.</param>
        public FieldWrapper(int i)
        {
            val = _field.fromInt(i);
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="i">The <see cref="double"/> equivalent element to wrap.</param>
        public FieldWrapper(double i)
        {
            val = _field.fromFraction(i);
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="i">The <see cref="ulong"/> equivalent element to wrap.</param>
        public FieldWrapper(ulong i)
        {
            val = _field.fromInt(i);
        }
        static FieldWrapper()
        {
            _field = Fields.getField<T>();
        }
        /// <summary>
        /// The wrapped value.
        /// </summary>
        public T val { get; }
        /// <summary>
        /// Converts an element by wrapping it.
        /// </summary>
        /// <param name="w">The element to wrap.</param>
        public static implicit operator FieldWrapper<T>(T w)
        {
            return new FieldWrapper<T>(w);
        }
        /// <summary>
        /// Converts an element by wrapping it.
        /// </summary>
        /// <param name="w">The <see cref="int"/> equivalent element to wrap.</param>
        public static implicit operator FieldWrapper<T>(int w)
        {
            return new FieldWrapper<T>(w);
        }
        /// <summary>
        /// Converts an element by wrapping it.
        /// </summary>
        /// <param name="w">The <see cref="double"/> equivalent element to wrap.</param>
        public static implicit operator FieldWrapper<T>(double w)
        {
            return new FieldWrapper<T>(w);
        }
        /// <summary>
        /// Converts an element by wrapping it.
        /// </summary>
        /// <param name="w">The <see cref="ulong"/> equivalent element to wrap.</param>
        public static implicit operator FieldWrapper<T>(ulong w)
        {
            return new FieldWrapper<T>(w);
        }
        /// <summary>
        /// Get the logarithm of the element.
        /// </summary>
        /// <param name="base">The base for the logarithm function.</param>
        /// <returns>The logarithm of <see cref="val"/> and <paramref name="base"/>.</returns>
        public FieldWrapper<T> log(T @base)
        {
            return _field.log(val, @base);
        } 
        /// <summary>
        /// Get the absolute value of the element.
        /// </summary>
        /// <returns><see cref="val"/>'s absolute value.</returns>
        public FieldWrapper<T> abs()
        {
            return _field.abs(val);
        }
        /// <summary>
        /// Get the element raised to an exponent.
        /// </summary>
        /// <param name="p">The exponent.</param>
        /// <returns><see cref="val"/> rasied to <paramref name="p"/>.</returns>
        public FieldWrapper<T> pow(FieldWrapper<T> p)
        {
            return _field.pow(val, p);
        }
        /// <summary>
        /// Get the element raised to an exponent.
        /// </summary>
        /// <param name="p">The exponent.</param>
        /// <returns><see cref="val"/> raised to <paramref name="p"/>.</returns>
        public FieldWrapper<T> pow(int p)
        {
            return _field.Pow(val, p);
        }
        /// <summary>
        /// Gets the inverted value of <see cref="val"/>.
        /// </summary>
        /// <returns>A <see cref="FieldWrapper{T}"/> wrapping <see cref="val"/>'s inverted value.</returns>
        public FieldWrapper<T> Invert()
        {
            return _field.Invert(val);
        } 
        /// <summary>
        /// Convert a <see cref="FieldWrapper{T}"/> to its underlying type.
        /// </summary>
        /// <param name="w">The <see cref="FieldWrapper{T}"/> to convert.</param>
        public static implicit operator T(FieldWrapper<T> w)
        {
            return w.val;
        }
        /// <summary>
        /// Convert a <see cref="FieldWrapper{T}"/> to a nullable <see cref="double"/>.
        /// </summary>
        /// <param name="w">The <see cref="FieldWrapper{T}"/> to convert.</param>
        public static explicit operator double?(FieldWrapper<T> w)
        {
            return _field.toDouble(w.val);
        }
        /// <summary>
        /// Convert a <see cref="FieldWrapper{T}"/> to a nullable <see cref="double"/>.
        /// </summary>
        /// <param name="w">The <see cref="FieldWrapper{T}"/> to convert.</param>
        /// <exception cref="InvalidCastException">If the <see cref="Field{T}"/>'s double conversion failed.</exception>
        public static explicit operator double (FieldWrapper<T> w)
        {
            var d = _field.toDouble(w.val);
            if (d != null)
                return d.Value;
            throw new InvalidCastException("cannot cast wrapper to double because the field's double conversion method returned null");
        }
        /// <summary>
        /// Adds two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The first addend.</param>
        /// <param name="w2">The second addend.</param>
        /// <returns>The sum of <paramref name="w1"/> and <paramref name="w2"/>.</returns>
        public static FieldWrapper<T> operator +(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.add(w1, w2);
        }
        /// <summary>
        /// Multiplies two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The first multiplicand.</param>
        /// <param name="w2">The second multiplicand.</param>
        /// <returns>The product of <paramref name="w1"/> and <paramref name="w2"/>.</returns>
        public static FieldWrapper<T> operator *(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.multiply(w1, w2);
        }
        /// <summary>
        /// Subtracts two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The minuend.</param>
        /// <param name="w2">The subtrahend.</param>
        /// <returns>The difference of <paramref name="w1"/> and <paramref name="w2"/>.</returns>
        public static FieldWrapper<T> operator -(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.subtract(w1, w2);
        }
        /// <summary>
        /// Negates a <see cref="FieldWrapper{T}"/>.
        /// </summary>
        /// <param name="w1">The element to negate.</param>
        /// <returns>The negative of <paramref name="w1"/>.</returns>
        public static FieldWrapper<T> operator -(FieldWrapper<T> w1)
        {
            return _field.Negate(w1);
        }
        /// <summary>
        /// Divides two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The dividend.</param>
        /// <param name="w2">The divisor.</param>
        /// <returns>The quotient of <paramref name="w1"/> and <paramref name="w2"/>.</returns>
        public static FieldWrapper<T> operator /(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.divide(w1, w2);
        }
        /// <summary>
        /// Modulo two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The dividend.</param>
        /// <param name="w2">The divisor.</param>
        /// <returns>The modulo of <paramref name="w1"/> and <paramref name="w2"/>.</returns>
        public static FieldWrapper<T> operator %(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.mod(w1, w2);
        }
        /// <summary>
        /// Raises a <see cref="FieldWrapper{T}"/> to the power of another.
        /// </summary>
        /// <param name="w1">The base.</param>
        /// <param name="w2">The exponent.</param>
        /// <returns>The power of <paramref name="w1"/> and <paramref name="w2"/>.</returns>
        public static FieldWrapper<T> operator ^(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.pow(w1, w2);
        }
        /// <summary>
        /// Raises a <see cref="FieldWrapper{T}"/> to the power of an integer.
        /// </summary>
        /// <param name="w1">The base.</param>
        /// <param name="w2">The exponent.</param>
        /// <returns>The power of <paramref name="w1"/> and <paramref name="w2"/>.</returns>
        public static FieldWrapper<T> operator ^(FieldWrapper<T> w1, int w2)
        {
            return _field.Pow(w1, w2);
        }
        /// <summary>
        /// Compare two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The first <see cref="FieldWrapper{T}"/> to compare</param>
        /// <param name="w2">The second <see cref="FieldWrapper{T}"/> to compare</param>
        /// <returns>Whether <paramref name="w1"/> is less than or equal to <paramref name="w2"/>.</returns>
        public static bool operator <=(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.Compare(w1, w2) <= 0;
        }
        /// <summary>
        /// Compare two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The first <see cref="FieldWrapper{T}"/> to compare</param>
        /// <param name="w2">The second <see cref="FieldWrapper{T}"/> to compare</param>
        /// <returns>Whether <paramref name="w1"/> is greater than or equal to <paramref name="w2"/>.</returns>
        public static bool operator >=(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.Compare(w1, w2) >= 0;
        }
        /// <summary>
        /// Compare two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The first <see cref="FieldWrapper{T}"/> to compare</param>
        /// <param name="w2">The second <see cref="FieldWrapper{T}"/> to compare</param>
        /// <returns>Whether <paramref name="w1"/> is less than <paramref name="w2"/>.</returns>
        public static bool operator <(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.Compare(w1, w2) < 0;
        }
        /// <summary>
        /// Compare two <see cref="FieldWrapper{T}"/>s.
        /// </summary>
        /// <param name="w1">The first <see cref="FieldWrapper{T}"/> to compare</param>
        /// <param name="w2">The second <see cref="FieldWrapper{T}"/> to compare</param>
        /// <returns>Whether <paramref name="w1"/> is greater than <paramref name="w2"/>.</returns>
        public static bool operator >(FieldWrapper<T> w1, FieldWrapper<T> w2)
        {
            return _field.Compare(w1, w2) > 0;
        }
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is T && this.CompareTo((T)(obj)) == 0;
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return val.GetHashCode();
        }
        /// <inheritdoc />
        public bool Equals(T other)
        {
            return Field.Equals(this,other);
        }
        /// <inheritdoc />
        public bool Equals(FieldWrapper<T> other)
        {
            return Equals(other.val);
        }
        /// <inheritdoc />
        public override string ToString()
        {
            return val.ToString();
        }
        /// <inheritdoc />
        public int CompareTo(T other)
        {
            return _field.Compare(val, other);
        }
        /// <inheritdoc />
        public int CompareTo(FieldWrapper<T> other)
        {
            return _field.Compare(val, other.val);
        }
        /// <summary>
        /// Get the <see cref="Field{T}"/> used for all arithmetic operations.
        /// </summary>
        public Field<T> Field
        {
            get
            {
                return _field;
            }
        }
        /// <summary>
        /// Get whether the element is strictly negative.
        /// </summary>
        public bool IsNegative
        {
            get
            {
                return _field.isNegative(val);
            }
        }
        /// <summary>
        /// Get whether the element is strictly positive.
        /// </summary>
        public bool IsPositive
        {
            get
            {
                return _field.isPositive(val);
            }
        }
        /// <summary>
        /// Get whether the element is zero.
        /// </summary>
        public bool isZero
        {
            get
            {
                return Field.Equals(Field.zero, this);
            }
        }
        /// <summary>
        /// Get whether the element is one.
        /// </summary>
        public bool isOne
        {
            get
            {
                return Field.Equals(Field.one, this);
            }
        }
    }
}
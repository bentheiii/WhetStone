using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Numerics;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    /// <summary>
    /// A <see cref="BigProduct"/> can be used to store arbitrarily large rational numbers by their prime factorization.
    /// </summary>
    /// <remarks>
    /// <para><see cref="BigProduct"/> stores numbers by their prime factorization, making it very speedy for multiplying, dividing, and raising to integer powers. But other than this, it cannot be otherwise changed.</para>
    /// <para>The <see cref="BigProduct"/> is mutable.</para>
    /// </remarks>
    public class BigProduct
    {
        private readonly IDictionary<int, int> _factors;
        /// <summary>
        /// The <see cref="BigProduct"/>'s sign.
        /// </summary>
        /// <value>1 if the <see cref="BigProduct"/> is positive. -1 if negative. 0 if the value is zero</value>
        public sbyte sign { get; private set; } = 1;
        /// <summary>
        /// Constructor for <see cref="BigProduct"/>
        /// </summary>
        /// <param name="initialvalue">The <see cref="BigProduct"/>'s initial value.</param>
        public BigProduct(int initialvalue = 1)
        {
            if (initialvalue == 0)
            {
                sign = 0;
                _factors = null;
                return;
            }
            _factors = new Dictionary<int, int>();
            if (initialvalue == 1)
                return;
            if (initialvalue < 0)
            {
                sign = -1;
                initialvalue = -initialvalue;
            }
            Multiply(initialvalue);
        }
        /// <summary>
        /// Multiplies the <see cref="BigProduct"/>'s value by <paramref name="n"/> raised to <paramref name="pow"/>.
        /// </summary>
        /// <param name="n">The root of the multiplicand.</param>
        /// <param name="pow">The power of the multiplicand.</param>
        /// <remarks>For sufficiently small <paramref name="n"/> (under approximately 10,000, see <see cref="smallestFactor.SmallestFactor(int,System.Nullable{int})"/>), running time is O(log(<paramref name="n"/>))</remarks>
        public void Multiply(int n, int pow = 1)
        {
            if (n == 0)
            {
                sign = 0;
                _factors.Clear();
                return;
            }
            if (sign == 0 || n == 1 || pow == 0)
                return;
            if (n < 0 && pow%2 != 0)
            {
                sign = (sbyte)-sign;
                n *= -1;
            }
            foreach (var factor in n.Primefactors().ToOccurancesSorted())
            {
                _factors.EnsureValue(factor.Item1);
                _factors[factor.Item1] += (pow*factor.Item2);
                if (_factors[factor.Item1] == 0)
                    _factors.Remove(factor.Item1);
            }
        }
        /// <summary>
        /// Divides the <see cref="BigProduct"/>'s value by <paramref name="n"/> raised to <paramref name="pow"/>.
        /// </summary>
        /// <param name="n">The root of the divisor.</param>
        /// <param name="pow">The power of the divisor.</param>
        /// <remarks>This is identical to <see cref="Multiply"/> with a negative pow.</remarks>
        public void Divide(int n, int pow = 1)
        {
            Multiply(n, -pow);
        }
        /// <summary>
        /// Multiplies the <see cref="BigProduct"/>'s value by <paramref name="n"/> factorial raised to <paramref name="pow"/>.
        /// </summary>
        /// <param name="n">The inverse factorial of the root of the multiplicand.</param>
        /// <param name="pow">The power of the multiplicand.</param>
        /// <remarks>
        /// <para>This uses Legendre's formula for divisibility of factorials.</para>
        /// <para>For sufficiently small <paramref name="n"/> (under approximately 20,000, see <see cref="primes.Primes(int)"/>), running time is O(log(<paramref name="n"/>)^2)</para>
        /// </remarks>
        public void MultiplyFactorial(int n, int pow = 1)
        {
            n.ThrowIfAbsurd(nameof(n));
            if (sign == 0 || n <= 1 || pow == 0)
                return;
            foreach (int prime in primes.Primes(n + 1))
            {
                int toadd = 0;
                int factor = prime;
                while (factor <= n)
                {
                    toadd += (n / factor);
                    factor *= prime;
                }
                _factors.EnsureValue(prime);
                _factors[prime] += toadd * pow;
                if (_factors[prime] == 0)
                    _factors.Remove(prime);
            }
        }
        /// <summary>
        /// Divides the <see cref="BigProduct"/>'s value by <paramref name="n"/> factorial raised to <paramref name="pow"/>.
        /// </summary>
        /// <param name="n">The inverse factorial of the root of the divisor.</param>
        /// <param name="pow">The power of the divisor.</param>
        /// <remarks>This is identical to <see cref="MultiplyFactorial"/> with a negative pow.</remarks>
        public void DivideFactorial(int n, int pow = 1)
        {
            MultiplyFactorial(n, -pow);
        }
        /// <summary>
        /// Raises the <see cref="BigProduct"/>'s value by <paramref name="p"/>.
        /// </summary>
        /// <param name="p">The power to raise the <see cref="BigProduct"/>'s value by.</param>
        /// <exception cref="InvalidOperationException">In case an attempt is made to raise zero by the power of zero.</exception>
        public void pow(int p)
        {
            if (sign == 0)
                return;
            if (p == 0)
            {
                if (sign == 0)
                    throw new InvalidOperationException("zero by the power of zero");
                sign = 1;
                _factors.Clear();
            }
            if (sign == -1 && p%2 == 0)
                sign = 1;
            foreach (int key in _factors.Keys.ToArray())
            {
                if (_factors[key] == 0)
                    _factors.Remove(key);
                _factors[key] *= p;
            }
        }
        /// <summary>
        /// Get the value of the <see cref="BigProduct"/> as a <see cref="int"/>.
        /// </summary>
        /// <returns>The value of the <see cref="BigProduct"/> as a <see cref="int"/>.</returns>
        /// <exception cref="InvalidOperationException">If the value cannot be represented as an <see cref="int"/>, use <see cref="toFraction"/> in those cases.</exception>
        public int toNum()
        {
            if (sign == 0)
                return 0;
            var ret = (int)sign;
            foreach (var factor in _factors)
            {
                if (factor.Value < 0)
                    throw new InvalidOperationException("Cannot return num of non-integer value.");
                ret *= factor.Key.pow(factor.Value);
            }
            return ret;
        }
        /// <summary>
        /// Get whether the <see cref="BigProduct"/> can be expressed as an integer.
        /// </summary>
        /// <returns>Whether the <see cref="BigProduct"/> can be expressed as an integer.</returns>
        /// <remarks>Just because <see cref="isInteger"/> returns true, doesn't mean that <see cref="toNum"/> will not throw an exception (for example, in case where the value is higher than <see cref="int"/> can contain).</remarks>
        public bool isInteger()
        {
            return sign == 0 || _factors.Values.All(a => a > 0);
        }
        /// <summary>
        /// Get the value of the <see cref="BigProduct"/> as a <see cref="BigRational"/>.
        /// </summary>
        /// <returns>The value of the <see cref="BigProduct"/> as a <see cref="BigRational"/>.</returns>
        public BigRational toFraction()
        {
            if (sign == 0)
                return BigRational.Zero;
            BigInteger num = sign == 1 ?BigInteger.One : BigInteger.MinusOne;
            BigInteger den = BigInteger.One;
            foreach (var factor in _factors)
            {
                var v = ((BigInteger)factor.Key).pow(factor.Value);
                if (factor.Value < 0)
                {
                    den *= v;
                }
                else
                {
                    num *= v;
                }
            }
            return new BigRational(num,den,false);
        }
    }
}

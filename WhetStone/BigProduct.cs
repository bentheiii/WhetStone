using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Numerics;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    public class BigProduct
    {
        private readonly IDictionary<int, BigInteger> _factors = new Dictionary<int, BigInteger>();
        private sbyte _sign = 1;
        public void Multiply(int n, int pow = 1)
        {
            if (n == 0)
            {
                _sign = 0;
                return;
            }
            if (_sign == 0)
                return;
            if (n < 0 && pow%2 != 0)
            {
                _sign *= -1;
                n *= -1;
            }
            foreach (int factor in n.Primefactors())
            {
                _factors.EnsureValue(factor, 0);
                _factors[factor] += pow;
            }
        }
        public void Divide(int n, int pow = 1)
        {
            Multiply(n, -pow);
        }
        public void MultiplyFactorial(int n, int pow = 1)
        {
            if (_sign == 0 || n <= 1)
                return;
            //Legendre's formula
            foreach (int prime in primes.Primes(n + 1))
            {
                int toadd = 0;
                int factor = prime;
                while (factor <= n)
                {
                    toadd += (n / factor);
                    factor *= prime;
                }
                _factors.EnsureValue(prime, 0);
                _factors[prime] += toadd * pow;
            }
        }
        public void DivideFactorial(int n, int pow = 1)
        {
            MultiplyFactorial(n, -pow);
        }
        public void pow(int p)
        {
            if (_sign == 0)
                return;
            if (_sign == -1 && p%2 == 0)
                _sign = 1;
            foreach (int key in _factors.Keys)
            {
                _factors[key] *= p;
            }
        }
        public BigInteger toNum()
        {
            if (_sign == 0)
                return BigInteger.Zero;
            return _factors.Select(a => ((BigInteger)a.Key).pow(a.Value)).Aggregate(BigInteger.One, (a, b) => a * b)*_sign;
        }
        public BigRational toFraction()
        {
            if (_sign == 0)
                return BigRational.Zero;
            return _factors.Select(a => ((BigRational)a.Key).pow(a.Value)).Aggregate(BigRational.One, (a, b) => a * b)*_sign;
        }
    }
}

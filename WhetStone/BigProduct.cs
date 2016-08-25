using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace WhetStone.NumbersMagic
{
    public class BigProduct
    {
        private readonly IDictionary<int, BigInteger> _factors = new Dictionary<int, BigInteger>();
        public void Multiply(int n, int pow = 1)
        {
            foreach (int factor in n.Primefactors())
            {
                _factors[factor] += pow;
            }
        }
        public void Divide(int n, int pow = 1)
        {
            Multiply(n, -pow);
        }
        public void MultiplyFactorial(int n, int pow = 1)
        {
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
                _factors[prime] += toadd * pow;
            }
        }
        public void DivideFactorial(int n, int pow = 1)
        {
            MultiplyFactorial(n, -pow);
        }
        public void pow(int p)
        {
            foreach (int key in _factors.Keys)
            {
                _factors[key] *= p;
            }
        }
        public BigInteger toNum()
        {
            return _factors.Select(a => ((BigInteger)a.Key).pow(a.Value)).Aggregate(BigInteger.One, (a, b) => a * b);
        }
    }
}

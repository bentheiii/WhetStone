using System;
using WhetStone.Arrays;
using WhetStone.Guard;
using WhetStone.RecursiveQuerier;
using WhetStone.SystemExtensions;

namespace WhetStone.NumbersMagic
{
    public class DivisibilityQuerier
    {
        public DivisibilityQuerier(int denominator)
        {
            this.denominator = denominator;
            PowQuerier = new PowQuerier<int>(denominator);
        }
        public int denominator { get; }
        public PowQuerier<int> PowQuerier { get; }
        public int Divisibility(int n, out int quotient)
        {
            IGuard<int> q = new Guard<int>();
            var ret = binarySearch.BinarySearch(i =>
            {
                int p = PowQuerier[i];
                if (n % p == 0)
                {
                    q.value = n / p;
                    return true;
                }
                return false;
            }, 0, Math.Log(n, denominator).ceil() + 1);
            quotient = q.value;
            return ret;
        }
    }
}

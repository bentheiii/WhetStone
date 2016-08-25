using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.NumbersMagic
{
    public static class pentagonals
    {
        public static IEnumerable<int> Pentagonals(int start = 0)
        {
            return countUp.CountUp(start).SelectMany(a => a == 0 ? a.Enumerate() : new[] { a, -a }).Select(a => (a * (3 * a - 1)) / 2);
        }
    }
}

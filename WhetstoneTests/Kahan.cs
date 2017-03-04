using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberStone;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class Kahan
    {
        [TestMethod]
        public void Simple()
        {
            const int parts = 99;
            const double addand = 1.0/parts;
            double sum = 0.0;
            KahanSum kahan = new KahanSum();
            foreach (int _ in range.Range(parts))
            {
                sum += addand;
                kahan.Add(addand);
            }
            Assert.AreNotEqual(sum,1.0);
            Assert.AreEqual(kahan.Sum,1.0);
        }
    }
}

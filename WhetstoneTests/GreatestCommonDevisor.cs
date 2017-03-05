using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberStone;

namespace Tests
{
    [TestClass]
    public class GreatestCommonDevisor
    {
        [TestMethod]
        public void Simple()
        {
            Assert.AreEqual(greatestCommonDivisor.GreatestCommonDivisor(3,96,99),3);
            Assert.AreEqual(greatestCommonDivisor.GreatestCommonDivisor(8, 96, 16), 8);
            Assert.AreEqual(greatestCommonDivisor.GreatestCommonDivisor(100, 80, 1010), 10);
        }
    }
}

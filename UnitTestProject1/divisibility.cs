using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberStone;

namespace Tests
{
    [TestClass]
    public class divisibility
    {
        [TestMethod] public void Simple()
        {
            Assert.AreEqual(3,24.Divisibility(2));
            Assert.AreEqual(4,(81*5).Divisibility(3));
            Assert.AreEqual(0,47.Divisibility(5));
        }
        [TestMethod]
        public void BigSimple()
        {
            Assert.AreEqual(3, ((BigInteger)24).Divisibility(2));
            Assert.AreEqual(4, ((BigInteger)81 * 5).Divisibility(3));
            Assert.AreEqual(0, ((BigInteger)47).Divisibility(5));
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class BinarySearch
    {
        [TestMethod]
        public void Fieldings()
        {
            var val = binarySearch.BinarySearch(x => x*x - 2, 0.0, 2.0, 1e-4, -1);
            Assert.AreEqual(val,Math.Sqrt(2),1e-3);
        }
    }
}

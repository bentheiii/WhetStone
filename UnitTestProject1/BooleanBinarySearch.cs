using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using static WhetStone.Looping.binarySearch.BooleanBinSearchStyle;

namespace Tests
{
    [TestClass]
    public class booleanBinarySearch
    {
        [TestMethod]
        public void SimpleLast()
        {
            Func<int,bool> func = i => i <= 100;
            Assert.AreEqual(binarySearch.BinarySearch(func,-100,1000),100);
            Assert.AreEqual(binarySearch.BinarySearch(func,0),100);
            Assert.AreEqual(binarySearch.BinarySearch(func, max: 1000), 100);
            Assert.AreEqual(binarySearch.BinarySearch(func), 100);
        }
        [TestMethod]
        public void SimpleFirst()
        {
            Func<int, bool> func = i => i >= 100;
            Assert.AreEqual(binarySearch.BinarySearch(func, -100, 1000, style:GetFirstTrue), 100);
            Assert.AreEqual(binarySearch.BinarySearch(func, 0, style: GetFirstTrue), 100);
            Assert.AreEqual(binarySearch.BinarySearch(func, max: 1000, style: GetFirstTrue), 100);
            Assert.AreEqual(binarySearch.BinarySearch(func, style: GetFirstTrue), 100);
        }
    }
}

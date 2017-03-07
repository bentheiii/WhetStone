using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class Ranges
    {
        [TestMethod]
        public void IntExPosSimples()
        {
            Assert.IsTrue(range.Range(10).SequenceEqual(0,1,2,3,4,5,6,7,8,9));
            Assert.IsTrue(range.Range(10,0,-3).SequenceEqual(10,7,4,1));
            Assert.IsTrue(range.Range(0).SequenceEqual());
            Assert.IsTrue(range.Range(-1).SequenceEqual());
            Assert.IsTrue(range.Range(1).SequenceEqual(0));
        }
        [TestMethod]
        public void GenExPosSimples()
        {
            Assert.IsTrue(range.Range(10.0).SequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9));
            Assert.IsTrue(range.Range(0.0).SequenceEqual());
            Assert.IsTrue(range.Range(-1.0).SequenceEqual());
            Assert.IsTrue(range.Range(1.5).SequenceEqual(0,1));
        }
        [TestMethod]
        public void IntIncPosSimples()
        {
            Assert.IsTrue(range.IRange(10).SequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            Assert.IsTrue(range.IRange(10, 0, -3).SequenceEqual(10, 7, 4, 1));
            Assert.IsTrue(range.IRange(0).SequenceEqual(0));
            Assert.IsTrue(range.IRange(-1).SequenceEqual());
            Assert.IsTrue(range.IRange(1).SequenceEqual(0,1));
        }
        [TestMethod]
        public void GenIncPosSimples()
        {
            Assert.IsTrue(range.IRange(10.0).SequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10));
            Assert.IsTrue(range.IRange(0.0).SequenceEqual(0));
            Assert.IsTrue(range.IRange(-1.0).SequenceEqual());
            Assert.IsTrue(range.IRange(1.5).SequenceEqual(0, 1));
        }
    }
}

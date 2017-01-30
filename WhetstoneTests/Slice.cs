using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class SliceTest
    {
        [TestMethod]
        public void Single()
        {
            var val = new[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 }.Slice(3);
            int v = 30;
            Assert.AreEqual(val.Count, 8);
            foreach (int i in val)
            {
                Assert.AreEqual(v, i);
                v += 10;
            }
            foreach (int i in range.Range(val.Count))
            {
                Assert.AreEqual(val[i], i * 10 + 30);
            }
        }
        [TestMethod]
        public void Simple()
        {
            var val = new[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 }.Slice(3, 8, 2);
            Assert.AreEqual(val.Count,3);
            int v = 30;
            foreach (int i in val)
            {
                Assert.AreEqual(v, i);
                v += 20;
            }
            foreach (int i in range.Range(val.Count))
            {
                Assert.AreEqual(val[i], i * 20 + 30);
            }
        }
        [TestMethod]
        public void Double()
        {
            var val0 = new[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 }.Slice(3, 8, 2);
            var val = val0.Slice(0,3,2);
            int v = 30;
            Assert.AreEqual(val.Count, 2);
            foreach (int i in val)
            {
                Assert.AreEqual(v, i);
                v += 40;
            }
            foreach (int i in range.Range(val.Count))
            {
                Assert.AreEqual(val[i], i * 40 + 30);
            }
        }
    }
}

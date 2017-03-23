using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class ListCache
    {
        [TestMethod]
        public void Simple()
        {
            int counter = 0;
            var val = range.Range(10).Select(a =>
            {
                counter++;
                return a;
            }).Cache();
            Assert.IsTrue(val.SequenceEqualIndices(0,1,2,3,4,5,6,7,8,9));
            val.Do();
            val.Do();
            Assert.AreEqual(10,counter);
        }
    }
}

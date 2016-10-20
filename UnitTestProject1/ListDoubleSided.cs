using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;


namespace Tests
{
    [TestClass]
    public class ListDoubleSidedTest
    {
        [TestMethod]
        public void Simple()
        {
            ListDoubleSided<int> val = new ListDoubleSided<int>();
            Assert.IsTrue(val.Count == 0);
            val.Add(6);
            Assert.IsTrue(val.SequenceEqual(6));
            val.Insert(0,0);
            Assert.IsTrue(val.SequenceEqual(0,6));
            val.Insert(1,5);
            Assert.IsTrue(val.SequenceEqual(0, 5, 6));
            val.Insert(2,2);
            val.RemoveAt(1);
            Assert.IsTrue(val.SequenceEqual(0, 2, 6));

        }
    }
}

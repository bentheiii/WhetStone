using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class Duplicates
    {
        [TestMethod]
        public void Simple()
        {
            var val = new int[] {1, 2, 5, 2, 8, 4, 5, 5, 5, 2, 8, 1}.Duplicates().OrderBy();
            Assert.IsTrue(val.SequenceEqual(1,2,5,8));
        }
    }
}

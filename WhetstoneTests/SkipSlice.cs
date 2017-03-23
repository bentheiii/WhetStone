using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class skipSlice
    {
        [TestMethod]
        public void Simple()
        {
            var val = range.Range(10).SkipSlice(3, 5);
            var arr = val.ToArray();
            Assert.IsTrue(val.SequenceEqualIndices(0,1,2,8,9));
        }
    }
}

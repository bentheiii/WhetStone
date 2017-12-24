using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class ToArray
    {
        [TestMethod]
        public void LimitToCap()
        {
            var val = countUp.CountUp();
            Assert.IsTrue(range.Range(10).SequenceEqual(val.ToArray(10,toArray.OverflowPolicy.End)));
        }
    }
}

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class countAtLeastTests
    {
        [TestMethod] public void SimpleTrue()
        {
            var val = new[]
            {
                Enumerable.Range(0, 6), Enumerable.Range(0, 7), Enumerable.Range(0, 10),
                range.Range(6), range.Range(7), range.Range(10)
            };
            foreach (var i in val)
            {
                Assert.IsTrue(i.CountAtLeast(6));
                Assert.IsTrue(i.CountAtLeast(5));
                
                Assert.IsTrue(i.CountAtLeast(3,a=>a%2==0));
                Assert.IsTrue(i.CountAtLeast(2, a => a % 2 == 0));
            }
        }
        [TestMethod]
        public void SimpleFalse()
        {
            var val = new[]
            {
                Enumerable.Range(0, 6), Enumerable.Range(0, 7), Enumerable.Range(0, 10),
                range.Range(6), range.Range(7), range.Range(10)
            };
            foreach (var i in val)
            {
                Assert.IsFalse(i.CountAtLeast(11));
                Assert.IsFalse(i.CountAtLeast(15));

                Assert.IsFalse(i.CountAtLeast(6, a => a % 2 == 0));
                Assert.IsFalse(i.CountAtLeast(8, a => a % 2 == 0));
            }
        }
    }
}

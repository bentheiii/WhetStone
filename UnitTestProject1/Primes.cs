using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberStone;

namespace Tests
{
    [TestClass]
    public class Primes
    {
        [TestMethod]
        public void List()
        {
            var val = primes.Primes(13);
            Assert.IsTrue(val.SequenceEqual(new [] {2,3,5,7,11}));
        }
    }
}

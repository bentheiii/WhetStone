using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class TrailTest
    {
        [TestMethod] public void Simple_Trail_List()
        {
            var trails = range.Range(10).Trail(3).Select(a => a.ToArray()).ToArray();

            foreach (var (i, s) in range.Range(8).Zip(trails))
            {
                Assert.AreEqual(i+0, s[0]);
                Assert.AreEqual(i+1, s[1]);
                Assert.AreEqual(i+2, s[2]);
            }

            Assert.AreEqual(trails.Length, 8);
        }
    }
}

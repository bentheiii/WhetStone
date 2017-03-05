using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class StrConcat
    {
        [TestMethod]
        public void Simple()
        {
            var val = range.Range(10).StrConcat("-");
            Assert.AreEqual(val, "0-1-2-3-4-5-6-7-8-9");
        }
        [TestMethod]
        public void SimpleDict()
        {
            var val = range.Range(10).Attach(a=>a*a).ToDictionary().StrConcat("-",";");
            Assert.AreEqual(val, "0-0;1-1;2-4;3-9;4-16;5-25;6-36;7-49;8-64;9-81");
        }
    }
}

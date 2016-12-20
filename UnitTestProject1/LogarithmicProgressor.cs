using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberStone;
using WhetStone.Looping;
using WhetStone.Random;
using WhetStone.SystemExtensions;

namespace Tests
{
    [TestClass]
    public class LogarithmicProgressor
    {
        [TestMethod]
        public void Simple()
        {
            var congen = new LocalRandomGenerator();
            const int totaltrials = 20;
            const int incspertrial = 500;
            foreach (int _ in range.Range(totaltrials))
            {
                var seed = congen.Int(int.MaxValue);
                var gen = new LocalRandomGenerator(seed);
                var total = gen.Int(1, 1000);
                var @base = gen.Int(2, 20, true);
                var val = new LogarithmicProgresser(@base,total);
                foreach (var __ in range.Range(incspertrial))
                {
                    var addant = gen.Int(3000);
                    val.Increment(addant);
                    total += addant;
                    Assert.AreEqual(total.log(@base).floor(), val.log, $"seed = {seed}");
                }
            }
        }
    }
}

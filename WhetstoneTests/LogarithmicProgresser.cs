using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using WhetStone.Random;
using WhetStone.SystemExtensions;

namespace Tests
{
    [TestClass]
    public class LogarithmicProgresser
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
                var val = new NumberStone.LogarithmicProgresser(@base,total);
                foreach (var __ in range.Range(incspertrial))
                {
                    var addant = gen.Int(3000);
                    val.Increment(addant);
                    total += addant;
                    Assert.AreEqual(Math.Log(total,@base).floor(), val.log, $"seed = {seed}");
                    Assert.AreEqual(total, val.value, $"seed = {seed}");
                }
            }
        }
    }
}

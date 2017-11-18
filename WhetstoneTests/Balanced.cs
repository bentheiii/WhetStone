using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using WhetStone.WordPlay;

namespace Tests
{
    [TestClass]
    public class Balanced
    {
        [TestMethod]
        public void Simple()
        {
            Assert.IsTrue("00(111(222)1)(11)000(11)".Balanced('(',')'));
            Assert.IsTrue("00(111(222)1)(11)000(11)".Balanced("(", ")"));
            Assert.IsTrue("00(111[222]1){11}000(11)".Balanced(new[] {('{', '}'), ('[', ']'), ('(', ')')}));
            Assert.IsTrue(CommonRegex.RegexDouble.Balanced(new[] { ('{', '}'), ('[', ']'), ('(', ')') }));
            Assert.IsTrue(CommonRegex.RegexDoubleNoSign.Balanced(new[] { ('{', '}'), ('[', ']'), ('(', ')') }));

            Assert.IsFalse("00(111(222)1)11)000(11)".Balanced('(', ')'));
            Assert.IsFalse("00(111(222)1)(11000(11)".Balanced("(", ")"));
            Assert.IsFalse("00(111[222)1){11}000(11)".Balanced(new[] { ('{', '}'), ('[', ']'), ('(', ')') }));

            Assert.IsFalse("00(111(22(3(4))2)1)(11)000(11)".Balanced('(', ')',3));
            Assert.IsTrue("00(111(222)1)(11)000(11)".Balanced("(", ")",2));
            Assert.IsTrue("00(111[222]1){11}000(11)".Balanced(new[] { ('{', '}'), ('[', ']'), ('(', ')') },2));
        }
    }
}

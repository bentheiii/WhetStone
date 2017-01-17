using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using WhetStone.WordPlay;

namespace Tests
{
    [TestClass]
    public class EditDistanceTests
    {
        [TestMethod]
        public void SimpleSteps()
        {
            Assert.AreEqual("kitten".AsList().EditSteps("sitting".AsList()).Count(), 3);
            var strings = new []{"kitten", "sitting", "sitten", "cat", "pat", "omega", "tribunal", "xxxxx", "      ", "", "t"}.Select(a=>a.AsList());
            foreach (var pair in strings.Join(@join.CartesianType.AllPairs))
            {
                var s = pair.Item1;
                var d = pair.Item2;

                var val = s.EditSteps(d).ToList();

                var p = new List<char>(s);

                foreach (var editStep in val)
                {
                    editStep.apply(p);
                }

                Assert.AreEqual(p.ConvertToString(),d.ConvertToString());
            }
        }
        [TestMethod] public void SimpleDist()
        {
            Assert.AreEqual("kitten".AsList().EditDistance("sitting".AsList()), 3);
            var strings = new[] { "kitten", "sitting", "sitten", "cat", "pat", "omega", "tribunal", "xxxxx", "      ", "", "t" }.Select(a => a.AsList());
            foreach (var pair in strings.Join(@join.CartesianType.AllPairs))
            {
                var s = pair.Item1;
                var d = pair.Item2;

                var val = s.EditSteps(d).ToList();
                var c = s.EditDistance(d);
                Assert.AreEqual(val.Count, c);
            }
        }

        [TestMethod]
        public void NoSubSteps()
        {
            Assert.AreEqual("kitten".AsList().EditSteps("sitting".AsList(), allowSub: false).Count(), 5);
            var strings = new[] { "kitten", "sitting", "sitten", "cat", "pat", "omega", "tribunal", "xxxxx", "      ", "", "t" }.Select(a => a.AsList());
            foreach (var pair in strings.Join(@join.CartesianType.AllPairs))
            {
                var s = pair.Item1;
                var d = pair.Item2;

                var val = s.EditSteps(d, allowSub: false).ToList();

                var p = new List<char>(s);

                foreach (var editStep in val)
                {
                    editStep.apply(p);
                }

                Assert.AreEqual(p.ConvertToString(), d.ConvertToString());
            }
        }
        [TestMethod]
        public void NoSubDist()
        {
            Assert.AreEqual("kitten".AsList().EditDistance("sitting".AsList(), allowSub: false), 5);
            var strings = new[] { "kitten", "sitting", "sitten", "cat", "pat", "omega", "tribunal", "xxxxx", "      ", "", "t" }.Select(a => a.AsList());
            foreach (var pair in strings.Join(@join.CartesianType.AllPairs))
            {
                var s = pair.Item1;
                var d = pair.Item2;

                var val = s.EditSteps(d, allowSub: false).ToList();
                var c = s.EditDistance(d, allowSub: false);
                Assert.AreEqual(val.Count, c);
            }
        }
    }
}

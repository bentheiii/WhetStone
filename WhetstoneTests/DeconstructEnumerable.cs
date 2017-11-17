using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class DeconstructEnumerable
    {
        [TestMethod] public void Simple()
        {
            int a, b, c, d, e, f, g;

            void Check()
            {
                Assert.IsTrue(new []{a,b,c,d,e,f,g}.SequenceEqual(range.Range(7)));
            }

            (a, b, c, d, e, f, g) = range.Range(7);
            Check();
            (a, b, c, d, e, f) = range.Range(6);
            Check();
            (a, b, c, d, e) = range.Range(5);
            Check();
            (a, b, c, d) = range.Range(4);
            Check();
            (a, b, c) = range.Range(3);
            Check();
            (a, b) = range.Range(2);
            Check();
        }
    }
}

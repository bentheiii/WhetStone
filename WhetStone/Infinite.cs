using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class infinite
    {
        public static IEnumerable<positionBind.Position> Infinite()
        {
            yield return positionBind.Position.First | positionBind.Position.Middle;
            while (true)
            {
                yield return positionBind.Position.Middle;
            }
        }
    }
}

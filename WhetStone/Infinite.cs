using System.Collections.Generic;

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

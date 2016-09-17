using WhetStone.Looping;

namespace NumberStone
{
    public static class leastCommonMultiple
    {
        public static int Leastcommonmultiple(params int[] vals)
        {
            return vals.GetProduct((a, b) => a * b) / greatestCommonDivisor.GreatestCommonDivisor(vals);
        }
    }
}

using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class rows
    {
        public static LockedList<LockedList<T>> Rows<T>(this T[,] @this)
        {
            return range.Range(@this.GetLength(0)).Select(a => range.Range(@this.GetLength(1)).Select(x => @this[a, x]));
        }
    }
}

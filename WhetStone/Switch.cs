using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class @switch
    {
        public static IEnumerable<T> Switch<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            var tors = new LinkedList<IEnumerator<T>>(@this.Select(a=>a.GetEnumerator()));
            while (tors.Any())
            {
                var todel = new List<LinkedListNode<IEnumerator<T>>>();
                var node = tors.First;
                while (node != null)
                {
                    if (!node.Value.MoveNext())
                    {
                        todel.Add(node);
                    }
                    else
                    {
                        yield return node.Value.Current;
                    }
                    node = node.Next;
                }
                todel.ForEach(tors.Remove);
            }
        }
    }
}

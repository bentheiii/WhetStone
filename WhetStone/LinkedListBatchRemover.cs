using System.Collections.Generic;

namespace WhetStone.Looping
{
    public class LinkedListBatchRemover<T>
    {
        private readonly LinkedList<T> _source;
        private readonly ICollection<LinkedListNode<T>> _toRemove;
        public LinkedListBatchRemover(LinkedList<T> source, int capacity)
        {
            _source = source;
            _toRemove = new List<LinkedListNode<T>>(capacity);
        }
        public void Add(LinkedListNode<T> toAdd)
        {
            _toRemove.Add(toAdd);
        }
        public void Commit()
        {
            foreach (LinkedListNode<T> node in _toRemove)
            {
                _source.Remove(node);
            }
            _toRemove.Clear();
        }
    }
    public static class LinkedListBatchRemoverExtentions
    {
        public static LinkedListBatchRemover<T> GetBatchRemover<T>(this LinkedList<T> source, int capacity=0)
        {
            return new LinkedListBatchRemover<T>(source, capacity);
        }
    }
}

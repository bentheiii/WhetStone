using System.Collections.Generic;

namespace WhetStone.LockedStructures
{
    internal class LockedListStringAdaptor : LockedList<char>
    {
        private readonly string _inner;
        public LockedListStringAdaptor(string inner)
        {
            _inner = inner;
        }
        public override IEnumerator<char> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
        public override int Count
        {
            get
            {
                return _inner.Length;
            }
        }
        public override int IndexOf(char item)
        {
            return _inner.IndexOf(item);
        }
        public override char this[int index]
        {
            get
            {
                return _inner[index];
            }
        }
    }
}
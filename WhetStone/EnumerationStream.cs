using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhetStone.Looping
{
    public class EnumerationStream : Stream
    {
        private IEnumerator<byte> _tor;
        private readonly Lazy<int> _lcount;
        public EnumerationStream(IEnumerable<byte> tor)
        {
            _tor = tor.GetEnumerator();
            _lcount = new Lazy<int>(tor.Count);
        }
        public override void Flush()
        {
            throw new NotSupportedException();
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_tor == null)
                return 0;
            int ret = 0;
            foreach (int i in range.Range(offset, count))
            {
                if (!_tor.MoveNext())
                {
                    _tor = null;
                    break;
                }
                buffer[i] = _tor.Current;
                ret++;
            }
            return ret;
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length
        {
            get
            {
                return _lcount.Value;
            }
        }
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}

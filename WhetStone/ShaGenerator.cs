using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using WhetStone.Arrays;

namespace WhetStone.Random
{
    public class ShaGenerator : ByteEnumeratorGenerator, IDisposable
    {
        private readonly SHA256 _sha;
        private readonly IList<byte> _seed;
        public ShaGenerator(IEnumerable<byte> seed)
        {
            _sha = SHA256.Create();
            _seed = new List<byte>(seed);
        }
        public override IEnumerable<byte> Bytes()
        {
            while (true)
            {
                _sha.ComputeHash(_seed.ToArray().Shuffle());
                _seed.Clear();
                foreach (byte b in _sha.Hash)
                {
                    yield return b;
                    _seed.Add(b);
                }
            }
        }
        public void Dispose()
        {
            ((IDisposable)_sha).Dispose();
        }
    }
}
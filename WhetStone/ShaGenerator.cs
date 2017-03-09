using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace WhetStone.Random
{
    /// <summary>
    /// A <see cref="RandomGenerator"/> using SHA512 hashing to generate <see cref="byte"/>s.
    /// </summary>
    public class ShaGenerator : ByteEnumeratorGenerator, IDisposable
    {
        private readonly SHA512 _sha;
        private readonly IList<byte> _seed;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="seed">The initial seed for the SHA generator.</param>
        public ShaGenerator(IEnumerable<byte> seed)
        {
            seed.ThrowIfNull(nameof(seed));
            _sha = SHA512.Create();
            _seed = new List<byte>(seed);
        }
        /// <inheritdoc />
        public override IEnumerable<byte> Bytes()
        {
            while (true)
            {
                _seed.Shuffle();
                _sha.ComputeHash(_seed.ToArray());
                _seed.Clear();
                foreach (byte b in _sha.Hash)
                {
                    yield return b;
                    _seed.Add(b);
                }
            }
        }
        /// <inheritdoc />
        public void Dispose()
        {
            _sha.Dispose();
        }
    }
}
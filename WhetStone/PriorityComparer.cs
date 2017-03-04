﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Comparison
{
    /// <summary>
    /// An <see cref="IComparer{T}"/> that compares elements through multiple <see cref="IComparer{T}"/>s, returning the first inequality.
    /// </summary>
    /// <typeparam name="T">The type of elements to compare.</typeparam>
    public class PriorityComparer<T> : IComparer<T>
    {
        private readonly IEnumerable<IComparer<T>> _comps;
        private PriorityComparer(IEnumerable<IComparer<T>> c)
        {
            this._comps = c;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="c">The <see cref="IComparer{T}"/> to use.</param>
        public PriorityComparer(params IComparer<T>[] c)
        {
            this._comps = c.ToArray();
        }
        /// <inheritdoc />
        public int Compare(T x, T y)
        {
            return _comps.Select(c => c.Compare(x, y)).FirstOrDefault(ret => ret != 0);
        }
    }
}

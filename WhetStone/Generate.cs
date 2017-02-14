using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class generate
    {
        /// <summary>
        /// Get an infinite <see cref="IEnumerable{T}"/> out of a generator function.
        /// </summary>
        /// <typeparam name="T">The type of the generated elements.</typeparam>
        /// <param name="gen">The generator that creates elements.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> whose members are generated on-the-fly with <paramref name="gen"/>.</returns>
        public static IEnumerable<T> Generate<T>(Func<T> gen)
        {
            while (true)
            {
                yield return gen();
            }
        }
    }
}

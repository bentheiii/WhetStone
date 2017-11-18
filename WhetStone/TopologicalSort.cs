using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class topologicalSort
    {
        /// <summary>
        /// Orders elements in an <see cref="IEnumerable{T}"/> so that each element appears after its dependents.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <typeparam name="TDependencies">The type of the dependency collection.</typeparam>
        /// <param name="elements">An <see cref="IEnumerable{T}"/> of tuples. The first element of the tuple is the element itself, the second is an <see cref="ICollection{T}"/> of its dependents.</param>
        /// <param name="allowMissingDependancy">Whether to ignore dependencies that are not in <paramref name="elements"/>.</param>
        /// <returns>All the elements in <paramref name="elements"/> ordered topologically.</returns>
        /// <exception cref="ArgumentException">If <paramref name="elements"/> contains cyclical dependencies or a dependency that does not exist in <paramref name="elements"/>.</exception>
        /// <remarks>A dependency collection can be empty or <see langword="null"/> to indicate no dependency.</remarks>
        public static IEnumerable<T> TopologicalSort<T, TDependencies>(this IEnumerable<(T, TDependencies)> elements, bool allowMissingDependancy = false) where TDependencies:IEnumerable<T>
        {
            elements.ThrowIfNull(nameof(elements));

            IDictionary<T, ICollection<T>> nodes = new Dictionary<T, ICollection<T>>();
            ISet<T> ready = new HashSet<T>();
            foreach (var element in elements)
            {
                IEnumerable<T> dependancies = element.Item2;
                if (dependancies != null && allowMissingDependancy)
                    dependancies = dependancies.Where(a => elements.Select(x => x.Item1).Contains(a)).Cache();
                if (dependancies == null || !dependancies.Any())
                    ready.Add(element.Item1);
                else
                    nodes.Add(element.Item1, new HashSet<T>(dependancies));
            }

            while (ready.Count != 0)
            {
                var next = ready.First();
                yield return next;
                ready.Remove(next);

                foreach (var node in nodes.Keys.ToArray())
                {
                    if (!nodes[node].Contains(next))
                        continue;

                    nodes[node].Remove(next);
                    if (nodes[node].Count == 0)
                    {
                        nodes.Remove(node);
                        ready.Add(node);
                    }
                }
            }

            if (nodes.Count != 0)
                throw new ArgumentException($"{nameof(elements)} contains cycles" +
                                            ", or dependencies not in elements.");
        }
    }
}

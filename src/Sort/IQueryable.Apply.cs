namespace Panner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class IQueryableExtensions
    {
        /// <summary>Applies order operations defined as <paramref name="particles"/>.</summary>
        /// <param name="source">The source.</param>
        /// <param name="particles">The particles.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="particles"/></exception>
        public static IOrderedQueryable<T> Apply<T>
        (
            this IQueryable<T> source,
            IEnumerable<ISortParticle<T>> particles
        )
        where T : class
        {
            if (particles is null)
            {
                throw new ArgumentNullException(nameof(particles));
            }

            // "Cast" to IOrderedQueryable
            var result = source.OrderBy(x => true);

            foreach (var p in particles)
            {
                result = p.ApplyTo(result);
            }

            return result;
        }
    }

    // Shortcut from IEnumerable<T>
    public static partial class IEnumerableExtensions
    {
        /// <summary>Applies order operations defined as <paramref name="particles"/>.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="particles"><typeparamref name="T"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> Apply<T>
        (
            this IEnumerable<T> source,
            IEnumerable<ISortParticle<T>> particles
        )
        where T : class
        => source.AsQueryable().Apply(particles);
    }
}

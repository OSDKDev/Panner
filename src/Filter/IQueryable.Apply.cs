namespace Panner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public static partial class IQueryableExtensions
    {
        /// <summary>Applies order operations defined as <paramref name="particles"/>.</summary>
        /// <param name="source">The source.</param>
        /// <param name="particles">The particles.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="particles"/></exception>
        public static IQueryable<T> Apply<T>
        (
            this IQueryable<T> source,
            IEnumerable<IFilterParticle<T>> particles
        )
        where T : class
        {
            if (particles is null)
            {
                throw new ArgumentNullException(nameof(particles));
            }

            var parameter = Expression.Parameter(typeof(T), "e");
            var result = source;

            foreach (var p in particles)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(p.GetExpression(parameter), parameter);
                result = result.Where(lambda);
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
        public static IQueryable<T> Apply<T>
        (
            this IEnumerable<T> source,
            IEnumerable<IFilterParticle<T>> particles
        )
        where T : class
        => source.AsQueryable().Apply(particles);
    }
}

namespace Panner.Filter.Particles
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class OrFilterParticle<TEntity> : IFilterParticle<TEntity>
        where TEntity : class
    {
        readonly IFilterParticle<TEntity>[] particles;

        public OrFilterParticle(params IFilterParticle<TEntity>[] particles)
        {
            if (particles.Length < 2)
            {
                throw new Exception("OrFilterParticle needs at least two particles.");
            }

            this.particles = particles;
        }

        public Expression GetExpression(ParameterExpression parameter)
        {

            var expressions = particles.Select(x => x.GetExpression(parameter));
            var result = expressions.First();

            foreach (var expr in expressions.Skip(1))
            {
                result = Expression.Or(result, expr);
            }

            return result;
        }

    }
}

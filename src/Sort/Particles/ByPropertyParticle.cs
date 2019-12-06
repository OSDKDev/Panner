namespace Panner.Sort.Particles
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ByPropertyParticle<T> : ISortParticle<T>
        where T : class
    {
        private readonly Type type;
        private readonly PropertyInfo property;
        private readonly bool descending;

        public ByPropertyParticle(PropertyInfo property, bool descending = false)
        {
            this.type = typeof(T);

            if (property.DeclaringType != this.type)
            {
                throw new ArgumentException($"Received property \"{property.Name}\" is not declared in type \"{this.type.Name}\"", nameof(property));
            }

            this.property = property;
            this.descending = descending;
        }

        public IOrderedQueryable<T> ApplyTo(IOrderedQueryable<T> source)
        {
            var command = this.descending ? "ThenByDescending" : "ThenBy";

            var parameter = Expression.Parameter(this.type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, this.property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { this.type, this.property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}

namespace Panner.Filter.Particles
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ByPropertyParticle<TEntity, TProperty> : IFilterParticle<TEntity>
        where TEntity : class
    {
        private static readonly Dictionary<Operator, Func<Expression, Expression, Expression>> operatorQueryMap;
        private readonly PropertyInfo property;
        private readonly Operator op;
        private readonly TProperty value;

        static ByPropertyParticle()
        {
            operatorQueryMap = new Dictionary<Operator, Func<Expression, Expression, Expression>>()
            {
                { Operator.Equal, (propertyValue, filterValue) => Expression.Equal(propertyValue, filterValue) },
                { Operator.NotEquals, (propertyValue, filterValue) => Expression.NotEqual(propertyValue, filterValue) },
                { Operator.GreaterThan, (propertyValue, filterValue) => Expression.GreaterThan(propertyValue, filterValue) },
                { Operator.LessThan, (propertyValue, filterValue) => Expression.LessThan(propertyValue, filterValue) },
            };
        }

        public ByPropertyParticle(PropertyInfo property, Operator op, TProperty value)
        {
            this.property = property;
            this.op = op;
            this.value = value;
        }

        public Expression GetExpression(ParameterExpression parameter)
        {
            var propertyExpression = Expression.PropertyOrField(parameter, this.property.Name);

            // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
            // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
            // Expression.Constant passed the target type to allow Nullable comparison
            // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
            Expression filterValue = Expression.Constant(this.value, typeof(TProperty));

            return operatorQueryMap[this.op](propertyExpression, filterValue);
        }
    }
}

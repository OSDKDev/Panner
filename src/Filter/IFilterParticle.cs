namespace Panner
{
    using System.Linq.Expressions;

    public interface IFilterParticle<T>
        where T : class
    {
        Expression GetExpression(ParameterExpression parameter);
    }
}

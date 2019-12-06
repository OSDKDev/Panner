namespace Panner
{
    using System.Linq;

    public interface ISortParticle<T>
        where T : class
    {
        IOrderedQueryable<T> ApplyTo(IOrderedQueryable<T> source);
    }
}

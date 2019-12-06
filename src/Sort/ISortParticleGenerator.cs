namespace Panner
{
    using Panner.Interfaces;

    public interface ISortParticleGenerator<T> : IParticleGenerator<ISortParticle<T>>
        where T : class
    {
    }
}

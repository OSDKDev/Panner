namespace Panner
{
    using Panner.Interfaces;

    public interface IFilterParticleGenerator<T> : IParticleGenerator<IFilterParticle<T>>
        where T : class
    {
    }
}

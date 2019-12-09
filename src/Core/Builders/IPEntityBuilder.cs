namespace Panner.Builders
{
    using Panner.Interfaces;

    public interface IPEntityBuilder
    {
        TParticleGenerator GetOrCreateGenerator<TParticle, TParticleGenerator>() where TParticleGenerator : class, IParticleGenerator<TParticle>, new();

        PEntity Build();
    }
}

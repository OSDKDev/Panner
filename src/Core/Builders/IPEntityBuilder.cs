namespace Panner.Builders
{
    using Panner.Interfaces;

    internal interface IPEntityBuilder
    {
        TParticleGenerator GetOrCreateGenerator<TParticle, TParticleGenerator>() where TParticleGenerator : class, IParticleGenerator<TParticle>, new();

        PEntity Build();
    }
}

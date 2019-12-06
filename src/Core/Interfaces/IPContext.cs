using System.Collections.Generic;
using Panner.Interfaces;

namespace Panner
{
    public interface IPContext
    {
        IEnumerable<IParticleGenerator<TParticle>> GetGenerators<TEntity, TParticle>();
    }
}
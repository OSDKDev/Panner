namespace Panner
{
    using System.Collections.Generic;
    using Panner.Interfaces;

    internal interface IPEntity
    {
        IEnumerable<IParticleGenerator<T>> GetGenerators<T>();
    }
}

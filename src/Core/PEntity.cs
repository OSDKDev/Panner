namespace Panner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Panner.Interfaces;

    public class PEntity : IPEntity
    {
        internal Dictionary<Type, List<object>> _ParticleGenerators;

        internal PEntity()
        {
            this._ParticleGenerators = new Dictionary<Type, List<object>>();
        }

        public IEnumerable<IParticleGenerator<T>> GetGenerators<T>()
        {
            this._ParticleGenerators.TryGetValue(typeof(T), out var particleGenerators);
            return particleGenerators?.Select(x => (IParticleGenerator<T>)x) ?? Enumerable.Empty<IParticleGenerator<T>>();
        }
    }
}

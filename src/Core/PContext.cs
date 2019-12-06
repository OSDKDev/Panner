namespace Panner
{
    using System;
    using System.Collections.Generic;
    using Panner.Interfaces;

    public class PContext : IPContext
    {
        internal IReadOnlyDictionary<Type, IPEntity> _PEntities { get; set; }

        internal PContext(IReadOnlyDictionary<Type, IPEntity> pEntities)
        {
            this._PEntities = pEntities;
        }

        public IEnumerable<IParticleGenerator<TParticle>> GetGenerators<TEntity, TParticle>()
        {
            if (!_PEntities.TryGetValue(typeof(TEntity), out var pEntity))
            {
                throw new Exception("Entity type not defined in context");
            }

            return pEntity.GetGenerators<TParticle>();
        }
    }
}

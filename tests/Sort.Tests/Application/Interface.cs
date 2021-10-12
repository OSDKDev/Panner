﻿namespace Sort.Tests.Application
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Panner;

    public class Interface
    {
        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value")]
        public void Builds()
        {
            IEnumerable<Interface> entities = Array.Empty<Interface>();
            IEnumerable<ISortParticle<Interface>> particles = Array.Empty<ISortParticle<Interface>>();

            IOrderedQueryable<Interface> works;

            works = entities.Apply(particles);
            works = entities.AsQueryable().Apply(particles);
            works = entities.ToArray().Apply(particles);
            works = entities.ToHashSet().Apply(particles);
            works = entities.ToList().Apply(particles);
        }
    }
}

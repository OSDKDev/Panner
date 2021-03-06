﻿namespace Panner.Builders //Namespace is intentional so extensions are picked up when builder is used.
{
    using Panner.Sort.Generators;

    public static partial class PPropertyBuilderExtensions
    {
        /// <summary>Marks the property as not sortable, referenced by a specific alias.</summary>
        public static PPropertyBuilder<T> IsNotSortableAs<T>(this PPropertyBuilder<T> pPropertyBuilder, string alias)
            where T : class
        {
            var generator = pPropertyBuilder.Entity.GetOrCreateGenerator<ISortParticle<T>, ByPropertyAliasParticleGenerator<T>>();
            generator.Remove(alias);
            return pPropertyBuilder;
        }
    }
}

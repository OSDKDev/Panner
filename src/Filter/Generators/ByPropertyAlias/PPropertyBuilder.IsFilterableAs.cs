namespace Panner.Builders //Namespace is intentional so extensions are picked up when builder is used.
{
    using Panner.Filter.Generators;

    public static partial class PPropertyBuilderExtensions
    {
        /// <summary>Marks the property as filterable, referenced by a specific alias.</summary>
        public static PPropertyBuilder<T> IsFilterableAs<T>(this PPropertyBuilder<T> pPropertyBuilder, string alias)
            where T : class
        {
            var generator = pPropertyBuilder.Entity.GetOrCreateGenerator<IFilterParticle<T>, ByPropertyAliasParticleGenerator<T>>();
            generator.Add(alias, pPropertyBuilder.PropertyInfo);
            return pPropertyBuilder;
        }
    }
}

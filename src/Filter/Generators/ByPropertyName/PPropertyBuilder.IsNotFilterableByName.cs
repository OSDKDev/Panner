namespace Panner.Builders //Namespace is intentional so extensions are picked up when builder is used.
{
    using Panner.Filter.Generators;

    public static partial class PPropertyBuilderExtensions
    {
        /// <summary>Marks the property as not filterable, referenced by its name.</summary>
        public static PPropertyBuilder<T> IsNotFilterableByName<T>(this PPropertyBuilder<T> pPropertyBuilder)
            where T : class
        {
            var generator = pPropertyBuilder.Entity.GetOrCreateGenerator<IFilterParticle<T>, ByPropertyNameParticleGenerator<T>>();
            generator.Remove(pPropertyBuilder.PropertyInfo);
            return pPropertyBuilder;
        }
    }
}

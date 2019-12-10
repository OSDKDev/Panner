namespace Panner.Builders //Namespace is intentional so extensions are picked up when builder is used.
{
    using System.Reflection;
    using Panner.Filter.Generators;

    public static partial class PEntityBuilderExtensions
    {
        /// <summary>Marks all properties of an entity as filterable, referenced by its name.</summary>
        public static PEntityBuilder<T> AllPropertiesAreFilterableByName<T>(this PEntityBuilder<T> pEntityBuilder)
            where T : class
        {
            var generator = pEntityBuilder.GetOrCreateGenerator<IFilterParticle<T>, ByPropertyNameParticleGenerator<T>>();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in properties)
                generator.Add(p);

            return pEntityBuilder;
        }
    }
}

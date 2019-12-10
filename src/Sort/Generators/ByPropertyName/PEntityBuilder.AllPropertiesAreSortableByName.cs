namespace Panner.Builders //Namespace is intentional so extensions are picked up when builder is used.
{
    using System.Reflection;
    using Panner.Sort.Generators;

    public static partial class PEntityBuilderExtensions
    {
        /// <summary>Marks all properties of an entity as sortable, referenced by its name.</summary>
        public static PEntityBuilder<T> AllPropertiesAreSortableByName<T>(this PEntityBuilder<T> pEntityBuilder)
            where T : class
        {
            var generator = pEntityBuilder.GetOrCreateGenerator<ISortParticle<T>, ByPropertyNameParticleGenerator<T>>();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in properties)
                generator.Add(p);

            return pEntityBuilder;
        }
    }
}

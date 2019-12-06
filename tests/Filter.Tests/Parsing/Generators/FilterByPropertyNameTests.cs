namespace Filter.Tests.Parsing.ByPropertyName
{
    using Moq;
    using Panner;
    using Panner.Filter.Generators;

    public class FilterByPropertyNameTests : FilterParticleGeneratorTests<ByPropertyNameParticleGenerator<FilterableOne>, IFilterParticle<FilterableOne>>
    {
        public FilterByPropertyNameTests() : base() { }

        protected override IPContext GetContext()
        {
            return new Mock<IPContext>(MockBehavior.Strict).Object;
        }

        protected override ByPropertyNameParticleGenerator<FilterableOne> GetGenerator()
        {
            var generator = new ByPropertyNameParticleGenerator<FilterableOne>();
            generator.Add(typeof(FilterableOne).GetProperty(nameof(FilterableOne.Filterable)));

            generator.Add(typeof(FilterableOne).GetProperty(nameof(FilterableOne.NonFilterable)));
            generator.Remove(typeof(FilterableOne).GetProperty(nameof(FilterableOne.NonFilterable)));
            return generator;
        }
    }
}

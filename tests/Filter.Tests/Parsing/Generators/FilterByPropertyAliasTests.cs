namespace Filter.Tests.Parsing.ByPropertyName
{
    using System;
    using Moq;
    using Panner;
    using Panner.Filter.Generators;
    using Xunit;

    public class FilterByPropertyAliasTests : FilterParticleGeneratorTests<ByPropertyAliasParticleGenerator<FilterableOne>, IFilterParticle<FilterableOne>>
    {
        public FilterByPropertyAliasTests() : base() { }

        protected override IPContext GetContext()
        {
            return new Mock<IPContext>(MockBehavior.Strict).Object;
        }

        protected override ByPropertyAliasParticleGenerator<FilterableOne> GetGenerator()
        {
            var generator = new ByPropertyAliasParticleGenerator<FilterableOne>();

            generator.Add("Filterable", typeof(FilterableOne).GetProperty(nameof(FilterableOne.Filterable)));

            generator.Add("NonFilterable", typeof(FilterableOne).GetProperty(nameof(FilterableOne.NonFilterable)));
            generator.Remove("NonFilterable");
            return generator;
        }

        [Fact]
        public void AliasDuplicationThrows()
        {
            var generator = new ByPropertyAliasParticleGenerator<FilterableOne>();
            generator.Add("Filterable", typeof(FilterableOne).GetProperty(nameof(FilterableOne.Filterable)));

            Assert.Throws<Exception>(() =>
            {
                generator.Add("Filterable", typeof(FilterableOne).GetProperty(nameof(FilterableOne.Filterable)));
            });
        }


    }
}

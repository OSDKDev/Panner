namespace Sort.Tests.Parsing.ByPropertyName
{
    using System;
    using Moq;
    using Panner;
    using Panner.Sort.Generators;
    using Xunit;

    public class SortByPropertyAliasTests : SortParticleGeneratorTests<ByPropertyAliasParticleGenerator<SortableOne>, ISortParticle<SortableOne>>
    {
        public SortByPropertyAliasTests() : base() { }
        protected override IPContext GetContext()
        {
            return new Mock<IPContext>(MockBehavior.Strict).Object;
        }
        protected override ByPropertyAliasParticleGenerator<SortableOne> GetGenerator()
        {
            var generator = new ByPropertyAliasParticleGenerator<SortableOne>();

            generator.Add("Sortable", typeof(SortableOne).GetProperty(nameof(SortableOne.Sortable)));

            generator.Add("NonSortable", typeof(SortableOne).GetProperty(nameof(SortableOne.NonSortable)));
            generator.Remove("NonSortable");
            return generator;
        }

        [Fact]
        public void AliasDuplicationThrows()
        {
            var generator = new ByPropertyAliasParticleGenerator<SortableOne>();
            generator.Add("Sortable", typeof(SortableOne).GetProperty(nameof(SortableOne.Sortable)));

            Assert.Throws<Exception>(() =>
            {
                generator.Add("Sortable", typeof(SortableOne).GetProperty(nameof(SortableOne.Sortable)));
            });
        }
    }
}

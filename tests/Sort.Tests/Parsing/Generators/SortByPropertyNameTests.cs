namespace Sort.Tests.Parsing.ByPropertyName
{
    using Moq;
    using Panner;
    using Panner.Sort.Generators;

    public class SortByPropertyNameTests : SortParticleGeneratorTests<ByPropertyNameParticleGenerator<SortableOne>, ISortParticle<SortableOne>>
    {
        public SortByPropertyNameTests() : base() { }

        protected override IPContext GetContext()
        {
            return new Mock<IPContext>(MockBehavior.Strict).Object;
        }
        protected override ByPropertyNameParticleGenerator<SortableOne> GetGenerator()
        {
            var generator = new ByPropertyNameParticleGenerator<SortableOne>();
            generator.Add(typeof(SortableOne).GetProperty(nameof(SortableOne.Sortable)));

            generator.Add(typeof(SortableOne).GetProperty(nameof(SortableOne.NonSortable)));
            generator.Remove(typeof(SortableOne).GetProperty(nameof(SortableOne.NonSortable)));
            return generator;
        }
    }
}

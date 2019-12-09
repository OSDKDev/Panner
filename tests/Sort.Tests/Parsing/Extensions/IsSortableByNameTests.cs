namespace Sort.Tests.Parsing.ByPropertyName
{
    using Panner;
    using Panner.Builders;
    using Panner.Sort.Generators;

    public class IsSortableByNameTests : GenericExtensionTests<ISortParticle<FakeEntity>, ByPropertyNameParticleGenerator<FakeEntity>>
    {
        public IsSortableByNameTests() : base(
            add: (x) => { x.IsSortableByName(); },
            remove: (x) => { x.IsNotSortableByName(); },
            retrieve: (x) => x.properties
        )
        { }
    }
}

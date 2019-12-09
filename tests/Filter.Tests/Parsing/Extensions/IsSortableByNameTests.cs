namespace Sort.Tests.Parsing.ByPropertyName
{
    using Panner;
    using Panner.Builders;
    using Panner.Filter.Generators;

    public class IsFilterableByNameTests : GenericExtensionTests<IFilterParticle<FakeEntity>, ByPropertyNameParticleGenerator<FakeEntity>>
    {
        public IsFilterableByNameTests() : base(
            add: (x) => { x.IsFilterableByName(); },
            remove: (x) => { x.IsNotFilterableByName(); },
            retrieve: (x) => x.properties
        )
        { }
    }
}

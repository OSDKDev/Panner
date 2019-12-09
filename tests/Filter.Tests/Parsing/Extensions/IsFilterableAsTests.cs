namespace Sort.Tests.Parsing.ByPropertyName
{
    using System.Linq;
    using Panner;
    using Panner.Builders;
    using Panner.Filter.Generators;

    public class IsFilterableAsTests : GenericExtensionTests<IFilterParticle<FakeEntity>, ByPropertyAliasParticleGenerator<FakeEntity>>
    {
        public IsFilterableAsTests() : base(
            add: (x) => { x.IsFilterableAs("FakeAlias"); },
            remove: (x) => { x.IsNotFilterableAs("FakeAlias"); },
            retrieve: (x) => { return x.properties.Values.ToList(); }
        )
        { }
    }
}

namespace Sort.Tests.Parsing.ByPropertyName
{
    using System.Linq;
    using Panner;
    using Panner.Builders;
    using Panner.Sort.Generators;

    public class IsSortableAsTests : GenericExtensionTests<ISortParticle<FakeEntity>, ByPropertyAliasParticleGenerator<FakeEntity>>
    {
        public IsSortableAsTests() : base(
            add: (x) => { x.IsSortableAs("FakeAlias"); },
            remove: (x) => { x.IsNotSortableAs("FakeAlias"); },
            retrieve: (x) => { return x.properties.Values.ToList(); }
        )
        { }
    }
}

namespace Filter.Tests.Parsing.ByPropertyName
{
    using System;
    using System.Collections.Generic;
    using Panner;
    using Xunit;

    public class FilterableOne
    {
        public int Filterable { get; set; }
        public int NonFilterable { get; set; }
        public int NeverFilterable { get; set; }
    }

    public abstract class FilterParticleGeneratorTests<TParticleGenerator, TParticle>
        where TParticle : IFilterParticle<FilterableOne>
        where TParticleGenerator : IFilterParticleGenerator<FilterableOne>
    {
        protected IPContext pContext { get; set; }
        protected TParticleGenerator particleGenerator { get; set; }

        public FilterParticleGeneratorTests()
        {
            this.pContext = GetContext();
            this.particleGenerator = GetGenerator();
        }

        protected abstract IPContext GetContext();
        protected abstract TParticleGenerator GetGenerator();

        public static IEnumerable<object[]> ValidInput =>
            new List<object[]>
            {
                new object[]{nameof(FilterableOne.Filterable) + "=1"},
                new object[]{nameof(FilterableOne.Filterable) + "=\"1\""},
            };

        public static IEnumerable<object[]> InvalidInput =>
            new List<object[]>
            {
                new object[]{string.Empty},
                new object[]{Guid.NewGuid().ToString()},
                new object[]{nameof(FilterableOne.Filterable) + "!<>1"},
                new object[]{nameof(FilterableOne.Filterable) + "=asd"},
                new object[]{nameof(FilterableOne.NonFilterable) + "=1"},
                new object[]{nameof(FilterableOne.NeverFilterable) + "=1"},
            };

        [Theory]
        [MemberData(nameof(InvalidInput))]
        public void OnErrorReturnsFalse(string input)
        {
            var result = this.particleGenerator.TryGenerate(this.pContext, input, out var _);
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(InvalidInput))]
        public void OnErrorOutputsNull(string input)
        {
            this.particleGenerator.TryGenerate(this.pContext, input, out var result);
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(ValidInput))]
        public void OnSuccessReturnsTrue(string input)
        {
            var result = this.particleGenerator.TryGenerate(this.pContext, input, out var _);
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(ValidInput))]
        public void OnSuccessOutputsParticle(string input)
        {
            this.particleGenerator.TryGenerate(this.pContext, input, out var result);
            Assert.NotNull(result);
        }
    }
}

namespace Sort.Tests.Parsing.ByPropertyName
{
    using System;
    using System.Collections.Generic;
    using Panner;
    using Xunit;

    public class SortableOne
    {
        public int Sortable { get; set; }
        public int NonSortable { get; set; }
        public int NeverSortable { get; set; }
    }

    public abstract class SortParticleGeneratorTests<TParticleGenerator, TParticle>
        where TParticle : ISortParticle<SortableOne>
        where TParticleGenerator : ISortParticleGenerator<SortableOne>
    {
        protected IPContext pContext { get; set; }
        protected TParticleGenerator particleGenerator { get; set; }

        public SortParticleGeneratorTests()
        {
            this.pContext = GetContext();
            this.particleGenerator = GetGenerator();
        }

        protected abstract IPContext GetContext();
        protected abstract TParticleGenerator GetGenerator();

        public static IEnumerable<object[]> ValidInput =>
            new List<object[]>
            {
                new object[]{nameof(SortableOne.Sortable)},
                new object[]{"-" + nameof(SortableOne.Sortable)},
            };

        public static IEnumerable<object[]> InvalidInput =>
            new List<object[]>
            {
                new object[]{string.Empty},
                new object[]{Guid.NewGuid().ToString()},
                new object[]{"-" + Guid.NewGuid().ToString()},
                new object[]{nameof(SortableOne.NonSortable)},
                new object[]{"-" + nameof(SortableOne.NonSortable)},
                new object[]{nameof(SortableOne.NeverSortable)},
                new object[]{"-" + nameof(SortableOne.NeverSortable) },
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

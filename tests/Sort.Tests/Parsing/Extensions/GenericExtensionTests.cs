namespace Sort.Tests.Parsing.ByPropertyName
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Moq;
    using Panner.Builders;
    using Panner.Interfaces;
    using Xunit;


    public class FakeEntity
    {
        public int FakeProp { get; set; }
    }

    public abstract class GenericExtensionTests<TParticle, TGenerator>
        where TGenerator : class, IParticleGenerator<TParticle>, new()
    {
        private Action<PPropertyBuilder<FakeEntity>> Add { get; set; }
        private Action<PPropertyBuilder<FakeEntity>> Remove { get; set; }
        private Func<TGenerator, List<PropertyInfo>> Retrieve { get; set; }

        public GenericExtensionTests(
            Action<PPropertyBuilder<FakeEntity>> add,
            Action<PPropertyBuilder<FakeEntity>> remove,
            Func<TGenerator, List<PropertyInfo>> retrieve
        )
        {
            this.Add = add;
            this.Remove = remove;
            this.Retrieve = retrieve;
        }

        public (Mock<IPEntityBuilder> entityBuilder, TGenerator generator, PPropertyBuilder<FakeEntity> propertyBuilder) GetMockEntityBuilder()

        {
            var entityBuilder = new Mock<IPEntityBuilder>(MockBehavior.Strict);
            var generator = new TGenerator();

            entityBuilder.Setup(x => x.GetOrCreateGenerator<TParticle, TGenerator>()).Returns(generator);

            var property = typeof(FakeEntity).GetProperty(nameof(FakeEntity.FakeProp));
            var propertyBuilder = new PPropertyBuilder<FakeEntity>(entityBuilder.Object, property);

            return (entityBuilder, generator, propertyBuilder);

        }

        [Fact]
        public void GetsOrCreatesGeneratorOnAdd()
        {
            var (entityBuilder, generator, propertyBuilder) = GetMockEntityBuilder();

            this.Add(propertyBuilder);

            entityBuilder.Verify(x => x.GetOrCreateGenerator<TParticle, TGenerator>(), Times.Once);
        }

        [Fact]
        public void GetsOrCeatedGeneratorOnRemove()
        {
            var (entityBuilder, generator, propertyBuilder) = GetMockEntityBuilder();

            this.Remove(propertyBuilder);

            entityBuilder.Verify(x => x.GetOrCreateGenerator<TParticle, TGenerator>(), Times.Once);
        }

        [Fact]
        public void AddedProperty()
        {
            var (entityBuilder, generator, propertyBuilder) = GetMockEntityBuilder();
            this.Add(propertyBuilder);

            Assert.Single(this.Retrieve(generator), propertyBuilder.PropertyInfo);
        }

        [Fact]
        public void RemovedProperty()
        {
            var (entityBuilder, generator, propertyBuilder) = GetMockEntityBuilder();
            this.Add(propertyBuilder);
            this.Remove(propertyBuilder);

            var collection = this.Retrieve(generator);

            Assert.DoesNotContain(propertyBuilder.PropertyInfo, collection);
        }
    }
}

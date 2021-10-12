namespace Sort.Tests.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Panner;
    using Xunit;

    public class IQueryableApplyTests
    {
        public IQueryable<IQueryableApplyTests> GetIQueryable()
        {
            return new List<IQueryableApplyTests>().AsQueryable();
        }

        public Mock<ISortParticle<IQueryableApplyTests>> GetParticle()
        {
            var p = new Mock<ISortParticle<IQueryableApplyTests>>(MockBehavior.Strict);
            return p;
        }

        [Fact]
        public void ThrowsOnNullParticles()
        {
            Assert.Throws<ArgumentNullException>("particles", () =>
            {
                IQueryableExtensions.Apply(
                    source: GetIQueryable(),
                    particles: (IEnumerable<ISortParticle<IQueryableApplyTests>>)null
                );
            });
        }


        [Fact]
        public void HandlesEmptyList()
        {
            var result = IQueryableExtensions.Apply(
                source: GetIQueryable(),
                particles: Array.Empty<ISortParticle<IQueryableApplyTests>>()
            );

            Assert.NotNull(result);
        }

        [Fact]
        public void ExecutesAll()
        {
            var queryable = GetIQueryable();
            var particles = new List<Mock<ISortParticle<IQueryableApplyTests>>>()
            {
                GetParticle(),
                GetParticle(),
                GetParticle(),
                GetParticle(),
            };

            // Create the MockSequence to validate the call order
            var sequence = new MockSequence();

            foreach (var p in particles)
            {
                p.InSequence(sequence)
                    .Setup(x => x.ApplyTo(It.IsAny<IOrderedQueryable<IQueryableApplyTests>>()))
                    .Returns<IOrderedQueryable<IQueryableApplyTests>>(x =>
                    {
                        return x;
                    });
            }

            var result = IQueryableExtensions.Apply(
                source: GetIQueryable(),
                particles: particles.Select(x => x.Object).AsEnumerable()
            );

            foreach (var p in particles)
            {
                p.Verify(x => x.ApplyTo(It.IsAny<IOrderedQueryable<IQueryableApplyTests>>()));
            }

        }
    }
}

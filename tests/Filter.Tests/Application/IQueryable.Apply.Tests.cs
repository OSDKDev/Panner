namespace Filter.Tests.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Moq;
    using Panner;
    using Xunit;

    public class IQueryableApplyTests
    {
        public IQueryable<IQueryableApplyTests> GetIQueryable()
        {
            return new List<IQueryableApplyTests>().AsQueryable();
        }

        public Mock<IFilterParticle<IQueryableApplyTests>> GetParticle()
        {
            var p = new Mock<IFilterParticle<IQueryableApplyTests>>(MockBehavior.Strict);
            return p;
        }

        [Fact]
        public void ThrowsOnNullParticles()
        {
            Assert.Throws<ArgumentNullException>("particles", () =>
            {
                IQueryableExtensions.Apply(
                    source: GetIQueryable(),
                    particles: (IEnumerable<IFilterParticle<IQueryableApplyTests>>)null
                );
            });
        }


        [Fact]
        public void HandlesEmptyList()
        {
            var result = IQueryableExtensions.Apply(
                source: GetIQueryable(),
                particles: Enumerable.Empty<IFilterParticle<IQueryableApplyTests>>()
            );

            Assert.NotNull(result);
        }

        [Fact]
        public void ExecutesAll()
        {
            var queryable = GetIQueryable();
            var particles = new List<Mock<IFilterParticle<IQueryableApplyTests>>>()
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
                    .Setup(x => x.GetExpression(It.IsAny<ParameterExpression>()))
                    .Returns<Expression>(x =>
                    {
                        return Expression.Constant(true);
                    });
            }

            var result = IQueryableExtensions.Apply(
                source: GetIQueryable(),
                particles: particles.Select(x => x.Object).AsEnumerable()
            );

            foreach (var p in particles)
            {
                p.Verify(x => x.GetExpression(It.IsAny<ParameterExpression>()));
            }

        }
    }
}

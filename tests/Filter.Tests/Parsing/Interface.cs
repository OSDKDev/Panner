namespace Filter.Tests.Parsing
{
    using System.Collections.Generic;
    using Panner;
    using Panner.Builders;
    using Xunit;

    public class Interface
    {
        public int Id { get; set; }

        private IPContext pContext { get; set; }

        public Interface()
        {
            var x = new PContextBuilder();

            x.Entity<Interface>()
                .Property(p => p.Id)
                .IsFilterableByName();

            pContext = x.Build();
        }

        [Fact]
        public void TryParse()
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var r1 = this.pContext.TryParseCsv("Id=1||Id=1", out IEnumerable<IFilterParticle<Interface>> particles1);
            var r2 = this.pContext.TryParseCsv("(Id=1,Id=2)||Id=3,Id=4", out IEnumerable<IFilterParticle<Interface>> particles2);
        }
    }
}

namespace Filter.Tests.Parsing
{
    using System.Collections.Generic;
    using Panner;
    using Panner.Builders;

    public class Interface
    {
        private IPContext pContext { get; set; }

        public Interface()
        {
            var x = new PContextBuilder();

            x.Entity<Interface>()
                .Property(p => p.pContext);

            pContext = x.Build();
        }

        public void TryParse()
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            this.pContext.TryParseCsv("input", out IEnumerable<IFilterParticle<Interface>> particles);
        }
    }
}

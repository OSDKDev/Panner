namespace Panner
{
    using System.Collections.Generic;
    using System.Linq;

    public static class PContextSortExtensions
    {
        public static bool TryParseCsv<T>(
            this IPContext pContext,
            string input,
            out IEnumerable<ISortParticle<T>> particles
        )
                where T : class
        {
            var particlesRaw = new List<ISortParticle<T>>();
            var generators = pContext.GetGenerators<T, ISortParticle<T>>();

            var splitInput = input
                .Split(',');

            bool wasAbleToParse = true;

            foreach (var sort in splitInput)
            {
                ISortParticle<T> particle = null;

                wasAbleToParse &= generators.Any(x => x.TryGenerate(pContext, sort, out particle));

                if (!(particle is null))
                    particlesRaw.Add(particle);
            }

            particles = wasAbleToParse ?
                particlesRaw :
                Enumerable.Empty<ISortParticle<T>>();

            return wasAbleToParse;
        }
    }
}

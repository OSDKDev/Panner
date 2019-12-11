namespace Panner
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Panner.Filter.Generators;
    using Panner.Interfaces;

    public static class PContextFilterExtensions
    {
        public static bool TryParseCsv<T>(
            this IPContext pContext,
            string input,
            out IEnumerable<IFilterParticle<T>> particles
        )
            where T : class
        {
            var particlesRaw = new List<IFilterParticle<T>>();

            var generators = new List<IParticleGenerator<IFilterParticle<T>>>()
            {
                // Default generators
                new ComplexLogicGenerator<T>()
            };

            generators.AddRange(pContext.GetGenerators<T, IFilterParticle<T>>());

            // Matching commas outside of quotes and parentheses
            Regex regx = new Regex("[,](?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))(?=(((?!\\)).)*\\()|[^\\(\\)]*$)");
            var splitInput = regx.Split(input);

            bool wasAbleToParse = true;

            foreach (var filter in splitInput)
            {
                IFilterParticle<T> particle = null;

                wasAbleToParse &= generators.Any(x => x.TryGenerate(pContext, filter, out particle));

                if (!(particle is null))
                    particlesRaw.Add(particle);
            }

            particles = wasAbleToParse ?
                particlesRaw :
                Enumerable.Empty<IFilterParticle<T>>();

            return wasAbleToParse;
        }
    }
}

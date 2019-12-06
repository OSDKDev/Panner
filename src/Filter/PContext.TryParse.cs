namespace Panner
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

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
            var generators = pContext.GetGenerators<T, IFilterParticle<T>>();

            Regex regx = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var splitInput = regx.Split(input);

            bool wasAbleToParse = true;

            foreach (var sort in splitInput)
            {
                IFilterParticle<T> particle = null;

                wasAbleToParse &= generators.Any(x => x.TryGenerate(pContext, sort, out particle));

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

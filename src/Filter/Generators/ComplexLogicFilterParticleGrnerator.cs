namespace Panner.Filter.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Panner.Filter.Particles;

    public class ComplexLogicGenerator<TEntity> : IFilterParticleGenerator<TEntity>
        where TEntity : class
    {
        internal readonly Type type;

        public ComplexLogicGenerator()
        {
            this.type = typeof(TEntity);
        }

        public bool TryGenerate(IPContext context, string input, out IFilterParticle<TEntity> particle)
        {
            input = input.Trim();

            // Attempt to split the input by all "||" not enclosed in quotes or parenthesis.
            Regex regex = new Regex("[||](?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))(?=(((?!\\)).)*\\()|[^\\(\\)]*$)");
            var splitInput = regex
                .Split(input)
                .Where(x => !string.IsNullOrWhiteSpace(x));

            // If we dont end up with at least two parts, there's no OR operation here.
            if (splitInput.Count() < 2)
            {
                particle = null;
                return false;
            }

            var trimmedInputs = splitInput.Select(x => x.Trim());

            // We parse each split input and OR it to the previous one.
            IFilterParticle<TEntity> resultParticle = null;
            foreach (var filter in trimmedInputs)
            {
                var cleanFilter = filter;

                if (filter.StartsWith("(") && filter.EndsWith(")"))
                {
                    cleanFilter = filter.Substring(1, filter.Length - 2);
                }

                if (!context.TryParseCsv(cleanFilter, out IEnumerable<IFilterParticle<TEntity>> particles))
                {
                    particle = null;
                    return false;
                }

                var partialResult = particles.Count() > 1 ?
                    new AndFilterParticle<TEntity>(particles.ToArray()) :
                    particles.Single();

                resultParticle = resultParticle is null ?
                    partialResult :
                    new OrFilterParticle<TEntity>(resultParticle, partialResult);
            }

            particle = resultParticle;
            return true;
        }
    }
}

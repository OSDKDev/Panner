namespace Panner.Sort.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Panner.Sort.Particles;

    public class ByPropertyAliasParticleGenerator<TEntity> : ISortParticleGenerator<TEntity>
        where TEntity : class
    {
        private readonly Dictionary<string, PropertyInfo> properties;

        public ByPropertyAliasParticleGenerator()
        {
            this.properties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
        }

        public void Add(string alias, PropertyInfo property)
        {
            if (this.properties.ContainsKey(alias))
            {
                throw new Exception("Duplicated alias in entity");
            }

            this.properties.Add(alias, property);
        }

        public void Remove(string alias)
        {
            this.properties.Remove(alias);
        }

        public bool TryGenerate(IPContext context, string input, out ISortParticle<TEntity> particle)
        {
            input = input.Trim();

            bool isDescending = false;
            string name;

            if (input.StartsWith("-"))
            {
                isDescending = true;
                name = input.Substring(1).TrimStart();
            }
            else
            {
                name = input;
            }

            if (!TryGetPropertyByAlias(name, out PropertyInfo property))
            {
                particle = null;
                return false;
            }

            particle = new ByPropertyParticle<TEntity>(
                property: property,
                descending: isDescending
            );

            return true;
        }

        private bool TryGetPropertyByAlias(
            string alias,
            out PropertyInfo property
        )
        {
            return this.properties.TryGetValue(alias, out property);

        }
    }
}

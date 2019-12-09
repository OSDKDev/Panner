namespace Panner.Sort.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Panner.Sort.Particles;

    public class ByPropertyNameParticleGenerator<TEntity> : ISortParticleGenerator<TEntity>
        where TEntity : class
    {
        internal readonly Type type;
        internal readonly List<PropertyInfo> properties;

        public ByPropertyNameParticleGenerator()
        {
            this.type = typeof(TEntity);
            this.properties = new List<PropertyInfo>();
        }

        public void Add(PropertyInfo property)
        {
            if (!this.properties.Contains(property))
                this.properties.Add(property);
        }

        public void Remove(PropertyInfo property)
        {
            this.properties.Remove(property);
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

            if (!TryGetPropertyByName(name, out var property))
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

        private bool TryGetPropertyByName(
            string name,
            out PropertyInfo property
        )
        {
            var flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var originalProperty = this.type.GetProperty(name, flags);

            property = this.properties.Contains(originalProperty) ?
                originalProperty :
                null;

            return !(property is null);
        }
    }
}

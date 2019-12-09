namespace Panner.Filter.Generators
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Panner.Filter.Particles;

    public class ByPropertyNameParticleGenerator<TEntity> : IFilterParticleGenerator<TEntity>
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

        public bool TryGenerate(IPContext context, string input, out IFilterParticle<TEntity> particle)
        {
            input = input.Trim();

            // Attempt to capture field name from filter string.
            var nameMatch = Regex.Match(input, "^([A-Za-z0-9_]*)");

            if (!nameMatch.Success || nameMatch.Length == 0)
            {
                particle = null;
                return false;
            }

            // Validate the property by name
            if (!TryGetPropertyByName(nameMatch.Value, out PropertyInfo property))
            {
                particle = null;
                return false;
            }

            // Remove the name out of the filter string.
            var remaining = input.Substring(nameMatch.Length).TrimStart();

            // Attempt to capture operator from filter string.
            // Keys are ordered by descending length to always match the longest one possible first. (eg, "!=" instead "=")
            var operatorKey = SymbolMapper.Symbols.FirstOrDefault(o => remaining.StartsWith(o));

            if (operatorKey is null)
            {
                particle = null;
                return false;
            }

            var op = SymbolMapper.Map(operatorKey);

            // Remove the operator out of the remaining filter string.
            var value = remaining.Substring(operatorKey.Length).Trim();

            // If encased in double quotes, we remove them.
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
            }

            try
            {
                // Attempt to convert/cast the value to the type of the property.
                var converter = TypeDescriptor.GetConverter(property.PropertyType);

                dynamic typedValue = converter.CanConvertFrom(value.GetType())
                    ? converter.ConvertFrom(value)
                    : Convert.ChangeType(value, property.PropertyType);

                // We generate our particle
                Type[] typeArgs = { typeof(TEntity), property.PropertyType };
                Type byPropertyParticleType = typeof(ByPropertyParticle<,>).MakeGenericType(typeArgs);
                particle = (IFilterParticle<TEntity>)Activator.CreateInstance(byPropertyParticleType, property, op, typedValue);

                return true;
            }
            catch
            {
                particle = null;
                return false;
            }
        }

        private bool TryGetPropertyByName(
            string name,
            out PropertyInfo property
        )
        {
            var flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo originalProperty = this.type.GetProperty(name, flags);

            property = this.properties.Contains(originalProperty) ?
                originalProperty :
                null;

            return !(property is null);
        }
    }
}

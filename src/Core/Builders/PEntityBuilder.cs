namespace Panner.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Panner.Interfaces;

    public class PEntityBuilder<T> : IPEntityBuilder
    {
        readonly PEntity _PEntity;
        readonly Dictionary<PropertyInfo, PPropertyBuilder<T>> _Builders;
        readonly Type _Type;

        internal PEntityBuilder(Type type)
        {
            this._PEntity = new PEntity();
            this._Builders = new Dictionary<PropertyInfo, PPropertyBuilder<T>>();
            this._Type = type;
        }

        public PEntityBuilder<T> Property<U>(Expression<Func<T, U>> expression, Action<PPropertyBuilder<T>> action)
        {
            var builder = this.Property(expression);
            action.Invoke(builder);
            return this;
        }

        public TParticleGenerator GetOrCreateGenerator<TParticle, TParticleGenerator>()
            where TParticleGenerator : class, IParticleGenerator<TParticle>, new()
        {
            TParticleGenerator result;

            if (!_PEntity._ParticleGenerators.TryGetValue(typeof(TParticle), out List<object> specificParticleGenerators))
            {
                specificParticleGenerators = new List<object>();
                this._PEntity._ParticleGenerators.Add(typeof(TParticle), specificParticleGenerators);
            }

            var uncastedParticleGenerator = specificParticleGenerators.SingleOrDefault(x => x is TParticleGenerator);

            if (uncastedParticleGenerator is null)
            {
                result = new TParticleGenerator();
                specificParticleGenerators.Add(result);
            }
            else
            {
                result = (TParticleGenerator)uncastedParticleGenerator;
            }

            return result;
        }

        public PPropertyBuilder<T> Property<U>(Expression<Func<T, U>> expression)
        {
            if (!(expression.Body is MemberExpression member))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    expression.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    expression.ToString()));

            if (this._Type != propInfo.ReflectedType &&
                !this._Type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    expression.ToString(),
                    this._Type));

            if (!_Builders.TryGetValue(propInfo, out PPropertyBuilder<T> builder))
            {
                builder = new PPropertyBuilder<T>(this, propInfo);
                _Builders.Add(propInfo, builder);
            }

            return builder;
        }

        public PEntity Build()
        {
            return _PEntity;
        }
    }
}

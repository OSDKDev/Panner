namespace Panner.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class PContextBuilder
    {
        readonly Dictionary<Type, IPEntityBuilder> _Builders;

        public PContextBuilder()
        {
            this._Builders = new Dictionary<Type, IPEntityBuilder>();
        }

        public PContextBuilder Entity<T>(Action<PEntityBuilder<T>> action)
        {
            var builder = this.Entity<T>();
            action.Invoke(builder);
            return this;
        }

        public PEntityBuilder<T> Entity<T>()
        {
            var type = typeof(T);

            if (!this.TryGet(out PEntityBuilder<T> builder))
            {
                builder = new PEntityBuilder<T>(type);
                _Builders.Add(type, builder);
            }

            return builder;
        }

        public bool TryGet<T>(out PEntityBuilder<T> value)
        {
            if (_Builders.TryGetValue(typeof(T), out var result) && result is PEntityBuilder<T>)
            {
                value = (PEntityBuilder<T>)result;
                return true;
            }

            value = null;
            return false;
        }

        public PContext Build()
        {
            var entities = _Builders
                .ToDictionary(kvp => kvp.Key, kvp => (IPEntity)kvp.Value.Build());

            return new PContext(new ReadOnlyDictionary<Type, IPEntity>(entities));
        }
    }
}

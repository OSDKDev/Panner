namespace Panner.Builders
{
    using System.Reflection;

    public class PPropertyBuilder<T>
    {
        readonly PropertyInfo _PropertyInfo;
        readonly PEntityBuilder<T> _EntityBuilder;

        public PPropertyBuilder(PEntityBuilder<T> entityBuilder, PropertyInfo propertyInfo)
        {
            this._PropertyInfo = propertyInfo;
            this._EntityBuilder = entityBuilder;
        }

        public PEntityBuilder<T> Entity => this._EntityBuilder;
        public PropertyInfo PropertyInfo => this._PropertyInfo;
    }
}

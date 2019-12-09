namespace Panner.Builders
{
    using System.Reflection;

    public class PPropertyBuilder<T>
    {
        readonly PropertyInfo _PropertyInfo;
        readonly IPEntityBuilder _EntityBuilder;

        public PPropertyBuilder(IPEntityBuilder entityBuilder, PropertyInfo propertyInfo)
        {
            this._PropertyInfo = propertyInfo;
            this._EntityBuilder = entityBuilder;
        }

        public IPEntityBuilder Entity => this._EntityBuilder;
        public PropertyInfo PropertyInfo => this._PropertyInfo;
    }
}

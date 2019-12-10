namespace Filter.Tests.Construction
{
    using Panner.Builders;

    public class Interface
    {
        private PEntityBuilder<Interface> pEntityBuilder { get; set; }
        private PPropertyBuilder<Interface> pPropertyBuilder { get; set; }

        public Interface()
        {
            this.pEntityBuilder = new PContextBuilder()
                .Entity<Interface>();

            this.pPropertyBuilder = this.pEntityBuilder
                .Property(p => p.pPropertyBuilder);
        }

        public void AllAreFilterable()
        {
            this.pEntityBuilder
                .AllPropertiesAreFilterableByName();
        }


        public void ByPropertyName()
        {
            this.pPropertyBuilder
                .IsFilterableByName()
                .IsNotFilterableByName()
                .IsFilterableByName()
                .IsNotFilterableByName();
        }


        public void ByPropertyAlias()
        {
            this.pPropertyBuilder
                .IsFilterableAs("Id")
                .IsNotFilterableAs("Id")
                .IsFilterableAs("Id")
                .IsNotFilterableAs("Id");
        }
    }
}

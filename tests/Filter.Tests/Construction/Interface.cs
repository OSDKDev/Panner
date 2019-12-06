namespace Filter.Tests.Construction
{
    using Panner.Builders;

    public class Interface
    {
        private PPropertyBuilder<Interface> pPropertyBuilder { get; set; }

        public Interface()
        {
            this.pPropertyBuilder = new PContextBuilder()
                .Entity<Interface>()
                .Property(p => p.pPropertyBuilder);
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

namespace Sort.Tests.Construction
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
                .IsSortableByName()
                .IsNotSortableByName()
                .IsSortableByName()
                .IsNotSortableByName();
        }

        public void ByPropertyAlias()
        {
            this.pPropertyBuilder
                .IsSortableAs("Id")
                .IsNotSortableAs("Id")
                .IsSortableAs("Id")
                .IsNotSortableAs("Id");
        }
    }
}

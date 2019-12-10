namespace Sort.Tests.Construction
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

        public void AllAreSortable()
        {
            this.pEntityBuilder
                .AllPropertiesAreSortableByName();
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

namespace Filter.Tests.Construction
{
    using Panner.Builders;

    public class Interface
    {
        public class FakePost
        {
            public FakeAuthor Author { get; set; }
        }

        public class FakeAuthor
        {
            public int Id { get; set; }
        }
        private PEntityBuilder<FakePost> pEntityBuilder { get; set; }
        private PPropertyBuilder<FakePost> pPropertyBuilder { get; set; }

        public Interface()
        {
            this.pEntityBuilder = new PContextBuilder()
                .Entity<FakePost>();

            this.pPropertyBuilder = this.pEntityBuilder
                .Property(p => p.Author.Id);
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

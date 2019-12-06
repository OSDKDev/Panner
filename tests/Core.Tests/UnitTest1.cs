namespace Core.Tests
{
    using Panner.Builders;

    public class FakeEntity
    {
        public int Id { get; set; }
    }

    public class UnitTest1
    {
        public void Test1()
        {
            var contextBuilder = new PContextBuilder();

            contextBuilder.Entity<FakeEntity>(entity =>
            {
            });

            var context = contextBuilder.Build();
        }
    }
}

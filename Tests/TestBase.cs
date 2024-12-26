namespace InnoShop.Tests
{
    // Test environment definition.
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DBFixture> { }

    /// <summary>
    /// Base class for tests.
    /// All test classes derived from this base class will share the same test environment defined in <see cref="DBFixture"/>.
    /// </summary>
    [Collection("Database collection")]
    public class TestBase
    {
        protected readonly DBFixture _fixture;

        public TestBase(DBFixture fixture)
        {
            _fixture = fixture;
        }
    }
}

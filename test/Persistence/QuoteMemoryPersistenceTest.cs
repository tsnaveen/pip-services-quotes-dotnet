using Xunit;

using PipServices.Commons.Data;
using PipServices.Quotes.Persistence;

namespace PipServices.Quotes.Test.Persistence
{

    public class QuoteMemoryPersistenceTest : AbstractTest
    {
        private TestModel Model { get; set; }

        protected override void Initialize()
        {
            Model = new TestModel();
        }

        protected override void Uninitialize()
        {
        }

        [Fact]
        public void It_Should_Clear_Async()
        {
            var quotesMemoryPersistence = new QuotesMemoryPersistence();

            quotesMemoryPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote1).Wait();

            Assert.Equal(1, quotesMemoryPersistence.ItemsCount);

            quotesMemoryPersistence.ClearAsync(Model.CorrelationId).Wait();

            Assert.Equal(0, quotesMemoryPersistence.ItemsCount);
        }

        [Fact]
        public void It_Should_Create_Async()
        {
            var quotesMemoryPersistence = new QuotesMemoryPersistence();

            quotesMemoryPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote1).Wait();

            Assert.Equal(1, quotesMemoryPersistence.ItemsCount);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Search_Filter()
        {
            var quotesMemoryPersistence = new QuotesMemoryPersistence();

            var filter = new FilterParams
            {
                { "search", "test" }
            };

            CreateTestQuotes(quotesMemoryPersistence);

            var result = quotesMemoryPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(quotesMemoryPersistence.ItemsCount, result.Length);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Author_Filter()
        {
            var quotesMemoryPersistence = new QuotesMemoryPersistence();
            var filter = new FilterParams
            {
                { "author", "Author Strange" }
            };

            CreateTestQuotes(quotesMemoryPersistence);

            var result = quotesMemoryPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(1, result.Length);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Status_Filter()
        {
            var quotesMemoryPersistence = new QuotesMemoryPersistence();
            var filter = new FilterParams
            {
                { "status", "new" }
            };

            CreateTestQuotes(quotesMemoryPersistence);

            var result = quotesMemoryPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(1, result.Length);
        }

        private void CreateTestQuotes(IQuotesPersistence quotesPersistence)
        {
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote1).Wait();
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote2).Wait();
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote3).Wait();
        }
    }
}

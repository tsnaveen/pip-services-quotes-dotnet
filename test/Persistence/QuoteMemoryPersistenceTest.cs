using Xunit;

using PipServices.Commons.Data;
using PipServices.Quotes.Data.Version1;

namespace PipServices.Quotes.Persistence
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

            Assert.Equal(quotesMemoryPersistence.ItemsCount, result.Data.Count);
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

            Assert.Equal(1, result.Data.Count);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Status_Filter()
        {
            var quotesMemoryPersistence = new QuotesMemoryPersistence();
            var filter = new FilterParams
            {
                { "status", QuoteStatusV1.New }
            };

            CreateTestQuotes(quotesMemoryPersistence);

            var result = quotesMemoryPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Search_Filter_For_Another_Language()
        {
            var quotesMemoryPersistence = new QuotesMemoryPersistence();

            var filter = new FilterParams
            {
                { "search", "citar" }
            };

            CreateTestQuotes(quotesMemoryPersistence);

            var result = quotesMemoryPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(1, result.Data.Count);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Null_Search_Filter()
        {
            var quotesMemoryPersistence = new QuotesMemoryPersistence();

            var filter = new FilterParams
            {
                { "search", string.Empty }
            };

            CreateTestQuotes(quotesMemoryPersistence);

            var result = quotesMemoryPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(4, result.Data.Count);
        }

        private void CreateTestQuotes(IQuotesPersistence quotesPersistence)
        {
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote1).Wait();
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote2).Wait();
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote3).Wait();
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote4).Wait();
        }
    }
}

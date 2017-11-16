using Xunit;

using PipServices.Commons.Data;
using PipServices.Quotes.Data.Version1;
using PipServices.Commons.Config;
using System;

namespace PipServices.Quotes.Persistence
{
    public class QuotesMongoDbPersistenceTest : AbstractTest
    {
        private TestModel Model { get; set; }
        private QuotesMongoDbPersistence quotesPersistence;

        public QuotesMongoDbPersistenceTest()
        {
        }

        protected override void Initialize()
        {
            Model = new TestModel();

            //var config = YamlConfigReader.ReadConfig(null, "./config/test_connections.yaml", null);
            //var dbConfig = config.GetSection("mongodb");

            var mongoUri = Environment.GetEnvironmentVariable("MONGO_URI");
            var mongoHost = Environment.GetEnvironmentVariable("MONGO_HOST") ?? "localhost";
            var mongoPort = Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27017";
            var mongoDatabase = Environment.GetEnvironmentVariable("MONGO_DB") ?? "test";

            if (mongoUri == null && mongoHost == null)
                return;

            var dbConfig = ConfigParams.FromTuples(
                "connection.uri", mongoUri,
                "connection.host", mongoHost,
                "connection.port", mongoPort,
                "connection.database", mongoDatabase
            );

            quotesPersistence = new QuotesMongoDbPersistence();
            quotesPersistence.Configure(dbConfig);

            quotesPersistence.OpenAsync(null).Wait();
            quotesPersistence.ClearAsync(null).Wait();
        }

        protected override void Uninitialize()
        {
        }

        [Fact]
        public void It_Should_Clear_Async()
        {
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote1).Wait();
            var quote = quotesPersistence.GetOneByIdAsync(Model.CorrelationId, Model.SampleQuote1.Id).Result;
            Assert.Equal(Model.SampleQuote1, quote);

            quotesPersistence.ClearAsync(Model.CorrelationId).Wait();
            quote = quotesPersistence.GetOneByIdAsync(Model.CorrelationId, Model.SampleQuote1.Id).Result;
            Assert.Null(quote);
        }

        [Fact]
        public void It_Should_Create_Async()
        {
            quotesPersistence.CreateAsync(Model.CorrelationId, Model.SampleQuote1).Wait();
            var quote = quotesPersistence.GetOneByIdAsync(Model.CorrelationId, Model.SampleQuote1.Id).Result;
            Assert.Equal(Model.SampleQuote1, quote);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Search_Filter()
        {
            var filter = new FilterParams
            {
                { "search", "test" }
            };

            CreateTestQuotes(quotesPersistence);

            var result = quotesPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(4, result.Data.Count);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Author_Filter()
        {
            var filter = new FilterParams
            {
                { "author", "Author Strange" }
            };

            CreateTestQuotes(quotesPersistence);

            var result = quotesPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(1, result.Data.Count);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Status_Filter()
        {
            var filter = new FilterParams
            {
                { "status", QuoteStatusV1.New }
            };

            CreateTestQuotes(quotesPersistence);

            var result = quotesPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Search_Filter_For_Another_Language()
        {
            var filter = new FilterParams
            {
                { "search", "citar" }
            };

            CreateTestQuotes(quotesPersistence);

            var result = quotesPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

            Assert.Equal(1, result.Data.Count);
        }

        [Fact]
        public void It_Should_Get_Page_Async_By_Null_Search_Filter()
        {
            var filter = new FilterParams
            {
                { "search", string.Empty }
            };

            CreateTestQuotes(quotesPersistence);

            var result = quotesPersistence.GetPageByFilterAsync(Model.CorrelationId, filter, null).Result;

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

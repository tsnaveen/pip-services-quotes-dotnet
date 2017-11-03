using Moq;

using PipServices.Commons.Config;
using PipServices.Commons.Convert;
using PipServices.Commons.Refer;
using PipServices.Quotes.Logic;
using PipServices.Quotes.Persistence;

using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace PipServices.Quotes.Services.Version1
{
    public class QuotesHttpServiceV1Test : AbstractTest
    {
        private QuotesHttpServiceV1 _service;

        private IQuotesController _quotesController;
        private IQuotesPersistence _quotesPersistence;

        private Mock<IQuotesController> _moqQuotesController;
        private Mock<IQuotesPersistence> _moqQuotesPersistence;

        ConfigParams _restConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", 3001 // another port for this test!
        );

        private TestModel Model { get; set; }

        protected override void Initialize()
        {
            Model = new TestModel();

            _moqQuotesController = new Mock<IQuotesController>();
            _quotesController = _moqQuotesController.Object;

            _moqQuotesController.Setup(c => c.GetCommandSet()).Returns(new QuotesCommandSet(_quotesController));

            _moqQuotesPersistence = new Mock<IQuotesPersistence>();
            _quotesPersistence = _moqQuotesPersistence.Object;

            _service = new QuotesHttpServiceV1();
            _service.Configure(_restConfig);

            var references = References.FromTuples(
                new Descriptor("pip-services-quotes", "persistence", "memory", "default", "1.0"), _quotesPersistence,
                new Descriptor("pip-services-quotes", "controller", "default", "default", "1.0"), _quotesController,
                new Descriptor("pip-services-quotes", "service", "http", "default", "1.0"), _service
            );
            _service.SetReferences(references);

            Task.Run(() => _service.OpenAsync(Model.CorrelationId));
            Thread.Sleep(1000); // Just let service a sec to be initialized
        }

        protected override void Uninitialize()
        {
            _service.CloseAsync(null);
        }

        [Fact] // Just ONE test to avoid issues with re-opening service on the same host
        public void It_Should_Perform_CRUD_Operations()
        {
            It_Should_Be_Opened();

            It_Should_Create_Quote_Async();

            It_Should_Update_Quote_Async();

            It_Should_Delete_Quote_Async();

            It_Should_Get_Quote_Async();

            It_Should_Get_Random_Quote_Async();

            It_Should_Get_Quotes_Async();
        }

        public void It_Should_Be_Opened()
        {
            Assert.True(_service.IsOpened());
        }

        public void It_Should_Create_Quote_Async()
        {
            var createCalled = false;
            _moqQuotesController.Setup(c => c.CreateQuoteAsync(Model.CorrelationId, Model.SampleQuote1)).Callback(() => createCalled = true);

            SendPostRequest("create_quote", new
            {
                correlation_id = Model.CorrelationId,
                quote = Model.SampleQuote1
            });

            Assert.True(createCalled);
        }

        public void It_Should_Update_Quote_Async()
        {
            var updateCalled = false;
            _moqQuotesController.Setup(c => c.UpdateQuoteAsync(Model.CorrelationId, Model.SampleQuote1)).Callback(() => updateCalled = true);

            SendPostRequest("update_quote", new
            {
                correlation_id = Model.CorrelationId,
                quote = Model.SampleQuote1
            });

            Assert.True(updateCalled);
        }

        public void It_Should_Delete_Quote_Async()
        {
            var deleteCalled = false;
            _moqQuotesController.Setup(c => c.DeleteQuoteByIdAsync(Model.CorrelationId, Model.SampleQuote1.Id)).Callback(() => deleteCalled = true);

            SendPostRequest("delete_quote_by_id", new
            {
                correlation_id = Model.CorrelationId,
                quote_id = Model.SampleQuote1.Id
            });

            Assert.True(deleteCalled);
        }

        public void It_Should_Get_Quote_Async()
        {
            var getCalled = false;
            _moqQuotesController.Setup(c => c.GetQuoteByIdAsync(Model.CorrelationId, Model.SampleQuote1.Id)).Callback(() => getCalled = true);

            SendPostRequest("get_quote_by_id", new
            {
                correlation_id = Model.CorrelationId,
                quote_id = Model.SampleQuote1.Id
            });

            Assert.True(getCalled);
        }

        public void It_Should_Get_Random_Quote_Async()
        {
            var getCalled = false;
            _moqQuotesController.Setup(c => c.GetRandomQuoteAsync(Model.CorrelationId, Model.FilterParams)).Callback(() => getCalled = true);

            SendPostRequest("get_random_quote", new
            {
                correlation_id = Model.CorrelationId,
                filter = Model.FilterParams
            });

            Assert.True(getCalled);
        }

        public void It_Should_Get_Quotes_Async()
        {
            var getCalled = false;
            _moqQuotesController.Setup(c => c.GetQuotesAsync(Model.CorrelationId, Model.FilterParams, Model.PagingParams)).Callback(() => getCalled = true);

            SendPostRequest("get_quotes", new
            {
                correlation_id = Model.CorrelationId,
                filter = Model.FilterParams,
                paging = Model.PagingParams
            });

            Assert.True(getCalled);
        }

        private static string SendPostRequest(string route, dynamic request)
        {
            using (var httpClient = new HttpClient())
            {
                using (var content = new StringContent(JsonConverter.ToJson(request), Encoding.UTF8, "application/json"))
                {
                    var response = httpClient.PostAsync("http://localhost:3001/quotes/" + route, content).Result;

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }

}

using Moq;

using PipServices.Commons.Data;
using PipServices.Commons.Refer;
using PipServices.Quotes.Data.Version1;
using PipServices.Quotes.Persistence;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace PipServices.Quotes.Logic
{
    public class QuoteControllerTest : AbstractTest
    {
        private QuotesController _quotesController;

        private IQuotesPersistence _quotesPersistence;
        private Mock<IQuotesPersistence> _moqQuotesPersistence;

        private TestModel Model { get; set; }

        protected override void Initialize()
        {
            Model = new TestModel();

            var references = new References();
            _quotesController = new QuotesController();

            _moqQuotesPersistence = new Mock<IQuotesPersistence>();
            _quotesPersistence = _moqQuotesPersistence.Object;

            references.Put(new Descriptor("pip-services-quotes", "persistence", "memory", "default", "1.0"), _quotesPersistence);
            references.Put(new Descriptor("pip-services-quotes", "controller", "default", "default", "1.0"), _quotesController);

            _quotesController.SetReferences(references);
        }

        protected override void Uninitialize()
        {
        }

        [Fact]
        public void It_Should_Create_Quote_Async()
        {
            var createCalled = false;
            _moqQuotesPersistence.Setup(p => p.CreateAsync(Model.CorrelationId, Model.SampleQuote1)).Callback(() => createCalled = true);

            _quotesController.CreateQuoteAsync(Model.CorrelationId, Model.SampleQuote1);

            Assert.True(createCalled);
        }

        [Fact]
        public void It_Should_Update_Quote_Async()
        {
            var updateCalled = false;
            _moqQuotesPersistence.Setup(p => p.UpdateAsync(Model.CorrelationId, Model.SampleQuote1)).Callback(() => updateCalled = true);

            _quotesController.UpdateQuoteAsync(Model.CorrelationId, Model.SampleQuote1);

            Assert.True(updateCalled);
        }

        [Fact]
        public void It_Should_Delete_Quote_Async()
        {
            var deleteCalled = false;
            _moqQuotesPersistence.Setup(p => p.DeleteByIdAsync(Model.CorrelationId, Model.SampleQuote1.Id)).Callback(() => deleteCalled = true);

            _quotesController.DeleteQuoteByIdAsync(Model.CorrelationId, Model.SampleQuote1.Id);

            Assert.True(deleteCalled);
        }

        [Fact]
        public void It_Should_Get_Quotes_Async()
        {
            var initialDataPage = new DataPage<QuoteV1>()
            {
                Data = new List<QuoteV1>() { Model.SampleQuote1, Model.SampleQuote2 },
                Total = 2
            };

            _moqQuotesPersistence.Setup(p => p.GetPageByFilterAsync(Model.CorrelationId, null, null)).Returns(Task.FromResult(initialDataPage));

            var resultDataPage = _quotesController.GetQuotesAsync(Model.CorrelationId, null, null).Result;
            Assert.Equal(initialDataPage.Data.Count, resultDataPage.Data.Count);
            Assert.Equal(initialDataPage.Total, resultDataPage.Total);
        }

        [Fact]
        public void It_Should_Get_One_Quote_Async()
        {
            var id = Model.SampleQuote2.Id;
            _moqQuotesPersistence.Setup(p => p.GetOneByIdAsync(Model.CorrelationId, id)).Returns(Task.FromResult(Model.SampleQuote2));

            var resultQuote = _quotesController.GetQuoteByIdAsync(Model.CorrelationId, id).Result;
            Assert.Equal(Model.SampleQuote2, resultQuote);
        }

        [Fact]
        public void It_Should_Get_One_Random_Quote_Async()
        {
            _moqQuotesPersistence.Setup(p => p.GetOneRandomAsync(Model.CorrelationId, null)).Returns(Task.FromResult(Model.SampleQuote3));

            var resultQuote = _quotesController.GetRandomQuoteAsync(Model.CorrelationId, null).Result;
            Assert.Equal(Model.SampleQuote3, resultQuote);
        }
    }
}

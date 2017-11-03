using System.Threading.Tasks;

using PipServices.Commons.Commands;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;
using PipServices.Quotes.Data.Version1;
using PipServices.Quotes.Persistence;

namespace PipServices.Quotes.Logic
{
    public class QuotesController : IConfigurable, ICommandable, IQuotesController
    {
        private static ConfigParams _defaultConfig = ConfigParams.FromTuples("dependencies.persistence", "pip-services-quotes:persistence:*:*:1.0");

        private DependencyResolver _dependencyResolver = new DependencyResolver(_defaultConfig);
        private IQuotesPersistence _persistence;
        private QuotesCommandSet _commandSet;

        public void Configure(ConfigParams config)
        {
            _dependencyResolver.Configure(config);
        }

        public void SetReferences(IReferences references)
        {
            _dependencyResolver.SetReferences(references);
            _persistence = _dependencyResolver.GetOneRequired<IQuotesPersistence>("persistence");
        }

        public CommandSet GetCommandSet()
        {
            return _commandSet ?? (_commandSet = new QuotesCommandSet(this));
        }

        public Task<QuoteV1[]> GetQuotesAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return _persistence.GetPageByFilterAsync(correlationId, filter, paging);
        }

        public Task<QuoteV1> GetRandomQuoteAsync(string correlationId, FilterParams filter)
        {
            return _persistence.GetOneRandomAsync(correlationId, filter);
        }

        public Task<QuoteV1> GetQuoteByIdAsync(string correlationId, string quoteId)
        {
            return _persistence.GetOneByIdAsync(correlationId, quoteId);
        }

        public Task<QuoteV1> CreateQuoteAsync(string correlationId, QuoteV1 quote)
        {
            return _persistence.CreateAsync(correlationId, quote);
        }

        public Task<QuoteV1> UpdateQuoteAsync(string correlationId, QuoteV1 quote)
        {
            return _persistence.UpdateAsync(correlationId, quote);
        }

        public Task<QuoteV1> DeleteQuoteByIdAsync(string correlationId, string quoteId)
        {
            return _persistence.DeleteByIdAsync(correlationId, quoteId);
        }
    }
}

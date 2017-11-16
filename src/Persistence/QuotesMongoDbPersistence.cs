using PipServices.Data.MongoDb;
using PipServices.Quotes.Data.Version1;
using PipServices.Commons.Data;

using System.Threading.Tasks;

namespace PipServices.Quotes.Persistence
{
    public class QuotesMongoDbPersistence : IdentifiableMongoDbPersistence<QuoteV1, string>, IQuotesPersistence
    {
        public QuotesMongoDbPersistence() 
            : base("quotes")
        {
        }

        public Task<QuoteV1> GetOneRandomAsync(string correlationId, FilterParams filter)
        {
            return GetOneRandomAsync(correlationId, QuotesPersistenceHelper.ComposeFilterDefinition(filter));
        }

        public Task<DataPage<QuoteV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return GetPageByFilterAsync(correlationId, QuotesPersistenceHelper.ComposeFilterDefinition(filter), paging);
        }
    }
}

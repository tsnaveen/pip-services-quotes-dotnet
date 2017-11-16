using System.Threading.Tasks;

using PipServices.Commons.Data;
using PipServices.Data.Memory;
using PipServices.Quotes.Data.Version1;

namespace PipServices.Quotes.Persistence
{
    public class QuotesMemoryPersistence : IdentifiableMemoryPersistence<QuoteV1, string>, IQuotesPersistence
    {
        public int ItemsCount { get { return _items.Count; } }

        public Task<QuoteV1> GetOneRandomAsync(string correlationId, FilterParams filter)
        {
            return GetOneRandomAsync(correlationId, QuotesPersistenceHelper.ComposeFilters(filter));
        }

        public Task<DataPage<QuoteV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return GetPageByFilterAsync(correlationId, QuotesPersistenceHelper.ComposeFilters(filter), paging);
        }
    }
}

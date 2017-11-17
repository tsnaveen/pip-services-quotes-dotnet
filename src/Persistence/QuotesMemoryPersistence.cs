using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using PipServices.Commons.Data;
using PipServices.Data.Memory;
using PipServices.Quotes.Data.Version1;

namespace PipServices.Quotes.Persistence
{
    public class QuotesMemoryPersistence : IdentifiableMemoryPersistence<QuoteV1, string>, IQuotesPersistence
    {
        public int ItemsCount { get { return _items.Count; } }

        public Task<QuoteV1> GetOneRandomAsync(string correlationId, FilterParams filterParams)
        {
            return base.GetOneRandomAsync(correlationId, ComposeFilter(filterParams));
        }

        public Task<DataPage<QuoteV1>> GetPageByFilterAsync(string correlationId, FilterParams filterParams, PagingParams paging)
        {
            return base.GetPageByFilterAsync(correlationId, ComposeFilter(filterParams), paging);
        }

        private IList<Func<QuoteV1, bool>> ComposeFilter(FilterParams filter)
        {
            var result = new List<Func<QuoteV1, bool>>();

            filter = filter ?? new FilterParams();

            var search = filter.GetAsNullableString("search");
            var id = filter.GetAsNullableString("id");
            var status = filter.GetAsNullableString("status");
            var author = filter.GetAsNullableString("author");

            result.Add(quote => string.IsNullOrWhiteSpace(search) || MatchSearch(quote, search));
            result.Add(quote => string.IsNullOrWhiteSpace(id) || quote.Id.Equals(id));
            result.Add(quote => string.IsNullOrWhiteSpace(status) || quote.Status.Equals(status));
            result.Add(quote => string.IsNullOrWhiteSpace(author) || quote.Author.Contains(author));

            return result;
        }

        private bool MatchSearch(QuoteV1 item, string search)
        {
            return (item.Text != null && item.Text.Contains(search)) ? true
                : (item.Author != null && item.Author.Contains(search)) ? true
                : (item.Status != null && item.Status.Contains(search)) ? true
                : false;
        }
    }
}

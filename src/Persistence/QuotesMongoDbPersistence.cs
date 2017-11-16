using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using PipServices.Commons.Data;
using PipServices.Quotes.Data.Version1;
using PipServices.Data.MongoDb;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Text.RegularExpressions;

namespace PipServices.Quotes.Persistence
{
    public class QuotesMongoDbPersistence : IdentifiableMongoDbPersistence<QuoteV1, string>, IQuotesPersistence
    {
        public QuotesMongoDbPersistence() : base("quotes")
        {
        }

        private FilterDefinition<QuoteV1> GetFilter(FilterParams filterParams)
        {
            var search = filterParams.GetAsNullableString("search");
            var tags = filterParams.GetAsObject("tags");

            var id = filterParams.GetAsNullableString("id");
            var status = filterParams.GetAsNullableString("status");
            var author = filterParams.GetAsNullableString("author");

            var builder = Builders<QuoteV1>.Filter;
            var filter = builder.Empty;
            if(id != null) filter &= builder.Eq(q => q.Id, id);
            if (status != null) filter &= builder.Eq(q => q.Status, status);
            if (author != null) filter &= builder.Eq(q => q.Author, new MultiString(author));
            if (!string.IsNullOrEmpty(search))
            {
                var searchFilter = builder.Where(q => q.Text.Any(l => l.Value.ToLower().Contains(search)));
                searchFilter |= builder.Where(q => q.Author.Any(l => l.Value.ToLower().Contains(search)));
                searchFilter |= builder.Where(q => q.Status.ToLower().Contains(search));
                filter &= searchFilter;
            }

            return filter;
        }

        public Task<QuoteV1> GetOneRandomAsync(string correlationId, FilterParams filterParams)
        {
            FilterDefinition<QuoteV1> filter = GetFilter(filterParams);
            int position = new Random().Next(0, (int) _collection.Count(filter));
            return Task.FromResult(_collection.Find(filter).Skip(position - 1).Single());
        }

        public Task<DataPage<QuoteV1>> GetPageByFilterAsync(string correlationId, FilterParams filterParams, PagingParams paging)
        {
            FilterDefinition<QuoteV1> filter = GetFilter(filterParams);

            var filteredItems = _collection.Find<QuoteV1>(filter);

            paging = paging ?? new PagingParams();
            var skip = paging.GetSkip(0);
            var take = paging.GetTake(_options.GetAsInteger("max_page_size"));

            _logger.Trace(correlationId, $"Retrieved {filteredItems.Count()} items");

            return Task.FromResult(new DataPage<QuoteV1>()
            {
                Data = filteredItems.Skip(skip).ToList(),
                Total = paging.Total ? filteredItems.Count() : (long?)null
            });
        }
    }
}

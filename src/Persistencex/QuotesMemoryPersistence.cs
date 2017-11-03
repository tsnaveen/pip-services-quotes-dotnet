using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using PipServices.Commons.Data;
using PipServices.Data.Memory;
using PipServices.Quotes.Data;

namespace PipServices.Quotes.Persistence
{
    public class QuotesMemoryPersistence : IdentifiableMemoryPersistence<QuoteV1, string>, IQuotesPersistence
    {
        public int ItemsCount { get { return _items.Count; } }

        public Task<QuoteV1> GetOneRandomAsync(string correlationId, FilterParams filter)
        {
            var filteredItems = Filter<QuoteV1>(_items, ComposeFilter(filter));

            return Task.FromResult(Sample(filteredItems));
        }

        public Task<QuoteV1[]> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            var filteredItems = Filter<QuoteV1>(_items, ComposeFilter(filter));

            paging = paging ?? new PagingParams();
            var skip = paging.GetSkip(0);
            var take = paging.GetTake(_maxPageSize);

            return Task.FromResult(filteredItems.Take(take).Skip(skip).ToArray());
        }

        // TODO: Move to the base class
        private QuoteV1 Sample(IList<QuoteV1> items)
        {
            if (items.Count > 0)
            {
                var randomIndex = new Random().Next(0, items.Count - 1);
                return items[randomIndex];
            }

            return null;
        }

        // TODO: Move to the base class
        private IList<T> Filter<T>(IList<T> items, IList<Func<T, bool>> filterFunctions)
        {
            var result = new List<T>();

            foreach (var item in items)
            {
                foreach (var filterFunction in filterFunctions)
                {
                    if (!filterFunction(item))
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private IList<Func<QuoteV1, bool>> ComposeFilter(FilterParams filter)
        {
            var result = new List<Func<QuoteV1, bool>>();

            filter = filter ?? new FilterParams();

            var search = filter.GetAsNullableString("search");
            var id = filter.GetAsNullableString("id");
            var status = filter.GetAsNullableString("status");
            var author = filter.GetAsNullableString("author");

            result.Add(quote => !MatchSearch(quote, search));
            result.Add(quote => quote.Id != id);
            result.Add(quote => quote.Status != status);
            result.Add(quote => !MatchMultilanguageString(quote.Author, author));

            return result;
        }

        private bool MatchSearch(QuoteV1 item, string search)
        {
            if (MatchMultilanguageString(item.Text, search))
            {
                return true;
            }

            if (MatchMultilanguageString(item.Author, search))
            {
                return true;
            }

            if (MatchString(item.Status, search))
            {
                return true;
            }

            return false;
        }

        private bool MatchMultilanguageString(MultiString multiString, string search)
        {
            if (multiString == null)
            {
                return false;
            }

            foreach (var language in multiString.Keys)
            {
                if (MatchString(multiString[language].ToString(), search))
                {
                    return true;
                }
            }

            return false;
        }

        private bool MatchString(string value, string search)
        {
            if (string.IsNullOrWhiteSpace(value) && string.IsNullOrWhiteSpace(search))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(search))
            {
                return false;
            }

            return value.ToLower().IndexOf(search.ToLower()) >= 0;
        }
    }
}

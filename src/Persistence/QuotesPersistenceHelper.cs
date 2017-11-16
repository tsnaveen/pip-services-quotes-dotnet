using MongoDB.Bson;
using MongoDB.Driver;

using PipServices.Commons.Data;
using PipServices.Quotes.Data.Version1;

using System;
using System.Collections.Generic;

namespace PipServices.Quotes.Persistence
{
    public static class QuotesPersistenceHelper
    {
        public static IList<Func<QuoteV1, bool>> ComposeFilters(FilterParams filter)
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
            result.Add(quote => string.IsNullOrWhiteSpace(author) || MatchMultilanguageString(quote.Author, author));

            return result;
        }

        public static FilterDefinition<QuoteV1> ComposeFilterDefinition(FilterParams filter)
        {
            var builder = Builders<QuoteV1>.Filter;
            var filterDefinition = Builders<QuoteV1>.Filter.Empty;

            filter = filter ?? new FilterParams();

            var search = filter.GetAsNullableString("search");
            var id = filter.GetAsNullableString("id");
            var status = filter.GetAsNullableString("status");
            var author = filter.GetAsNullableString("author");

            if (!string.IsNullOrWhiteSpace(id))
            {
                filterDefinition = filterDefinition & builder.Eq(quote => quote.Id, id);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                filterDefinition = filterDefinition & builder.Eq(quote => quote.Status, status);
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                var bsonRegularExpression = new BsonRegularExpression($"{author}", "i");
                var authorFilterDefinition = builder.Regex($"Author.{MultiString.English}", bsonRegularExpression);
                authorFilterDefinition |= builder.Regex($"Author.{MultiString.Spanish}", bsonRegularExpression);
                authorFilterDefinition |= builder.Regex($"Author.{MultiString.French}", bsonRegularExpression);
                authorFilterDefinition |= builder.Regex($"Author.{MultiString.German}", bsonRegularExpression);
                authorFilterDefinition |= builder.Regex($"Author.{MultiString.Russian}", bsonRegularExpression);

                filterDefinition = filterDefinition & authorFilterDefinition;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var bsonRegularExpression = new BsonRegularExpression($"{search}", "i");
                var searchFilterDefinition = builder.Regex($"Text.{MultiString.English}", bsonRegularExpression);
                searchFilterDefinition |= builder.Regex($"Text.{MultiString.Spanish}", bsonRegularExpression);
                searchFilterDefinition |= builder.Regex($"Text.{MultiString.French}", bsonRegularExpression);
                searchFilterDefinition |= builder.Regex($"Text.{MultiString.German}", bsonRegularExpression);
                searchFilterDefinition |= builder.Regex($"Text.{MultiString.Russian}", bsonRegularExpression);

                searchFilterDefinition |= builder.Regex($"Author.{MultiString.English}", bsonRegularExpression);
                searchFilterDefinition |= builder.Regex($"Author.{MultiString.Spanish}", bsonRegularExpression);
                searchFilterDefinition |= builder.Regex($"Author.{MultiString.French}", bsonRegularExpression);
                searchFilterDefinition |= builder.Regex($"Author.{MultiString.German}", bsonRegularExpression);
                searchFilterDefinition |= builder.Regex($"Author.{MultiString.Russian}", bsonRegularExpression);

                searchFilterDefinition |= builder.Regex($"Status", bsonRegularExpression);

                filterDefinition = filterDefinition & searchFilterDefinition;
            }

            return filterDefinition;
        }

        private static bool MatchSearch(QuoteV1 item, string search)
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

        private static bool MatchMultilanguageString(MultiString multiString, string search)
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

        private static bool MatchString(string value, string search)
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

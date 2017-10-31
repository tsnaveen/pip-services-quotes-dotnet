using PipServices.Commons.Commands;
using PipServices.Commons.Convert;
using PipServices.Commons.Data;
using PipServices.Commons.Validate;
using PipServices.Quotes.Data;

namespace PipServices.Quotes.Logic
{
    public class QuotesCommandSet : CommandSet
    {
        private IQuotesController _logic;

        public QuotesCommandSet(IQuotesController logic)
        {
            _logic = logic;

            AddCommand(MakeGetQuotesCommand());
            AddCommand(MakeGetRandomQuoteCommand());
            AddCommand(MakeGetQuoteByIdCommand());
            AddCommand(MakeCreateQuoteCommand());
            AddCommand(MakeUpdateQuoteCommand());
            AddCommand(MakeDeleteQuoteByIdCommand());
        }

        private ICommand MakeGetQuotesCommand()
        {
            return new Command(
                "get_quotes",
                new ObjectSchema()
                    .WithOptionalProperty("filter", new FilterParamsSchema())
                    .WithOptionalProperty("paging", new PagingParamsSchema()),
                async (correlationId, parameters) => 
                {
                    var filter = FilterParams.FromValue(parameters.Get("filter"));
                    var paging = PagingParams.FromValue(parameters.Get("paging"));
                    return await _logic.GetQuotesAsync(correlationId, filter, paging);
                });
        }

        private ICommand MakeGetRandomQuoteCommand()
        {
            return new Command(
                "get_random_quote",
                new ObjectSchema()
                    .WithOptionalProperty("filter", new FilterParamsSchema()),
                async (correlationId, parameters) =>
                {
                    var filter = FilterParams.FromValue(parameters.Get("filter"));
                    return await _logic.GetRandomQuoteAsync(correlationId, filter);
                });
        }

        private ICommand MakeGetQuoteByIdCommand()
        {
            return new Command(
                "get_quote_by_id",
                new ObjectSchema()
                    .WithRequiredProperty("quote_id", TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var quoteId = parameters.GetAsString("quote_id");
                    return await _logic.GetQuoteByIdAsync(correlationId, quoteId);
                });
        }

        private ICommand MakeCreateQuoteCommand()
        {
            return new Command(
                "create_quote",
                new ObjectSchema()
                    .WithRequiredProperty("quote", new QuoteV1Schema()),
                async (correlationId, parameters) =>
                {
                    var quote = parameters.Get("quote") as QuoteV1;
                    return await _logic.CreateQuoteAsync(correlationId, quote);
                });
        }

        private ICommand MakeUpdateQuoteCommand()
        {
            return new Command(
                "update_quote",
                new ObjectSchema()
                    .WithRequiredProperty("quote", new QuoteV1Schema()),
                async (correlationId, parameters) =>
                {
                    var quote = parameters.Get("quote") as QuoteV1;
                    return await _logic.UpdateQuoteAsync(correlationId, quote);
                });
        }

        private ICommand MakeDeleteQuoteByIdCommand()
        {
            return new Command(
                "delete_quote_by_id",
                new ObjectSchema()
                    .WithOptionalProperty("quote_id", TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var quoteId = parameters.GetAsString("quote_id");
                    return await _logic.DeleteQuoteByIdAsync(correlationId, quoteId);
                });
        }
    }
}

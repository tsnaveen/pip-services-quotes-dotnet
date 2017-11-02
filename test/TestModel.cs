using PipServices.Commons.Data;
using PipServices.Quotes.Data;

using System.Collections.Generic;

namespace PipServices.Quotes.Test
{
    public class TestModel
    {
        public string CorrelationId { get; set; }

        public QuoteV1 SampleQuote1 { get; set; }
        public QuoteV1 SampleQuote2 { get; set; }
        public QuoteV1 SampleQuote3 { get; set; }
        public QuoteV1 SampleQuote4 { get; set; }

        public FilterParams FilterParams { get; set; }
        public PagingParams PagingParams { get; set; }

        public TestModel()
        {
            CorrelationId = "1";

            SampleQuote1 = new QuoteV1("1", new MultiString("Test Quote 1"), new MultiString("Author 1"), QuoteStatusV1.New);
            SampleQuote2 = new QuoteV1("2", new MultiString("Test Quote 2"), new MultiString("Author 2"), QuoteStatusV1.Completed);
            SampleQuote3 = new QuoteV1("3", new MultiString("Test Quote 3"), new MultiString("Author Strange"));
            SampleQuote4 = new QuoteV1("4", new MultiString(new Dictionary<string, object>()
                {
                    { "en", "Test English Quote"},
                    { "es", "Test Spanish Citar"},

                }), 
                new MultiString(new Dictionary<string, object>()
                {
                    { "en", "English Author"},
                    { "es", "Spanish Autor"},
                }), QuoteStatusV1.Translating);

            FilterParams = new FilterParams();
            PagingParams = new PagingParams();
        }
    }
}

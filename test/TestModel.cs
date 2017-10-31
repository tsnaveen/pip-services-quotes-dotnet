using PipServices.Commons.Data;
using PipServices.Quotes.Data;

namespace PipServices.Quotes.Test
{
    public class TestModel
    {
        public string CorrelationId { get; set; }

        public QuoteV1 SampleQuote1 { get; set; }
        public QuoteV1 SampleQuote2 { get; set; }
        public QuoteV1 SampleQuote3 { get; set; }

        public FilterParams FilterParams { get; set; }
        public PagingParams PagingParams { get; set; }

        public TestModel()
        {
            CorrelationId = "1";

            SampleQuote1 = new QuoteV1(new MultiString("Test Quote 1"), new MultiString("Author 1"), "New");
            SampleQuote2 = new QuoteV1(new MultiString("Test Quote 2"), new MultiString("Author 2"), "Old");
            SampleQuote3 = new QuoteV1(new MultiString("Test Quote 3"), new MultiString("Author Strange"));

            FilterParams = new FilterParams();
            PagingParams = new PagingParams();
        }
    }
}

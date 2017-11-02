using PipServices.Container;
using PipServices.Quotes.Build;

namespace PipServices.Quotes.Container
{
    public class QuotesProcess : ProcessContainer
    {
        public QuotesProcess()
            : base("quotes", "Inspirational quotes microservice")
        {
            _factories.Add(new QuotesServiceFactory());
        }
    }
}

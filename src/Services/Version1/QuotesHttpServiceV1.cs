using PipServices.Commons.Refer;
using PipServices.Net.Rest;

namespace PipServices.Quotes.Services.Version1
{
    public class QuotesHttpServiceV1 : CommandableHttpService
    {
        public QuotesHttpServiceV1()
            : base("quotes")
        {
            _dependencyResolver.Put("controller", new Descriptor("pip-services-quotes", "controller", "default", "*", "1.0"));
        }
    }
}

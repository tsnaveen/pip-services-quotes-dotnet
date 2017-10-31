using PipServices.Commons.Config;
using PipServices.Data.File;
using PipServices.Quotes.Data;

namespace PipServices.Quotes.Persistence
{
    public class QuotesFilePersistence : QuotesMemoryPersistence
    {
        protected JsonFilePersister<QuoteV1> _persister;

        public QuotesFilePersistence(string path)
        {
            _persister = new JsonFilePersister<QuoteV1>(path);
            _loader = _persister;
            _saver = _persister;
        }

        public override void Configure(ConfigParams config)
        {
            base.Configure(config);

            _persister.Configure(config);
        }
    }
}

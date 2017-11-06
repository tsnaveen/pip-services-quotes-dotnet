using PipServices.Commons.Config;
using PipServices.Data.File;
using PipServices.Quotes.Data.Version1;

namespace PipServices.Quotes.Persistence
{
    public class QuotesFilePersistence : QuotesMemoryPersistence
    {
        protected JsonFilePersister<QuoteV1> _persister;

        public QuotesFilePersistence()
        {
            _persister = new JsonFilePersister<QuoteV1>();
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

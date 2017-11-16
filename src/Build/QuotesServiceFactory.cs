
using PipServices.Commons.Build;
using PipServices.Commons.Refer;
using PipServices.Quotes.Logic;
using PipServices.Quotes.Persistence;
using PipServices.Quotes.Services.Version1;

namespace PipServices.Quotes.Build
{
    public class QuotesServiceFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("pip-services-quotes", "factory", "default", "default", "1.0");
        public static Descriptor MemoryPersistenceDescriptor = new Descriptor("pip-services-quotes", "persistence", "memory", "*", "1.0");
        public static Descriptor FilePersistenceDescriptor = new Descriptor("pip-services-quotes", "persistence", "file", "*", "1.0");
        public static Descriptor MongoDbPersistenceDescriptor = new Descriptor("pip-services-quotes", "persistence", "mongodb", "*", "1.0");
        public static Descriptor ControllerDescriptor = new Descriptor("pip-services-quotes", "controller", "default", "*", "1.0");
        public static Descriptor HttpServiceDescriptor = new Descriptor("pip-services-quotes", "service", "http", "*", "1.0");

        public QuotesServiceFactory()
        {
            RegisterAsType(MemoryPersistenceDescriptor, typeof(QuotesMemoryPersistence));
            RegisterAsType(FilePersistenceDescriptor, typeof(QuotesFilePersistence));
            RegisterAsType(MongoDbPersistenceDescriptor, typeof(QuotesMongoDbPersistence));
            RegisterAsType(ControllerDescriptor, typeof(QuotesController));
            RegisterAsType(HttpServiceDescriptor, typeof(QuotesHttpServiceV1));
        }
    }
}

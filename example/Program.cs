using PipServices.Quotes.Container;

using System;

namespace PipServices.Quotes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var task = (new QuotesProcess()).RunAsync(args);
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }

            //var controller = new QuotesController();
            //var service = new QuotesHttpServiceV1();

            //service.Configure(ConfigParams.FromTuples(
            //    "connection.protocol", "http",
            //    "connection.host", "localhost",
            //    "connection.port", 3000
            //));

            //service.SetReferences(References.FromTuples(
            //    new Descriptor("pip-services-quotes", "controller", "default", "default", "1.0"), controller,
            //    new Descriptor("pip-services-quotes", "service", "rest", "default", "1.0"), service
            //));

            //var task = service.OpenAsync(null);
            //task.Wait();
        }
    }
}

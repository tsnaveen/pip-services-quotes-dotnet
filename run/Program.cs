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
        }
    }
}

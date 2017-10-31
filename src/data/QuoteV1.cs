using PipServices.Commons.Data;

namespace PipServices.Quotes.Data
{
    public class QuoteV1 : IStringIdentifiable
    {
        public string Id { get; set; }
        public MultiString Text { get; private set; }
        public MultiString Author { get; private set; }
        public string Status { get; private set; }
        public string[] Tags { get; private set; }
        public string[] All_Tags { get; private set; }

        public QuoteV1(object text, object author, string status = "", string[] tags = null, string[] allTags = null)
        {
            Id = IdGenerator.NextLong();
            Text = text is string ? new MultiString(text as string) : text as MultiString;
            Author = author is string ? new MultiString(author as string) : author as MultiString;
            Status = !string.IsNullOrWhiteSpace(status) ? status : QuoteStatusV1.New;
            Tags = tags ?? new string[] { };
            All_Tags = allTags ?? new string[] { };
        }

    }
}

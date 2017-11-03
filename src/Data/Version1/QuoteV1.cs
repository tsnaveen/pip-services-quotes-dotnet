using PipServices.Commons.Data;

using System.Collections.Generic;

namespace PipServices.Quotes.Data.Version1
{
    public class QuoteV1 : IStringIdentifiable
    {
        public string Id { get; set; }
        public MultiString Text { get; set; }
        public MultiString Author { get; set; }
        public string Status { get; set; }
        public string[] Tags { get; set; }
        public string[] All_Tags { get; set; }

        public QuoteV1(string id, object text, object author, string status = "", string[] tags = null, string[] allTags = null)
        {
            Id = !string.IsNullOrWhiteSpace(id) ? id : IdGenerator.NextLong();
            Text = ExtractMultiString(text);
            Author = ExtractMultiString(author);
            Status = !string.IsNullOrWhiteSpace(status) ? status : QuoteStatusV1.New;
            Tags = tags ?? new string[] { };
            All_Tags = allTags ?? new string[] { };
        }

        public override bool Equals(object obj)
        {
            var quote = obj as QuoteV1;

            return quote != null &&
                quote.Id.Equals(Id) &&
                quote.Text.Equals(Text) &&
                quote.Author.Equals(Author) &&
                quote.Status.Equals(Status);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private MultiString ExtractMultiString(object obj)
        {
            if (obj is MultiString)
            {
                return obj as MultiString;
            }

            if (obj is string)
            {
                return new MultiString(obj as string);
            }

            return new MultiString(obj as Dictionary<string, string>);
        }
    }
}

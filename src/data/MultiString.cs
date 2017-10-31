namespace PipServices.Quotes.Data
{
    // TODO: Move to Pip.Services.Common
    public class MultiString
    {
        public string Text { get; private set; }
        public string Language { get; private set; }

        public MultiString(string text)
            : this("en", text)
        {
        }

        public MultiString(string language, string text)
        {
            Language = "en";
            Text = text;
        }
    }
}

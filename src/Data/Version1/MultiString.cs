
using System.Collections.Generic;
using System.Linq;

namespace PipServices.Quotes.Data.Version1
{
    // TODO: Move to Pip.Services.Common
    public class MultiString : List<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>
    {
        public const string English = "en";
        public const string Spanish = "sp";
        public const string French = "fr";
        public const string German = "de";
        public const string Russian = "ru";

        public MultiString()
        {
        }

        public MultiString(Dictionary<string, string> map)
        {
            foreach (string key in map.Keys)
            {
                Add(new KeyValuePair<string, string>(key, map[key]));
            }
        }

        public MultiString(IEnumerable<KeyValuePair<string, string>> map)
            : base(map)
        {
        }

        public MultiString(string text)
            : this(English, text)
        {
        }

        public MultiString(string language, string text)
        {
            this.Add(new KeyValuePair<string, string>(language,text));
        }

        public override bool Equals(object obj)
        {
            var multiString = obj as MultiString;

            return multiString != null &&
                multiString.Count == Count &&
                !multiString.Except(this).Any();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

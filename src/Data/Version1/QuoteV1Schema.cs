using PipServices.Commons.Convert;
using PipServices.Commons.Validate;

namespace PipServices.Quotes.Data.Version1
{
    public class QuoteV1Schema : ObjectSchema
    {
        public QuoteV1Schema()
        {
            WithOptionalProperty("id", TypeCode.String);
            //WithRequiredProperty("text", TypeCode.Map);   // it might be string or dictionary
            //WithOptionalProperty("author", TypeCode.Map); // it might be string or dictionary
            WithOptionalProperty("status", TypeCode.String);
            WithOptionalProperty("tags", new ArraySchema(TypeCode.String));
            WithOptionalProperty("all_tags", new ArraySchema(TypeCode.String));
        }
    }
}

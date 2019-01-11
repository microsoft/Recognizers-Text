using System.Collections.Generic;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DummyTimeZoneParser : IDateTimeParser
    {
        public ParseResult Parse(ExtractResult result)
        {
            return Parse(result, DateObject.Now);
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            return null;
        }
    }
}
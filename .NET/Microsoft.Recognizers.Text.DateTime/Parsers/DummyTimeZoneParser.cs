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
            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = null,
                TimexStr = null,
                ResolutionStr = ""
            };

            return ret;
        }
    }
}
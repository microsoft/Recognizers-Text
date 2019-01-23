using System.Collections.Generic;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeParser : IParser
    {
        DateTimeParseResult Parse(ExtractResult er, DateObject reference);

        List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults);
    }

    public class DateTimeParseResult : ParseResult
    {
        // TimexStr is only used in extractors related with date and time
        // It will output the TIMEX representation of a time string.
        public string TimexStr { get; set; } = string.Empty;
    }
}

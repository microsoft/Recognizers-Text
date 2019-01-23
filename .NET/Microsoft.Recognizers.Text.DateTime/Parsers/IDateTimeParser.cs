// ReSharper disable InconsistentNaming
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
        //TimexStr is only used in extractors related with date and time
        //It will output the TIMEX representation of a time string.
        public string TimexStr { get; set; } = "";
    }

    public static class TimeTypeConstants
    {
        public const string DATE = "date";
        public const string DATETIME = "dateTime";
        public const string DATETIMEALT = "dateTimeAlt";
        public const string DURATION = "duration";
        public const string SET = "set";
        public const string TIME = "time";

        // Internal SubType for Future/Past in DateTimeResolutionResult
        public const string START_DATE = "startDate";
        public const string END_DATE = "endDate";
        public const string START_DATETIME = "startDateTime";
        public const string END_DATETIME = "endDateTime";
        public const string START_TIME = "startTime";
        public const string END_TIME = "endTime";
    }
}

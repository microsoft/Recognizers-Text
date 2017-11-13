// ReSharper disable InconsistentNaming

using Microsoft.Recognizers.Text.Number;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeParser : IParser
    {
        DateTimeParseResult Parse(ExtractResult er, DateObject reference);
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
        public const string START_DATE = "startDate";
        public const string END_DATE = "endDate";
        public const string DATETIME = "dateTime";
        public const string START_DATETIME = "startDateTime";
        public const string END_DATETIME = "endDateTime";
        public const string DURATION = "duration";
        public const string SET = "set";
        public const string TIME = "time";
        public const string VALUE = "value";
        public const string START_TIME = "startTime";
        public const string END_TIME = "endTime";

        public const string START = "start";
        public const string END = "end";

        public const string beforeMod = "before";
        public const string afterMod = "after";
        public const string sinceMod = "since";
    }
}

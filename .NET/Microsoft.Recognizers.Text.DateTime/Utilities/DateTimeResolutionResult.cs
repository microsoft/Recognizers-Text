using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeResolutionResult
    {
        public DateTimeResolutionResult()
        {
            Success = false;
        }

        public bool Success { get; set; }

        public string Timex { get; set; }

        public bool IsLunar { get; set; }

        public string Mod { get; set; }

        public string Comment { get; set; }

        public Dictionary<string, string> FutureResolution { get; set; }

        public Dictionary<string, string> PastResolution { get; set; }

        public object FutureValue { get; set; }

        public object PastValue { get; set; }

        public List<object> SubDateTimeEntities { get; set; }

        public TimeZoneResolutionResult TimeZoneResolution { get; set; }
    }

    public class TimeZoneResolutionResult
    {
        public string Value { get; set; }

        public int UtcOffsetMins { get; set; }

        public string TimeZoneText { get; set; }
    }
}
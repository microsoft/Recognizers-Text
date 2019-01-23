namespace Microsoft.Recognizers.Text.DateTime
{
    public class TimeZoneResolutionResult
    {
        public string Value { get; set; }

        public int UtcOffsetMins { get; set; }

        public string TimeZoneText { get; set; }
    }
}
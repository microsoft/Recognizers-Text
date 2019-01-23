namespace Microsoft.Recognizers.Text.DateTime
{
    public class TimeOfDayResolutionResult
    {
        public string Timex { get; set; }

        public int BeginHour { get; set; }

        public int EndHour { get; set; }

        public int EndMin { get; set; }
    }
}
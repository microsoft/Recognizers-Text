namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public class TimeResult
    {
        public int Hour { get; set; }

        public int Minute { get; set; }

        public int Second { get; set; }

        public int LowBound { get; set; } = -1;
    }
}

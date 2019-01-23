using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class RangeTimexComponents
    {
        public string BeginTimex { get; set; }

        public string EndTimex { get; set; }

        public string DurationTimex { get; set; }

        public bool IsValid { get; set; } = false;
    }
}

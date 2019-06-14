using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class URLConfiguration : ISequenceConfiguration
    {
        public URLConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

        public Regex IpUrlRegex { get; set; }

        public Regex UrlRegex { get; set; }

    }
}

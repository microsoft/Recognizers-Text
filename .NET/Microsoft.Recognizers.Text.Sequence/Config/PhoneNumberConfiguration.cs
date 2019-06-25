using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class PhoneNumberConfiguration : ISequenceConfiguration
    {
        public PhoneNumberConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

        public string WordBoundariesRegex { get; set; }

        public string NonWordBoundariesRegex { get; set; }

        public string EndWordBoundariesRegex { get; set; }

    }
}

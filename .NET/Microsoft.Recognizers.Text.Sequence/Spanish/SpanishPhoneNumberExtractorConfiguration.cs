using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.Sequence.Spanish
{
    public class SpanishPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public SpanishPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

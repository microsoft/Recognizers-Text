using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.Sequence.Turkish
{
    public class TurkishPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public TurkishPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

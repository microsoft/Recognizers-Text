using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Sequence.Dutch
{
    public class DutchPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public DutchPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

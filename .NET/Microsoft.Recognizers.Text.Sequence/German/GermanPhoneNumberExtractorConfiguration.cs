using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.Sequence.German
{
    public class GermanPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public GermanPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

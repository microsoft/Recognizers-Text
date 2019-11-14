using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.Sequence.Korean
{
    public class KoreanPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public KoreanPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Hindi;

namespace Microsoft.Recognizers.Text.Sequence.Hindi
{
    public class HindiPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public HindiPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

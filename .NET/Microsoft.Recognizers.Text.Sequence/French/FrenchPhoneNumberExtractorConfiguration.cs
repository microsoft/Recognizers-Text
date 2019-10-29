using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Sequence.French
{
    public class FrenchPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public FrenchPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

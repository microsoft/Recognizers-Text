using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.Sequence.Swedish
{
    public class SwedishPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public SwedishPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

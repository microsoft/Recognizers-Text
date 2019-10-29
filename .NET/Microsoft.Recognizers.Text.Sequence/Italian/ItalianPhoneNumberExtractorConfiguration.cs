using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.Sequence.Italian
{
    public class ItalianPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public ItalianPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}

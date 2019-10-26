using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EnglishPhoneNumberExtractorConfiguration : PhoneNumberConfiguration
    {
        public EnglishPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            WordBoundariesRegex = BasePhoneNumbers.WordBoundariesRegex;
            NonWordBoundariesRegex = BasePhoneNumbers.NonWordBoundariesRegex;
            EndWordBoundariesRegex = BasePhoneNumbers.EndWordBoundariesRegex;
            ColonPrefixCheckRegex = new Regex(BasePhoneNumbers.ColonPrefixCheckRegex);
            ColonMarkers = (List<char>)BasePhoneNumbers.ColonMarkers;
            ForbiddenPrefixMarkers = (List<char>)BasePhoneNumbers.ForbiddenPrefixMarkers;
            ForbiddenSuffixMarkers = (List<char>)BasePhoneNumbers.ForbiddenSuffixMarkers;
            FalsePositivePrefixRegex = new Regex(PhoneNumbersDefinitions.FalsePositivePrefixRegex);
        }
    }
}

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Sequence.Chinese
{
    public class ChinesePhoneNumberExtractorConfiguration : PhoneNumberConfiguration
    {
        public ChinesePhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            WordBoundariesRegex = PhoneNumbersDefinitions.WordBoundariesRegex;
            NonWordBoundariesRegex = PhoneNumbersDefinitions.NonWordBoundariesRegex;
            EndWordBoundariesRegex = PhoneNumbersDefinitions.EndWordBoundariesRegex;
            ColonPrefixCheckRegex = RegexCache.Get(PhoneNumbersDefinitions.ColonPrefixCheckRegex);
            ForbiddenPrefixMarkers = (List<char>)PhoneNumbersDefinitions.ForbiddenPrefixMarkers;
        }
    }
}
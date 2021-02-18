using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BasePhoneNumberExtractorConfiguration : PhoneNumberConfiguration
    {
        public BasePhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            WordBoundariesRegex = BasePhoneNumbers.WordBoundariesRegex;
            NonWordBoundariesRegex = BasePhoneNumbers.NonWordBoundariesRegex;
            EndWordBoundariesRegex = BasePhoneNumbers.EndWordBoundariesRegex;
            ColonPrefixCheckRegex = RegexCache.Get(BasePhoneNumbers.ColonPrefixCheckRegex);
            ColonMarkers = (List<char>)BasePhoneNumbers.ColonMarkers;
            ForbiddenPrefixMarkers = (List<char>)BasePhoneNumbers.ForbiddenPrefixMarkers;
            ForbiddenSuffixMarkers = (List<char>)BasePhoneNumbers.ForbiddenSuffixMarkers;
        }
    }
}
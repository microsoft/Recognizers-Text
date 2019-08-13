using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

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
            ColonBeginRegex = new Regex(BasePhoneNumbers.ColonBeginRegex);
            ColonMarkers = (List<char>)BasePhoneNumbers.ColonMarkers;
            BoundaryStartMarkers = (List<char>)BasePhoneNumbers.BoundaryStartMarkers;
            BoundaryEndMarkers = (List<char>)BasePhoneNumbers.BoundaryEndMarkers;
        }
    }
}

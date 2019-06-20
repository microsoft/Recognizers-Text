using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EnglishPhoneNumberExtractorConfiguration : PhoneNumberConfiguration
    {
        public EnglishPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            BRPhoneNumberRegex = new Regex(BasePhoneNumbers.BRPhoneNumberRegex, RegexOptions.Compiled);
            GeneralPhoneNumberRegex = new Regex(BasePhoneNumbers.GeneralPhoneNumberRegex, RegexOptions.Compiled);
            UKPhoneNumberRegex = new Regex(BasePhoneNumbers.UKPhoneNumberRegex, RegexOptions.Compiled);
            DEPhoneNumberRegex = new Regex(BasePhoneNumbers.DEPhoneNumberRegex, RegexOptions.Compiled);
            USPhoneNumberRegex = new Regex(BasePhoneNumbers.USPhoneNumberRegex, RegexOptions.Compiled);
            CNPhoneNumberRegex = new Regex(BasePhoneNumbers.CNPhoneNumberRegex, RegexOptions.Compiled);
            DKPhoneNumberRegex = new Regex(BasePhoneNumbers.DKPhoneNumberRegex, RegexOptions.Compiled);
            ITPhoneNumberRegex = new Regex(BasePhoneNumbers.ITPhoneNumberRegex, RegexOptions.Compiled);
            NLPhoneNumberRegex = new Regex(BasePhoneNumbers.NLPhoneNumberRegex, RegexOptions.Compiled);
            SpecialPhoneNumberRegex = new Regex(BasePhoneNumbers.SpecialPhoneNumberRegex, RegexOptions.Compiled);
        }
    }
}

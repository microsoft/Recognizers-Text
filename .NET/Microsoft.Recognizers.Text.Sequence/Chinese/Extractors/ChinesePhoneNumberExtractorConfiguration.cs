using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Sequence.Chinese
{
    public class ChinesePhoneNumberExtractorConfiguration : PhoneNumberConfiguration
    {
        public ChinesePhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            BRPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.BRPhoneNumberRegex, RegexOptions.Compiled);
            GeneralPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.GeneralPhoneNumberRegex, RegexOptions.Compiled);
            UKPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.UKPhoneNumberRegex, RegexOptions.Compiled);
            DEPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.DEPhoneNumberRegex, RegexOptions.Compiled);
            USPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.USPhoneNumberRegex, RegexOptions.Compiled);
            CNPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.CNPhoneNumberRegex, RegexOptions.Compiled);
            DKPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.DKPhoneNumberRegex, RegexOptions.Compiled);
            ITPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.ITPhoneNumberRegex, RegexOptions.Compiled);
            NLPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.NLPhoneNumberRegex, RegexOptions.Compiled);
            SpecialPhoneNumberRegex = new Regex(PhoneNumbersDefinitions.SpecialPhoneNumberRegex, RegexOptions.Compiled);
        }
    }
}
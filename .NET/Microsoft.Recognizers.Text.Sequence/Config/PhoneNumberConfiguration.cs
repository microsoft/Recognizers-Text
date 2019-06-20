using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class PhoneNumberConfiguration : ISequenceConfiguration
    {
        public PhoneNumberConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

        public Regex BRPhoneNumberRegex { get; set; }

        public Regex GeneralPhoneNumberRegex { get; set; }

        public Regex UKPhoneNumberRegex { get; set; }

        public Regex DEPhoneNumberRegex { get; set; }

        public Regex USPhoneNumberRegex { get; set; }

        public Regex CNPhoneNumberRegex { get; set; }

        public Regex DKPhoneNumberRegex { get; set; }

        public Regex ITPhoneNumberRegex { get; set; }

        public Regex NLPhoneNumberRegex { get; set; }

        public Regex SpecialPhoneNumberRegex { get; set; }

    }
}

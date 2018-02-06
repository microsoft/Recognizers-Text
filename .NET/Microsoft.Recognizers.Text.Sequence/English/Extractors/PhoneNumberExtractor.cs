using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    class PhoneNumberExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.MODEL_PHONE_NUMBER;

        public PhoneNumberExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BasePhoneNumbers.BrazilPhoneNumberRegex), "BrazilPhoneNumber"
                },
                {
                    new Regex(BasePhoneNumbers.GeneralPhoneNumberRegex), "GeneralPhoneNumber"
                },
                {
                    new Regex(BasePhoneNumbers.UkPhoneNumberRegex), "UKPhoneNumberRegex"
                },
                {
                    new Regex(BasePhoneNumbers.GermanyPhoneNumberRegex), "GermanyPhoneNumberRegex"
                }

            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}

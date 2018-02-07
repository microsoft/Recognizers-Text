using Microsoft.Recognizers.Definitions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    class BasePhoneNumberExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.MODEL_PHONE_NUMBER;

        public BasePhoneNumberExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BasePhoneNumbers.BrazilPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_BRAZIL
                },
                {
                    new Regex(BasePhoneNumbers.GeneralPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_GENERAL
                },
                {
                    new Regex(BasePhoneNumbers.UkPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_UK
                },
                {
                    new Regex(BasePhoneNumbers.GermanyPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_GERMANY
                }

            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}

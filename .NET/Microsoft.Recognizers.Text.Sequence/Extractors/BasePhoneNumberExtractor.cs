using Microsoft.Recognizers.Definitions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BasePhoneNumberExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_PHONE_NUMBER;

        private static List<char> SeparatorCharList => BasePhoneNumbers.SeparatorCharList.ToList();

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
                },
                {
                    new Regex(BasePhoneNumbers.USPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_US
                },
                {
                    new Regex(BasePhoneNumbers.CNPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_CN
                },
                {
                    new Regex(BasePhoneNumbers.SpecialPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_SPECIAL
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        public override List<ExtractResult> Extract(string text)
        {
            var ers = base.Extract(text);

            foreach (var er in ers)
            {
                if (er.Start != 0)
                {
                    var ch = text[(int)(er.Start - 1)];
                    if (SeparatorCharList.Contains(ch))
                    {
                        ers.Remove(er);
                    }
                }
            }
            return ers;
        }
    }
}

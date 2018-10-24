using Microsoft.Recognizers.Definitions;
using System;
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

        private static List<char> OperatorList => BasePhoneNumbers.OperatorList.ToList();

        private static List<char> SeparatorCharList => BasePhoneNumbers.SeparatorCharList.ToList();

        public BasePhoneNumberExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BasePhoneNumbers.BRPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_BR
                },
                {
                    new Regex(BasePhoneNumbers.GeneralPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_GENERAL
                },
                {
                    new Regex(BasePhoneNumbers.UKPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_UK
                },
                {
                    new Regex(BasePhoneNumbers.DEPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_DE
                },
                {
                    new Regex(BasePhoneNumbers.USPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_US
                },
                {
                    new Regex(BasePhoneNumbers.CNPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_CN
                },
                {
                    new Regex(BasePhoneNumbers.DKPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_DK
                },
                {
                    new Regex(BasePhoneNumbers.ITPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_IT
                },
                {
                    new Regex(BasePhoneNumbers.NLPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_NL
                },
                {
                    new Regex(BasePhoneNumbers.SpecialPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_SPECIAL
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        private bool CheckFormattedPhoneNumber(string phoneNumberText)
        {
            return Regex.IsMatch(phoneNumberText, BasePhoneNumbers.FormatIndicatorRegex);
        }

        public override List<ExtractResult> Extract(string text)
        {
            var ers = base.Extract(text);

            foreach (var er in ers)
            {
                if (er.Start != 0)
                {
                    var ch = text[(int)(er.Start - 1)];
                    if (OperatorList.Contains(ch))
                    {
                        if (SeparatorCharList.Contains(ch) &&
                            CheckFormattedPhoneNumber(er.Text) && 
                            er.Start >= 2)
                        {
                            var chGap = text[(int)(er.Start - 2)];
                            if (!Char.IsNumber(chGap))
                            {
                                continue;
                            }
                        }
                        ers.Remove(er);
                    }
                }
            }
            return ers;
        }
    }
}

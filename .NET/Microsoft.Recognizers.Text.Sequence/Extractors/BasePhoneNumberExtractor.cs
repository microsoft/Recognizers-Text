using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BasePhoneNumberExtractor : BaseSequenceExtractor
    {
        private static readonly Regex InternationDialingPrefixRegex = new Regex(BasePhoneNumbers.InternationDialingPrefixRegex);

        public BasePhoneNumberExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BasePhoneNumbers.BRPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_BR
                },
                {
                    new Regex(BasePhoneNumbers.GeneralPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_GENERAL
                },
                {
                    new Regex(BasePhoneNumbers.UKPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_UK
                },
                {
                    new Regex(BasePhoneNumbers.DEPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_DE
                },
                {
                    new Regex(BasePhoneNumbers.USPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_US
                },
                {
                    new Regex(BasePhoneNumbers.CNPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_CN
                },
                {
                    new Regex(BasePhoneNumbers.DKPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_DK
                },
                {
                    new Regex(BasePhoneNumbers.ITPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_IT
                },
                {
                    new Regex(BasePhoneNumbers.NLPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_NL
                },
                {
                    new Regex(BasePhoneNumbers.SpecialPhoneNumberRegex),
                    Constants.PHONE_NUMBER_REGEX_SPECIAL
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_PHONE_NUMBER;

        private static List<char> BoundaryMarkers => BasePhoneNumbers.BoundaryMarkers.ToList();

        private static List<char> SpecialBoundaryMarkers => BasePhoneNumbers.SpecialBoundaryMarkers.ToList();

        public override List<ExtractResult> Extract(string text)
        {
            var ers = base.Extract(text);

            foreach (var er in ers)
            {
                if (er.Start != 0)
                {
                    var ch = text[(int)(er.Start - 1)];
                    if (BoundaryMarkers.Contains(ch))
                    {
                        if (SpecialBoundaryMarkers.Contains(ch) &&
                            CheckFormattedPhoneNumber(er.Text) &&
                            er.Start >= 2)
                        {
                            var charGap = text[(int)(er.Start - 2)];
                            if (!char.IsNumber(charGap) && !char.IsWhiteSpace(charGap))
                            {
                                continue;
                            }

                            // check the international dialing prefix
                            var front = text.Substring(0, (int)(er.Start - 1));
                            if (InternationDialingPrefixRegex.IsMatch(front))
                            {
                                var moveOffset = InternationDialingPrefixRegex.Match(front).Length + 1;
                                er.Start = er.Start - moveOffset;
                                er.Length = er.Length + moveOffset;
                                er.Text = text.Substring((int)er.Start, (int)er.Length);
                                continue;
                            }
                        }

                        ers.Remove(er);
                    }
                }
            }

            return ers;
        }

        private bool CheckFormattedPhoneNumber(string phoneNumberText)
        {
            return Regex.IsMatch(phoneNumberText, BasePhoneNumbers.FormatIndicatorRegex);
        }
    }
}

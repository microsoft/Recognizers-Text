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

        private PhoneNumberConfiguration config;

        public BasePhoneNumberExtractor(PhoneNumberConfiguration config)
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    config.BRPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_BR
                },
                {
                    config.GeneralPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_GENERAL
                },
                {
                    config.UKPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_UK
                },
                {
                    config.DEPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_DE
                },
                {
                    config.USPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_US
                },
                {
                    config.CNPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_CN
                },
                {
                    config.DKPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_DK
                },
                {
                    config.ITPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_IT
                },
                {
                    config.NLPhoneNumberRegex,
                    Constants.PHONE_NUMBER_REGEX_NL
                },
                {
                    config.SpecialPhoneNumberRegex,
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

            // filter hexadecimal address like 00 10 00 31 46 D9 E9 11
            var maskMatchCollection = Regex.Matches(text, BasePhoneNumbers.PhoneNumberMaskRegex);

            for (var index = ers.Count - 1; index >= 0; --index)
            {
                foreach (Match m in maskMatchCollection)
                {
                    if (ers[index].Start >= m.Index &&
                        ers[index].Start + ers[index].Length <= m.Index + m.Length)
                    {
                        ers.RemoveAt(index);
                        break;
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

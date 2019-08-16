using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BasePhoneNumberExtractor : BaseSequenceExtractor
    {
        private static readonly Regex InternationDialingPrefixRegex = new Regex(BasePhoneNumbers.InternationDialingPrefixRegex);

        private static readonly Regex PreCheckPhoneNumberRegex = new Regex(BasePhoneNumbers.PreCheckPhoneNumberRegex, RegexOptions.Compiled);

        private PhoneNumberConfiguration config;

        public BasePhoneNumberExtractor(PhoneNumberConfiguration config)
        {
            this.config = config;

            var wordBoundariesRegex = config.WordBoundariesRegex;
            var nonWordBoundariesRegex = config.NonWordBoundariesRegex;
            var endWordBoundariesRegex = config.EndWordBoundariesRegex;

            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BasePhoneNumbers.GeneralPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_GENERAL
                },
                {
                    new Regex(BasePhoneNumbers.BRPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_BR
                },
                {
                    new Regex(BasePhoneNumbers.UKPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_UK
                },
                {
                    new Regex(BasePhoneNumbers.DEPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_DE
                },
                {
                    new Regex(BasePhoneNumbers.USPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_US
                },
                {
                    new Regex(BasePhoneNumbers.CNPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_CN
                },
                {
                    new Regex(BasePhoneNumbers.DKPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_DK
                },
                {
                    new Regex(BasePhoneNumbers.ITPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_IT
                },
                {
                    new Regex(BasePhoneNumbers.NLPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_NL
                },
                {
                    new Regex(BasePhoneNumbers.SpecialPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_SPECIAL
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_PHONE_NUMBER;

        private static List<char> SpecialBoundaryMarkers => BasePhoneNumbers.SpecialBoundaryMarkers.ToList();

        public override List<ExtractResult> Extract(string text)
        {
            if (!PreCheckPhoneNumberRegex.IsMatch(text))
            {
                return new List<ExtractResult>();
            }

            var ers = base.Extract(text);

            foreach (var er in ers)
            {
                if (CountDigits(er.Text) < 7 && er.Data.ToString() != "ITPhoneNumber")
                {
                    ers.Remove(er);
                    continue;
                }

                if (er.Start + er.Length < text.Length)
                {
                    var ch = text[(int)(er.Start + er.Length)];
                    if (BasePhoneNumbers.ForbiddenSuffixMarkers.Contains(ch))
                    {
                        ers.Remove(er);
                        continue;
                    }
                }

                if (er.Start != 0)
                {
                    var ch = text[(int)(er.Start - 1)];
                    if (BasePhoneNumbers.BoundaryMarkers.Contains(ch))
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

                        // Handle cases like "-1234567" and "-1234+5678"
                        ers.Remove(er);
                    }

                    if (this.config.ForbiddenPrefixMarkers.Contains(ch))
                    {
                        // Handle "tel:123456".
                        if (BasePhoneNumbers.ColonMarkers.Contains(ch))
                        {
                            var front = text.Substring(0, (int)(er.Start - 1));
                            if (this.config.ColonPrefixCheckRegex.IsMatch(front))
                            {
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

        private int CountDigits(string candidateString)
        {
            var count = 0;
            foreach (var t in candidateString)
            {
                if (char.IsNumber(t))
                {
                    ++count;
                }
            }

            return count;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BasePhoneNumberExtractor : BaseSequenceExtractor
    {
        private static readonly Regex InternationalDialingPrefixRegex = RegexCache.Get(BasePhoneNumbers.InternationDialingPrefixRegex);

        private static readonly Regex PreCheckPhoneNumberRegex = RegexCache.Get(BasePhoneNumbers.PreCheckPhoneNumberRegex, RegexOptions.Compiled);

        private static readonly Regex SSNFilterRegex = RegexCache.Get(BasePhoneNumbers.SSNFilterRegex, RegexOptions.Compiled);

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
                    RegexCache.Get(BasePhoneNumbers.GeneralPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_GENERAL
                },
                {
                    RegexCache.Get(BasePhoneNumbers.BRPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_BR
                },
                {
                    RegexCache.Get(BasePhoneNumbers.UKPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_UK
                },
                {
                    RegexCache.Get(BasePhoneNumbers.DEPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_DE
                },
                {
                    RegexCache.Get(BasePhoneNumbers.USPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_US
                },
                {
                    RegexCache.Get(BasePhoneNumbers.CNPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_CN
                },
                {
                    RegexCache.Get(BasePhoneNumbers.DKPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_DK
                },
                {
                    RegexCache.Get(BasePhoneNumbers.ITPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_IT
                },
                {
                    RegexCache.Get(BasePhoneNumbers.NLPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.PHONE_NUMBER_REGEX_NL
                },
                {
                    RegexCache.Get(BasePhoneNumbers.SpecialPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
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

            for (var i = 0; i < ers.Count; i++)
            {
                var er = ers[i];
                if ((CountDigits(er.Text) < 7 && er.Data.ToString() != "ITPhoneNumber") || SSNFilterRegex.IsMatch(er.Text))
                {
                    ers.Remove(er);
                    i--;
                    continue;
                }

                if (CountDigits(er.Text) == 16 && !er.Text.StartsWith("+", StringComparison.Ordinal))
                {
                    ers.Remove(er);
                    i--;
                    continue;
                }

                if (CountDigits(er.Text) == 15)
                {
                    var flag = false;
                    foreach (var numSpan in er.Text.Split(' '))
                    {
                        if (CountDigits(numSpan) == 4 || CountDigits(numSpan) == 3)
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag == false)
                    {
                        ers.Remove(er);
                        i--;
                        continue;
                    }
                }

                if (er.Start + er.Length < text.Length)
                {
                    var ch = text[(int)(er.Start + er.Length)];
                    if (BasePhoneNumbers.ForbiddenSuffixMarkers.Contains(ch))
                    {
                        ers.Remove(er);
                        i--;
                        continue;
                    }
                }

                if (er.Start != 0)
                {
                    var ch = text[(int)(er.Start - 1)];
                    var front = text.Substring(0, (int)(er.Start - 1));

                    if (this.config.FalsePositivePrefixRegex != null &&
                            this.config.FalsePositivePrefixRegex.IsMatch(front))
                    {
                        ers.Remove(er);
                        i--;
                        continue;
                    }

                    if (BasePhoneNumbers.BoundaryMarkers.Contains(ch))
                    {
                        if (SpecialBoundaryMarkers.Contains(ch) &&
                            CheckFormattedPhoneNumber(er.Text) &&
                            er.Start >= 2)
                        {
                            var charGap = text[(int)(er.Start - 2)];
                            if (!char.IsNumber(charGap) && !char.IsWhiteSpace(charGap))
                            {
                                // check if the extracted string has a non-digit string before "-".
                                var flag = RegexCache.IsMatch(text.Substring(0, (int)(er.Start - 2)), @"^[^0-9]+$");

                                // Handle cases like "91a-677-0060".
                                if (char.IsLower(charGap) && !flag)
                                {
                                    ers.Remove(er);
                                    i--;
                                }

                                continue;
                            }

                            // check the international dialing prefix
                            if (InternationalDialingPrefixRegex.IsMatch(front))
                            {
                                var moveOffset = InternationalDialingPrefixRegex.Match(front).Length + 1;
                                er.Start = er.Start - moveOffset;
                                er.Length = er.Length + moveOffset;
                                er.Text = text.Substring((int)er.Start, (int)er.Length);
                                continue;
                            }
                        }

                        // Handle cases like "-1234567" and "-1234+5678"
                        ers.Remove(er);
                        i--;
                    }

                    if (this.config.ForbiddenPrefixMarkers.Contains(ch))
                    {
                        // Handle "tel:123456".
                        if (BasePhoneNumbers.ColonMarkers.Contains(ch))
                        {
                            if (this.config.ColonPrefixCheckRegex.IsMatch(front))
                            {
                                continue;
                            }
                        }

                        ers.Remove(er);
                        i--;
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

        private static bool CheckFormattedPhoneNumber(string phoneNumberText)
        {
            return RegexCache.IsMatch(phoneNumberText, BasePhoneNumbers.FormatIndicatorRegex);
        }

        private static int CountDigits(string candidateString)
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

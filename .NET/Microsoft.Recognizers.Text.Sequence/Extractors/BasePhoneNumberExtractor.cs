using Microsoft.Recognizers.Definitions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BasePhoneNumberExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_PHONE_NUMBER;

        private static List<char> SeparatorCharList => BasePhoneNumbers.SeparatorCharList.ToList();

        private static bool MatchAnyKeyWord(string text, int anchor_head, int anchor_end, int proximity = 100)
        {
            bool ContainNoCase(string toCheck, string keyword) =>
                toCheck.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;

            var BeforeAnchorSubstring = text.Substring(Math.Max(0, anchor_head - proximity), Math.Min(anchor_head, proximity));
            var AfterAnchorSubstring = text.Substring(Math.Min(anchor_end + 1, text.Length - 1), Math.Min(text.Length - 1 - anchor_end, proximity));
            return BasePhoneNumbers.PhoneNumberKeyWords.Any(o =>
                ContainNoCase(BeforeAnchorSubstring, o) || ContainNoCase(AfterAnchorSubstring, o));
        }

        public BasePhoneNumberExtractor()
        {
            var regexes = new Dictionary<Regex, string>();

            void InsertRegexes(Dictionary<string, string> o)
            {
                foreach (var regex_pair in o)
                {
                    regexes.Add(new Regex(regex_pair.Value), regex_pair.Key);
                }
            }

            InsertRegexes(BasePhoneNumbers.GeneralPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.BRPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.UKPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.DEPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.USPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.CNPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.DKPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.ITPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.NLPhoneNumberRegex);
            InsertRegexes(BasePhoneNumbers.SpecialPhoneNumberRegex);

            Regexes = regexes.ToImmutableDictionary();
        }

        public override List<ExtractResult> Extract(string text)
        {
            var result = new List<ExtractResult>();

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            var matchSource = new Dictionary<Match, string>();
            var matched = new bool[text.Length];

            //Traverse every match results to see each position in the text is matched or not.
            var collections = Regexes.ToDictionary(o => o.Key.Matches(text), p => p.Value);
            foreach (var collection in collections)
            {
                foreach (Match m in collection.Key)
                {
                    //if the detected part follows separator char, then it is not a phone number
                    if (m.Index != 0)
                    {
                        var ch = text[(int)(m.Index - 1)];
                        if (SeparatorCharList.Contains(ch))
                        {
                            continue;
                        }
                    }
                    for (var j = 0; j < m.Length; j++)
                    {
                        matched[m.Index + j] = true;
                    }

                    // Keep Source Data for extra information
                    matchSource.Add(m, collection.Value);
                }
            }

            //Form the extracted results from all the matched intervals in the text and score the results.
            var lastNotMatched = -1;
            for (var i = 0; i < text.Length; i++)
            {
                if (matched[i])
                {
                    if (i + 1 == text.Length || !matched[i + 1])
                    {
                        var start = lastNotMatched + 1;
                        var length = i - lastNotMatched;
                        var substr = text.Substring(start, length);

                        double confidence_loss = 1;
                        //In this way, we first choose the longest matched sequences and then calculate score among them
                        foreach (var matchCase in matchSource)
                        {
                            if (matchCase.Key.Index == start && matchCase.Key.Length == length)
                            {
                                confidence_loss = confidence_loss * (1 - BasePhoneNumbers.PhoneNumberFormatConfidence[matchCase.Value]);
                            }
                        }
                        int proximity = (int)BasePhoneNumbers.PhoneNumberScoreParameters["MatchProximity"];
                        if (MatchAnyKeyWord(text, start, start + length - 1, proximity))
                        {
                            confidence_loss = confidence_loss / BasePhoneNumbers.PhoneNumberScoreParameters["MatchAward"];
                        }

                        result.Add(new ExtractResult
                        {
                            Start = start,
                            Length = length,
                            Text = substr,
                            Type = ExtractType,
                            Data = null,
                            Score = 1 - confidence_loss
                        });

                    }
                }
                else
                {
                    lastNotMatched = i;
                }
            }
            return result;
        }
    }
}

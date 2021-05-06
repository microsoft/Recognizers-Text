using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseQuotedTextExtractor : BaseSequenceExtractor
    {
        private QuotedTextConfiguration config;

        public BaseQuotedTextExtractor(QuotedTextConfiguration config)
        {
            this.config = config;

            var regexes = new Dictionary<Regex, string>
            {
                {
                    config.QuotedTextRegex1,
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    config.QuotedTextRegex2,
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    config.QuotedTextRegex3,
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    config.QuotedTextRegex4,
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    config.QuotedTextRegex5,
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    config.QuotedTextRegex6,
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    config.QuotedTextRegex7,
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    config.QuotedTextRegex8,
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    config.QuotedTextRegex9,
                    Constants.QUOTED_TEXT_REGEX
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_QUOTED_TEXT;

        public override List<ExtractResult> Extract(string text)
        {
            var result = new List<ExtractResult>();

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            var matchSource = new Dictionary<Match, string>();
            var matched = new bool[text.Length];

            // Traverse every match results to see each position in the text is matched or not.
            var collections = Regexes.ToDictionary(o => o.Key.Matches(text), p => p.Value);
            foreach (var collection in collections)
            {
                for (int k = 0; k < text.Length; k++)
                {
                    matched[k] = false;
                }

                foreach (Match m in collection.Key)
                {
                    if (IsValidMatch(m))
                    {
                        for (var j = 0; j < m.Length; j++)
                        {
                            matched[m.Index + j] = true;
                        }

                        // Keep Source Data for extra information
                        matchSource.Add(m, collection.Value);
                    }
                }

                GetResult(matched, text, matchSource, result);
            }

            // Form the extracted results mark all the matched intervals in the text.
            return PostFilter(result);
        }

        public void GetResult(bool[] matched, string text, Dictionary<Match, string> matchSource, List<ExtractResult> result)
        {
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
                        bool MatchFunc(Match o) => o.Index == start && o.Length == length;

                        if (matchSource.Keys.Any(MatchFunc))
                        {
                            var srcMatch = matchSource.Keys.First(MatchFunc);
                            result.Add(new ExtractResult
                            {
                                Start = start,
                                Length = length,
                                Text = substr,
                                Type = ExtractType,
                                Data = matchSource.ContainsKey(srcMatch) ? matchSource[srcMatch] : null,
                            });
                        }
                    }
                }
                else
                {
                    lastNotMatched = i;
                }
            }
        }
    }
}
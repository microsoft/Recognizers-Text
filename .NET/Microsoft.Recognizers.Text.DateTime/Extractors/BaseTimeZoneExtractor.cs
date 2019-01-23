using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeZoneExtractor : IDateTimeZoneExtractor
    {
        private static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEZONE; // "TimeZone";

        private readonly ITimeZoneExtractorConfiguration config;

        public BaseTimeZoneExtractor(ITimeZoneExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject reference)
        {
            var tokens = new List<Token>();

            var normalizedText = QueryProcessor.RemoveDiacritics(text);

            tokens.AddRange(MatchTimeZones(normalizedText));
            tokens.AddRange(MatchLocationTimes(normalizedText));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public List<ExtractResult> RemoveAmbiguousTimezone(List<ExtractResult> ers)
        {
            ers.RemoveAll(o => config.AmbiguousTimezoneList.Contains(o.Text.ToLowerInvariant()));
            return ers;
        }

        private IEnumerable<Token> MatchLocationTimes(string text)
        {
            var ret = new List<Token>();

            if (config.LocationTimeSuffixRegex == null)
            {
                return ret;
            }

            var timeMatch = config.LocationTimeSuffixRegex.Matches(text);

            if (timeMatch.Count != 0)
            {
                var lastMatchIndex = timeMatch[timeMatch.Count - 1].Index;
                var matches = config.LocationMatcher.Find(text.Substring(0, lastMatchIndex).ToLowerInvariant());
                var locationMatches = MatchingUtil.RemoveSubMatches(matches);

                var i = 0;
                foreach (Match match in timeMatch)
                {
                    var hasCityBefore = false;

                    while (i < locationMatches.Count && locationMatches[i].End <= match.Index)
                    {
                        hasCityBefore = true;
                        i++;

                        if (i == locationMatches.Count)
                        {
                            break;
                        }
                    }

                    if (hasCityBefore && locationMatches[i - 1].End == match.Index)
                    {
                        ret.Add(new Token(locationMatches[i - 1].Start, match.Index + match.Length));
                    }

                    if (i == locationMatches.Count)
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        private List<Token> MatchTimeZones(string text)
        {
            var ret = new List<Token>();

            // Direct UTC matches
            var directUtc = this.config.DirectUtcRegex.Matches(text);
            foreach (Match match in directUtc)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            var matches = this.config.TimeZoneMatcher.Find(text);
            foreach (MatchResult<string> match in matches)
            {
                ret.Add(new Token(match.Start, match.Start + match.Length));
            }

            return ret;
        }
    }
}
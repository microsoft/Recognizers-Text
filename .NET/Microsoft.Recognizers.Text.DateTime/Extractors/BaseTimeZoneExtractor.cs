using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeZoneExtractor : IDateTimeZoneExtractor
    {
        private const string ExtractorName = Constants.SYS_DATETIME_TIMEZONE; // "TimeZone";

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

            // If normalized and original texts have different lengths, re-calculate indices
            var reIndex = text.Length > normalizedText.Length;

            tokens.AddRange(MatchTimeZones(normalizedText, text, reIndex));
            tokens.AddRange(MatchLocationTimes(normalizedText, tokens, text, reIndex));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public List<ExtractResult> RemoveAmbiguousTimezone(List<ExtractResult> ers)
        {
            ers.RemoveAll(o => config.AmbiguousTimezoneList.Contains(o.Text));
            return ers;
        }

        private IEnumerable<Token> MatchLocationTimes(string text, List<Token> tokens, string originalText, bool reIndex)
        {
            var ret = new List<Token>();

            if (config.LocationTimeSuffixRegex is null)
            {
                return ret;
            }

            var timeMatch = config.LocationTimeSuffixRegex.Matches(text);

            // Before calling a Find() in location matcher, check if all the matched suffixes by
            // LocationTimeSuffixRegex are already inside tokens extracted by TimeZone matcher.
            // If so, don't call the Find() as they have been extracted by TimeZone matcher, otherwise, call it.
            bool isAllSuffixInsideTokens = true;

            foreach (Match match in timeMatch)
            {
                bool isInside = false;
                foreach (Token token in tokens)
                {
                    if (token.Start <= match.Index && token.End >= match.Index + match.Length)
                    {
                        isInside = true;
                        break;
                    }
                }

                if (!isInside)
                {
                    isAllSuffixInsideTokens = false;
                }

                if (!isAllSuffixInsideTokens)
                {
                    break;
                }
            }

            if (timeMatch.Count != 0 && !isAllSuffixInsideTokens)
            {
                var lastMatchIndex = timeMatch[timeMatch.Count - 1].Index;

                var matches = config.LocationMatcher.Find(text.Substring(0, lastMatchIndex));
                var locationMatches = MatchingUtil.RemoveSubMatches(matches);

                if (reIndex)
                {
                    foreach (var locMatch in locationMatches)
                    {
                        locMatch.Start = originalText.IndexOf(locMatch.CanonicalValues.FirstOrDefault(), locMatch.Start, StringComparison.Ordinal);
                    }
                }

                var i = 0;
                foreach (Match match in timeMatch)
                {
                    var hasCityBefore = false;

                    var index = match.Index;

                    if (reIndex)
                    {
                        index = originalText.IndexOf(match.Value, match.Index, StringComparison.Ordinal);
                    }

                    while (i < locationMatches.Count && locationMatches[i].End <= index)
                    {
                        hasCityBefore = true;
                        i++;

                        if (i == locationMatches.Count)
                        {
                            break;
                        }
                    }

                    if (hasCityBefore && locationMatches[i - 1].End == index)
                    {
                        ret.Add(new Token(locationMatches[i - 1].Start, index + match.Length));
                    }

                    if (i == locationMatches.Count)
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        private List<Token> MatchTimeZones(string text, string originalText, bool reIndex)
        {
            var ret = new List<Token>();

            // Direct UTC matches
            if (this.config.DirectUtcRegex != null)
            {
                var directUtc = this.config.DirectUtcRegex.Matches(text);
                foreach (Match match in directUtc)
                {

                    var index = match.Index;

                    if (reIndex)
                    {
                        index = originalText.IndexOf(match.Value, match.Index, StringComparison.Ordinal);
                    }

                    ret.Add(new Token(index, index + match.Length));

                }

                var matches = this.config.TimeZoneMatcher.Find(text);
                foreach (MatchResult<string> match in matches)
                {
                    var index = match.Start;

                    if (reIndex)
                    {
                        index = originalText.IndexOf(match.CanonicalValues.FirstOrDefault(), match.Start, StringComparison.Ordinal);
                    }

                    ret.Add(new Token(index, index + match.Length));
                }
            }

            return ret;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Utilities;
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
            var normalizedText = FormatUtility.RemoveDiacritics(text);
            var tokens = new List<Token>();
            tokens.AddRange(TimeZoneMatch(normalizedText));
            tokens.AddRange(CityTimeMatch(normalizedText));
            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public List<ExtractResult> RemoveAmbiguousTimezone(List<ExtractResult> ers)
        {
            ers.RemoveAll(o => config.AmbiguousTimezoneList.Contains(o.Text.ToLowerInvariant()));
            return ers;
        }

        private IEnumerable<Token> CityTimeMatch(string text)
        {
            var ret = new List<Token>();

            if (config.CityTimeSuffixRegex == null)
            {
                return ret;
            }

            var timeMatch = config.CityTimeSuffixRegex.Matches(text);

            if (timeMatch.Count != 0)
            {
                var lastMatchIndex = timeMatch[timeMatch.Count - 1].Index;
                var cityMatchResult = config.CityMatcher.Find(text.Substring(0, lastMatchIndex).ToLowerInvariant()).ToList();

                var i = 0;
                foreach (Match match in timeMatch)
                {
                    var hasCityBefore = false;

                    while (i < cityMatchResult.Count && cityMatchResult[i].End <= match.Index)
                    {
                        hasCityBefore = true;
                        i++;

                        if (i == cityMatchResult.Count)
                        {
                            break;
                        }
                    }

                    if (hasCityBefore && cityMatchResult[i - 1].End == match.Index)
                    {
                        ret.Add(new Token(cityMatchResult[i - 1].Start, match.Index + match.Length));
                    }

                    if (i == cityMatchResult.Count)
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        private List<Token> TimeZoneMatch(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.TimeZoneRegexes)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length));
                }
            }
            return ret;
        }
    }
}
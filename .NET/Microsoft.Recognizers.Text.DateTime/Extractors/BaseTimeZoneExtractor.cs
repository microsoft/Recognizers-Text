using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeZoneExtractor : IDateTimeExtractor
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
            tokens.AddRange(TimeZoneMatch(text));
            tokens.AddRange(CityTimeMatch(text));
            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        private IEnumerable<Token> CityTimeMatch(string text)
        {
            var ret = new List<Token>();
            var cityMatchResult = config.CityStringMatcher.Find(text.ToLowerInvariant());

            foreach (var result in cityMatchResult)
            {
                var afterString = text.Substring(result.End);
                var suffixMatch = config.CityTimeSuffixRegex.Match(afterString);

                if (suffixMatch.Success && suffixMatch.Index == 0)
                {
                    ret.Add(new Token(result.Start, result.End + suffixMatch.Length)); 
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
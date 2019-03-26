using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseSetExtractorConfiguration : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.SetUnitRegex, RegexOptions.Singleline);

        public static readonly Regex EachUnitRegex = new Regex(DateTimeDefinitions.SetEachUnitRegex, RegexOptions.Singleline);

        public static readonly Regex EachPrefixRegex = new Regex(DateTimeDefinitions.SetEachPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.SetLastRegex, RegexOptions.Singleline);

        public static readonly Regex EachDayRegex = new Regex(DateTimeDefinitions.SetEachDayRegex, RegexOptions.Singleline);

        private static readonly ChineseDurationExtractorConfiguration DurationExtractor = new ChineseDurationExtractorConfiguration();
        private static readonly ChineseTimeExtractorConfiguration TimeExtractor = new ChineseTimeExtractorConfiguration();
        private static readonly ChineseDateExtractorConfiguration DateExtractor = new ChineseDateExtractorConfiguration();
        private static readonly ChineseDateTimeExtractorConfiguration DateTimeExtractor = new ChineseDateTimeExtractorConfiguration();

        public static List<Token> MatchEachDuration(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();

            var ers = DurationExtractor.Extract(text, referenceTime);
            foreach (var er in ers)
            {
                // "each last summer" doesn't make sense
                if (LastRegex.IsMatch(er.Text))
                {
                    continue;
                }

                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = EachPrefixRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, er.Start + er.Length ?? 0));
                }
            }

            return ret;
        }

        public static List<Token> MatchEachUnit(string text)
        {
            var ret = new List<Token>();

            // handle "each month"
            var matches = EachUnitRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        public static List<Token> TimeEveryday(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var ers = TimeExtractor.Extract(text, referenceTime);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = EachDayRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length + (er.Length ?? 0)));
                }
            }

            return ret;
        }

        public static List<Token> MatchEachDate(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var ers = DateExtractor.Extract(text, referenceTime);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = EachPrefixRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length + (er.Length ?? 0)));
                }
            }

            return ret;
        }

        public static List<Token> MatchEachDateTime(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var ers = DateTimeExtractor.Extract(text, referenceTime);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = EachPrefixRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length + (er.Length ?? 0)));
                }
            }

            return ret;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchEachUnit(text));
            tokens.AddRange(MatchEachDuration(text, referenceTime));
            tokens.AddRange(TimeEveryday(text, referenceTime));
            tokens.AddRange(MatchEachDate(text, referenceTime));
            tokens.AddRange(MatchEachDateTime(text, referenceTime));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }
    }
}
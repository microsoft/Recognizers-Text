using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class SetExtractorChs : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.SetUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachUnitRegex = new Regex(DateTimeDefinitions.SetEachUnitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachPrefixRegex = new Regex(DateTimeDefinitions.SetEachPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.SetLastRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachDayRegex = new Regex(DateTimeDefinitions.SetEachDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly DurationExtractorChs DurationExtractor = new DurationExtractorChs();
        private static readonly TimeExtractorChs TimeExtractor = new TimeExtractorChs();
        private static readonly DateExtractorChs DateExtractor = new DateExtractorChs();
        private static readonly DateTimeExtractorChs DateTimeExtractor = new DateTimeExtractorChs();

        public List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchEachUnit(text));
            tokens.AddRange(MatchEachDuration(text));
            tokens.AddRange(TimeEveryday(text));
            tokens.AddRange(MatchEachDate(text));
            tokens.AddRange(MatchEachDateTime(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public List<Token> MatchEachDuration(string text)
        {
            var ret = new List<Token>();

            var ers = DurationExtractor.Extract(text);
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

        public List<Token> MatchEachUnit(string text)
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

        public List<Token> TimeEveryday(string text)
        {
            var ret = new List<Token>();
            var ers = TimeExtractor.Extract(text);
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

        public List<Token> MatchEachDate(string text)
        {
            var ret = new List<Token>();
            var ers = DateExtractor.Extract(text);
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

        public List<Token> MatchEachDateTime(string text)
        {
            var ret = new List<Token>();
            var ers = DateTimeExtractor.Extract(text);
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
    }
}
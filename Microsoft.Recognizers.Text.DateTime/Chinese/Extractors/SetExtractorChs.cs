using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class SetExtractorChs : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        public static readonly Regex UnitRegex =
            new Regex(
                @"(?<unit>年|月|周|星期|日|天|小时|时|分钟|分|秒钟|秒)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public static readonly Regex EachUnitRegex = new Regex(
            $@"(?<each>(每个|每一|每)\s*{UnitRegex})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachPrefixRegex = new Regex(@"(?<each>(每)\s*$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(@"(?<last>last|this|next)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EachDayRegex = new Regex(@"(每|每一)(天|日)\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly DurationExtractorChs _durationExtractor = new DurationExtractorChs();
        private static readonly TimeExtractorChs _timeExtractor = new TimeExtractorChs();
        private static readonly DateExtractorChs _dateExtractor = new DateExtractorChs();
        private static readonly DateTimeExtractorChs _dateTimeExtractor = new DateTimeExtractorChs();

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

            var ers = _durationExtractor.Extract(text);
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
            var ers = _timeExtractor.Extract(text);
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
            var ers = _dateExtractor.Extract(text);
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
            var ers = _dateTimeExtractor.Extract(text);
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
using Microsoft.Recognizers.Text.DateTime.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Extractors
{
    public class BaseSetExtractor : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        private readonly ISetExtractorConfiguration config;

        public BaseSetExtractor(ISetExtractorConfiguration config)
        {
            this.config = config;
        }

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

            var ers = this.config.DurationExtractor.Extract(text);
            foreach (var er in ers)
            {
                // "each last summer" doesn't make sense
                if (this.config.LastRegex.IsMatch(er.Text))
                {
                    continue;
                }

                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = this.config.EachPrefixRegex.Match(beforeStr);
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
            // handle "daily", "monthly"
            var matches = this.config.PeriodicRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            // handle "each month"
            matches = this.config.EachUnitRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        public virtual List<Token> TimeEveryday(string text)
        {
            var ret = new List<Token>();
            var ers = this.config.TimeExtractor.Extract(text);
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                if (string.IsNullOrEmpty(afterStr) && this.config.BeforeEachDayRegex != null)
                {
                    var beforeStr = text.Substring(0, er.Start ?? 0);
                    var beforeMatch = this.config.BeforeEachDayRegex.Match(beforeStr);
                    if (beforeMatch.Success)
                    {
                        ret.Add(new Token(beforeMatch.Index, (er.Start + er.Length ?? 0)));
                    }
                }
                else
                {
                    var match = this.config.EachDayRegex.Match(afterStr);
                    if (match.Success)
                    {
                        ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                    }
                }
            }
            return ret;
        }

        public List<Token> MatchEachDate(string text)
        {
            var ret = new List<Token>();
            var ers = this.config.DateExtractor.Extract(text);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = this.config.EachPrefixRegex.Match(beforeStr);
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
            var ers = this.config.DateTimeExtractor.Extract(text);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);
                var match = this.config.EachPrefixRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, match.Index + match.Length + (er.Length ?? 0)));
                }
            }
            return ret;
        }
    }
}

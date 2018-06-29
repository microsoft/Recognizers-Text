using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseSetExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_SET;

        private readonly ISetExtractorConfiguration config;

        public BaseSetExtractor(ISetExtractorConfiguration config)
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
            tokens.AddRange(MatchEachUnit(text));
            tokens.AddRange(MatchEachDuration(text, reference));
            tokens.AddRange(TimeEveryday(text, reference));
            tokens.AddRange(MatchEach(config.DateExtractor, text, reference));
            tokens.AddRange(MatchEach(config.TimeExtractor, text, reference));
            tokens.AddRange(MatchEach(config.DateTimeExtractor, text, reference));
            tokens.AddRange(MatchEach(config.DatePeriodExtractor, text, reference));
            tokens.AddRange(MatchEach(config.TimePeriodExtractor, text, reference));
            tokens.AddRange(MatchEach(config.DateTimePeriodExtractor, text, reference));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        public List<Token> MatchEachDuration(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var ers = this.config.DurationExtractor.Extract(text, reference);
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

        public virtual List<Token> TimeEveryday(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.TimeExtractor.Extract(text, reference);
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

        public List<Token> MatchEach(IDateTimeExtractor extractor, string text, DateObject reference)
        {
            var ret = new List<Token>();
            var matches = config.SetEachRegex.Matches(text);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var trimedText = text.Remove(match.Index, match.Length);
                    var ers = extractor.Extract(trimedText, reference);
                    foreach (var er in ers)
                    {
                        if (er.Start <= match.Index && (er.Start+er.Length) > match.Index)
                        {
                            ret.Add(new Token(er.Start ?? 0, (er.Start + match.Length + er.Length) ?? 0));
                        }
                    }
                }
            }

            // handle "Mondays"
            matches = this.config.SetWeekDayRegex.Matches(text);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var trimedText = text.Remove(match.Index, match.Length);
                    trimedText = trimedText.Insert(match.Index, match.Groups["weekday"].ToString());

                    var ers = extractor.Extract(trimedText, reference);
                    foreach (var er in ers)
                    {
                        if (er.Start <= match.Index && er.Text.Contains(match.Groups["weekday"].Value))
                        {
                            var len = (er.Length ?? 0) + 1;
                            if (match.Groups[Constants.PrefixGroupName].ToString() != string.Empty)
                            {
                                len += match.Groups[Constants.PrefixGroupName].ToString().Length;
                            }
                            ret.Add(new Token(er.Start ?? 0, er.Start + len ?? 0));
                        }
                    }
                }
            }

            return ret;
        }
    }
}

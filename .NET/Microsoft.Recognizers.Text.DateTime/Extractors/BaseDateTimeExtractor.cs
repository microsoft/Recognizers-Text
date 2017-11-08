using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIME; // "DateTime";

        private readonly IDateTimeExtractorConfiguration config;

        public BaseDateTimeExtractor(IDateTimeExtractorConfiguration config)
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
            tokens.AddRange(MergeDateAndTime(text, reference));
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(TimeOfTodayBefore(text, reference));
            tokens.AddRange(TimeOfTodayAfter(text, reference));
            tokens.AddRange(SpecialTimeOfDate(text, reference));
            tokens.AddRange(DurationWithBeforeAndAfter(text, reference));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // match now
        public List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();
            text = text.Trim().ToLower();

            // handle "now"
            var matches = this.config.NowRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        // merge a Date entity and a Time entity, like "at 7 tomorrow"
        public List<Token> MergeDateAndTime(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.DatePointExtractor.Extract(text, reference);
            if (ers.Count == 0)
            {
                return ret;
            }

            ers.AddRange(this.config.TimePointExtractor.Extract(text, reference));
            if (ers.Count < 2)
            {
                return ret;
            }

            ers = ers.OrderBy(o => o.Start).ToList();

            var i = 0;
            while (i < ers.Count - 1)
            {
                var j = i + 1;
                while (j < ers.Count && ers[i].IsOverlap(ers[j]))
                {
                    j++;
                }

                if (j >= ers.Count)
                {
                    break;
                }

                if (ers[i].Type.Equals(Constants.SYS_DATETIME_DATE) && ers[j].Type.Equals(Constants.SYS_DATETIME_TIME) ||
                    ers[i].Type.Equals(Constants.SYS_DATETIME_TIME) && ers[j].Type.Equals(Constants.SYS_DATETIME_DATE))
                {
                    var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                    var middleEnd = ers[j].Start ?? 0;
                    if (middleBegin > middleEnd)
                    {
                        i = j + 1;
                        continue;
                    }

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLower();
                    if (this.config.IsConnector(middleStr))
                    {
                        var begin = ers[i].Start ?? 0;
                        var end = (ers[j].Start ?? 0) + (ers[j].Length ?? 0);
                        ret.Add(new Token(begin, end));
                    }

                    i = j + 1;
                    continue;
                }
                i = j;
            }

            // handle "in the afternoon" at the end of entity
            for (var idx = 0; idx < ret.Count; idx++)
            {
                var afterStr = text.Substring(ret[idx].End);
                var match = this.config.SuffixRegex.Match(afterStr);
                if (match.Success)
                {
                    ret[idx] = new Token(ret[idx].Start, ret[idx].End + match.Length);
                }
            }

            return ret;
        }

        // parse a specific time of today, tonight, this afternoon, like "seven this afternoon"
        public List<Token> TimeOfTodayAfter(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var ers = this.config.TimePointExtractor.Extract(text, reference);

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                if (string.IsNullOrEmpty(afterStr))
                {
                    continue;
                }

                var match = this.config.TimeOfTodayAfterRegex.Match(afterStr);
                if (match.Success)
                {
                    var begin = er.Start ?? 0;
                    var end = (er.Start ?? 0) + (er.Length ?? 0) + match.Length;
                    ret.Add(new Token(begin, end));
                }
            }

            var matches = this.config.SimpleTimeOfTodayAfterRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        // parse a specific time of today, tonight, this afternoon, "this afternoon at 7"
        public List<Token> TimeOfTodayBefore(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.TimePointExtractor.Extract(text, reference);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);

                // handle "this morningh at 7am"
                var innerMatch = this.config.TimeOfDayRegex.Match(er.Text);
                if (innerMatch.Success && innerMatch.Index == 0)
                {
                    beforeStr = text.Substring(0, (er.Start ?? 0) + innerMatch.Length);
                }

                if (string.IsNullOrEmpty(beforeStr))
                {
                    continue;
                }

                var match = this.config.TimeOfTodayBeforeRegex.Match(beforeStr);
                if (match.Success)
                {
                    var begin = match.Index;
                    var end = er.Start + er.Length ?? 0;
                    ret.Add(new Token(begin, end));
                }
            }

            var matches = this.config.SimpleTimeOfTodayBeforeRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        public List<Token> SpecialTimeOfDate(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.DatePointExtractor.Extract(text, reference);

            // handle "the end of the day"
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);

                var match = this.config.TheEndOfRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, er.Start + er.Length ?? 0));
                }
                else
                {
                    var afterStr = text.Substring(er.Start + er.Length ?? 0);

                    match = this.config.TheEndOfRegex.Match(afterStr);
                    if (match.Success)
                    {
                        ret.Add(new Token(er.Start ?? 0, er.Start + er.Length + match.Index + match.Length ?? 0));
                    }
                }
            }

            return ret;
        }

        // process case like "two minutes ago" "three hours later"
        private List<Token> DurationWithBeforeAndAfter(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var durationEr = config.DurationExtractor.Extract(text, reference);
            foreach (var er in durationEr)
            {
                var match = config.UnitRegex.Match(er.Text);
                if (!match.Success)
                {
                    continue;
                }

                ret = AgoLaterUtil.ExtractorDurationWithBeforeAndAfter(text, er, ret, config.UtilityConfiguration);
            }

            return ret;
        }
    }
}

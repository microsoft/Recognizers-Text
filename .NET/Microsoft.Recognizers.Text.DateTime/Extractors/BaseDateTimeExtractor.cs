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
            tokens.AddRange(SpecialTimeOfDay(text, reference));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // Match "now"
        public List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();
            text = text.Trim().ToLower();

            // Handle "now"
            var matches = this.config.NowRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        // Merge a Date entity and a Time entity, like "at 7 tomorrow"
        public List<Token> MergeDateAndTime(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var dateErs = this.config.DatePointExtractor.Extract(text, reference);
            if (dateErs.Count == 0)
            {
                return ret;
            }

            var timeErs = this.config.TimePointExtractor.Extract(text, reference);
            var timeNumMatches = this.config.NumberAsTimeRegex.Matches(text);
            if (timeErs.Count == 0 && timeNumMatches.Count == 0)
            {
                return ret;
            }

            var ers = dateErs;
            ers.AddRange(timeErs);

            // handle cases which use numbers as time points
            // only enabled in CalendarMode
            if ((this.config.Options & DateTimeOptions.CalendarMode) != 0)
            {
                var numErs = new List<ExtractResult>();
                for (var idx = 0; idx < timeNumMatches.Count; idx++)
                {
                    var match = timeNumMatches[idx];
                    var node = new ExtractResult
                    {
                        Start = match.Index,
                        Length = match.Length,
                        Text = match.Value,
                        Type = Number.Constants.SYS_NUM_INTEGER
                    };
                    numErs.Add(node);
                }
                ers.AddRange(numErs);
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
                    ers[i].Type.Equals(Constants.SYS_DATETIME_TIME) && ers[j].Type.Equals(Constants.SYS_DATETIME_DATE) ||
                    ers[i].Type.Equals(Constants.SYS_DATETIME_DATE) && ers[j].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                {
                    var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                    var middleEnd = ers[j].Start ?? 0;
                    if (middleBegin > middleEnd)
                    {
                        i = j + 1;
                        continue;
                    }

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLower();
                    var valid = false;
                    // for cases like "tomorrow 3",  "tomorrow at 3"
                    if (ers[j].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                    {
                        var match = this.config.DateNumberConnectorRegex.Match(middleStr);
                        if (string.IsNullOrEmpty(middleStr) || match.Success)
                        {
                            valid = true;
                        }
                    }
                    else
                    {
                        if (this.config.IsConnector(middleStr))
                        {
                            valid = true;
                        }
                    }

                    if (valid)
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

            // Handle "in the afternoon" at the end of entity
            for (var idx = 0; idx < ret.Count; idx++)
            {
                var afterStr = text.Substring(ret[idx].End);
                var match = this.config.SuffixRegex.Match(afterStr);
                if (match.Success)
                {
                    ret[idx] = new Token(ret[idx].Start, ret[idx].End + match.Length);
                }
            }

            // Handle "day" prefixes
            for (var idx = 0; idx < ret.Count; idx++)
            {
                var beforeStr = text.Substring(0, ret[idx].Start);
                var match = this.config.UtilityConfiguration.CommonDatePrefixRegex.Match(beforeStr);
                if (match.Success)
                {
                    ret[idx] = new Token(ret[idx].Start - match.Length, ret[idx].End);
                }
            }

            return ret;
        }

        // Parses a specific time of today, tonight, this afternoon, like "seven this afternoon"
        public List<Token> TimeOfTodayAfter(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var ers = this.config.TimePointExtractor.Extract(text, reference);

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                if (string.IsNullOrEmpty(afterStr))
                {
                    continue; //@here
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

        // Parse a specific time of today, tonight, this afternoon, "this afternoon at 7"
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

        // Special case for 'the end of today'
        public List<Token> SpecialTimeOfDay(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var match = this.config.TheEndOfRegex.Match(text);
            if (match.Success)
            {
                ret.Add(new Token(match.Index, text.Length));
            }

            return ret;
        }

        // Process case like "two minutes ago" "three hours later"
        private List<Token> DurationWithBeforeAndAfter(string text, DateObject reference)
        {
            var ret = new List<Token>();

            var durationEr = config.DurationExtractor.Extract(text, reference);
            foreach (var er in durationEr)
            {
                // if it is a multiple duration and its type is equal to Date then skip it.
                if (er.Data != null && er.Data.ToString() == Constants.MultipleDuration_Date)
                {
                    continue;
                }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

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

        // Match "now"
        public List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();

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
                        Type = Number.Constants.SYS_NUM_INTEGER,
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

                if ((ers[i].Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal) &&
                     ers[j].Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal)) ||
                    (ers[i].Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal) &&
                     ers[j].Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal)) ||
                    (ers[i].Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal) &&
                     ers[j].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal)))
                {

                    var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                    var middleEnd = ers[j].Start ?? 0;
                    if (middleBegin > middleEnd)
                    {
                        i = j + 1;
                        continue;
                    }

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();
                    var valid = false;

                    // for cases like "tomorrow 3",  "tomorrow at 3"
                    if (ers[j].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                    {
                        var match = this.config.DateNumberConnectorRegex.Match(middleStr);
                        if (string.IsNullOrEmpty(middleStr) || match.Success)
                        {
                            valid = true;
                        }
                    }
                    else
                    {
                        // For case like "3 pm or later on monday"
                        var match = this.config.SuffixAfterRegex.Match(middleStr);
                        if (match.Success)
                        {
                            middleStr = middleStr.Substring(match.Index + match.Length, middleStr.Length - match.Length).Trim();
                        }

                        if (!(match.Success && middleStr.Length == 0))
                        {
                            if (this.config.IsConnector(middleStr))
                            {
                                valid = true;
                            }
                        }
                    }

                    if (valid)
                    {
                        var begin = ers[i].Start ?? 0;
                        var end = (ers[j].Start ?? 0) + (ers[j].Length ?? 0);

                        ExtendWithDateTimeAndYear(ref begin, ref end, text, reference);

                        ret.Add(new Token(begin, end));
                        i = j + 1;
                        continue;
                    }
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

        // Parse a specific time of today, tonight, this afternoon, "this afternoon at 7"
        public List<Token> TimeOfTodayBefore(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.TimePointExtractor.Extract(text, reference);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);

                // handle "this morning at 7am"
                var innerMatch = this.config.TimeOfDayRegex.MatchBegin(er.Text, trim: true);

                if (innerMatch.Success)
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

            // Handle "the end of the day"
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);

                var match = this.config.SpecificEndOfRegex.MatchEnd(beforeStr, trim: true);
                if (match.Success)
                {
                    ret.Add(new Token(match.Index, er.Start + er.Length ?? 0));
                }
                else
                {
                    var afterStr = text.Substring(er.Start + er.Length ?? 0);

                    match = this.config.SpecificEndOfRegex.MatchBegin(afterStr, trim: true);
                    if (match.Success)
                    {
                        ret.Add(new Token(er.Start ?? 0, er.Start + er.Length + match.Index + match.Length ?? 0));
                    }
                }
            }

            // Handle "eod, end of day"
            MatchCollection eod = this.config.UnspecificEndOfRegex.Matches(text);
            foreach (Match match in eod)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
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
                if (er.Data != null && er.Data.ToString() is Constants.MultipleDuration_Date)
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

        // Handle case like "Wed Oct 26 15:50:06 2016" which year and month separated by time.
        private void ExtendWithDateTimeAndYear(ref int startIndex, ref int endIndex, string text, DateObject reference)
        {

            // Check whether there's a year behind.
            var suffix = text.Substring(endIndex);
            var matchYear = this.config.YearSuffix.Match(suffix);
            if (matchYear.Success && matchYear.Index == 0)
            {
                var checkYear = config.DatePointExtractor.GetYearFromText(this.config.YearRegex.Match(text));
                var year = config.DatePointExtractor.GetYearFromText(matchYear);
                if (year >= Constants.MinYearNum && year <= Constants.MaxYearNum && checkYear == year)
                {
                    endIndex += matchYear.Length;
                }
            }
        }
    }
}

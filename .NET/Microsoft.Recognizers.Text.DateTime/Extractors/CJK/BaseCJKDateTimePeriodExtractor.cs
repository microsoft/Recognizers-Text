// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDateTimePeriodExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIMEPERIOD;

        private readonly ICJKDateTimePeriodExtractorConfiguration config;

        public BaseCJKDateTimePeriodExtractor(ICJKDateTimePeriodExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            // Date and time Extractions should be extracted from the text only once, and shared in the methods below, passed by value
            var dateErs = this.config.SingleDateExtractor.Extract(text, referenceTime);
            var timeErs = this.config.SingleTimeExtractor.Extract(text, referenceTime);
            var timeRangeErs = this.config.TimePeriodExtractor.Extract(text, referenceTime);
            var dateTimeErs = this.config.SingleDateTimeExtractor.Extract(text, referenceTime);

            var tokens = new List<Token>();
            tokens.AddRange(MergeDateAndTimePeriod(text, new List<ExtractResult>(dateErs), new List<ExtractResult>(timeRangeErs)));
            tokens.AddRange(MergeTwoTimePoints(text, new List<ExtractResult>(dateTimeErs), new List<ExtractResult>(timeErs)));
            tokens.AddRange(MatchDuration(text, referenceTime));
            tokens.AddRange(MatchRelativeUnit(text));
            tokens.AddRange(MatchNumberWithUnit(text));
            tokens.AddRange(MatchNight(text, referenceTime));
            tokens.AddRange(MergeDateWithTimePeriodSuffix(text, new List<ExtractResult>(dateErs), new List<ExtractResult>(timeErs)));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // merge Date and Time period
        private List<Token> MergeDateAndTimePeriod(string text, List<ExtractResult> dateErs, List<ExtractResult> timeRangeErs)
        {
            var ret = new List<Token>();
            var timePoints = new List<ExtractResult>();

            // handle the overlap problem
            var j = 0;
            for (var i = 0; i < dateErs.Count; i++)
            {
                timePoints.Add(dateErs[i]);
                while (j < timeRangeErs.Count && timeRangeErs[j].Start + timeRangeErs[j].Length <= dateErs[i].Start)
                {
                    timePoints.Add(timeRangeErs[j]);
                    j++;
                }

                while (j < timeRangeErs.Count && timeRangeErs[j].IsOverlap(dateErs[i]))
                {
                    j++;
                }
            }

            for (; j < timeRangeErs.Count; j++)
            {
                timePoints.Add(timeRangeErs[j]);
            }

            timePoints = timePoints.OrderBy(o => o.Start).ToList();

            // merge {Date} {TimePeriod}
            var idx = 0;
            while (idx < timePoints.Count - 1)
            {
                if (timePoints[idx].Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal) &&
                    timePoints[idx + 1].Type.Equals(Constants.SYS_DATETIME_TIMEPERIOD, StringComparison.Ordinal))
                {
                    var middleBegin = timePoints[idx].Start + timePoints[idx].Length ?? 0;
                    var middleEnd = timePoints[idx + 1].Start ?? 0;

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();
                    if (string.IsNullOrWhiteSpace(middleStr) || this.config.PrepositionRegex.IsMatch(middleStr))
                    {
                        var periodBegin = timePoints[idx].Start ?? 0;
                        var periodEnd = (timePoints[idx + 1].Start ?? 0) + (timePoints[idx + 1].Length ?? 0);
                        ret.Add(new Token(periodBegin, periodEnd));
                        idx += 2;
                        continue;
                    }

                    idx++;
                }

                idx++;
            }

            return ret;
        }

        private List<Token> MergeTwoTimePoints(string text, List<ExtractResult> dateTimeErs, List<ExtractResult> timeErs)
        {
            var ret = new List<Token>();
            var timePoints = new List<ExtractResult>();

            // handle the overlap problem
            var j = 0;
            for (var i = 0; i < dateTimeErs.Count; i++)
            {
                timePoints.Add(dateTimeErs[i]);
                while (j < timeErs.Count && timeErs[j].Start + timeErs[j].Length <= dateTimeErs[i].Start)
                {
                    timePoints.Add(timeErs[j]);
                    j++;
                }

                while (j < timeErs.Count && timeErs[j].IsOverlap(dateTimeErs[i]))
                {
                    j++;
                }
            }

            for (; j < timeErs.Count; j++)
            {
                timePoints.Add(timeErs[j]);
            }

            timePoints = timePoints.OrderBy(o => o.Start).ToList();

            // merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
            var idx = 0;
            while (idx < timePoints.Count - 1)
            {
                // if both ends are Time. then this is a TimePeriod, not a DateTimePeriod
                if (timePoints[idx].Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal) &&
                    timePoints[idx + 1].Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
                {
                    idx++;
                    continue;
                }

                var middleBegin = timePoints[idx].Start + timePoints[idx].Length ?? 0;
                var middleEnd = timePoints[idx + 1].Start ?? 0;

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();

                // handle "{TimePoint} to {TimePoint}"
                if (this.config.TillRegex.IsExactMatch(middleStr, trim: true))
                {
                    var periodBegin = timePoints[idx].Start ?? 0;
                    var periodEnd = (timePoints[idx + 1].Start ?? 0) + (timePoints[idx + 1].Length ?? 0);

                    // handle "from"
                    var beforeStr = text.Substring(0, periodBegin);
                    if (this.config.GetFromTokenIndex(beforeStr, out int index))
                    {
                        periodBegin = index;
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }

                // handle "between {TimePoint} and {TimePoint}"
                if (this.config.HasConnectorToken(middleStr))
                {
                    var periodBegin = timePoints[idx].Start ?? 0;
                    var periodEnd = (timePoints[idx + 1].Start ?? 0) + (timePoints[idx + 1].Length ?? 0);

                    // handle "between"
                    var afterStr = text.Substring(periodEnd);
                    if (this.config.GetBetweenTokenIndex(afterStr, out int index))
                    {
                        ret.Add(new Token(periodBegin, periodEnd + index));
                        idx += 2;
                        continue;
                    }
                }

                idx++;
            }

            return ret;
        }

        private List<Token> MatchNight(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var matches = this.config.SpecificTimeOfDayRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            // Date followed by morning, afternoon
            var ers = this.config.SingleDateExtractor.Extract(text, referenceTime);
            if (ers.Count == 0)
            {
                return ret;
            }

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = this.config.TimeOfDayRegex.Match(afterStr);
                if (match.Success)
                {
                    var middleStr = afterStr.Substring(0, match.Index);
                    if (string.IsNullOrWhiteSpace(middleStr) || this.config.PrepositionRegex.IsMatch(middleStr))
                    {
                        ret.Add(new Token(er.Start ?? 0, er.Start + er.Length + match.Index + match.Length ?? 0));
                    }
                }
            }

            return ret;
        }

        // Cases like "2015年1月1日の2時以降", "On January 1, 2015 after 2:00"
        private IEnumerable<Token> MergeDateWithTimePeriodSuffix(string text, List<ExtractResult> dateErs, List<ExtractResult> timeErs)
        {
            var ret = new List<Token>();

            if (!dateErs.Any())
            {
                return ret;
            }

            if (!timeErs.Any())
            {
                return ret;
            }

            var ers = dateErs;
            ers.AddRange(timeErs);

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

                if (ers[i].Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal) &&
                    ers[j].Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
                {
                    var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                    var middleEnd = ers[j].Start ?? 0;
                    if (middleBegin > middleEnd)
                    {
                        i = j + 1;
                        continue;
                    }

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();

                    if (middleStr.Length != Constants.INVALID_CONNECTOR_CODE)
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

            return ret;
        }

        // Extract patterns that involve durations e.g. "Within 5 hours from now"
        private List<Token> MatchDuration(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var durationExtractions = config.DurationExtractor.Extract(text, reference);

            foreach (var durationExtraction in durationExtractions)
            {
                var timeUnitMatch = config.UnitRegex.Match(durationExtraction.Text);
                if (!timeUnitMatch.Success)
                {
                    continue;
                }

                var duration = new Token(durationExtraction.Start ?? 0, durationExtraction.Start + durationExtraction.Length ?? 0);
                var beforeStr = text.Substring(0, duration.Start);
                var afterStr = text.Substring(duration.Start + duration.Length);

                if (string.IsNullOrWhiteSpace(beforeStr) && string.IsNullOrWhiteSpace(afterStr))
                {
                    continue;
                }

                var startOut = -1;
                var endOut = -1;
                var match = config.FutureRegex.Match(afterStr);

                var inPrefixMatch = config.ThisRegex.Match(beforeStr);
                var inPrefix = inPrefixMatch.Success;

                if (match.Groups[Constants.WithinGroupName].Success)
                {
                    var startToken = inPrefix ? inPrefixMatch.Index : duration.Start;
                    var withinlength = match.Groups[Constants.WithinGroupName].Value.Length;
                    var endToken = duration.End + (inPrefix ? 0 : match.Index + match.Length);

                    match = config.UnitRegex.Match(text.Substring(duration.Start, duration.Length));

                    if (match.Success)
                    {
                        startOut = startToken;
                        endOut = inPrefix ? endToken + withinlength : endToken;
                    }

                    Token token = new Token(startOut, endOut);
                    ret.Add(token);
                }
            }

            return ret;
        }

        private List<Token> MatchRelativeUnit(string text)
        {
            var ret = new List<Token>();
            var matches = this.config.RestOfDateRegex.Matches(text);

            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        private List<Token> MatchNumberWithUnit(string text)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var ers = this.config.CardinalExtractor.Extract(text);

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = this.config.FollowedUnit.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    durations.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }

                match = this.config.PastRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    durations.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }
            }

            var matches = this.config.UnitRegex.Matches(text);
            foreach (Match match in matches)
            {
                durations.Add(new Token(match.Index, match.Index + match.Length));
            }

            foreach (var duration in durations)
            {
                var beforeStr = text.Substring(0, duration.Start);
                if (string.IsNullOrWhiteSpace(beforeStr))
                {
                    continue;
                }

                var match = this.config.PastRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }

                match = this.config.FutureRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(match.Index, duration.End));
                }

                match = this.config.TimePeriodLeftRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    ret.Add(new Token(match.Index, duration.End));
                }
            }

            return ret;
        }
    }
}

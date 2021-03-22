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
            var tokens = new List<Token>();
            tokens.AddRange(MergeDateAndTimePeriod(text, referenceTime));
            tokens.AddRange(MergeTwoTimePoints(text, referenceTime));
            tokens.AddRange(MatchNumberWithUnit(text));
            tokens.AddRange(MatchNight(text, referenceTime));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // merge Date and Time period
        private List<Token> MergeDateAndTimePeriod(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var er1 = this.config.SingleDateExtractor.Extract(text, referenceTime);
            var er2 = this.config.TimePeriodExtractor.Extract(text, referenceTime);
            var timePoints = new List<ExtractResult>();

            // handle the overlap problem
            var j = 0;
            for (var i = 0; i < er1.Count; i++)
            {
                timePoints.Add(er1[i]);
                while (j < er2.Count && er2[j].Start + er2[j].Length <= er1[i].Start)
                {
                    timePoints.Add(er2[j]);
                    j++;
                }

                while (j < er2.Count && er2[j].IsOverlap(er1[i]))
                {
                    j++;
                }
            }

            for (; j < er2.Count; j++)
            {
                timePoints.Add(er2[j]);
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

        private List<Token> MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var er1 = this.config.SingleDateTimeExtractor.Extract(text, referenceTime);
            var er2 = this.config.SingleTimeExtractor.Extract(text, referenceTime);
            var timePoints = new List<ExtractResult>();

            // handle the overlap problem
            var j = 0;
            for (var i = 0; i < er1.Count; i++)
            {
                timePoints.Add(er1[i]);
                while (j < er2.Count && er2[j].Start + er2[j].Length <= er1[i].Start)
                {
                    timePoints.Add(er2[j]);
                    j++;
                }

                while (j < er2.Count && er2[j].IsOverlap(er1[i]))
                {
                    j++;
                }
            }

            for (; j < er2.Count; j++)
            {
                timePoints.Add(er2[j]);
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
            }

            return ret;
        }
    }
}

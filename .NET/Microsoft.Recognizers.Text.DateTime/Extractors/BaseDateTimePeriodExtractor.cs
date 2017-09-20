using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimePeriodExtractor : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIMEPERIOD;

        private readonly IDateTimePeriodExtractorConfiguration config;

        public BaseDateTimePeriodExtractor(IDateTimePeriodExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchSimpleCases(text));
            tokens.AddRange(MergeTwoTimePoints(text));
            tokens.AddRange(MatchDuration(text));
            tokens.AddRange(MatchNight(text));
            tokens.AddRange(MatchRelativeUnit(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        private List<Token> MatchSimpleCases(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.SimpleCasesRegex)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    var followedStr = text.Substring(match.Index + match.Length);
                    if (string.IsNullOrEmpty(followedStr))
                    {
                        // has a date before?
                        var beforeStr = text.Substring(0, match.Index);
                        var er = this.config.SingleDateExtractor.Extract(beforeStr);
                        if (er.Count > 0)
                        {
                            var begin = er[0].Start ?? 0;
                            var end = (er[0].Start ?? 0) + (er[0].Length ?? 0);

                            var middleStr = beforeStr.Substring(begin + (er[0].Length ?? 0)).Trim().ToLower();
                            if (string.IsNullOrEmpty(middleStr) || this.config.PrepositionRegex.IsMatch(middleStr))
                            {
                                ret.Add(new Token(begin, match.Index + match.Length));
                            }
                        }
                    }
                    else
                    {
                        // is it followed by a date?
                        var er = this.config.SingleDateExtractor.Extract(followedStr);
                        if (er.Count > 0)
                        {
                            var begin = er[0].Start ?? 0;
                            var end = (er[0].Start ?? 0) + (er[0].Length ?? 0);
                            var middleStr = followedStr.Substring(0, begin).Trim().ToLower();
                            if (string.IsNullOrEmpty(middleStr) || this.config.PrepositionRegex.IsMatch(middleStr))
                            {
                                ret.Add(new Token(match.Index, match.Index + match.Length + end));
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private List<Token> MergeTwoTimePoints(string text)
        {
            var ret = new List<Token>();
            var er1 = this.config.SingleDateTimeExtractor.Extract(text);
            var er2 = this.config.SingleTimeExtractor.Extract(text);
            var timePoints = new List<ExtractResult>();
            
            // handle the overlap problem
            var j = 0;
            for (var i = 0; i < er1.Count; i++)
            {
                timePoints.Add(er1[i]);

                while (j < er2.Count && er2[j].Start + er2[j].Length < er1[i].Start)
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
                if (timePoints[idx].Type.Equals(Constants.SYS_DATETIME_TIME) &&
                    timePoints[idx + 1].Type.Equals(Constants.SYS_DATETIME_TIME))
                {
                    idx++;
                    continue;
                }

                var middleBegin = timePoints[idx].Start + timePoints[idx].Length ?? 0;
                var middleEnd = timePoints[idx + 1].Start ?? 0;

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();
                var match = this.config.TillRegex.Match(middleStr);

                // handle "{TimePoint} to {TimePoint}"
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    var periodBegin = timePoints[idx].Start ?? 0;
                    var periodEnd = (timePoints[idx + 1].Start ?? 0) + (timePoints[idx + 1].Length ?? 0);

                    // handle "from"
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    int fromIndex;

                    if (this.config.GetFromTokenIndex(beforeStr, out fromIndex)
                        || this.config.GetBetweenTokenIndex(beforeStr, out fromIndex))
                    {
                        periodBegin = fromIndex;
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
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    int beforeIndex;

                    if (this.config.GetBetweenTokenIndex(beforeStr, out beforeIndex))
                    {
                        periodBegin = beforeIndex;
                        ret.Add(new Token(periodBegin, periodEnd));
                        idx += 2;
                        continue;
                    }
                }

                idx++;
            }

            return ret;
        }

        private List<Token> MatchNight(string text)
        {
            var ret = new List<Token>();

            var matches = this.config.SpecificTimeOfDayRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            // Date followed by morning, afternoon
            // morning, afternoon followed by Date
            var ers = this.config.SingleDateExtractor.Extract(text);
            if (ers.Count == 0)
            {
                return ret;
            }

            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);

                var match = this.config.PeriodTimeOfDayWithDateRegex.Match(afterStr);
                if (match.Success && string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index)))
                {
                    ret.Add(new Token(er.Start ?? 0, er.Start + er.Length + match.Index + match.Length ?? 0));
                }

                var prefixStr = text.Substring(0, er.Start?? 0);

                match = this.config.PeriodTimeOfDayWithDateRegex.Match(prefixStr);
                if (match.Success && string.IsNullOrWhiteSpace(prefixStr.Substring(match.Index + match.Length)))
                {
                    var midStr = text.Substring(match.Index + match.Length, er.Start - match.Index - match.Length ?? 0);
                    if (!string.IsNullOrEmpty(midStr) && string.IsNullOrWhiteSpace(midStr))
                    {
                        ret.Add(new Token(match.Index, er.Start + er.Length ?? 0));
                    }
                }

            }

            return ret;
        }

        //TODO: this can be abstracted with the similar method in BaseDatePeriodExtractor
        private List<Token> MatchDuration(string text)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var durationExtractions = config.DurationExtractor.Extract(text);
            foreach (var durationExtraction in durationExtractions)
            {
                var match = config.TimeUnitRegex.Match(durationExtraction.Text);
                if (match.Success)
                {
                    durations.Add(new Token(durationExtraction.Start ?? 0,
                        (durationExtraction.Start + durationExtraction.Length ?? 0)));
                }
            }

            foreach (var duration in durations)
            {
                var beforeStr = text.Substring(0, duration.Start).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(beforeStr))
                {
                    continue;
                }

                var match = this.config.PastPrefixRegex.Match(beforeStr);
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }

                match = this.config.NextPrefixRegex.Match(beforeStr);
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    ret.Add(new Token(match.Index, duration.End));
                }
            }

            return ret;
        }

        private List<Token> MatchRelativeUnit(string text)
        {
            var ret = new List<Token>();

            var matches = config.RelativeTimeUnitRegex.Matches(text);
            if (matches.Count == 0)
            {
                matches = this.config.RestOfDateTimeRegex.Matches(text);
            }
            foreach(Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }
    }
}

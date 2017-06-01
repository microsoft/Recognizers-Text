using Microsoft.Recognizers.Text.DateTime.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Extractors
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
            tokens.AddRange(MatchNubmerWithUnit(text));
            tokens.AddRange(MatchNight(text));

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
            timePoints.Sort(delegate (ExtractResult er_1, ExtractResult er_2)
            {
                var start1 = er_1.Start ?? 0;
                var start2 = er_2.Start ?? 0;
                if (start1 < start2)
                {
                    return -1;
                }
                if (start1 == start2)
                {
                    return 0;
                }
                return 1;
            });


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
                    if (this.config.GetFromTokenIndex(beforeStr, out fromIndex))
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
            var matches = this.config.SpecificNightRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            // Date followed by morning, afternoon
            var ers = this.config.SingleDateExtractor.Extract(text);
            if (ers.Count == 0)
            {
                return ret;
            }
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = this.config.NightRegex.Match(afterStr);
                if (match.Success)// && string.IsNullOrWhiteSpace(afterStr.Substring(0, match.Index)))
                {
                    ret.Add(new Token(er.Start ?? 0, er.Start + er.Length + match.Index + match.Length ?? 0));
                }
            }

            return ret;
        }

        private List<Token> MatchNubmerWithUnit(string text)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var ers = this.config.CardinalExtractor.Extract(text);
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = this.config.FollowedUnit.Match(afterStr);
                if (match.Success && match.Index == 0)
                {
                    durations.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }
            }
            var matches = this.config.NumberCombinedWithUnit.Matches(text);
            foreach (Match match in matches)
            {
                durations.Add(new Token(match.Index, match.Index + match.Length));
            }

            matches = this.config.UnitRegex.Matches(text);
            foreach (Match match in matches)
            {
                durations.Add(new Token(match.Index, match.Index + match.Length));
            }

            foreach (var duration in durations)
            {
                var beforeStr = text.Substring(0, duration.Start).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(beforeStr))
                {
                    continue;
                }
                var match = this.config.PastRegex.Match(beforeStr);
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }
                var futureMatches = this.config.FutureRegex.Matches(beforeStr);
                if (futureMatches.Count > 0)
                {
                    match = futureMatches[futureMatches.Count - 1];
                    if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                    {
                        ret.Add(new Token(match.Index, duration.End));
                    }
                }
            }

            return ret;
        }
    }
}

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.Chinese.Extractors;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Extractors
{
    public class DateTimePeriodExtractorChs : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIMEPERIOD;

        public static readonly Regex TillRegex = new Regex(@"(?<till>到|直到|--|-|—|——)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PrepositionRegex = new Regex(@"(?<prep>^\s*的|在\s*$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourRegex =
            new Regex(
                @"(?<hour>00|01|02|03|04|05|06|07|08|09|0|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourNumRegex =
            new Regex(
                @"(?<hour>[零〇一二两三四五六七八九]|二十[一二三四]?|十[一二三四五六七八九]?)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ZhijianRegex = new Regex(@"^\s*(之间|之内|期间|中间|间)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex(@"这个|这一个|这|这一",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(@"上个|上一个|上|上一",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextRegex = new Regex(@"下个|下一个|下|下一",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DescRegex = new Regex(@"(?<desc>pm|am|p\.m\.|a\.m\.|p|a)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NightRegex = new Regex(@"(?<night>凌晨|清晨|早上|早|上午|中午|下午|午后|晚上|夜里|夜晚|半夜|夜间|深夜|傍晚|晚)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecificNightRegex =
            new Regex($@"((({ThisRegex}|{NextRegex}|{LastRegex})\s+{NightRegex})|(今晚|今早|今晨|明晚|明早|明晨|昨晚))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex =
            new Regex(@"(个)?(?<unit>(小时|分钟|秒钟|时|分|秒))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\.\d*)?){UnitRegex}\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(@"(?<past>(前|上|之前))",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FutureRegex = new Regex(@"(?<past>(后|下|之后))",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly TimeExtractorChs _singleTimeExtractor = new TimeExtractorChs();
        private static readonly DateTimeExtractorChs _timeWithDateExtractor = new DateTimeExtractorChs();
        private static readonly DateExtractorChs _singleDateExtractor = new DateExtractorChs();
        private static readonly CardinalExtractor _cardinalExtractor = new CardinalExtractor();
        private static readonly TimePeriodExtractorChs _timePeriodExtractor = new TimePeriodExtractorChs();


        public List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MergeDateAndTimePeriod(text));
            tokens.AddRange(MergeTwoTimePoints(text));
            tokens.AddRange(MatchNubmerWithUnit(text));
            tokens.AddRange(MatchNight(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // merge Date and Time peroid
        private List<Token> MergeDateAndTimePeriod(string text)
        {
            var ret = new List<Token>();
            var er1 = _singleDateExtractor.Extract(text);
            var er2 = _timePeriodExtractor.Extract(text);
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
            timePoints.Sort(delegate(ExtractResult er_1, ExtractResult er_2)
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

            // merge {Date} {TimePeriod}
            var idx = 0;
            while (idx < timePoints.Count - 1)
            {
                if (timePoints[idx].Type.Equals(Constants.SYS_DATETIME_DATE) &&
                    timePoints[idx + 1].Type.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
                {
                    var middleBegin = timePoints[idx].Start + timePoints[idx].Length ?? 0;
                    var middleEnd = timePoints[idx + 1].Start ?? 0;

                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim();
                    if (string.IsNullOrWhiteSpace(middleStr) || PrepositionRegex.IsMatch(middleStr))
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

        private List<Token> MergeTwoTimePoints(string text)
        {
            var ret = new List<Token>();
            var er1 = _timeWithDateExtractor.Extract(text);
            var er2 = _singleTimeExtractor.Extract(text);
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
            timePoints.Sort(delegate(ExtractResult er_1, ExtractResult er_2)
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
                var match = TillRegex.Match(middleStr);
                // handle "{TimePoint} to {TimePoint}"
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    var periodBegin = timePoints[idx].Start ?? 0;
                    var periodEnd = (timePoints[idx + 1].Start ?? 0) + (timePoints[idx + 1].Length ?? 0);

                    // handle "from"
                    var beforeStr = text.Substring(0, periodBegin).ToLowerInvariant();
                    if (beforeStr.Trim().EndsWith("从"))
                    {
                        periodBegin = beforeStr.LastIndexOf("从");
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }
                // handle "between {TimePoint} and {TimePoint}"
                if (middleStr.Equals("和") || middleStr.Equals("与") || middleStr.Equals("到"))
                {
                    var periodBegin = timePoints[idx].Start ?? 0;
                    var periodEnd = (timePoints[idx + 1].Start ?? 0) + (timePoints[idx + 1].Length ?? 0);

                    // handle "between"
                    var afterStr = text.Substring(periodEnd).ToLowerInvariant();
                    match = ZhijianRegex.Match(afterStr);
                    if (match.Success)
                    {
                        ret.Add(new Token(periodBegin, periodEnd + match.Length));
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
            var matches = SpecificNightRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            // Date followed by morning, afternoon
            var ers = _singleDateExtractor.Extract(text);
            if (ers.Count == 0)
            {
                return ret;
            }
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = NightRegex.Match(afterStr);
                if (match.Success)
                {
                    var middleStr = afterStr.Substring(0, match.Index);
                    if (string.IsNullOrWhiteSpace(middleStr) || PrepositionRegex.IsMatch(middleStr))
                    {
                        ret.Add(new Token(er.Start ?? 0, er.Start + er.Length + match.Index + match.Length ?? 0));
                    }
                }
            }

            return ret;
        }

        private List<Token> MatchNubmerWithUnit(string text)
        {
            var ret = new List<Token>();

            var durations = new List<Token>();
            var ers = _cardinalExtractor.Extract(text);
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = FollowedUnit.Match(afterStr);
                if (match.Success && match.Index == 0)
                {
                    durations.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }
            }

            var matches = UnitRegex.Matches(text);
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
                var match = PastRegex.Match(beforeStr);
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    ret.Add(new Token(match.Index, duration.End));
                    continue;
                }
                match = FutureRegex.Match(beforeStr);
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    ret.Add(new Token(match.Index, duration.End));
                }
            }

            return ret;
        }
    }
}
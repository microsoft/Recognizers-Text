using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class DateTimePeriodParserChs : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIMEPERIOD;

        private static readonly IExtractor _singleDateExtractor = new DateExtractorChs();
        private static readonly IExtractor _singleTimeExtractor = new TimeExtractorChs();
        private static readonly IExtractor _timeWithDateExtractor = new DateTimeExtractorChs();
        private static readonly IExtractor _timePeriodExtractor = new TimePeriodExtractorChs();
        private static readonly IExtractor _cardinalExtractor = new CardinalExtractor();

        private static readonly IParser _cardinalParser =
            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal,
                new ChineseNumberParserConfiguration());

        public static readonly Regex MORegex = new Regex(@"(凌晨|清晨|早上|早|上午)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AFRegex = new Regex(@"(中午|下午|午后|傍晚)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EVRegex = new Regex(@"(晚上|夜里|夜晚|晚)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NIRegex = new Regex(@"(半夜|夜间|深夜)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private readonly IFullDateTimeParserConfiguration config;

        public DateTimePeriodParserChs(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceTime = refDate;

            object value = null;
            if (er.Type.Equals(ParserName))
            {
                var innerResult = MergeDateAndTimePeriod(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = MergeTwoTimePoints(er.Text, referenceTime);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseSpecificNight(er.Text, referenceTime);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseNumberWithUnit(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATETIME,
                            Util.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATETIME,
                            Util.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item2)
                        }
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATETIME,
                            Util.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATETIME,
                            Util.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item2)
                        }
                    };
                    value = innerResult;
                }
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = value,
                TimexStr = value == null ? "" : ((DTParseResult) value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        private DTParseResult MergeDateAndTimePeriod(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var er1 = _singleDateExtractor.Extract(text);
            var er2 = _timePeriodExtractor.Extract(text);
            if (er1.Count != 1 || er2.Count != 1)
            {
                return ret;
            }

            var pr1 = this.config.DateParser.Parse(er1[0], referenceTime);
            var pr2 = this.config.TimePeriodParser.Parse(er2[0], referenceTime);
            var timerange = (Tuple<DateObject, DateObject>) ((DTParseResult) pr2.Value).FutureValue;
            var beginTime = timerange.Item1;
            var endTime = timerange.Item2;
            var futureDate = (DateObject) ((DTParseResult) pr1.Value).FutureValue;
            var pastDate = (DateObject) ((DTParseResult) pr1.Value).PastValue;

            ret.FutureValue =
                new Tuple<DateObject, DateObject>(
                    new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, beginTime.Hour, beginTime.Minute,
                        beginTime.Second),
                    new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, endTime.Hour, endTime.Minute,
                        endTime.Second));
            ret.PastValue =
                new Tuple<DateObject, DateObject>(
                    new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, beginTime.Hour, beginTime.Minute,
                        beginTime.Second),
                    new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, endTime.Hour, endTime.Minute,
                        endTime.Second));

            var splited = pr2.TimexStr.Split('T');
            if (splited.Length != 4)
            {
                return ret;
            }
            var dateStr = pr1.TimexStr;
            ret.Timex = splited[0] + dateStr + "T" + splited[1] + dateStr + "T" + splited[2] + "T" + splited[3];

            ret.Success = true;
            return ret;
        }

        private DTParseResult MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            DateTimeParseResult pr1 = null, pr2 = null;
            bool bothHasDate = false, beginHasDate = false, endHasDate = false;
            var er1 = _singleTimeExtractor.Extract(text);
            var er2 = _timeWithDateExtractor.Extract(text);
            if (er2.Count == 2)
            {
                pr1 = this.config.DateTimeParser.Parse(er2[0], referenceTime);
                pr2 = this.config.DateTimeParser.Parse(er2[1], referenceTime);
                bothHasDate = true;
            }
            else if (er2.Count == 1 && er1.Count == 2)
            {
                if (!er2[0].IsOverlap(er1[0]))
                {
                    pr1 = this.config.TimeParser.Parse(er1[0], referenceTime);
                    pr2 = this.config.DateTimeParser.Parse(er2[0], referenceTime);
                    endHasDate = true;
                }
                else
                {
                    pr1 = this.config.DateTimeParser.Parse(er2[0], referenceTime);
                    pr2 = this.config.TimeParser.Parse(er1[1], referenceTime);
                    beginHasDate = true;
                }
            }
            else if (er2.Count == 1 && er1.Count == 1)
            {
                if (er1[0].Start < er2[0].Start)
                {
                    pr1 = this.config.TimeParser.Parse(er1[0], referenceTime);
                    pr2 = this.config.DateTimeParser.Parse(er2[0], referenceTime);
                    endHasDate = true;
                }
                else
                {
                    pr1 = this.config.DateTimeParser.Parse(er2[0], referenceTime);
                    pr2 = this.config.TimeParser.Parse(er1[0], referenceTime);
                    beginHasDate = true;
                }
            }
            else if (er1.Count == 2)
            {
                // if both ends are Time. then this is a TimePeriod, not a DateTimePeriod
                return ret;
            }
            else
            {
                return ret;
            }

            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }
            DateObject futureBegin = (DateObject) ((DTParseResult) pr1.Value).FutureValue,
                futureEnd = (DateObject) ((DTParseResult) pr2.Value).FutureValue;
            DateObject pastBegin = (DateObject) ((DTParseResult) pr1.Value).PastValue,
                pastEnd = (DateObject) ((DTParseResult) pr2.Value).PastValue;
            if (futureBegin > futureEnd)
            {
                futureBegin = pastBegin;
            }
            if (pastEnd < pastBegin)
            {
                pastEnd = futureEnd;
            }

            if (bothHasDate)
            {
                ret.Timex =
                    $"({pr1.TimexStr},{pr2.TimexStr},PT{Convert.ToInt32((futureEnd - futureBegin).TotalHours)}H)";
                // do nothing
            }
            else if (beginHasDate)
            {
                // TODO: Handle "明天下午两点到五点"
                futureEnd = new DateObject(futureBegin.Year, futureBegin.Month, futureBegin.Day,
                    futureEnd.Hour, futureEnd.Minute, futureEnd.Second);
                pastEnd = new DateObject(pastBegin.Year, pastBegin.Month, pastBegin.Day,
                    pastEnd.Hour, pastEnd.Minute, pastEnd.Second);
                var dateStr = pr1.TimexStr.Split('T')[0];
                ret.Timex =
                    $"({pr1.TimexStr},{dateStr + pr2.TimexStr},PT{Convert.ToInt32((futureEnd - futureBegin).TotalHours)}H)";
            }
            else if (endHasDate)
            {
                // TODO: Handle "明天下午两点到五点"
                futureBegin = new DateObject(futureEnd.Year, futureEnd.Month, futureEnd.Day,
                    futureBegin.Hour, futureBegin.Minute, futureBegin.Second);
                pastBegin = new DateObject(pastEnd.Year, pastEnd.Month, pastEnd.Day,
                    pastBegin.Hour, pastBegin.Minute, pastBegin.Second);
                var dateStr = pr2.TimexStr.Split('T')[0];
                ret.Timex =
                    $"({dateStr + pr1.TimexStr},{pr2.TimexStr},PT{Convert.ToInt32((futureEnd - futureBegin).TotalHours)}H)";
            }

            ret.FutureValue = new Tuple<DateObject, DateObject>(futureBegin, futureEnd);
            ret.PastValue = new Tuple<DateObject, DateObject>(pastBegin, pastEnd);
            ret.Success = true;
            return ret;
        }


        // parse "this night"
        private DTParseResult ParseSpecificNight(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var trimedText = text.Trim().ToLowerInvariant();
            int beginHour, endHour, endMin = 0;
            var timeStr = string.Empty;

            // handle 昨晚，今晨
            var match = DateTimePeriodExtractorChs.SpecificNightRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var swift = 0;
                switch (trimedText)
                {
                    case "今晚":
                        swift = 0;
                        timeStr = "TEV";
                        beginHour = 16;
                        endHour = 20;
                        break;
                    case "今早":
                    case "今晨":
                        swift = 0;
                        timeStr = "TMO";
                        beginHour = 8;
                        endHour = 12;
                        break;
                    case "明晚":
                        swift = 1;
                        timeStr = "TEV";
                        beginHour = 16;
                        endHour = 20;
                        break;
                    case "明早":
                    case "明晨":
                        swift = 1;
                        timeStr = "TMO";
                        beginHour = 8;
                        endHour = 12;
                        break;
                    case "昨晚":
                        swift = -1;
                        timeStr = "TEV";
                        beginHour = 16;
                        endHour = 20;
                        break;
                    default:
                        return ret;
                }

                var date = referenceTime.AddDays(swift).Date;
                int day = date.Day, month = date.Month, year = date.Year;

                ret.Timex = Util.FormatDate(date) + timeStr;
                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(new DateObject(year, month, day, beginHour, 0, 0),
                            new DateObject(year, month, day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }

            // handle morning, afternoon..
            if (MORegex.IsMatch(trimedText))
            {
                timeStr = "TMO";
                beginHour = 8;
                endHour = 12;
            }
            else if (AFRegex.IsMatch(trimedText))
            {
                timeStr = "TAF";
                beginHour = 12;
                endHour = 16;
            }
            else if (EVRegex.IsMatch(trimedText))
            {
                timeStr = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (NIRegex.IsMatch(trimedText))
            {
                timeStr = "TNI";
                beginHour = 20;
                endHour = 23;
                endMin = 59;
            }
            else
            {
                return ret;
            }

            match = DateTimePeriodExtractorChs.SpecificNightRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var swift = 0;
                if (DateTimePeriodExtractorChs.NextRegex.IsMatch(trimedText))
                {
                    swift = 1;
                }
                else if (DateTimePeriodExtractorChs.LastRegex.IsMatch(trimedText))
                {
                    swift = -1;
                }

                var date = referenceTime.AddDays(swift).Date;
                int day = date.Day, month = date.Month, year = date.Year;

                ret.Timex = Util.FormatDate(date) + timeStr;
                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(new DateObject(year, month, day, beginHour, 0, 0),
                            new DateObject(year, month, day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }


            // handle Date followed by morning, afternoon
            match = DateTimePeriodExtractorChs.NightRegex.Match(trimedText);
            if (match.Success)
            {
                var beforeStr = trimedText.Substring(0, match.Index).Trim();
                var ers = _singleDateExtractor.Extract(beforeStr);
                if (ers.Count == 0 || ers[0].Length != beforeStr.Length)
                {
                    return ret;
                }
                var pr = this.config.DateParser.Parse(ers[0], referenceTime);
                var futureDate = (DateObject) ((DTParseResult) pr.Value).FutureValue;
                var pastDate = (DateObject) ((DTParseResult) pr.Value).PastValue;
                ret.Timex = pr.TimexStr + timeStr;
                ret.FutureValue =
                    new Tuple<DateObject, DateObject>(
                        new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, beginHour, 0, 0),
                        new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, endHour, endMin, endMin));
                ret.PastValue =
                    new Tuple<DateObject, DateObject>(
                        new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, beginHour, 0, 0),
                        new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        // parse "in 20 minutes"
        private DTParseResult ParseNumberWithUnit(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var numStr = string.Empty;
            var unitStr = string.Empty;

            // if there are spaces between nubmer and unit
            var ers = _cardinalExtractor.Extract(text);
            if (ers.Count == 1)
            {
                var pr = _cardinalParser.Parse(ers[0]);
                var srcUnit = text.Substring(ers[0].Start + ers[0].Length ?? 0).Trim().ToLower();
                if (srcUnit.StartsWith("个"))
                {
                    srcUnit = srcUnit.Substring(1);
                }
                var beforeStr = text.Substring(0, ers[0].Start ?? 0).Trim().ToLowerInvariant();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    numStr = pr.ResolutionStr;
                    unitStr = this.config.UnitMap[srcUnit];
                    var prefixMatch = DateTimePeriodExtractorChs.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime.AddHours(-(double) pr.Value);
                                endDate = referenceTime;
                                break;
                            case "M":
                                beginDate = referenceTime.AddMinutes(-(double) pr.Value);
                                endDate = referenceTime;
                                break;
                            case "S":
                                beginDate = referenceTime.AddSeconds(-(double) pr.Value);
                                endDate = referenceTime;
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({Util.LuisDate(beginDate)}T{Util.LuisTime(beginDate)},{Util.LuisDate(endDate)}T{Util.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                    prefixMatch = DateTimePeriodExtractorChs.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddHours((double) pr.Value);
                                break;
                            case "M":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddMinutes((double) pr.Value);
                                break;
                            case "S":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddSeconds((double) pr.Value);
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({Util.LuisDate(beginDate)}T{Util.LuisTime(beginDate)},{Util.LuisDate(endDate)}T{Util.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                }
            }

            // handle "last hour"
            var match = DateTimePeriodExtractorChs.UnitRegex.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value.ToLower();
                var beforeStr = text.Substring(0, match.Index).Trim().ToLowerInvariant();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];
                    var prefixMatch = DateTimePeriodExtractorChs.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime.AddHours(-1);
                                endDate = referenceTime;
                                break;
                            case "M":
                                beginDate = referenceTime.AddMinutes(-1);
                                endDate = referenceTime;
                                break;
                            case "S":
                                beginDate = referenceTime.AddSeconds(-1);
                                endDate = referenceTime;
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({Util.LuisDate(beginDate)}T{Util.LuisTime(beginDate)},{Util.LuisDate(endDate)}T{Util.LuisTime(endDate)},PT1{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                    prefixMatch = DateTimePeriodExtractorChs.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "H":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddHours(1);
                                break;
                            case "M":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddMinutes(1);
                                break;
                            case "S":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddSeconds(1);
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({Util.LuisDate(beginDate)}T{Util.LuisTime(beginDate)},{Util.LuisDate(endDate)}T{Util.LuisTime(endDate)},PT1{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                }
            }
            return ret;
        }
    }
}
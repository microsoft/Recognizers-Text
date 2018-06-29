using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class DateTimePeriodParserChs : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIMEPERIOD;

        private static readonly IDateTimeExtractor SingleDateExtractor = new DateExtractorChs();
        private static readonly IDateTimeExtractor SingleTimeExtractor = new TimeExtractorChs();
        private static readonly IDateTimeExtractor TimeWithDateExtractor = new DateTimeExtractorChs();
        private static readonly IDateTimeExtractor TimePeriodExtractor = new TimePeriodExtractorChs();
        private static readonly IExtractor CardinalExtractor = new CardinalExtractor();

        private static readonly IParser CardinalParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal,
                                                                                               new ChineseNumberParserConfiguration());

        public static readonly Regex MORegex = new Regex(DateTimeDefinitions.DateTimePeriodMORegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AFRegex = new Regex(DateTimeDefinitions.DateTimePeriodAFRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EVRegex = new Regex(DateTimeDefinitions.DateTimePeriodEVRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NIRegex = new Regex(DateTimeDefinitions.DateTimePeriodNIRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

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
                            FormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATETIME,
                            FormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item2)
                        }
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATETIME,
                            FormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATETIME,
                            FormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item2)
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
                TimexStr = value == null ? "" : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = ""
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private DateTimeResolutionResult MergeDateAndTimePeriod(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            var er1 = SingleDateExtractor.Extract(text, referenceTime);
            var er2 = TimePeriodExtractor.Extract(text, referenceTime);
            if (er1.Count != 1 || er2.Count != 1)
            {
                return ret;
            }

            var pr1 = this.config.DateParser.Parse(er1[0], referenceTime);
            var pr2 = this.config.TimePeriodParser.Parse(er2[0], referenceTime);
            var timerange = (Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr2.Value).FutureValue;
            var beginTime = timerange.Item1;
            var endTime = timerange.Item2;
            var futureDate = (DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue;
            var pastDate = (DateObject)((DateTimeResolutionResult)pr1.Value).PastValue;

            ret.FutureValue =
                new Tuple<DateObject, DateObject>(
                    DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, beginTime.Hour, beginTime.Minute,
                        beginTime.Second),
                    DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, endTime.Hour, endTime.Minute,
                        endTime.Second));

            ret.PastValue =
                new Tuple<DateObject, DateObject>(
                    DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, beginTime.Hour, beginTime.Minute,
                        beginTime.Second),
                    DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, endTime.Hour, endTime.Minute,
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

        private DateTimeResolutionResult MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            DateTimeParseResult pr1 = null, pr2 = null;
            bool bothHaveDates = false, beginHasDate = false, endHasDate = false;

            var er1 = SingleTimeExtractor.Extract(text, referenceTime);
            var er2 = TimeWithDateExtractor.Extract(text, referenceTime);

            var rightTime = DateObject.MinValue.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, referenceTime.Day);
            var leftTime = DateObject.MinValue.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, referenceTime.Day);

            if (er2.Count == 2)
            {
                pr1 = this.config.DateTimeParser.Parse(er2[0], referenceTime);
                pr2 = this.config.DateTimeParser.Parse(er2[1], referenceTime);
                bothHaveDates = true;
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

            DateObject futureBegin = (DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue,
                futureEnd = (DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue;

            DateObject pastBegin = (DateObject)((DateTimeResolutionResult)pr1.Value).PastValue,
                pastEnd = (DateObject)((DateTimeResolutionResult)pr2.Value).PastValue;

            if (futureBegin > futureEnd)
            {
                futureBegin = pastBegin;
            }

            if (pastEnd < pastBegin)
            {
                pastEnd = futureEnd;
            }

            if (bothHaveDates)
            {
                rightTime = DateObject.MinValue.SafeCreateFromValue(futureEnd.Year, futureEnd.Month, futureEnd.Day);
                leftTime = DateObject.MinValue.SafeCreateFromValue(futureBegin.Year, futureBegin.Month, futureBegin.Day);
            }
            else if (beginHasDate)
            {
                // TODO: Handle "明天下午两点到五点"
                futureEnd = DateObject.MinValue.SafeCreateFromValue(futureBegin.Year, futureBegin.Month, futureBegin.Day,
                    futureEnd.Hour, futureEnd.Minute, futureEnd.Second);
                pastEnd = DateObject.MinValue.SafeCreateFromValue(pastBegin.Year, pastBegin.Month, pastBegin.Day,
                    pastEnd.Hour, pastEnd.Minute, pastEnd.Second);

                leftTime = DateObject.MinValue.SafeCreateFromValue(futureBegin.Year, futureBegin.Month, futureBegin.Day);
            }
            else if (endHasDate)
            {
                // TODO: Handle "明天下午两点到五点"
                futureBegin = DateObject.MinValue.SafeCreateFromValue(futureEnd.Year, futureEnd.Month, futureEnd.Day,
                    futureBegin.Hour, futureBegin.Minute, futureBegin.Second);
                pastBegin = DateObject.MinValue.SafeCreateFromValue(pastEnd.Year, pastEnd.Month, pastEnd.Day,
                    pastBegin.Hour, pastBegin.Minute, pastBegin.Second);

                rightTime = DateObject.MinValue.SafeCreateFromValue(futureEnd.Year, futureEnd.Month, futureEnd.Day);
            }

            var leftResult = (DateTimeResolutionResult)pr1.Value;
            var rightResult = (DateTimeResolutionResult)pr2.Value;
            var leftResultTime = (DateObject)leftResult.FutureValue;
            var rightResultTime = (DateObject)rightResult.FutureValue;

            int day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;

            //check if the right time is smaller than the left time, if yes, add one day
            int hour = leftResultTime.Hour > 0 ? leftResultTime.Hour : 0,
                min = leftResultTime.Minute > 0 ? leftResultTime.Minute : 0,
                second = leftResultTime.Second > 0 ? leftResultTime.Second : 0;

            leftTime = leftTime.AddHours(hour);
            leftTime = leftTime.AddMinutes(min);
            leftTime = leftTime.AddSeconds(second);
            DateObject.MinValue.SafeCreateFromValue(year, month, day, hour, min, second);

            hour = rightResultTime.Hour > 0 ? rightResultTime.Hour : 0;
            min = rightResultTime.Minute > 0 ? rightResultTime.Minute : 0;
            second = rightResultTime.Second > 0 ? rightResultTime.Second : 0;

            rightTime = rightTime.AddHours(hour);
            rightTime = rightTime.AddMinutes(min);
            rightTime = rightTime.AddSeconds(second);

            //the right side time contains "ampm", while the left side doesn't
            if (rightResult.Comment != null && rightResult.Comment.Equals(Constants.Comment_AmPm) &&
                leftResult.Comment == null && rightTime < leftTime)
            {
                rightTime = rightTime.AddHours(Constants.HalfDayHourCount);
            }

            if (rightTime < leftTime)
            {
                rightTime = rightTime.AddDays(1);
            }

            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(leftTime, rightTime);

            var leftTimex = "";
            var rightTimex = "";

            //"X" is timex token for not determined time
            if (!pr1.TimexStr.Contains("X") && !pr2.TimexStr.Contains("X"))
            {
                leftTimex = FormatUtil.LuisDateTime(leftTime);
                rightTimex = FormatUtil.LuisDateTime(rightTime);
            }
            else
            {
                leftTimex = pr1.TimexStr;
                rightTimex = pr2.TimexStr;
            }

            ret.Timex = $"({leftTimex},{rightTimex},PT{Convert.ToInt32((rightTime - leftTime).TotalHours)}H)";

            ret.Success = true;
            return ret;
        }

        // parse "this night"
        private DateTimeResolutionResult ParseSpecificNight(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimedText = text.Trim().ToLowerInvariant();
            int beginHour, endHour, endMin = 0;
            string timeStr;

            // handle 昨晚，今晨
            var match = DateTimePeriodExtractorChs.SpecificTimeOfDayRegex.Match(trimedText);
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
                        endHour = Constants.HalfDayHourCount;
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
                        endHour = Constants.HalfDayHourCount;
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

                ret.Timex = FormatUtil.FormatDate(date) + timeStr;
                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
                            DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }

            // handle morning, afternoon..
            if (MORegex.IsMatch(trimedText))
            {
                timeStr = "TMO";
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (AFRegex.IsMatch(trimedText))
            {
                timeStr = "TAF";
                beginHour = Constants.HalfDayHourCount;
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

            match = DateTimePeriodExtractorChs.SpecificTimeOfDayRegex.Match(trimedText);
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

                ret.Timex = FormatUtil.FormatDate(date) + timeStr;
                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
                            DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }


            // handle Date followed by morning, afternoon
            match = DateTimePeriodExtractorChs.TimeOfDayRegex.Match(trimedText);
            if (match.Success)
            {
                var beforeStr = trimedText.Substring(0, match.Index).Trim();
                var ers = SingleDateExtractor.Extract(beforeStr, referenceTime);
                if (ers.Count == 0 || ers[0].Length != beforeStr.Length)
                {
                    return ret;
                }

                var pr = this.config.DateParser.Parse(ers[0], referenceTime);
                var futureDate = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                var pastDate = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;

                ret.Timex = pr.TimexStr + timeStr;

                ret.FutureValue =
                    new Tuple<DateObject, DateObject>(
                        DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, beginHour, 0, 0),
                        DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, endHour, endMin, endMin));

                ret.PastValue =
                    new Tuple<DateObject, DateObject>(
                        DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, beginHour, 0, 0),
                        DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, endHour, endMin, endMin));

                ret.Success = true;

                return ret;
            }

            return ret;
        }

        // parse "in 20 minutes"
        private DateTimeResolutionResult ParseNumberWithUnit(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            string numStr, unitStr;

            // if there are spaces between nubmer and unit
            var ers = CardinalExtractor.Extract(text);
            if (ers.Count == 1)
            {
                var pr = CardinalParser.Parse(ers[0]);
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
                                beginDate = referenceTime.AddHours(-(double)pr.Value);
                                endDate = referenceTime;
                                break;
                            case "M":
                                beginDate = referenceTime.AddMinutes(-(double)pr.Value);
                                endDate = referenceTime;
                                break;
                            case "S":
                                beginDate = referenceTime.AddSeconds(-(double)pr.Value);
                                endDate = referenceTime;
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
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
                                endDate = referenceTime.AddHours((double)pr.Value);
                                break;
                            case "M":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddMinutes((double)pr.Value);
                                break;
                            case "S":
                                beginDate = referenceTime;
                                endDate = referenceTime.AddSeconds((double)pr.Value);
                                break;
                            default:
                                return ret;
                        }

                        ret.Timex =
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
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
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT1{unitStr[0]})";
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
                            $"({FormatUtil.LuisDate(beginDate)}T{FormatUtil.LuisTime(beginDate)},{FormatUtil.LuisDate(endDate)}T{FormatUtil.LuisTime(endDate)},PT1{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                }
            }
            return ret;
        }

        public static string BuildTimex(TimeResult timeResult)
        {
            var build = new StringBuilder("T");
            if (timeResult.Hour >= 0)
            {
                build.Append(timeResult.Hour.ToString("D2"));
            }

            if (timeResult.Minute >= 0)
            {
                build.Append(":" + timeResult.Minute.ToString("D2"));
            }

            if (timeResult.Second >= 0)
            {
                build.Append(":" + timeResult.Second.ToString("D2"));
            }

            return build.ToString();
        }

        public static TimeResult DateObject2TimeResult(DateObject dateTime)
        {
            var timeResult = new TimeResult
            {
                Hour = dateTime.Hour,
                Minute = dateTime.Minute,
                Second = dateTime.Second
            };

            return timeResult;
        }
    }
}
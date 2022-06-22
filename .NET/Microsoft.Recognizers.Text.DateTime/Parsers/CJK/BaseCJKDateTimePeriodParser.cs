// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDateTimePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIMEPERIOD;

        private readonly ICJKDateTimePeriodParserConfiguration config;

        public BaseCJKDateTimePeriodParser(ICJKDateTimePeriodParserConfiguration configuration)
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
            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
            {
                var innerResult = MergeDateAndTimePeriod(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = MergeTwoTimePoints(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseDuration(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseSpecificNight(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseNumberWithUnit(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseRelativeUnit(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATETIME,
                            DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)innerResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATETIME,
                            DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)innerResult.FutureValue).Item2)
                        },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATETIME,
                            DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)innerResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATETIME,
                            DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)innerResult.PastValue).Item2)
                        },
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
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
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

            var er1 = this.config.DateExtractor.Extract(text, referenceTime);
            var er2 = this.config.TimePeriodExtractor.Extract(text, referenceTime);
            if (er1.Count != 1 || er2.Count != 1)
            {
                return ret;
            }

            var pr1 = this.config.DateParser.Parse(er1[0], referenceTime);
            var pr2 = this.config.TimePeriodParser.Parse(er2[0], referenceTime);
            var timeRange = (Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr2.Value).FutureValue;
            var beginTime = timeRange.Item1;
            var endTime = timeRange.Item2;
            var futureDate = (DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue;
            var pastDate = (DateObject)((DateTimeResolutionResult)pr1.Value).PastValue;

            // handle cases with time like 25時 which resolve to the next day
            var swiftDay = 0;
            var timexHours = TimexUtility.ParseHoursFromTimePeriodTimex(pr2.TimexStr);
            if (!this.config.AmPmDescRegex.Match(text).Success && timexHours.Item1 < Constants.HalfDayHourCount && timexHours.Item2 < Constants.HalfDayHourCount)
            {
                ret.Comment = Constants.Comment_AmPm;
            }

            if (timexHours.Item1 > Constants.DayHourCount)
            {
                pastDate = pastDate.AddDays(1);
                futureDate = futureDate.AddDays(1);
            }
            else if (timexHours.Item2 > Constants.DayHourCount)
            {
                swiftDay++;
            }

            var pastDateAlt = pastDate.AddDays(swiftDay);
            var futureDateAlt = futureDate.AddDays(swiftDay);

            ret.FutureValue =
                new Tuple<DateObject, DateObject>(
                    DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, beginTime.Hour, beginTime.Minute, beginTime.Second),
                    DateObject.MinValue.SafeCreateFromValue(futureDateAlt.Year, futureDateAlt.Month, futureDateAlt.Day, endTime.Hour, endTime.Minute, endTime.Second));

            ret.PastValue =
                new Tuple<DateObject, DateObject>(
                    DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, beginTime.Hour, beginTime.Minute, beginTime.Second),
                    DateObject.MinValue.SafeCreateFromValue(pastDateAlt.Year, pastDateAlt.Month, pastDateAlt.Day, endTime.Hour, endTime.Minute, endTime.Second));

            ret.Timex = TimexUtility.GenerateSplitDateTimePeriodTimex(pr1.TimexStr, pr2.TimexStr);
            ret.Success = !string.IsNullOrEmpty(ret.Timex);

            return ret;
        }

        private DateTimeResolutionResult MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            DateTimeParseResult pr1 = null, pr2 = null;
            bool bothHaveDates = false, beginHasDate = false, endHasDate = false;

            var er1 = this.config.TimeExtractor.Extract(text, referenceTime);
            var er2 = this.config.DateTimeExtractor.Extract(text, referenceTime);

            var rightTime = DateObject.MinValue.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, referenceTime.Day);
            var leftTime = DateObject.MinValue.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, referenceTime.Day);

            var match = config.FutureRegex.Match(text);

            // cases including 'within' are processed in ParseDuration
            if (match.Groups[Constants.WithinGroupName].Success)
            {
                return ParseDuration(text, referenceTime);
            }

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

            DateObject pastBegin = (DateObject)((DateTimeResolutionResult)pr1.Value).PastValue;

            if (futureBegin > futureEnd)
            {
                futureBegin = pastBegin;
            }

            if (bothHaveDates)
            {
                rightTime = DateObject.MinValue.SafeCreateFromValue(futureEnd.Year, futureEnd.Month, futureEnd.Day);
                leftTime = DateObject.MinValue.SafeCreateFromValue(futureBegin.Year, futureBegin.Month, futureBegin.Day);
            }
            else if (beginHasDate)
            {
                leftTime = DateObject.MinValue.SafeCreateFromValue(futureBegin.Year, futureBegin.Month, futureBegin.Day);
            }
            else if (endHasDate)
            {
                rightTime = DateObject.MinValue.SafeCreateFromValue(futureEnd.Year, futureEnd.Month, futureEnd.Day);
            }

            var leftResult = (DateTimeResolutionResult)pr1.Value;
            var rightResult = (DateTimeResolutionResult)pr2.Value;
            var leftResultTime = (DateObject)leftResult.FutureValue;
            var rightResultTime = (DateObject)rightResult.FutureValue;

            // check if the right time is smaller than the left time, if yes, add one day
            int hour = leftResultTime.Hour > 0 ? leftResultTime.Hour : 0,
                min = leftResultTime.Minute > 0 ? leftResultTime.Minute : 0,
                second = leftResultTime.Second > 0 ? leftResultTime.Second : 0;

            leftTime = leftTime.AddHours(hour).AddMinutes(min).AddSeconds(second);

            hour = rightResultTime.Hour > 0 ? rightResultTime.Hour : 0;
            min = rightResultTime.Minute > 0 ? rightResultTime.Minute : 0;
            second = rightResultTime.Second > 0 ? rightResultTime.Second : 0;

            rightTime = rightTime.AddHours(hour).AddMinutes(min).AddSeconds(second);

            // the right side time contains "ampm", while the left side doesn't
            if (rightResult.Comment is Constants.Comment_AmPm &&
                leftResult.Comment == null && rightTime < leftTime)
            {
                rightTime = rightTime.AddHours(Constants.HalfDayHourCount);
            }

            if (rightTime < leftTime)
            {
                rightTime = rightTime.AddDays(1);
            }

            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(leftTime, rightTime);

            var leftTimex = pr1.TimexStr;
            var rightTimex = pr2.TimexStr;
            if (beginHasDate)
            {
                rightTimex = DateTimeFormatUtil.LuisDateShortTime(rightTime, pr2.TimexStr);
            }
            else if (endHasDate)
            {
                leftTimex = DateTimeFormatUtil.LuisDateShortTime(leftTime, pr1.TimexStr);
            }

            ret.Timex = TimexUtility.GenerateDateTimePeriodTimex(leftTimex, rightTimex, rightTime - leftTime);
            ret.Success = true;
            return ret;
        }

        private DateTimeResolutionResult ParseDuration(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var ers = config.DurationExtractor.Extract(text, referenceTime);

            if (ers.Count == 1)
            {
                var pr = config.DurationParser.Parse(ers[0]);
                var afterStr = text.Substring((pr.Start ?? 0) + (pr.Length ?? 0)).Trim();

                if (pr.Value != null)
                {
                    var swiftSeconds = 0;
                    var mod = string.Empty;
                    var durationResult = (DateTimeResolutionResult)pr.Value;
                    if (durationResult.PastValue is double && durationResult.FutureValue is double)
                    {
                        swiftSeconds = (int)((double)durationResult.FutureValue);
                    }

                    DateObject beginTime;
                    var endTime = beginTime = referenceTime;
                    var match = config.FutureRegex.Match(afterStr);

                    if (match.Groups[Constants.WithinGroupName].Success)
                    {
                        endTime = beginTime.AddSeconds(swiftSeconds);

                        ret.Timex = TimexUtility.GenerateDateTimePeriodTimex(beginTime, endTime, durationResult.Timex);

                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginTime, endTime);
                        ret.Success = true;

                        if (!string.IsNullOrEmpty(mod))
                        {
                            ((DateTimeResolutionResult)pr.Value).Mod = mod;
                        }

                        ret.SubDateTimeEntities = new List<object> { pr };
                        return ret;
                    }
                }
            }

            return ret;
        }

        // Parse cases like "this night"
        private DateTimeResolutionResult ParseSpecificNight(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim();
            int beginHour, endHour, endMin = 0;
            string timeStr;

            // Handle 昨晚 (last night)，今晨 (this morning)
            if (this.config.SpecificTimeOfDayRegex.IsExactMatch(trimmedText, trim: true))
            {
                // handle the ambiguous case "ぎりぎり" [the latest possible time]
                var latest = this.config.SpecificTimeOfDayRegex.Match(text);
                if (latest.Groups[Constants.LatestGroupName].Success)
                {
                    DateObject beginDate, endDate;
                    beginDate = referenceTime.AddMinutes(-1);
                    endDate = referenceTime;
                    var diff = endDate - beginDate;
                    ret.Timex = TimexUtility.GenerateDateTimePeriodTimex(beginDate, endDate);
                    ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                    ret.Success = true;
                    return ret;
                }

                if (!this.config.GetMatchedTimeRangeAndSwift(trimmedText, out timeStr, out beginHour, out endHour, out endMin, out int swift))
                {
                    return ret;
                }

                if (this.config.NextRegex.IsMatch(trimmedText))
                {
                    swift = 1;
                }
                else if (this.config.LastRegex.IsMatch(trimmedText))
                {
                    swift = -1;
                }

                var date = referenceTime.AddDays(swift).Date;
                int day = date.Day, month = date.Month, year = date.Year;

                ret.Timex = DateTimeFormatUtil.FormatDate(date) + timeStr;
                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(
                            DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
                            DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }

            // Handle cases like morning, afternoon
            if (!this.config.GetMatchedTimeRange(trimmedText, out timeStr, out beginHour, out endHour, out endMin))
            {
                return ret;
            }

            if (this.config.SpecificTimeOfDayRegex.IsExactMatch(trimmedText, trim: true))
            {
                var swift = 0;
                if (this.config.NextRegex.IsMatch(trimmedText))
                {
                    swift = 1;
                }
                else if (this.config.LastRegex.IsMatch(trimmedText))
                {
                    swift = -1;
                }

                var date = referenceTime.AddDays(swift).Date;
                int day = date.Day, month = date.Month, year = date.Year;

                ret.Timex = DateTimeFormatUtil.FormatDate(date) + timeStr;
                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(
                            DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
                            DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }

            // handle Date followed by morning, afternoon
            var match = this.config.TimeOfDayRegex.Match(trimmedText);

            if (match.Success)
            {
                var beforeStr = trimmedText.Substring(0, match.Index).Trim();
                var ers = this.config.DateExtractor.Extract(beforeStr, referenceTime);

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
            string unitStr;

            // if there are spaces between number and unit
            var ers = this.config.CardinalExtractor.Extract(text);
            if (ers.Count == 1)
            {
                var pr = this.config.CardinalParser.Parse(ers[0]);
                var srcUnit = text.Substring(ers[0].Start + ers[0].Length ?? 0).Trim();

                if (srcUnit.StartsWith("个", StringComparison.Ordinal))
                {
                    srcUnit = srcUnit.Substring(1);
                }

                var beforeStr = text.Substring(0, ers[0].Start ?? 0);
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    var numStr = pr.ResolutionStr;
                    unitStr = this.config.UnitMap[srcUnit];
                    var prefixMatch = this.config.PastRegex.MatchExact(beforeStr, trim: true);

                    if (prefixMatch.Success)
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
                            $"({DateTimeFormatUtil.LuisDate(beginDate)}T{DateTimeFormatUtil.LuisTime(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)}T{DateTimeFormatUtil.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }

                    prefixMatch = this.config.FutureRegex.MatchExact(beforeStr, trim: true);

                    if (!prefixMatch.Success)
                    {
                        prefixMatch = this.config.TimePeriodLeftRegex.MatchEnd(beforeStr, trim: true);

                    }

                    if (prefixMatch.Success)
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
                            $"({DateTimeFormatUtil.LuisDate(beginDate)}T{DateTimeFormatUtil.LuisTime(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)}T{DateTimeFormatUtil.LuisTime(endDate)},PT{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                }
            }

            // handle "last hour"
            var match = this.config.UnitRegex.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups[Constants.UnitGroupName].Value;
                var beforeStr = text.Substring(0, match.Index).Trim();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];

                    if (this.config.PastRegex.IsExactMatch(beforeStr, trim: true))
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
                            $"({DateTimeFormatUtil.LuisDate(beginDate)}T{DateTimeFormatUtil.LuisTime(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)}T{DateTimeFormatUtil.LuisTime(endDate)},PT1{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }

                    if (this.config.FutureRegex.IsExactMatch(beforeStr, trim: true))
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
                            $"({DateTimeFormatUtil.LuisDate(beginDate)}T{DateTimeFormatUtil.LuisTime(beginDate)},{DateTimeFormatUtil.LuisDate(endDate)}T{DateTimeFormatUtil.LuisTime(endDate)},PT1{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                }
            }

            return ret;
        }

        private DateTimeResolutionResult ParseRelativeUnit(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            var match = this.config.RestOfDateRegex.Match(text);

            if (match.Success)
            {
                var srcUnit = match.Groups[Constants.UnitGroupName].Value;

                var unitStr = config.UnitMap[srcUnit];

                int swiftValue = 1;
                DateObject beginTime;
                var endTime = beginTime = referenceTime;

                if (config.UnitMap.ContainsKey(srcUnit))
                {
                    ret.Timex = TimexUtility.GenerateRelativeUnitDateTimePeriodTimex(ref beginTime, ref endTime, referenceTime, unitStr, swiftValue);

                    ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginTime, endTime);
                    ret.Success = !string.IsNullOrEmpty(ret.Timex);

                    return ret;
                }
            }

            return ret;
        }
    }
}
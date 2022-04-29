// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDateTimeParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIME; // "DateTime";

        private readonly ICJKDateTimeParserConfiguration config;

        public BaseCJKDateTimeParser(ICJKDateTimeParserConfiguration configuration)
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
                var innerResult = MergeDateAndTime(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = ParseBasicRegex(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseTimeOfSpecialDayRegex(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParserDurationWithAgoAndLater(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATETIME, DateTimeFormatUtil.FormatDateTime((DateObject)innerResult.FutureValue) },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATETIME, DateTimeFormatUtil.FormatDateTime((DateObject)innerResult.PastValue) },
                    };

                    innerResult.IsLunar = IsLunarCalendar(er.Text);

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

        private DateTimeResolutionResult ParseBasicRegex(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim();

            // handle "现在"
            var match = this.config.NowRegex.MatchExact(trimmedText, trim: true);

            if (match.Success)
            {

                this.config.GetMatchedNowTimex(trimmedText, out string timex);
                ret.Timex = timex;
                ret.FutureValue = ret.PastValue = referenceTime;
                ret.Success = true;

                return ret;
            }

            return ret;
        }

        // parse if lunar contains
        private bool IsLunarCalendar(string text)
        {
            var trimmedText = text.Trim();
            var match = this.config.LunarRegex.Match(trimmedText);
            if (match.Success)
            {
                return true;
            }

            return this.config.LunarHolidayRegex.IsMatch(trimmedText);
        }

        // merge a Date entity and a Time entity
        private DateTimeResolutionResult MergeDateAndTime(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var matchAgoLater = this.config.AgoLaterRegex.Match(text);

            // cases with ago or later are processed in ParserDurationWithAgoAndLater
            if (matchAgoLater.Success)
            {
                return ret;
            }

            var er1 = this.config.DateExtractor.Extract(text, referenceTime);
            if (er1.Count == 0)
            {
                return ret;
            }

            var er2 = this.config.TimeExtractor.Extract(text, referenceTime);
            if (er2.Count == 0)
            {
                return ret;
            }

            // TODO: Add reference time
            var pr1 = this.config.DateParser.Parse(er1[0], referenceTime.Date);
            var pr2 = this.config.TimeParser.Parse(er2[0], referenceTime);

            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            var futureDate = (DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue;
            var pastDate = (DateObject)((DateTimeResolutionResult)pr1.Value).PastValue;
            var time = (DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue;

            var hour = time.Hour;
            var min = time.Minute;
            var sec = time.Second;

            // handle morning, afternoon
            if (this.config.SimplePmRegex.IsMatch(text) && hour < Constants.HalfDayHourCount)
            {
                hour += Constants.HalfDayHourCount;
            }
            else if (this.config.SimpleAmRegex.IsMatch(text) && hour >= Constants.HalfDayHourCount)
            {
                hour -= Constants.HalfDayHourCount;
            }

            var timeStr = pr2.TimexStr;
            if (timeStr.EndsWith(Constants.Comment_AmPm, StringComparison.Ordinal))
            {
                timeStr = timeStr.Substring(0, timeStr.Length - 4);
            }

            timeStr = "T" + hour.ToString("D2", CultureInfo.InvariantCulture) + timeStr.Substring(3);
            ret.Timex = pr1.TimexStr + timeStr;

            var val = (DateTimeResolutionResult)pr2.Value;

            if (hour <= Constants.HalfDayHourCount && !this.config.SimplePmRegex.IsMatch(text) && !this.config.SimpleAmRegex.IsMatch(text) &&
                !string.IsNullOrEmpty(val.Comment))
            {
                // ret.Timex += "ampm";
                ret.Comment = Constants.Comment_AmPm;
            }

            ret.FutureValue = DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, hour, min, sec);
            ret.PastValue = DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, hour, min, sec);
            ret.Success = true;

            return ret;
        }

        private DateTimeResolutionResult ParseTimeOfSpecialDayRegex(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var ers = this.config.TimeExtractor.Extract(text, referenceTime);

            // Handle 'eod', 'end of day'
            var eod = this.config.TimeOfSpecialDayRegex.Match(text);

            if (eod.Groups[Constants.SpecificEndOfGroupName].Success && ers.Count == 0)
            {
                ret = ParseSpecialTimeOfDate(text, referenceTime);
                return ret;
            }

            if (eod.Success && ers.Count != 1)
            {
                ret = DateTimeFormatUtil.ResolveEndOfDay(DateTimeFormatUtil.FormatDate(referenceTime), referenceTime, referenceTime);
                return ret;
            }

            if (ers.Count != 1)
            {
                return ret;
            }

            // TODO: Add reference time
            var pr = this.config.TimeParser.Parse(ers[0], referenceTime);
            if (pr.Value == null)
            {
                return ret;
            }

            var time = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;

            var hour = time.Hour;
            var min = time.Minute;
            var sec = time.Second;

            var match = this.config.TimeOfSpecialDayRegex.Match(text);

            if (match.Success)
            {
                var matchStr = match.Value;

                var swift = 0;

                this.config.AdjustByTimeOfDay(matchStr, ref hour, ref swift);

                var date = referenceTime.AddDays(swift).Date;

                // in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
                var timeStr = pr.TimexStr;
                if (timeStr.EndsWith(Constants.Comment_AmPm, StringComparison.Ordinal))
                {
                    timeStr = timeStr.Substring(0, timeStr.Length - 4);
                }

                // handle less and more mode
                if (match.Groups[Constants.LessGroupName].Success)
                {
                    ret.Mod = Constants.LESS_THAN_MOD;
                }
                else if (match.Groups[Constants.MoreGroupName].Success)
                {
                    ret.Mod = Constants.MORE_THAN_MOD;
                }

                timeStr = "T" + hour.ToString("D2", CultureInfo.InvariantCulture) + timeStr.Substring(3);

                ret.Timex = DateTimeFormatUtil.FormatDate(date) + timeStr;
                ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(date.Year, date.Month, date.Day, hour, min, sec);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseSpecialTimeOfDate(string text, DateObject refDateTime)
        {

            var ret = new DateTimeResolutionResult();
            var ers = this.config.DateExtractor.Extract(text, refDateTime);
            if (ers.Count != 1)
            {
                return ret;
            }

            var pr = this.config.DateParser.Parse(ers[0], refDateTime);
            var futureDate = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
            var pastDate = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;

            ret = DateTimeFormatUtil.ResolveEndOfDay(pr.TimexStr, futureDate, pastDate);

            return ret;
        }

        // handle cases like "5分钟前", "1小时以后"
        private DateTimeResolutionResult ParserDurationWithAgoAndLater(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var durationRes = this.config.DurationExtractor.Extract(text, referenceDate);

            if (durationRes.Count > 0)
            {
                var matchAgoLater = this.config.AgoLaterRegex.Match(text);

                if (matchAgoLater.Success)
                {
                    var pr = config.DurationParser.Parse(durationRes[0], referenceDate);
                    var isFuture = matchAgoLater.Groups[Constants.LaterGroupName].Success;
                    var timex = pr.TimexStr;

                    var resultDateTime = DurationParsingUtil.ShiftDateTime(timex, referenceDate, future: isFuture);
                    ret.Timex = $"{DateTimeFormatUtil.LuisDateTime(resultDateTime)}";
                    ret.FutureValue = ret.PastValue = resultDateTime;
                    ret.SubDateTimeEntities = new List<object> { pr };

                    ret.Success = true;
                    return ret;
                }

                var match = this.config.DateTimePeriodUnitRegex.Match(text);
                if (match.Success)
                {
                    var suffix = text.Substring((int)durationRes[0].Start + (int)durationRes[0].Length).Trim();
                    var srcUnit = match.Groups[Constants.UnitGroupName].Value;

                    var numberStr = text.Substring((int)durationRes[0].Start, match.Index - (int)durationRes[0].Start).Trim();
                    var number = ConvertCJKToNum(numberStr);

                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        var unitStr = this.config.UnitMap[srcUnit];

                        var beforeMatch = this.config.BeforeRegex.Match(suffix);
                        if (beforeMatch.Success && suffix.StartsWith(beforeMatch.Value, StringComparison.InvariantCulture))
                        {
                            DateObject date;
                            switch (unitStr)
                            {
                                case Constants.TimexHour:
                                    date = referenceDate.AddHours(-number);
                                    break;
                                case Constants.TimexMinute:
                                    date = referenceDate.AddMinutes(-number);
                                    break;
                                case Constants.TimexSecond:
                                    date = referenceDate.AddSeconds(-number);
                                    break;
                                default:
                                    return ret;
                            }

                            ret.Timex = $"{DateTimeFormatUtil.LuisDate(date)}";
                            ret.FutureValue = ret.PastValue = date;
                            ret.Success = true;
                            return ret;
                        }

                        var afterMatch = this.config.AfterRegex.Match(suffix);
                        if (afterMatch.Success && suffix.StartsWith(afterMatch.Value, StringComparison.Ordinal))
                        {
                            DateObject date;
                            switch (unitStr)
                            {
                                case Constants.TimexHour:
                                    date = referenceDate.AddHours(number);
                                    break;
                                case Constants.TimexMinute:
                                    date = referenceDate.AddMinutes(number);
                                    break;
                                case Constants.TimexSecond:
                                    date = referenceDate.AddSeconds(number);
                                    break;
                                default:
                                    return ret;
                            }

                            ret.Timex = $"{DateTimeFormatUtil.LuisDate(date)}";
                            ret.FutureValue = ret.PastValue = date;
                            ret.Success = true;
                            return ret;
                        }
                    }
                }
            }

            return ret;
        }

        // convert CJK Number to Integer
        private int ConvertCJKToNum(string numStr)
        {
            var num = -1;
            var er = this.config.IntegerExtractor.Extract(numStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                {
                    num = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));
                }
            }

            return num;
        }
    }
}
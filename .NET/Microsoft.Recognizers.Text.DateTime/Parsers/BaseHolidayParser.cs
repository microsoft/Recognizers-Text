// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseHolidayParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date"

        private static bool inclusiveEndPeriod = false;

        private readonly IHolidayParserConfiguration config;

        public BaseHolidayParser(IHolidayParserConfiguration config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            if (er.Metadata?.IsHolidayWeekend ?? false)
            {
                return ParseHolidayWeekend(er, refDate);
            }
            else
            {
                return ParseSingleDate(er, refDate);
            }
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        // This will parse a holiday to the date of a single day
        private DateTimeParseResult ParseSingleDate(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;
            object value = null;

            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
            {
                var innerResult = ParseHolidayRegexMatch(er.Text, referenceDate);

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.FutureValue) },
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.PastValue) },
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

        // This will parse to a date ranging between the holiday and the closest weekend
        // cases: "Thanksgiving weekend", "weekend of Halloween"
        private DateTimeParseResult ParseHolidayWeekend(ExtractResult er, DateObject referenceDate)
        {
            var dateTimeRes = new DateTimeResolutionResult();

            if (!string.IsNullOrEmpty(er.Metadata?.HolidayName))
            {
                var holidayName = er.Metadata.HolidayName;

                // resolve holiday
                var holidayEr = new ExtractResult
                {
                    Start = 0,
                    Length = holidayName.Length,
                    Text = holidayName,
                    Type = Constants.SYS_DATETIME_DATE,
                    Data = null,
                    Metadata = new Metadata { IsHoliday = true },
                };
                var result = (DateTimeResolutionResult)this.Parse(holidayEr, referenceDate).Value;

                if (!result.Success)
                {
                    dateTimeRes.FutureResolution = dateTimeRes.PastResolution = new Dictionary<string, string>();
                }
                else
                {
                    // get closest weekend to the holiday(s)
                    var futureWeekend = GetClosestHolidayWeekend((DateObject)result.FutureValue);
                    var pastWeekend = futureWeekend;

                    if (result.FutureValue == result.PastValue)
                    {
                        dateTimeRes.Timex = TimexUtility.GenerateWeekendTimex(futureWeekend.Item1);
                    }
                    else
                    {
                        dateTimeRes.Timex = result.Timex;
                        pastWeekend = GetClosestHolidayWeekend((DateObject)result.PastValue);
                    }

                    dateTimeRes.Success = true;
                    dateTimeRes.FutureValue = futureWeekend;
                    dateTimeRes.PastValue = pastWeekend;

                    dateTimeRes.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATE,
                            DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)dateTimeRes.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATE,
                            DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)dateTimeRes.FutureValue).Item2)
                        },
                        {
                            DateTimeResolutionKey.Timex,
                            TimexUtility.GenerateWeekendTimex(futureWeekend.Item1)
                        },
                    };

                    dateTimeRes.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_DATE,
                            DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)dateTimeRes.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_DATE,
                            DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)dateTimeRes.PastValue).Item2)
                        },
                        {
                            DateTimeResolutionKey.Timex,
                            TimexUtility.GenerateWeekendTimex(pastWeekend.Item1)
                        },
                    };
                }
            }
            else
            {
                dateTimeRes.FutureResolution = dateTimeRes.PastResolution = new Dictionary<string, string>();
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Metadata = er.Metadata,
                Value = dateTimeRes,
                TimexStr = dateTimeRes == null ? string.Empty : ((DateTimeResolutionResult)dateTimeRes).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        private Tuple<DateObject, DateObject> GetClosestHolidayWeekend(DateObject dateTimeObject)
        {
            // this week's Saturday
            var startDate = dateTimeObject.This(DayOfWeek.Saturday);
            var endDate = dateTimeObject.This(DayOfWeek.Sunday);

            // is last weekend closer than this one? i.e. is the input Monday or Tuesday?
            if (dateTimeObject.DayOfWeek == DayOfWeek.Monday || dateTimeObject.DayOfWeek == DayOfWeek.Tuesday)
            {
                startDate = startDate.AddDays(-7);
                endDate = dateTimeObject;
            }
            else if (dateTimeObject.DayOfWeek != DayOfWeek.Sunday)
            {
                startDate = dateTimeObject;
            }

            endDate = inclusiveEndPeriod ? endDate : endDate.AddDays(1);

            return new Tuple<DateObject, DateObject>(startDate, endDate);
        }

        private DateTimeResolutionResult ParseHolidayRegexMatch(string text, DateObject referenceDate)
        {
            foreach (var regex in this.config.HolidayRegexList)
            {
                var match = regex.MatchExact(text, trim: true);

                if (match.Success)
                {
                    // Value string will be set in Match2Date method
                    var ret = Match2Date(match.Match, referenceDate);
                    return ret;
                }
            }

            return new DateTimeResolutionResult();
        }

        private DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var holidayStr = this.config.SanitizeHolidayToken(match.Groups["holiday"].Value);

            // get year (if exist)
            var yearStr = match.Groups["year"].Value;
            var orderStr = match.Groups["order"].Value;
            int year;
            var hasYear = false;
            var swift = 0;

            if (!string.IsNullOrEmpty(yearStr))
            {
                year = int.Parse(yearStr, CultureInfo.InvariantCulture);
                hasYear = true;
            }
            else if (!string.IsNullOrEmpty(orderStr))
            {
                swift = this.config.GetSwiftYear(orderStr);
                if (swift < -1)
                {
                    return ret;
                }

                year = referenceDate.Year + swift;
                hasYear = true;
            }
            else
            {
                year = referenceDate.Year;
            }

            string holidayKey = string.Empty;
            foreach (var holidayPair in this.config.HolidayNames)
            {
                if (holidayPair.Value.Contains(holidayStr))
                {
                    holidayKey = holidayPair.Key;
                    break;
                }
            }

            var timexStr = string.Empty;
            if (!string.IsNullOrEmpty(holidayKey))
            {
                var value = referenceDate;
                if (this.config.HolidayFuncDictionary.TryGetValue(holidayKey, out Func<int, DateObject> function))
                {
                    // With relative holidays like 'next(last) easter' the year must not be shifted
                    // when the reference date precedes(follows) the holiday date.
                    if (string.IsNullOrEmpty(yearStr) && swift != 0)
                    {
                        value = function(referenceDate.Year);
                        if ((swift > 0 && value < referenceDate) || (swift < 0 && value > referenceDate))
                        {
                            value = function(year);
                        }
                        else
                        {
                            year = referenceDate.Year;
                        }
                    }
                    else
                    {
                        value = function(year);
                    }

                    // @TODO should be checking if variable holiday to produce better timex. Fixing is a breaking change.
                    this.config.VariableHolidaysTimexDictionary.TryGetValue(holidayKey, out timexStr);
                    if (string.IsNullOrEmpty(timexStr))
                    {
                        timexStr = $"-{value.Month:D2}-{value.Day:D2}";
                    }
                }

                if (function == null)
                {
                    return ret;
                }

                if (value.Equals(DateObject.MinValue))
                {
                    ret.Timex = string.Empty;
                    ret.FutureValue = ret.PastValue = DateObject.MinValue;
                    ret.Success = true;
                    return ret;
                }

                if (hasYear)
                {
                    ret.Timex = year.ToString("D4", CultureInfo.InvariantCulture) + timexStr;
                    ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(year, value.Month, value.Day);
                    ret.Success = true;
                    return ret;
                }

                ret.Timex = "XXXX" + timexStr;
                ret.FutureValue = GetFutureValue(value, referenceDate, holidayKey);
                ret.PastValue = GetPastValue(value, referenceDate, holidayKey);
                ret.Success = true;

                return ret;
            }

            return ret;
        }

        private DateObject GetFutureValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value < referenceDate)
            {
                if (this.config.HolidayFuncDictionary.TryGetValue(holiday, out Func<int, DateObject> function))
                {
                    return function(value.Year + 1);
                }
            }

            return value;
        }

        private DateObject GetPastValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value >= referenceDate)
            {
                if (this.config.HolidayFuncDictionary.TryGetValue(holiday, out Func<int, DateObject> function))
                {
                    return function(value.Year - 1);
                }
            }

            return value;
        }
    }
}
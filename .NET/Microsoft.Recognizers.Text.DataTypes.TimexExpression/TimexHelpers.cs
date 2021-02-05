// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public enum TimexUnit
    {
        /// <summary>
        /// Year
        /// </summary>
        Year,

        /// <summary>
        /// Month
        /// </summary>
        Month,

        /// <summary>
        /// Week
        /// </summary>
        Week,

        /// <summary>
        /// Day
        /// </summary>
        Day,

        /// <summary>
        /// Hour
        /// </summary>
        Hour,

        /// <summary>
        /// Minute
        /// </summary>
        Minute,

        /// <summary>
        /// Second
        /// </summary>
        Second,
    }

    public static class TimexHelpers
    {
        public static readonly Dictionary<TimexUnit, string> TimexUnitToStringMap = new Dictionary<TimexUnit, string>
        {
            { TimexUnit.Year, Constants.TimexYear },
            { TimexUnit.Month, Constants.TimexMonth },
            { TimexUnit.Week, Constants.TimexWeek },
            { TimexUnit.Day, Constants.TimexDay },
            { TimexUnit.Hour, Constants.TimexHour },
            { TimexUnit.Minute, Constants.TimexMinute },
            { TimexUnit.Second, Constants.TimexSecond },
        };

        public static readonly List<TimexUnit> TimeTimexUnitList = new List<TimexUnit> { TimexUnit.Hour, TimexUnit.Minute, TimexUnit.Second };

        public static TimexRange ExpandDateTimeRange(TimexProperty timex)
        {
            var types = timex.Types.Count != 0 ? timex.Types : TimexInference.Infer(timex);

            if (types.Contains(Constants.TimexTypes.Duration))
            {
                var start = CloneDateTime(timex);
                var duration = CloneDuration(timex);
                return new TimexRange
                {
                    Start = start,
                    End = TimexDateTimeAdd(start, duration),
                    Duration = duration,
                };
            }
            else
            {
                if (timex.Year != null)
                {
                    var range = new TimexRange { Start = new TimexProperty { Year = timex.Year }, End = new TimexProperty() };
                    if (timex.Month != null)
                    {
                        range.Start.Month = timex.Month;
                        range.Start.DayOfMonth = 1;
                        range.End.Year = timex.Year;
                        range.End.Month = timex.Month + 1;
                        range.End.DayOfMonth = 1;
                    }
                    else
                    {
                        range.Start.Month = 1;
                        range.Start.DayOfMonth = 1;
                        range.End.Year = timex.Year + 1;
                        range.End.Month = 1;
                        range.End.DayOfMonth = 1;
                    }

                    return range;
                }
            }

            return new TimexRange { Start = new TimexProperty(), End = new TimexProperty() };
        }

        public static TimexRange ExpandTimeRange(TimexProperty timex)
        {
            if (!timex.Types.Contains(Constants.TimexTypes.TimeRange))
            {
                throw new ArgumentException("argument must be a timerange", nameof(timex));
            }

            if (timex.PartOfDay != null)
            {
                switch (timex.PartOfDay)
                {
                    case "DT":
                        timex = new TimexProperty(TimexCreator.Daytime);
                        break;
                    case "MO":
                        timex = new TimexProperty(TimexCreator.Morning);
                        break;
                    case "AF":
                        timex = new TimexProperty(TimexCreator.Afternoon);
                        break;
                    case "EV":
                        timex = new TimexProperty(TimexCreator.Evening);
                        break;
                    case "NI":
                        timex = new TimexProperty(TimexCreator.Night);
                        break;
                    default:
                        throw new ArgumentException("unrecognized part of day timerange", nameof(timex));
                }
            }

            var start = new TimexProperty { Hour = timex.Hour, Minute = timex.Minute, Second = timex.Second };
            var duration = CloneDuration(timex);

            return new TimexRange
            {
                Start = start,
                End = TimeAdd(start, duration),
                Duration = duration,
            };
        }

        public static TimexProperty TimexDateAdd(TimexProperty start, TimexProperty duration)
        {
            if (start.DayOfWeek != null)
            {
                var end = start.Clone();
                if (duration.Days != null)
                {
                    end.DayOfWeek += (int)duration.Days;
                }

                return end;
            }

            if (start.Month != null && start.DayOfMonth != null)
            {
                decimal? durationDays = duration.Days;

                if (durationDays == null && duration.Weeks != null)
                {
                    durationDays = 7 * duration.Weeks.Value;
                }

                if (durationDays != null)
                {
                    if (start.Year != null)
                    {
                        var d = new DateObject(start.Year.Value, start.Month.Value, start.DayOfMonth.Value, 0, 0, 0);
                        d = d.AddDays((double)durationDays.Value);

                        return new TimexProperty
                        {
                            Year = d.Year,
                            Month = d.Month,
                            DayOfMonth = d.Day,
                        };
                    }
                    else
                    {
                        var d = new DateObject(2001, start.Month.Value, start.DayOfMonth.Value, 0, 0, 0);
                        d = d.AddDays((double)durationDays.Value);

                        return new TimexProperty
                        {
                            Month = d.Month,
                            DayOfMonth = d.Day,
                        };
                    }
                }

                if (duration.Years != null)
                {
                    if (start.Year != null)
                    {
                        return new TimexProperty
                        {
                            Year = (int)(start.Year.Value + duration.Years.Value),
                            Month = start.Month,
                            DayOfMonth = start.DayOfMonth,
                        };
                    }
                }

                if (duration.Months != null)
                {
                    if (start.Month != null)
                    {
                        return new TimexProperty
                        {
                            Year = start.Year,
                            Month = (int)(start.Month + duration.Months),
                            DayOfMonth = start.DayOfMonth,
                        };
                    }
                }
            }

            return start;
        }

        public static string GenerateDateTimex(int year, int monthOrWeekOfYear, int day, int weekOfMonth, bool byWeek)
        {
            var yearString = year == Constants.InvalidValue ? Constants.TimexFuzzyYear : TimexDateHelpers.FixedFormatNumber(year, 4);
            var monthWeekString = monthOrWeekOfYear == Constants.InvalidValue ? Constants.TimexFuzzyMonth : TimexDateHelpers.FixedFormatNumber(monthOrWeekOfYear, 2);
            string dayString;
            if (byWeek)
            {
                dayString = day.ToString(CultureInfo.InvariantCulture);
                if (weekOfMonth != Constants.InvalidValue)
                {
                    monthWeekString = monthWeekString + $"-{Constants.TimexFuzzyWeek}-" + weekOfMonth.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    monthWeekString = Constants.TimexWeek + monthWeekString;
                }
            }
            else
            {
                dayString = day == Constants.InvalidValue ? Constants.TimexFuzzyDay : TimexDateHelpers.FixedFormatNumber(day, 2);
            }

            return $"{yearString}-{monthWeekString}-{dayString}";
        }

        public static TimexProperty TimexTimeAdd(TimexProperty start, TimexProperty duration)
        {
            var result = start.Clone();
            if (duration.Minutes != null)
            {
                result.Minute += (int)duration.Minutes.Value;
                if (result.Minute.Value > 59)
                {
                    result.Hour = (result.Hour ?? 0) + 1;
                    result.Minute = result.Minute.Value % 60;
                }
            }

            if (duration.Hours != null)
            {
                result.Hour += (int)duration.Hours.Value;
            }

            if (result.Hour != null && result.Hour.Value > 23)
            {
                var days = Math.Floor(result.Hour.Value / 24m);
                var hour = result.Hour.Value % 24;
                result.Hour = hour;

                if (result.Year != null && result.Month != null && result.DayOfMonth != null)
                {
                    var d = new DateObject(result.Year.Value, result.Month.Value, result.DayOfMonth.Value, 0, 0, 0);
                    d = d.AddDays((double)days);

                    result.Year = d.Year;
                    result.Month = d.Month;
                    result.DayOfMonth = d.Day;

                    return result;
                }

                if (result.DayOfWeek != null)
                {
                    result.DayOfWeek += (int)days;
                    return result;
                }
            }

            return result;
        }

        public static TimexProperty TimexDateTimeAdd(TimexProperty start, TimexProperty duration)
        {
            return TimexTimeAdd(TimexDateAdd(start, duration), duration);
        }

        public static DateObject DateFromTimex(TimexProperty timex)
        {
            return new DateObject(timex.Year ?? 2001, timex.Month ?? 1, timex.DayOfMonth ?? 1, timex.Hour ?? 0, timex.Minute ?? 0, timex.Second ?? 0);
        }

        public static Time TimeFromTimex(TimexProperty timex)
        {
            return new Time(timex.Hour ?? 0, timex.Minute ?? 0, timex.Second ?? 0);
        }

        public static DateRange DateRangeFromTimex(TimexProperty timex)
        {
            var expanded = ExpandDateTimeRange(timex);
            return new DateRange
            {
                Start = DateFromTimex(expanded.Start),
                End = DateFromTimex(expanded.End),
            };
        }

        public static TimeRange TimeRangeFromTimex(TimexProperty timex)
        {
            var expanded = ExpandTimeRange(timex);
            return new TimeRange
            {
                Start = TimeFromTimex(expanded.Start),
                End = TimeFromTimex(expanded.End),
            };
        }

        public static string GenerateCompoundDurationTimex(List<string> timexList)
        {
            var isTimeDurationAlreadyExist = false;
            var timexBuilder = new StringBuilder(Constants.GeneralPeriodPrefix);

            foreach (string timexComponent in timexList)
            {
                // The Time Duration component occurs first time
                if (!isTimeDurationAlreadyExist && IsTimeDurationTimex(timexComponent))
                {
                    timexBuilder.AppendFormat(CultureInfo.InvariantCulture, $"{Constants.TimeTimexPrefix}{GetDurationTimexWithoutPrefix(timexComponent)}");
                    isTimeDurationAlreadyExist = true;
                }
                else
                {
                    timexBuilder.AppendFormat(CultureInfo.InvariantCulture, $"{GetDurationTimexWithoutPrefix(timexComponent)}");
                }
            }

            return timexBuilder.ToString();
        }

        public static string GenerateDateTimex(int year, int month, int day, bool byWeek)
        {
            var yearString = year == Constants.InvalidValue ? Constants.TimexFuzzyYear : TimexDateHelpers.FixedFormatNumber(year, 4);
            var monthString = month == Constants.InvalidValue ? Constants.TimexFuzzyMonth : TimexDateHelpers.FixedFormatNumber(month, 2);
            string dayString;
            if (byWeek)
            {
                dayString = day.ToString(CultureInfo.InvariantCulture);
                monthString = Constants.TimexWeek + monthString;
            }
            else
            {
                dayString = day == Constants.InvalidValue ? Constants.TimexDay : TimexDateHelpers.FixedFormatNumber(day, 2);
            }

            return $"{yearString}-{monthString}-{dayString}";
        }

        public static string GenerateDurationTimex(TimexUnit unit, decimal value)
        {
            if (value == Constants.InvalidValue)
            {
                return string.Empty;
            }

            var timexBuilder = new StringBuilder(Constants.GeneralPeriodPrefix);
            if (TimeTimexUnitList.Contains(unit))
            {
                timexBuilder.AppendFormat(CultureInfo.InvariantCulture, Constants.TimeTimexPrefix);
            }

            timexBuilder.AppendFormat(CultureInfo.InvariantCulture, value.ToString(CultureInfo.InvariantCulture));
            timexBuilder.AppendFormat(CultureInfo.InvariantCulture, TimexUnitToStringMap[unit]);
            return timexBuilder.ToString();
        }

        public static string FormatResolvedDateValue(string dateValue, string timeValue)
        {
            return $"{dateValue} {timeValue}";
        }

        private static TimexProperty TimeAdd(TimexProperty start, TimexProperty duration)
        {
            int second = (int)(start.Second + (duration.Seconds ?? 0));
            int minute = (int)(start.Minute + (second / 60) + (duration.Minutes ?? 0));
            int hour = (int)(start.Hour + (minute / 60) + (duration.Hours ?? 0));

            return new TimexProperty
            {
                Hour = (hour == 24 && minute % 60 == 0 && second % 60 == 0) ? hour : hour % 24,
                Minute = minute % 60,
                Second = second % 60,
            };
        }

        private static TimexProperty CloneDateTime(TimexProperty timex)
        {
            var result = timex.Clone();
            result.Years = null;
            result.Months = null;
            result.Weeks = null;
            result.Days = null;
            result.Hours = null;
            result.Minutes = null;
            result.Seconds = null;
            return result;
        }

        private static TimexProperty CloneDuration(TimexProperty timex)
        {
            var result = timex.Clone();
            result.Year = null;
            result.Month = null;
            result.DayOfMonth = null;
            result.DayOfWeek = null;
            result.WeekOfYear = null;
            result.WeekOfMonth = null;
            result.Season = null;
            result.Hour = null;
            result.Minute = null;
            result.Second = null;
            result.Weekend = null;
            result.PartOfDay = null;
            return result;
        }

        private static bool IsTimeDurationTimex(string timex)
        {
            return timex.StartsWith($"{Constants.GeneralPeriodPrefix}{Constants.TimeTimexPrefix}", StringComparison.Ordinal);
        }

        private static string GetDurationTimexWithoutPrefix(string timex)
        {
            // Remove "PT" prefix for TimeDuration, Remove "P" prefix for DateDuration
            return timex.Substring(IsTimeDurationTimex(timex) ? 2 : 1);
        }
    }
}

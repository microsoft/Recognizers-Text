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
                    Tuple<TimexProperty, TimexProperty> dateRange;
                    if (timex.Month != null && timex.WeekOfMonth != null)
                    {
                        dateRange = MonthWeekDateRange(timex.Year.Value, timex.Month.Value, timex.WeekOfMonth.Value);
                    }
                    else if (timex.Month != null)
                    {
                        dateRange = MonthDateRange(timex.Year.Value, timex.Month.Value);
                    }
                    else if (timex.WeekOfYear != null)
                    {
                        dateRange = YearWeekDateRange(timex.Year.Value, timex.WeekOfYear.Value, timex.Weekend);
                    }
                    else
                    {
                        dateRange = YearDateRange(timex.Year.Value);
                    }

                    return new TimexRange { Start = dateRange.Item1, End = dateRange.Item2 };
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

        public static Tuple<TimexProperty, TimexProperty> MonthWeekDateRange(int year, int month, int weekOfMonth)
        {
            var start = GenerateMonthWeekDateStart(year, month, weekOfMonth);
            var end = start.AddDays(7);

            return new Tuple<TimexProperty, TimexProperty>(
                new TimexProperty { Year = start.Year, Month = start.Month, DayOfMonth = start.Day },
                new TimexProperty { Year = end.Year, Month = end.Month, DayOfMonth = end.Day });
        }

        public static Tuple<TimexProperty, TimexProperty> MonthDateRange(int year, int month)
        {
            return new Tuple<TimexProperty, TimexProperty>(
                new TimexProperty { Year = year, Month = month, DayOfMonth = 1 },
                new TimexProperty { Year = month == 12 ? year + 1 : year, Month = month == 12 ? 1 : month + 1, DayOfMonth = 1 });
        }

        public static Tuple<TimexProperty, TimexProperty> YearDateRange(int year)
        {
            return new Tuple<TimexProperty, TimexProperty>(
                new TimexProperty { Year = year, Month = 1, DayOfMonth = 1 },
                new TimexProperty { Year = year + 1, Month = 1, DayOfMonth = 1 });
        }

        public static Tuple<TimexProperty, TimexProperty> YearWeekDateRange(int year, int weekOfYear, bool? isWeekend)
        {
            var firstMondayInWeek = FirstDateOfWeek(year, weekOfYear, System.Globalization.CultureInfo.InvariantCulture);

            var start = (isWeekend == null || isWeekend.Value == false) ?
                            firstMondayInWeek :
                            TimexDateHelpers.DateOfNextDay(DayOfWeek.Saturday, firstMondayInWeek);
            var end = firstMondayInWeek + TimeSpan.FromDays(7);

            return new Tuple<TimexProperty, TimexProperty>(
                new TimexProperty { Year = start.Year, Month = start.Month, DayOfMonth = start.Day },
                new TimexProperty { Year = end.Year, Month = end.Month, DayOfMonth = end.Day });
        }

        // this is based on https://stackoverflow.com/questions/19901666/get-date-of-first-and-last-day-of-week-knowing-week-number/34727270
        public static DateObject FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo cultureInfo)
        {
            // ISO uses FirstFourDayWeek, and Monday as first day of week, according to https://en.wikipedia.org/wiki/ISO_8601
            var jan1 = new DateObject(year, 1, 1);
            int daysOffset = (int)DayOfWeek.Monday - (int)jan1.DayOfWeek;
            var firstWeekDay = jan1.AddDays(daysOffset);

            int firstWeek = cultureInfo.Calendar.GetWeekOfYear(jan1, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }

            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        public static DateObject GenerateMonthWeekDateStart(int year, int month, int weekOfMonth)
        {
            var dateInWeek = new DateObject(year, month, 1 + ((weekOfMonth - 1) * 7));

            // Align the date of the week according to Thursday, base on ISO 8601, https://en.wikipedia.org/wiki/ISO_8601
            if (dateInWeek.DayOfWeek > DayOfWeek.Thursday)
            {
                dateInWeek = dateInWeek.AddDays(7 - (int)dateInWeek.DayOfWeek + 1);
            }
            else
            {
                dateInWeek = dateInWeek.AddDays(1 - (int)dateInWeek.DayOfWeek);
            }

            return dateInWeek;
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

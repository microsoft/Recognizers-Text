// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    internal static class TimexConvertEnglish
    {
        public static string ConvertTimexToString(TimexProperty timex)
        {
            if (timex.IsPresent)
            {
                return TimexConstantsEnglish.Now;
            }

            if (timex.IsDateTimeRange)
            {
                return ConvertDateTimeRange(timex);
            }

            if (timex.IsDateRange)
            {
                return ConvertDateRange(timex);
            }

            if (timex.IsDuration)
            {
                return ConvertDuration(timex);
            }

            if (timex.IsTimeRange)
            {
                return ConvertTimeRange(timex);
            }

            // TODO: where appropriate delegate most the formatting delegate to Date.toLocaleString(options)
            if (timex.IsDateTime)
            {
                return ConvertDateTime(timex);
            }

            if (timex.IsDate)
            {
                return ConvertDate(timex);
            }

            if (timex.IsTime)
            {
                return ConvertTime(timex);
            }

            return string.Empty;
        }

        public static string ConvertTimexSetToString(TimexSet timexSet)
        {
            var timex = timexSet.Timex;
            if (timex.Types.Contains(Constants.TimexTypes.Duration))
            {
                return $"{TimexConstantsEnglish.Every} {ConvertTimexDurationToString(timex, false)}";
            }
            else
            {
                return $"{TimexConstantsEnglish.Every} {ConvertTimexToString(timex)}";
            }
        }

        public static string ConvertTime(TimexProperty timex)
        {
            if (timex.Hour == 0 && timex.Minute == 0 && timex.Second == 0)
            {
                return TimexConstantsEnglish.Midnight;
            }

            if (timex.Hour == 12 && timex.Minute == 0 && timex.Second == 0)
            {
                return TimexConstantsEnglish.Midday;
            }

            var hour = (timex.Hour == 0) ? "12" : (timex.Hour > 12) ? (timex.Hour - 12).Value.ToString(CultureInfo.InvariantCulture) : timex.Hour.Value.ToString(CultureInfo.InvariantCulture);
            var minute = (timex.Minute == 0 && timex.Second == 0) ? string.Empty : Constants.TimeTimexConnector + timex.Minute.Value.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            var second = (timex.Second == 0) ? string.Empty : Constants.TimeTimexConnector + timex.Second.Value.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            var period = timex.Hour < 12 ? Constants.AM : Constants.PM;

            return $"{hour}{minute}{second}{period}";
        }

        public static string ConvertDate(TimexProperty timex)
        {
            if (timex.DayOfWeek != null)
            {
                return TimexConstantsEnglish.Days[timex.DayOfWeek.Value - 1];
            }

            var parts = new List<string>(3);

            if (timex.DayOfMonth.HasValue)
            {
                var dayOfMonth = timex.DayOfMonth.Value;

                // Ordinals 11 to 13 are special in english as they end in th
                var abbreviation = TimexConstantsEnglish.DateAbbreviation[(dayOfMonth is > 9 and < 14 ? 9 : dayOfMonth) % 10];
                parts.Add($"{dayOfMonth}{abbreviation}");
            }

            if (timex.Month != null)
            {
                parts.Add(TimexConstantsEnglish.Months[timex.Month.Value - 1]);
            }

            if (timex.Year != null)
            {
                parts.Add(timex.Year.ToString());
            }

            return parts.Count > 0 ? string.Join(" ", parts) : string.Empty;
        }

        private static string ConvertDurationPropertyToString(decimal value, string property, bool includeSingleCount)
        {
            if (value == 1)
            {
                return includeSingleCount ? "1 " + property : property;
            }
            else
            {
                return $"{value} {property}{Constants.TimeDurationUnit}";
            }
        }

        private static string ConvertTimexDurationToString(TimexProperty timex, bool includeSingleCount)
        {
            var parts = new List<string>();
            if (timex.Years != null)
            {
                parts.Add(ConvertDurationPropertyToString(timex.Years.Value, Constants.YearUnit, includeSingleCount));
            }

            if (timex.Months != null)
            {
                parts.Add(ConvertDurationPropertyToString(timex.Months.Value, Constants.MonthUnit, includeSingleCount));
            }

            if (timex.Weeks != null)
            {
                parts.Add(ConvertDurationPropertyToString(timex.Weeks.Value, Constants.WeekUnit, includeSingleCount));
            }

            if (timex.Days != null)
            {
                parts.Add(ConvertDurationPropertyToString(timex.Days.Value, Constants.DayUnit, includeSingleCount));
            }

            if (timex.Hours != null)
            {
                parts.Add(ConvertDurationPropertyToString(timex.Hours.Value, Constants.HourUnit, includeSingleCount));
            }

            if (timex.Minutes != null)
            {
                parts.Add(ConvertDurationPropertyToString(timex.Minutes.Value, Constants.MinuteUnit, includeSingleCount));
            }

            if (timex.Seconds != null)
            {
                parts.Add(ConvertDurationPropertyToString(timex.Seconds.Value, Constants.SecondUnit, includeSingleCount));
            }

            return parts.Count > 0 ? string.Join(" ", parts) : string.Empty;
        }

        private static string ConvertDuration(TimexProperty timex)
        {
            return ConvertTimexDurationToString(timex, true);
        }

        private static string ConvertDateRange(TimexProperty timex)
        {
            if (timex.IsDate && !timex.IsDuration)
            {
                return ConvertDate(timex);
            }

            var parts = new List<string>();
            var start = timex;
            var end = TimexProperty.Empty;

            if (timex.Types.Count > 1)
            {
                var dateRange = TimexHelpers.ExpandDateTimeRange(timex);
                start = dateRange.Start;
                end = dateRange.End;
            }

            // output the start of the range
            ConvertRangePoint(parts, start);

            if (end == TimexProperty.Empty)
            {
                return parts.Count > 0 ? string.Join(" ", parts) : string.Empty;
            }

            // seasons are a little special, easier to handle them separately
            if (end.IsSeason)
            {
                if (start.Season != end.Season || start.Year != end.Year)
                {
                    parts.Add(TimexConstantsEnglish.To);
                    parts.Add(TimexConstantsEnglish.Seasons[end.Season]);
                    if (end.Year != null)
                    {
                        parts.Add(end.Year.Value.ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    TryAddDuration(parts, timex);
                }

                return string.Join(" ", parts);
            }

            parts.Add(TimexConstantsEnglish.To);

            // output the end of the range
            ConvertRangePoint(parts, end);

            return parts.Count > 0 ? string.Join(" ", parts) : string.Empty;
        }

        private static void ConvertRangePoint(List<string> parts, TimexProperty timex)
        {
            // Timex with season can only have an optional year
            if (timex.IsSeason)
            {
                parts.Add(TimexConstantsEnglish.Seasons[timex.Season]);
            }

            if (timex.Month != null)
            {
                var timexMonth = $"{TimexConstantsEnglish.Months[timex.Month.Value - 1]}";

                // first/second/third/fourth week of {month}
                if (timex.WeekOfMonth != null)
                {
                    parts.Add(TimexConstantsEnglish.Weeks[timex.WeekOfMonth.Value - 1]);
                    parts.Add(TimexConstantsEnglish.Week);
                    parts.Add(TimexConstantsEnglish.Of);
                }

                parts.Add(timexMonth);
            }

            if (timex.DayOfWeek != null)
            {
                parts.Add(TimexConstantsEnglish.Days[timex.DayOfWeek.Value - 1]);
            }

            if (timex.Weekend != null)
            {
                parts.Add(TimexConstantsEnglish.Weekend);
                parts.Add(TimexConstantsEnglish.Of);
            }

            if (timex.WeekOfYear != null)
            {
                parts.Add(TimexConstantsEnglish.Week);
                parts.Add(timex.WeekOfYear.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (timex.Year != null)
            {
                parts.Add(timex.Year.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static string ConvertTimeRange(TimexProperty timex)
        {
            return TimexConstantsEnglish.DayParts[timex.PartOfDay];
        }

        private static string ConvertDateTime(TimexProperty timex)
        {
            return $"{ConvertTime(timex)} {ConvertDate(timex)}";
        }

        private static string ConvertDateTimeRange(TimexProperty timex)
        {
            var parts = new List<string>();

            bool splitDates = timex.Types.Contains(Constants.TimexTypes.DateRange);

            var range = splitDates
                ? TimexHelpers.ExpandDateTimeRange(timex)
                : TimexHelpers.ExpandTimeRange(timex);

            var start = range.Start;
            var end = range.End;

            splitDates &= start.DateFromTimex() != end.DateFromTimex();

            bool splitTimes = timex.IsTimeRange && !timex.IsPartOfDay;

            TryAddDate(parts, splitDates ? start : timex);
            TryAddTime(parts, splitTimes ? start : timex);

            if (splitDates)
            {
                parts.Add(TimexConstantsEnglish.To);
                TryAddDate(parts, end);
                TryAddTime(parts, end);
                TryAddDuration(parts, end);
            }
            else if (splitTimes)
            {
                parts.Add(TimexConstantsEnglish.To);
                TryAddTime(parts, end);
                TryAddDuration(parts, end);
            }

            if (parts.Count > 0)
            {
                return string.Join(" ", parts);
            }

            return string.Empty;
        }

        private static bool TryAddDate(List<string> list, TimexProperty timexProperty)
        {
            var ret = timexProperty.Types.Contains(Constants.TimexTypes.Date);
            if (ret)
            {
                list.Add(ConvertDate(timexProperty));
            }

            return ret;
        }

        private static bool TryAddTime(List<string> list, TimexProperty timexProperty)
        {
            var ret = timexProperty.Types.Contains(Constants.TimexTypes.Time) || timexProperty.Types.Contains(Constants.TimexTypes.TimeRange);
            if (ret)
            {
                list.Add(timexProperty.PartOfDay is not null ? ConvertTimeRange(timexProperty) : ConvertTime(timexProperty));
            }

            return ret;
        }

        private static bool TryAddDuration(List<string> list, TimexProperty timexProperty)
        {
            var ret = timexProperty.Types.Contains(Constants.TimexTypes.Duration);
            if (ret)
            {
                list.Add(ConvertDuration(timexProperty));
            }

            return ret;
        }

    }
}

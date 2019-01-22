// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    internal static class TimexConvertEnglish
    {
        public static string ConvertTimexToString(TimexProperty timex)
        {
            var types = timex.Types.Count != 0 ? timex.Types : TimexInference.Infer(timex);

            if (types.Contains(Constants.TimexTypes.Present))
            {
                return "now";
            }

            if (types.Contains(Constants.TimexTypes.DateTimeRange))
            {
                return ConvertDateTimeRange(timex);
            }

            if (types.Contains(Constants.TimexTypes.DateRange))
            {
                return ConvertDateRange(timex);
            }

            if (types.Contains(Constants.TimexTypes.Duration))
            {
                return ConvertDuration(timex);
            }

            if (types.Contains(Constants.TimexTypes.TimeRange))
            {
                return ConvertTimeRange(timex);
            }

            // TODO: where appropriate delegate most the formatting delegate to Date.toLocaleString(options)
            if (types.Contains(Constants.TimexTypes.DateTime))
            {
                return ConvertDateTime(timex);
            }

            if (types.Contains(Constants.TimexTypes.Date))
            {
                return ConvertDate(timex);
            }

            if (types.Contains(Constants.TimexTypes.Time))
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
                return $"every {ConvertTimexDurationToString(timex, false)}";
            }
            else
            {
                return $"every {ConvertTimexToString(timex)}";
            }
        }

        public static string ConvertTime(TimexProperty timex)
        {
            if (timex.Hour == 0 && timex.Minute == 0 && timex.Second == 0)
            {
                return "midnight";
            }

            if (timex.Hour == 12 && timex.Minute == 0 && timex.Second == 0)
            {
                return "midday";
            }

            var hour = (timex.Hour == 0) ? "12" : (timex.Hour > 12) ? (timex.Hour - 12).ToString() : timex.Hour.ToString();
            var minute = (timex.Minute == 0 && timex.Second == 0) ? string.Empty : ":" + timex.Minute.ToString().PadLeft(2, '0');
            var second = (timex.Second == 0) ? string.Empty : ":" + timex.Second.ToString().PadLeft(2, '0');
            var period = timex.Hour < 12 ? "AM" : "PM";

            return $"{hour}{minute}{second}{period}";
        }

        public static string ConvertDate(TimexProperty timex)
        {
            if (timex.DayOfWeek != null)
            {
                return TimexConstantsEnglish.Days[timex.DayOfWeek.Value - 1];
            }

            var month = TimexConstantsEnglish.Months[timex.Month.Value - 1];
            var date = timex.DayOfMonth.ToString();
            var abbreviation = TimexConstantsEnglish.DateAbbreviation[int.Parse(date[date.Length - 1].ToString())];

            if (timex.Year != null)
            {
                return $"{date}{abbreviation} {month} {timex.Year}".Trim();
            }

            return $"{date}{abbreviation} {month}";
        }

        private static string ConvertDurationPropertyToString(decimal value, string property, bool includeSingleCount)
        {
            if (value == 1)
            {
                return includeSingleCount ? "1 " + property : property;
            }
            else
            {
                return $"{value} {property}s";
            }
        }

        private static string ConvertTimexDurationToString(TimexProperty timex, bool includeSingleCount)
        {
            if (timex.Years != null)
            {
                return ConvertDurationPropertyToString(timex.Years.Value, "year", includeSingleCount);
            }

            if (timex.Months != null)
            {
                return ConvertDurationPropertyToString(timex.Months.Value, "month", includeSingleCount);
            }

            if (timex.Weeks != null)
            {
                return ConvertDurationPropertyToString(timex.Weeks.Value, "week", includeSingleCount);
            }

            if (timex.Days != null)
            {
                return ConvertDurationPropertyToString(timex.Days.Value, "day", includeSingleCount);
            }

            if (timex.Hours != null)
            {
                return ConvertDurationPropertyToString(timex.Hours.Value, "hour", includeSingleCount);
            }

            if (timex.Minutes != null)
            {
                return ConvertDurationPropertyToString(timex.Minutes.Value, "minute", includeSingleCount);
            }

            if (timex.Seconds != null)
            {
                return ConvertDurationPropertyToString(timex.Seconds.Value, "second", includeSingleCount);
            }

            return string.Empty;
        }

        private static string ConvertDuration(TimexProperty timex)
        {
            return ConvertTimexDurationToString(timex, true);
        }

        private static string ConvertDateRange(TimexProperty timex)
        {
            var season = (timex.Season != null) ? TimexConstantsEnglish.Seasons[timex.Season] : string.Empty;

            var year = (timex.Year != null) ? timex.Year.ToString() : string.Empty;

            if (timex.WeekOfYear != null)
            {
                if (timex.Weekend != null)
                {
                    throw new NotImplementedException();
                }
            }

            if (timex.Month != null)
            {
                var month = $"{TimexConstantsEnglish.Months[timex.Month.Value - 1]}";
                if (timex.WeekOfMonth != null)
                {
                    return $"{TimexConstantsEnglish.Weeks[timex.WeekOfMonth.Value - 1]} week of {month}";
                }
                else
                {
                    return $"{month} {year}".Trim();
                }
            }

            return $"{season} {year}".Trim();
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
            if (timex.Types.Contains(Constants.TimexTypes.TimeRange))
            {
                return $"{ConvertDate(timex)} {ConvertTimeRange(timex)}";
            }

            // date + time + duration
            // - OR -
            // date + duration
            return string.Empty;
        }
    }
}

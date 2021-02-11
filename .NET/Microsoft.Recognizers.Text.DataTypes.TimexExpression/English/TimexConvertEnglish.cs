// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    internal static class TimexConvertEnglish
    {
        public static string ConvertTimexToString(TimexProperty timex)
        {
            var types = timex.Types.Count != 0 ? timex.Types : TimexInference.Infer(timex);

            if (types.Contains(Constants.TimexTypes.Present))
            {
                return TimexConstantsEnglish.Now;
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

            var date = timex.DayOfMonth.Value.ToString(CultureInfo.InvariantCulture);
            var abbreviation = TimexConstantsEnglish.DateAbbreviation[int.Parse(date[date.Length - 1].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)];

            if (timex.Month != null)
            {
                var month = TimexConstantsEnglish.Months[timex.Month.Value - 1];
                if (timex.Year != null)
                {
                    return $"{date}{abbreviation} {month} {timex.Year}".Trim();
                }

                return $"{date}{abbreviation} {month}";
            }

            return $"{date}{abbreviation}";
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
            string result = string.Empty;
            if (timex.Years != null)
            {
                result += ConvertDurationPropertyToString(timex.Years.Value, Constants.YearUnit, includeSingleCount);
            }

            if (timex.Months != null)
            {
                result += ConvertDurationPropertyToString(timex.Months.Value, Constants.MonthUnit, includeSingleCount);
            }

            if (timex.Weeks != null)
            {
                result += ConvertDurationPropertyToString(timex.Weeks.Value, Constants.WeekUnit, includeSingleCount);
            }

            if (timex.Days != null)
            {
                result += ConvertDurationPropertyToString(timex.Days.Value, Constants.DayUnit, includeSingleCount);
            }

            if (timex.Hours != null)
            {
                result += ConvertDurationPropertyToString(timex.Hours.Value, Constants.HourUnit, includeSingleCount);
            }

            if (timex.Minutes != null)
            {
                result += ConvertDurationPropertyToString(timex.Minutes.Value, Constants.MinuteUnit, includeSingleCount);
            }

            if (timex.Seconds != null)
            {
                result += ConvertDurationPropertyToString(timex.Seconds.Value, Constants.SecondUnit, includeSingleCount);
            }

            return result;
        }

        private static string ConvertDuration(TimexProperty timex)
        {
            return ConvertTimexDurationToString(timex, true);
        }

        private static string ConvertDateRange(TimexProperty timex)
        {
            var season = (timex.Season != null) ? TimexConstantsEnglish.Seasons[timex.Season] : string.Empty;

            var year = (timex.Year != null) ? timex.Year.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;

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

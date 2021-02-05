// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    internal static class TimexRelativeConvertEnglish
    {
        public static string ConvertTimexToStringRelative(TimexProperty timex, DateObject date)
        {
            var types = timex.Types.Count != 0 ? timex.Types : TimexInference.Infer(timex);

            if (types.Contains(Constants.TimexTypes.DateTimeRange))
            {
                return ConvertDateTimeRange(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.DateRange))
            {
                return ConvertDateRange(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.DateTime))
            {
                return ConvertDateTime(timex, date);
            }

            if (types.Contains(Constants.TimexTypes.Date))
            {
                return ConvertDate(timex, date);
            }

            return TimexConvert.ConvertTimexToString(timex);
        }

        private static string GetDateDay(DayOfWeek day)
        {
            var index = ((int)day == 0) ? 6 : (int)day - 1;
            return TimexConstantsEnglish.Days[index];
        }

        private static string ConvertDate(TimexProperty timex, DateObject date)
        {
            if (timex.Year != null && timex.Month != null && timex.DayOfMonth != null)
            {
                var timexDate = new DateObject(timex.Year.Value, timex.Month.Value, timex.DayOfMonth.Value);

                if (TimexDateHelpers.DatePartEquals(timexDate, date))
                {
                    return TimexConstantsEnglish.Today;
                }

                var tomorrow = TimexDateHelpers.Tomorrow(date);
                if (TimexDateHelpers.DatePartEquals(timexDate, tomorrow))
                {
                    return TimexConstantsEnglish.Tomorrow;
                }

                var yesterday = TimexDateHelpers.Yesterday(date);
                if (TimexDateHelpers.DatePartEquals(timexDate, yesterday))
                {
                    return TimexConstantsEnglish.Yesterday;
                }

                if (TimexDateHelpers.IsThisWeek(timexDate, date))
                {
                    return $"{TimexConstantsEnglish.This} {GetDateDay(timexDate.DayOfWeek)}";
                }

                if (TimexDateHelpers.IsNextWeek(timexDate, date))
                {
                    return $"{TimexConstantsEnglish.Next} {GetDateDay(timexDate.DayOfWeek)}";
                }

                if (TimexDateHelpers.IsLastWeek(timexDate, date))
                {
                    return $"{TimexConstantsEnglish.Last} {GetDateDay(timexDate.DayOfWeek)}";
                }
            }

            return TimexConvertEnglish.ConvertDate(timex);
        }

        private static string ConvertDateTime(TimexProperty timex, DateObject date)
        {
            return $"{ConvertDate(timex, date)} {TimexConvertEnglish.ConvertTime(timex)}";
        }

        private static string ConvertDateRange(TimexProperty timex, DateObject date)
        {
            if (timex.Year != null)
            {
                var year = date.Year;
                if (timex.Year == year)
                {
                    if (timex.WeekOfYear != null)
                    {
                        var thisWeek = TimexDateHelpers.WeekOfYear(date);
                        if (thisWeek == timex.WeekOfYear)
                        {
                            return timex.Weekend != null ? $"{TimexConstantsEnglish.This} {TimexConstantsEnglish.Weekend}" : $"{TimexConstantsEnglish.This} {Constants.WeekUnit}";
                        }

                        if (thisWeek == timex.WeekOfYear + 1)
                        {
                            return timex.Weekend != null ? $"{TimexConstantsEnglish.Last} {TimexConstantsEnglish.Weekend}" : $"{TimexConstantsEnglish.Last} {Constants.WeekUnit}";
                        }

                        if (thisWeek == timex.WeekOfYear - 1)
                        {
                            return timex.Weekend != null ? $"{TimexConstantsEnglish.Next} {TimexConstantsEnglish.Weekend}" : $"{TimexConstantsEnglish.Next} {Constants.WeekUnit}";
                        }
                    }

                    if (timex.Month != null)
                    {
                        if (timex.Month == date.Month)
                        {
                            return $"{TimexConstantsEnglish.This} {Constants.MonthUnit}";
                        }

                        if (timex.Month == date.Month + 1)
                        {
                            return $"{TimexConstantsEnglish.Next} {Constants.MonthUnit}";
                        }

                        if (timex.Month == date.Month - 1)
                        {
                            return $"{TimexConstantsEnglish.Last} {Constants.MonthUnit}";
                        }
                    }

                    return (timex.Season != null) ? $"{TimexConstantsEnglish.This} {TimexConstantsEnglish.Seasons[timex.Season]}" : $"{TimexConstantsEnglish.This} {Constants.YearUnit}";
                }

                if (timex.Year == year + 1)
                {
                    return (timex.Season != null) ? $"{TimexConstantsEnglish.Next} {TimexConstantsEnglish.Seasons[timex.Season]}" : $"{TimexConstantsEnglish.Next} {Constants.YearUnit}";
                }

                if (timex.Year == year - 1)
                {
                    return (timex.Season != null) ? $"{TimexConstantsEnglish.Last} {TimexConstantsEnglish.Seasons[timex.Season]}" : $"{TimexConstantsEnglish.Last} {Constants.YearUnit}";
                }
            }

            return string.Empty;
        }

        private static string ConvertDateTimeRange(TimexProperty timex, DateObject date)
        {
            if (timex.Year != null && timex.Month != null && timex.DayOfMonth != null)
            {
                var timexDate = new DateObject(timex.Year.Value, timex.Month.Value, timex.DayOfMonth.Value);

                if (timex.PartOfDay != null)
                {
                    if (TimexDateHelpers.DatePartEquals(timexDate, date))
                    {
                        if (timex.PartOfDay == Constants.TimexNight)
                        {
                            return TimexConstantsEnglish.Tonight;
                        }
                        else
                        {
                            return $"{TimexConstantsEnglish.This} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                        }
                    }

                    var tomorrow = TimexDateHelpers.Tomorrow(date);
                    if (TimexDateHelpers.DatePartEquals(timexDate, tomorrow))
                    {
                        return $"{TimexConstantsEnglish.Tomorrow} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    var yesterday = TimexDateHelpers.Yesterday(date);
                    if (TimexDateHelpers.DatePartEquals(timexDate, yesterday))
                    {
                        return $"{TimexConstantsEnglish.Yesterday} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    if (TimexDateHelpers.IsNextWeek(timexDate, date))
                    {
                        return $"{TimexConstantsEnglish.Next} {GetDateDay(timexDate.DayOfWeek)} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    if (TimexDateHelpers.IsLastWeek(timexDate, date))
                    {
                        return $"{TimexConstantsEnglish.Last} {GetDateDay(timexDate.DayOfWeek)} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }
                }
            }

            return string.Empty;
        }
    }
}

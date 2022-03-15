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
            if (timex.IsDateTimeRange)
            {
                return ConvertDateTimeRange(timex, date);
            }

            if (timex.IsDateRange)
            {
                return ConvertDateRange(timex, date);
            }

            if (timex.IsDateTime)
            {
                return ConvertDateTime(timex, date);
            }

            if (timex.IsDate)
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
            if (timex.IsDefinite)
            {
                var timexDate = timex.DateFromTimex();

                if (timexDate.IsSameDate(date))
                {
                    return TimexConstantsEnglish.Today;
                }

                if (timexDate.IsTomorrow(date))
                {
                    return TimexConstantsEnglish.Tomorrow;
                }

                if (timexDate.IsYesterday(date))
                {
                    return TimexConstantsEnglish.Yesterday;
                }

                if (timexDate.IsThisWeek(date))
                {
                    return $"{TimexConstantsEnglish.This} {GetDateDay(timexDate.DayOfWeek)}";
                }

                if (timexDate.IsNextWeek(date))
                {
                    return $"{TimexConstantsEnglish.Next} {GetDateDay(timexDate.DayOfWeek)}";
                }

                if (timexDate.IsLastWeek(date))
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
                var timexDate = timex.DateFromTimex();
                if (timexDate.IsSameYear(date))
                {
                    if (timex.WeekOfYear != null)
                    {
                        if (timexDate.IsThisWeek(date))
                        {
                            return timex.Weekend != null ? $"{TimexConstantsEnglish.This} {TimexConstantsEnglish.Weekend}" : $"{TimexConstantsEnglish.This} {Constants.WeekUnit}";
                        }

                        if (timexDate.IsNextWeek(date))
                        {
                            return timex.Weekend != null ? $"{TimexConstantsEnglish.Next} {TimexConstantsEnglish.Weekend}" : $"{TimexConstantsEnglish.Next} {Constants.WeekUnit}";
                        }

                        if (timexDate.IsLastWeek(date))
                        {
                            return timex.Weekend != null ? $"{TimexConstantsEnglish.Last} {TimexConstantsEnglish.Weekend}" : $"{TimexConstantsEnglish.Last} {Constants.WeekUnit}";
                        }
                    }

                    if (timex.Month != null)
                    {
                        if (timexDate.IsSameMonth(date))
                        {
                            return $"{TimexConstantsEnglish.This} {Constants.MonthUnit}";
                        }

                        if (timexDate.IsNextMonth(date))
                        {
                            return $"{TimexConstantsEnglish.Next} {Constants.MonthUnit}";
                        }

                        if (timexDate.IsLastMonth(date))
                        {
                            return $"{TimexConstantsEnglish.Last} {Constants.MonthUnit}";
                        }
                    }

                    return timex.IsSeason ? $"{TimexConstantsEnglish.This} {TimexConstantsEnglish.Seasons[timex.Season]}" : $"{TimexConstantsEnglish.This} {Constants.YearUnit}";
                }

                if (timexDate.IsNextYear(date))
                {
                    return timex.IsSeason ? $"{TimexConstantsEnglish.Next} {TimexConstantsEnglish.Seasons[timex.Season]}" : $"{TimexConstantsEnglish.Next} {Constants.YearUnit}";
                }

                if (timexDate.IsLastYear(date))
                {
                    return timex.IsSeason ? $"{TimexConstantsEnglish.Last} {TimexConstantsEnglish.Seasons[timex.Season]}" : $"{TimexConstantsEnglish.Last} {Constants.YearUnit}";
                }
            }

            return timex.ToString();
        }

        private static string ConvertDateTimeRange(TimexProperty timex, DateObject date)
        {
            if (timex.IsDefinite)
            {
                var timexDate = timex.DateFromTimex();

                if (timex.IsPartOfDay)
                {
                    if (timexDate.IsSameDate(date))
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

                    if (timexDate.IsTomorrow(date))
                    {
                        return $"{TimexConstantsEnglish.Tomorrow} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    if (timexDate.IsYesterday(date))
                    {
                        return $"{TimexConstantsEnglish.Yesterday} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    if (timexDate.IsNextWeek(date))
                    {
                        return $"{TimexConstantsEnglish.Next} {GetDateDay(timexDate.DayOfWeek)} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    if (timexDate.IsLastWeek(date))
                    {
                        return $"{TimexConstantsEnglish.Last} {GetDateDay(timexDate.DayOfWeek)} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }
                }
            }

            return string.Empty;
        }
    }
}

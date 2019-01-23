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
                    return "today";
                }

                var tomorrow = TimexDateHelpers.Tomorrow(date);
                if (TimexDateHelpers.DatePartEquals(timexDate, tomorrow))
                {
                    return "tomorrow";
                }

                var yesterday = TimexDateHelpers.Yesterday(date);
                if (TimexDateHelpers.DatePartEquals(timexDate, yesterday))
                {
                    return "yesterday";
                }

                if (TimexDateHelpers.IsThisWeek(timexDate, date))
                {
                    return $"this {GetDateDay(timexDate.DayOfWeek)}";
                }

                if (TimexDateHelpers.IsNextWeek(timexDate, date))
                {
                    return $"next {GetDateDay(timexDate.DayOfWeek)}";
                }

                if (TimexDateHelpers.IsLastWeek(timexDate, date))
                {
                    return $"last {GetDateDay(timexDate.DayOfWeek)}";
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
                            return timex.Weekend != null ? "this weekend" : "this week";
                        }

                        if (thisWeek == timex.WeekOfYear + 1)
                        {
                            return timex.Weekend != null ? "last weekend" : "last week";
                        }

                        if (thisWeek == timex.WeekOfYear - 1)
                        {
                            return timex.Weekend != null ? "next weekend" : "next week";
                        }
                    }

                    if (timex.Month != null)
                    {
                        if (timex.Month == date.Month)
                        {
                            return "this month";
                        }

                        if (timex.Month == date.Month + 1)
                        {
                            return "next month";
                        }

                        if (timex.Month == date.Month - 1)
                        {
                            return "last month";
                        }
                    }

                    return (timex.Season != null) ? $"this {TimexConstantsEnglish.Seasons[timex.Season]}" : "this year";
                }

                if (timex.Year == year + 1)
                {
                    return (timex.Season != null) ? $"next {TimexConstantsEnglish.Seasons[timex.Season]}" : "next year";
                }

                if (timex.Year == year - 1)
                {
                    return (timex.Season != null) ? $"last {TimexConstantsEnglish.Seasons[timex.Season]}" : "last year";
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
                        if (timex.PartOfDay == "NI")
                        {
                            return "tonight";
                        }
                        else
                        {
                            return $"this {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                        }
                    }

                    var tomorrow = TimexDateHelpers.Tomorrow(date);
                    if (TimexDateHelpers.DatePartEquals(timexDate, tomorrow))
                    {
                        return $"tomorrow {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    var yesterday = TimexDateHelpers.Yesterday(date);
                    if (TimexDateHelpers.DatePartEquals(timexDate, yesterday))
                    {
                        return $"yesterday {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    if (TimexDateHelpers.IsNextWeek(timexDate, date))
                    {
                        return $"next {GetDateDay(timexDate.DayOfWeek)} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }

                    if (TimexDateHelpers.IsLastWeek(timexDate, date))
                    {
                        return $"last {GetDateDay(timexDate.DayOfWeek)} {TimexConstantsEnglish.DayParts[timex.PartOfDay]}";
                    }
                }
            }

            return string.Empty;
        }
    }
}

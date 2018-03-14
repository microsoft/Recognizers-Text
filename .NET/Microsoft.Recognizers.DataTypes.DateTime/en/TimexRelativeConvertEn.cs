// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Recognizers.DataTypes.DateTime
{
    internal static class TimexRelativeConvertEn
    {
        public static string ConvertTimexToStringRelative(Timex timex, System.DateTime date)
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
            return TimexConstantsEn.Days[index];
        }

        private static string ConvertDate(Timex timex, System.DateTime date)
        {
            if (timex.Year != null && timex.Month != null && timex.DayOfMonth != null)
            {
                var timexDate = new System.DateTime(timex.Year.Value, timex.Month.Value, timex.DayOfMonth.Value);

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
            return TimexConvertEn.ConvertDate(timex);
        }

        private static string ConvertDateTime(Timex timex, System.DateTime date)
        {
            return $"{ConvertDate(timex, date)} {TimexConvertEn.ConvertTime(timex)}";
        }

        private static string ConvertDateRange(Timex timex, System.DateTime date)
        {
            if (timex.Year != null)
            {
                var year = date.Year;
                if (timex.Year == year) {
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
                        if (thisWeek == timex.WeekOfYear - 1) {
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
                    return (timex.Season != null) ? $"this {TimexConstantsEn.Seasons[timex.Season]}" : "this year";
                }
                if (timex.Year == year + 1)
                {
                    return (timex.Season != null) ? $"next {TimexConstantsEn.Seasons[timex.Season]}" : "next year";
                }
                if (timex.Year == year - 1)
                {
                    return (timex.Season != null) ? $"last {TimexConstantsEn.Seasons[timex.Season]}" : "last year";
                }
            }
            return string.Empty;
        }

        private static string ConvertDateTimeRange(Timex timex, System.DateTime date)
        {
            if (timex.Year != null && timex.Month != null && timex.DayOfMonth != null)
            {
                var timexDate = new System.DateTime(timex.Year.Value, timex.Month.Value, timex.DayOfMonth.Value);

                if (timex.PartOfDay != null)
                {
                    if (TimexDateHelpers.DatePartEquals(timexDate, date))
                    {
                        if (timex.PartOfDay == "NI")
                        {
                            return "tonight";
                        }
                        else {
                            return $"this {TimexConstantsEn.DayParts[timex.PartOfDay]}";
                        }
                    }
                    var tomorrow = TimexDateHelpers.Tomorrow(date);
                    if (TimexDateHelpers.DatePartEquals(timexDate, tomorrow))
                    {
                        return $"tomorrow {TimexConstantsEn.DayParts[timex.PartOfDay]}";
                    }
                    var yesterday = TimexDateHelpers.Yesterday(date);
                    if (TimexDateHelpers.DatePartEquals(timexDate, yesterday))
                    {
                        return $"yesterday {TimexConstantsEn.DayParts[timex.PartOfDay]}";
                    }

                    if (TimexDateHelpers.IsNextWeek(timexDate, date))
                    {
                        return $"next {GetDateDay(timexDate.DayOfWeek)} {TimexConstantsEn.DayParts[timex.PartOfDay]}";
                    }

                    if (TimexDateHelpers.IsLastWeek(timexDate, date))
                    {
                        return $"last {GetDateDay(timexDate.DayOfWeek)} {TimexConstantsEn.DayParts[timex.PartOfDay]}";
                    }
                }
            }
            return string.Empty;
        }
    }
}

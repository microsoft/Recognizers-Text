// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexDateHelpers
    {
        public static DateObject Tomorrow(DateObject date)
        {
            return date.AddDays(1);
        }

        public static DateObject Yesterday(DateObject date)
        {
            return date.AddDays(-1);
        }

        public static bool DatePartEquals(DateObject dateX, DateObject dateY)
        {
            return (dateX.Year == dateY.Year) && (dateX.Month == dateY.Month) && (dateX.Day == dateY.Day);
        }

        public static bool IsDateInWeek(DateObject date, DateObject startOfWeek)
        {
            var d = startOfWeek;
            for (int i = 0; i < 7; i++)
            {
                if (DatePartEquals(date, d))
                {
                    return true;
                }

                d = d.AddDays(1);
            }

            return false;
        }

        public static bool IsThisWeek(DateObject date, DateObject referenceDate)
        {
            // Note ISO 8601 week starts on a Monday
            var startOfWeek = referenceDate;
            while (startOfWeek.DayOfWeek > DayOfWeek.Monday)
            {
                startOfWeek = startOfWeek.AddDays(-1);
            }

            return IsDateInWeek(date, startOfWeek);
        }

        public static bool IsNextWeek(DateObject date, DateObject referenceDate)
        {
            var nextWeekDate = referenceDate.AddDays(7);
            return IsThisWeek(date, nextWeekDate);
        }

        public static bool IsLastWeek(DateObject date, DateObject referenceDate)
        {
            var nextWeekDate = referenceDate.AddDays(-7);
            return IsThisWeek(date, nextWeekDate);
        }

        public static int WeekOfYear(DateObject date)
        {
            var ds = new DateObject(date.Year, 1, 1);
            var de = new DateObject(date.Year, date.Month, date.Day);
            int weeks = 1;

            while (ds < de)
            {
                var dayOfWeek = ds.DayOfWeek;

                var isoDayOfWeek = (dayOfWeek == 0) ? 7 : (int)dayOfWeek;
                if (isoDayOfWeek == 7)
                {
                    weeks++;
                }

                ds = ds.AddDays(1);
            }

            return weeks;
        }

        public static string FixedFormatNumber(int? n, int size)
        {
            return n.Value.ToString().PadLeft(size, '0');
        }

        public static DateObject DateOfLastDay(DayOfWeek day, DateObject referenceDate)
        {
            var result = referenceDate;
            result = result.AddDays(-1);

            while (result.DayOfWeek != day)
            {
                result = result.AddDays(-1);
            }

            return result;
        }

        public static DateObject DateOfNextDay(DayOfWeek day, DateObject referenceDate)
        {
            var result = referenceDate;
            result = result.AddDays(1);

            while (result.DayOfWeek != day)
            {
                result = result.AddDays(1);
            }

            return result;
        }

        public static List<DateObject> DatesMatchingDay(DayOfWeek day, DateObject start, DateObject end)
        {
            var result = new List<DateObject>();
            var d = start;

            while (!DatePartEquals(d, end))
            {
                if (d.DayOfWeek == day)
                {
                    result.Add(d);
                }

                d = d.AddDays(1);
            }

            return result;
        }
    }
}

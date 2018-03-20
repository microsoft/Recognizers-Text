// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexDateHelpers
    {
        public static System.DateTime Tomorrow(System.DateTime date)
        {
            return date.AddDays(1);
        }

        public static System.DateTime Yesterday(System.DateTime date)
        {
            return date.AddDays(-1);
        }

        public static bool DatePartEquals(System.DateTime dateX, System.DateTime dateY)
        {
            return (dateX.Year == dateY.Year) && (dateX.Month == dateY.Month) && (dateX.Day == dateY.Day);
        }

        public static bool IsDateInWeek(System.DateTime date, System.DateTime startOfWeek)
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

        public static bool IsThisWeek(System.DateTime date, System.DateTime referenceDate)
        {
            // Note ISO 8601 week starts on a Monday
            var startOfWeek = referenceDate;
            while (startOfWeek.DayOfWeek > DayOfWeek.Monday)
            {
                startOfWeek = startOfWeek.AddDays(-1);
            }

            return IsDateInWeek(date, startOfWeek);
        }

        public static bool IsNextWeek(System.DateTime date, System.DateTime referenceDate)
        {
            var nextWeekDate = referenceDate.AddDays(7);
            return IsThisWeek(date, nextWeekDate);
        }

        public static bool IsLastWeek(System.DateTime date, System.DateTime referenceDate)
        {
            var nextWeekDate = referenceDate.AddDays(-7);
            return IsThisWeek(date, nextWeekDate);
        }

        public static int WeekOfYear(System.DateTime date)
        {
            var ds = new System.DateTime(date.Year, 1, 1);
            var de = new System.DateTime(date.Year, date.Month, date.Day);
            int weeks = 1;

            while (ds < de)
            {
                var csDayOfWeek = ds.DayOfWeek;

                var isoDayOfWeek = (csDayOfWeek == 0) ? 7 : (int)csDayOfWeek;
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

        public static System.DateTime DateOfLastDay(DayOfWeek day, System.DateTime referenceDate)
        {
            var result = referenceDate;
            result = result.AddDays(-1);

            while (result.DayOfWeek != day)
            {
                result = result.AddDays(-1);
            }

            return result;
        }

        public static System.DateTime DateOfNextDay(DayOfWeek day, System.DateTime referenceDate)
        {
            var result = referenceDate;
            result = result.AddDays(1);

            while (result.DayOfWeek != day)
            {
                result = result.AddDays(1);
            }

            return result;
        }

        public static List<System.DateTime> DatesMatchingDay(DayOfWeek day, System.DateTime start, System.DateTime end)
        {
            var result = new List<System.DateTime>();
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

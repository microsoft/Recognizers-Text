// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexDateHelpers
    {
        public static DateObject Tomorrow(this DateObject date)
        {
            return date.AddDays(1);
        }

        public static DateObject Yesterday(this DateObject date)
        {
            return date.TryAddDays(-1, out var ret) ? ret : date;
        }

        /// <summary>
        /// Compares the year, month and day of two date objects.
        /// </summary>
        /// <param name="dateX">The first date to compare.</param>
        /// <param name="dateY">The second date to compare.</param>
        /// <returns>Whether the date parts of the two date objects match.</returns>
        public static bool IsSameDate(this DateObject dateX, DateObject dateY)
        {
            return dateX.IsSameYear(dateY) && dateX.DayOfYear == dateY.DayOfYear;
        }

        /// <summary>
        /// Check if this is the day after <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>True if <paramref name="date" /> is the day after <paramref name="referenceDate"/>.</returns>
        public static bool IsTomorrow(this DateObject date, DateObject referenceDate)
        {
            return referenceDate.Tomorrow().IsSameDate(date);
        }

        /// <summary>
        /// Check if this is the day before <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>True if <paramref name="date" /> is the day before <paramref name="referenceDate"/>.</returns>
        public static bool IsYesterday(this DateObject date, DateObject referenceDate)
        {
            return referenceDate.Yesterday().IsSameDate(date);
        }

        public static bool IsSameYear(this DateObject dateX, DateObject dateY)
        {
            return dateX.Year == dateY.Year;
        }

        /// <summary>
        /// Check if this is the year after <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>True if <paramref name="date" /> is the year after <paramref name="referenceDate"/>.</returns>
        public static bool IsNextYear(this DateObject date, DateObject referenceDate)
        {
            return date.Year == referenceDate.Year + 1;
        }

        /// <summary>
        /// Check if this is the year before <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>True if <paramref name="date" /> is the year before <paramref name="referenceDate"/>.</returns>
        public static bool IsLastYear(this DateObject date, DateObject referenceDate)
        {
            return date.Year == referenceDate.Year - 1;
        }

        public static bool IsSameMonth(this DateObject dateX, DateObject dateY)
        {
            return dateX.IsSameYear(dateY) && dateX.Month == dateY.Month;
        }

        /// <summary>
        /// Check if this is the month after <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>True if <paramref name="date" /> is the month after <paramref name="referenceDate"/>.</returns>
        public static bool IsNextMonth(this DateObject date, DateObject referenceDate)
        {
            return referenceDate.AddMonths(1).IsSameMonth(date);
        }

        /// <summary>
        /// Check if this is the month before <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>True if <paramref name="date" /> is the month before <paramref name="referenceDate"/>.</returns>
        public static bool IsLastMonth(this DateObject date, DateObject referenceDate)
        {
            return referenceDate.TryAddMonths(-1, out referenceDate) && referenceDate.IsSameMonth(date);
        }

        public static bool IsThisWeek(this DateObject date, DateObject referenceDate)
        {
            // Note ISO 8601 week starts on a Monday
            if (!date.TryGetStartOfWeek(out var thisMonday) ||
                !referenceDate.TryGetStartOfWeek(out var targetMonday))
            {
                return false;
            }

            return thisMonday.IsSameDate(targetMonday);
        }

        /// <summary>
        /// Check if this is the week after <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>True if <paramref name="date" /> is the week after <paramref name="referenceDate"/>.</returns>
        public static bool IsNextWeek(this DateObject date, DateObject referenceDate)
        {
            return date.IsThisWeek(referenceDate.AddDays(7));
        }

        /// <summary>
        /// Check if the <paramref name="date"/> is the week before <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <returns>True if <paramref name="date" /> is the week before <paramref name="referenceDate"/>.</returns>
        public static bool IsLastWeek(this DateObject date, DateObject referenceDate)
        {
            return referenceDate.TryAddDays(-7, out referenceDate) && date.IsThisWeek(referenceDate);
        }

        public static int WeekOfYear(this DateObject date)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;

            // Workaround to get ISO 8601 week number.
            // (A better solution would be to use ISOWeek.GetWeekOfYear but it seems currently unsupported)
            DayOfWeek day = culture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            int weeks = culture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return weeks;
        }

        public static string FixedFormatNumber(int? n, int size)
        {
            return n.Value.ToString(CultureInfo.InvariantCulture).PadLeft(size, '0');
        }

        public static bool TryGetStartOfWeek(this DateObject date, out DateObject ret, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {
            return date.TryAddDays(firstDayOfWeek.Normalize() - date.DayOfWeek.Normalize(), out ret);
        }

        /// <summary>
        /// Get the date of <paramref name="day"/> before the <paramref name="referenceDate"/>.
        /// For example, if the <paramref name="referenceDate"/> is a Thursday and <paramref name="day"/> is Wednesday, this would return the day before <paramref name="referenceDate"/>.
        /// If the <paramref name="referenceDate"/> is a Wednesday and <paramref name="day"/> is Thursday, this would return the thursday of the week before <paramref name="referenceDate"/>.
        /// </summary>
        /// <param name="referenceDate">The reference date to calculate the day of the week from.</param>
        /// <param name="day">The day of the week.</param>
        /// <returns>The date corresponding to the day of the week before the reference date.</returns>
        public static DateObject DateOfLastDay(this DateObject referenceDate, DayOfWeek day)
        {
            var days = day.Normalize() - referenceDate.DayOfWeek.Normalize();
            if (referenceDate.TryAddDays(days >= 0 ? days - 7 : days, out var ret))
            {
                return ret;
            }

            return referenceDate;
        }

        public static DateObject DateOfNextDay(this DateObject referenceDate, DayOfWeek day)
        {
            var days = day.Normalize() - referenceDate.DayOfWeek.Normalize();
            if (referenceDate.TryAddDays(days <= 0 ? 7 - Math.Abs(days) : days, out var ret))
            {
                return ret;
            }

            return referenceDate;
        }

        public static List<DateObject> DatesMatchingDay(DayOfWeek day, DateObject start, DateObject end)
        {
            var result = new List<DateObject>();
            var d = start;

            while (!IsSameDate(d, end))
            {
                if (d.DayOfWeek == day)
                {
                    result.Add(d);
                }

                d = d.AddDays(1);
            }

            return result;
        }

        private static bool TryAddMonths(this DateObject date, int shift, out DateObject value)
        {
            value = date;
            try
            {
                value = date.AddMonths(shift);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static bool TryAddDays(this DateObject date, int shift, out DateObject value)
        {
            var ret = shift > 0 || date.Year > 1 || date.DayOfYear + shift > 0;
            value = date;
            if (ret)
            {
                value = date.AddDays(shift);
            }

            return ret;
        }

        private static int Normalize(this DayOfWeek day)
        {
            var dayNumber = (int)day;
            return dayNumber < (int)DayOfWeek.Monday ? 7 - dayNumber : dayNumber;
        }
    }
}

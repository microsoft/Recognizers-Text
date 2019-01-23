// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexCreator
    {
        // The following constants are consistent with the Recognizer results
        public static readonly string Monday = "XXXX-WXX-1";
        public static readonly string Tuesday = "XXXX-WXX-2";
        public static readonly string Wednesday = "XXXX-WXX-3";
        public static readonly string Thursday = "XXXX-WXX-4";
        public static readonly string Friday = "XXXX-WXX-5";
        public static readonly string Saturday = "XXXX-WXX-6";
        public static readonly string Sunday = "XXXX-WXX-7";
        public static readonly string Morning = "(T08,T12,PT4H)";
        public static readonly string Afternoon = "(T12,T16,PT4H)";
        public static readonly string Evening = "(T16,T20,PT4H)";
        public static readonly string Daytime = "(T08,T18,PT10H)";
        public static readonly string Night = "(T20,T24,PT10H)";

        public static string Today(DateObject date = default(DateObject))
        {
            return TimexProperty.FromDate(date == default(DateObject) ? DateObject.Now : date).TimexValue;
        }

        public static string Tomorrow(DateObject date = default(DateObject))
        {
            var d = (date == default(DateObject)) ? DateObject.Now : date;
            d = d.AddDays(1);
            return TimexProperty.FromDate(d).TimexValue;
        }

        public static string Yesterday(DateObject date = default(DateObject))
        {
            var d = (date == default(DateObject)) ? DateObject.Now : date;
            d = d.AddDays(-1);
            return TimexProperty.FromDate(d).TimexValue;
        }

        public static string WeekFromToday(DateObject date = default(DateObject))
        {
            var d = (date == default(DateObject)) ? DateObject.Now : date;
            var t = TimexProperty.FromDate(d);
            t.Days = 7;
            return t.TimexValue;
        }

        public static string WeekBackFromToday(DateObject date = default(DateObject))
        {
            var d = (date == default(DateObject)) ? DateObject.Now : date;
            d = d.AddDays(-7);
            var t = TimexProperty.FromDate(d);
            t.Days = 7;
            return t.TimexValue;
        }

        public static string ThisWeek(DateObject date = default(DateObject))
        {
            var d = (date == default(DateObject)) ? DateObject.Now : date;
            d = d.AddDays(-7);
            var start = TimexDateHelpers.DateOfNextDay(DayOfWeek.Monday, d);
            var t = TimexProperty.FromDate(start);
            t.Days = 7;
            return t.TimexValue;
        }

        public static string NextWeek(DateObject date = default(DateObject))
        {
            var d = (date == default(DateObject)) ? DateObject.Now : date;
            var start = TimexDateHelpers.DateOfNextDay(DayOfWeek.Monday, d);
            var t = TimexProperty.FromDate(start);
            t.Days = 7;
            return t.TimexValue;
        }

        public static string LastWeek(DateObject date = default(DateObject))
        {
            var d = (date == default(DateObject)) ? DateObject.Now : date;
            var start = TimexDateHelpers.DateOfLastDay(DayOfWeek.Monday, d);
            start = start.AddDays(-7);
            var t = TimexProperty.FromDate(start);
            t.Days = 7;
            return t.TimexValue;
        }

        public static string NextWeeksFromToday(int n, DateObject date = default(DateObject))
        {
            var d = (date == default(DateObject)) ? DateObject.Now : date;
            var t = TimexProperty.FromDate(d);
            t.Days = n * 7;
            return t.TimexValue;
        }
    }
}

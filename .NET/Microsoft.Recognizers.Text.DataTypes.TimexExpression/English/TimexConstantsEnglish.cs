// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    internal static class TimexConstantsEnglish
    {
        public static readonly string[] Days =
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday",
        };

        public static readonly string[] Months =
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December",
        };

        public static readonly string[] DateAbbreviation =
        {
            "th",
            "st",
            "nd",
            "rd",
            "th",
            "th",
            "th",
            "th",
            "th",
            "th",
        };

        public static readonly string[] Hours =
        {
            "midnight", "1AM", "2AM", "3AM", "4AM", "5AM", "6AM", "7AM", "8AM", "9AM", "10AM", "11AM",
            "midday", "1PM", "2PM", "3PM", "4PM", "5PM", "6PM", "7PM", "8PM", "9PM", "10PM", "11PM",
        };

        public static readonly IDictionary<string, string> Seasons = new Dictionary<string, string>
        {
            { "SP", "spring" },
            { "SU", "summer" },
            { "FA", "fall" },
            { "WI", "winter" },
        };

        public static readonly string[] Weeks =
        {
            "first",
            "second",
            "third",
            "forth",
        };

        public static readonly IDictionary<string, string> DayParts = new Dictionary<string, string>
        {
            { "DT", "daytime" },
            { "NI", "night" },
            { "MO", "morning" },
            { "AF", "afternoon" },
            { "EV", "evening" },
        };
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class Constants
    {
        // Timex
        public const string TimexYear = "Y";
        public const string TimexMonth = "M";
        public const string TimexMonthFull = "MON";
        public const string TimexWeek = "W";
        public const string TimexDay = "D";
        public const string TimexBusinessDay = "BD";
        public const string TimexWeekend = "WE";
        public const string TimexHour = "H";
        public const string TimexMinute = "M";
        public const string TimexSecond = "S";
        public const string TimexNight = "NI";
        public const char TimexFuzzy = 'X';
        public const string TimexFuzzyYear = "XXXX";
        public const string TimexFuzzyMonth = "XX";
        public const string TimexFuzzyWeek = "WXX";
        public const string TimexFuzzyDay = "XX";
        public const string DateTimexConnector = "-";
        public const string TimeTimexConnector = ":";
        public const string GeneralPeriodPrefix = "P";
        public const string TimeTimexPrefix = "T";

        public const string YearUnit = "year";
        public const string MonthUnit = "month";
        public const string WeekUnit = "week";
        public const string DayUnit = "day";
        public const string HourUnit = "hour";
        public const string MinuteUnit = "minute";
        public const string SecondUnit = "second";
        public const string TimeDurationUnit = "s";

        public const string AM = "AM";
        public const string PM = "PM";

        public const int InvalidValue = -1;

        public static class TimexTypes
        {
            public static readonly string Present = "present";
            public static readonly string Definite = "definite";
            public static readonly string Date = "date";
            public static readonly string DateTime = "datetime";
            public static readonly string DateRange = "daterange";
            public static readonly string Duration = "duration";
            public static readonly string Time = "time";
            public static readonly string TimeRange = "timerange";
            public static readonly string DateTimeRange = "datetimerange";
        }
    }
}

// ReSharper disable InconsistentNaming

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class Constants
    {
        public const string SYS_DATETIME_DATE = "date";
        public const string SYS_DATETIME_TIME = "time";
        public const string SYS_DATETIME_DATEPERIOD = "daterange";
        public const string SYS_DATETIME_DATETIME = "datetime";
        public const string SYS_DATETIME_TIMEPERIOD = "timerange";
        public const string SYS_DATETIME_DATETIMEPERIOD = "datetimerange";
        public const string SYS_DATETIME_DURATION = "duration";
        public const string SYS_DATETIME_SET = "set";
        public const string SYS_DATETIME_DATETIMEALT = "datetimealt";

        //Model Name
        public const string MODEL_DATETIME = "datetime";
        
        //Multiple Duration Types
        public const string MultipleDuration_DateTime = "multipleDurationDateTime";
        public const string MultipleDuration_Date = "multipleDurationDate";
        public const string MultipleDuration_Time = "multipleDurationTime";

        //Comment
        public const string Comment_AmPm = "ampm";
        public const string Comment_Early = "early";
        public const string Comment_Late = "late";
        public const string Comment_WeekOf = "WeekOf";
        public const string Comment_MonthOf = "MonthOf";


        //Invalid year
        public const int InvalidYear = int.MinValue;
    }
}
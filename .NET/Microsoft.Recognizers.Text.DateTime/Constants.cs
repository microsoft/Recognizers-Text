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

        // Model Name
        public const string MODEL_DATETIME = "datetime";

        // Multiple Duration Types
        public const string Multiple_Duration_Type = "multipleDurationType";
        public const string Multiple_Duration_DateTime = "multipleDurationDateTime";
        public const string Multiple_Duration_Date = "multipleDurationDate";
        public const string Multiple_Duration_Time = "multipleDurationTime";
        
        // DateTime Parse
        public const string Resolve = "resolve";
        public const string Resolve_To_Past = "resolveToPast";
        public const string Resolve_To_Future = "resolveToFuture";

        // In the ExtractResult data
        public const string Context = "context";
        public const string Context_Type_RELATIVE_PREFIX = "relativePrefix";
        public const string Context_Type_AM_PM = "AmPm";
        public const string SubType = "subType";

        // Comment - internal tag used during entity processing, never exposed to users. 
        // Tags are filtered out in BaseMergedParser DateTimeResolution()
        public const string Comment = "Comment";
        // AmPm time representation for time parser
        public const string Comment_AmPm = "ampm";
        // Prefix early/late for time parser
        public const string Comment_Early = "early";
        public const string Comment_Late = "late";
        // Parse week of date format
        public const string Comment_WeekOf = "WeekOf";
        public const string Comment_MonthOf = "MonthOf";

        // Mod Value
        public const string BEFORE_MOD = "before";
        public const string AFTER_MOD = "after";
        public const string SINCE_MOD = "since";

        public const string EARLY_MOD = "start";
        public const string MID_MOD = "mid";
        public const string LATE_MOD = "end";

        // Invalid year
        public const int InvalidYear = int.MinValue;
    }
}
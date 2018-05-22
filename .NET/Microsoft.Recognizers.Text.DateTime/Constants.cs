﻿// ReSharper disable InconsistentNaming

using Microsoft.Recognizers.Definitions;

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
        public const string SYS_DATETIME_TIMEZONE = "timezone";

        // Model Name
        public const string MODEL_DATETIME = "datetime";

        // Multiple Duration Types
        public const string MultipleDuration_Type = "multipleDurationType";
        public const string MultipleDuration_DateTime = "multipleDurationDateTime";
        public const string MultipleDuration_Date = "multipleDurationDate";
        public const string MultipleDuration_Time = "multipleDurationTime";
        
        // DateTime Parse
        public const string Resolve = "resolve";
        public const string ResolveToPast = "resolveToPast";
        public const string ResolveToFuture = "resolveToFuture";

        // In the ExtractResult data
        public const string Context = "context";
        public const string ContextType_RelativePrefix = "relativePrefix";
        public const string ContextType_RelativeSuffix = "relativeSuffix";
        public const string ContextType_AmPm = "AmPm";
        public const string SubType = "subType";

        // Comment - internal tag used during entity processing, never exposed to users. 
        // Tags are filtered out in BaseMergedDateTimeParser DateTimeResolution()
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
        // "before" -> To mean "preceding in time". I.e. Does not include the extracted datetime entity in the resolution's ending point. Equivalent to "<"
        public const string BEFORE_MOD = "before";

        // "after" -> To mean "following in time". I.e. Does not include the extracted datetime entity in the resolution's starting point. Equivalent to ">"
        public const string AFTER_MOD = "after";

        // "since" -> Same as "after", but including the extracted datetime entity. Equivalent to ">="
        public const string SINCE_MOD = "since";

        // "until" -> Same as "before", but including the extracted datetime entity. Equivalent to "<="
        public const string UNTIL_MOD = "until";

        public const string EARLY_MOD = "start";
        public const string MID_MOD = "mid";
        public const string LATE_MOD = "end";

        public const string MORE_THAN_MOD = "more";
        public const string LESS_THAN_MOD = "less";

        // Invalid year
        public const int InvalidYear = int.MinValue;

        public static readonly int MinYearNum = int.Parse(BaseDateTime.MinYearNum);
        public static readonly int MaxYearNum = int.Parse(BaseDateTime.MaxYearNum);

        public static readonly int MaxTwoDigitYearFutureNum = int.Parse(BaseDateTime.MaxTwoDigitYearFutureNum);
        public static readonly int MinTwoDigitYearPastNum = int.Parse(BaseDateTime.MinTwoDigitYearPastNum);

        // special value for timezone
        public const int InvalidOffsetValue = -10000;
        public const string UtcOffsetMinsKey = "UtcOffsetMins";
        public const string ResolveTimeZone = "resolveTimeZone";
        public const int PositiveSign = 1;
        public const int NegativeSign = -1;

        public const int TrimesterMonthCount = 3;
        public const int SemesterMonthCount = 6;
        public const int WeekDayCount = 7;

        // hours of one half day
        public const int HalfDayHourCount = 12;
        // hours of a half mid-day-duration
        public const int HalfMidDayDurationHourCount = 2;

        public const string DefaultLanguageFallback_MDY = "MDY";
        public const string DefaultLanguageFallback_DMY = "DMY";
    }
}
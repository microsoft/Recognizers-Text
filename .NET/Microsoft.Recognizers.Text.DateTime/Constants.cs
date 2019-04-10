using System.Diagnostics.CodeAnalysis;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.DateTime
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310: CSharp.Naming : Field names must not contain underscores.", Justification = "Constant names are written in upper case so they can be readily distinguished from camel case variable names.")]
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
        public const string MultipleDuration_Prefix = "multipleDuration";
        public const string MultipleDuration_Type = MultipleDuration_Prefix + "Type";
        public const string MultipleDuration_DateTime = MultipleDuration_Prefix + "DateTime";
        public const string MultipleDuration_Date = MultipleDuration_Prefix + "Date";
        public const string MultipleDuration_Time = MultipleDuration_Prefix + "Time";

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

        // MOD Value
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

        public const string REF_UNDEF_MOD = "ref_undef";

        public const string APPROX_MOD = "approx";

        // These are some particular values for timezone recognition
        public const int InvalidOffsetValue = -10000;
        public const string UtcOffsetMinsKey = "utcOffsetMins";
        public const string TimeZoneText = "timezoneText";
        public const string TimeZone = "timezone";
        public const string ResolveTimeZone = "resolveTimeZone";
        public const int PositiveSign = 1;
        public const int NegativeSign = -1;

        public const int TrimesterMonthCount = 3;
        public const int SemesterMonthCount = 6;
        public const int WeekDayCount = 7;
        public const int CenturyYearsCount = 100;
        public const int MaxWeekOfMonth = 5;
        public const int MaxMonth = 12;
        public const int MinMonth = 1;

        // hours of one half day
        public const int HalfDayHourCount = 12;

        // hours of a half mid-day-duration
        public const int HalfMidDayDurationHourCount = 2;

        // the length of four digits year, e.g., 2018
        public const int FourDigitsYearLength = 4;

        public const string DefaultLanguageFallback_MDY = "MDY";
        public const string DefaultLanguageFallback_DMY = "DMY";

        // Groups' names for named groups in regexes
        public const string NextGroupName = "next";
        public const string AmGroupName = "am";
        public const string PmGroupName = "pm";
        public const string ImplicitAmGroupName = "iam";
        public const string ImplicitPmGroupName = "ipm";
        public const string PrefixGroupName = "prefix";
        public const string SuffixGroupName = "suffix";
        public const string DescGroupName = "desc";
        public const string SecondGroupName = "sec";
        public const string MinuteGroupName = "min";
        public const string HourGroupName = "hour";
        public const string YearGroupName = "year";
        public const string TimeOfDayGroupName = "timeOfDay";
        public const string BusinessDayGroupName = "business";
        public const string LeftAmPmGroupName = "leftDesc";
        public const string RightAmPmGroupName = "rightDesc";

        public const string DECADE_UNIT = "10Y";
        public const string FORTNIGHT_UNIT = "2W";

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
        public const char TimexFuzzy = 'X';
        public const string TimexFuzzyYear = "XXXX";
        public const string TimexFuzzyMonth = "XX";
        public const string TimexFuzzyWeek = "WXX";
        public const string TimexFuzzyDay = "XX";
        public const string DateTimexConnector = "-";
        public const string TimeTimexConnector = ":";
        public const string GeneralPeriodPrefix = "P";
        public const string TimeTimexPrefix = "T";

        // Timex of TimeOfDay
        public const string EarlyMorning = "TDA";
        public const string Morning = "TMO";
        public const string Afternoon = "TAF";
        public const string Evening = "TEV";
        public const string Daytime = "TDT";
        public const string Night = "TNI";
        public const string BusinessHour = "TBH";

        // Invalid year
        public const int InvalidYear = int.MinValue;
        public const int InvalidMonth = int.MinValue;
        public const int InvalidDay = int.MinValue;
        public const int InvalidHour = int.MinValue;
        public const int InvalidMinute = int.MinValue;
        public const int InvalidSecond = int.MinValue;

        // Invalid year non-constant
        public static readonly int MinYearNum = int.Parse(BaseDateTime.MinYearNum);
        public static readonly int MaxYearNum = int.Parse(BaseDateTime.MaxYearNum);

        public static readonly int MaxTwoDigitYearFutureNum = int.Parse(BaseDateTime.MaxTwoDigitYearFutureNum);
        public static readonly int MinTwoDigitYearPastNum = int.Parse(BaseDateTime.MinTwoDigitYearPastNum);

        // Timex non-constant
        public static readonly string[] DatePeriodTimexSplitter = { ",", "(", ")" };
    }
}
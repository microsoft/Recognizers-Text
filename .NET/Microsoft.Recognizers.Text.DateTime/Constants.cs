// ReSharper disable InconsistentNaming

using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class Constants
    {
        public const string SYSDATETIMEDATE = "date";
        public const string SYSDATETIMETIME = "time";
        public const string SYSDATETIMEDATEPERIOD = "daterange";
        public const string SYSDATETIMEDATETIME = "datetime";
        public const string SYSDATETIMETIMEPERIOD = "timerange";
        public const string SYSDATETIMEDATETIMEPERIOD = "datetimerange";
        public const string SYSDATETIMEDURATION = "duration";
        public const string SYSDATETIMESET = "set";
        public const string SYSDATETIMEDATETIMEALT = "datetimealt";
        public const string SYSDATETIMETIMEZONE = "timezone";

        // Model Name
        public const string MODELDATETIME = "datetime";

        // Multiple Duration Types
        public const string MultipleDurationPrefix = "multipleDuration";
        public const string MultipleDurationType = MultipleDurationPrefix + "Type";
        public const string MultipleDurationDateTime = MultipleDurationPrefix + "DateTime";
        public const string MultipleDurationDate = MultipleDurationPrefix + "Date";
        public const string MultipleDurationTime = MultipleDurationPrefix + "Time";

        // DateTime Parse
        public const string Resolve = "resolve";
        public const string ResolveToPast = "resolveToPast";
        public const string ResolveToFuture = "resolveToFuture";

        // In the ExtractResult data
        public const string Context = "context";
        public const string ContextTypeRelativePrefix = "relativePrefix";
        public const string ContextTypeRelativeSuffix = "relativeSuffix";
        public const string ContextTypeAmPm = "AmPm";
        public const string SubType = "subType";

        // Comment - internal tag used during entity processing, never exposed to users.
        // Tags are filtered out in BaseMergedDateTimeParser DateTimeResolution()
        public const string Comment = "Comment";

        // AmPm time representation for time parser
        public const string CommentAmPm = "ampm";

        // Prefix early/late for time parser
        public const string CommentEarly = "early";
        public const string CommentLate = "late";

        // Parse week of date format
        public const string CommentWeekOf = "WeekOf";
        public const string CommentMonthOf = "MonthOf";

        // MOD Value
        // "before" -> To mean "preceding in time". I.e. Does not include the extracted datetime entity in the resolution's ending point. Equivalent to "<"
        public const string BEFOREMOD = "before";

        // "after" -> To mean "following in time". I.e. Does not include the extracted datetime entity in the resolution's starting point. Equivalent to ">"
        public const string AFTERMOD = "after";

        // "since" -> Same as "after", but including the extracted datetime entity. Equivalent to ">="
        public const string SINCEMOD = "since";

        // "until" -> Same as "before", but including the extracted datetime entity. Equivalent to "<="
        public const string UNTILMOD = "until";

        public const string EARLYMOD = "start";
        public const string MIDMOD = "mid";
        public const string LATEMOD = "end";

        public const string MORETHANMOD = "more";
        public const string LESSTHANMOD = "less";

        public const string REFUNDEFMOD = "ref_undef";

        public const string APPROXMOD = "approx";

        // Invalid year
        public const int InvalidYear = int.MinValue;
        public const int InvalidMonth = int.MinValue;
        public const int InvalidDay = int.MinValue;
        public const int InvalidHour = int.MinValue;
        public const int InvalidMinute = int.MinValue;
        public const int InvalidSecond = int.MinValue;

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

        public const string DefaultLanguageFallbackMDY = "MDY";
        public const string DefaultLanguageFallbackDMY = "DMY";

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
        public const string TimeOfDayGroupName = "timeOfDay";
        public const string BusinessDayGroupName = "business";
        public const string LeftAmPmGroupName = "leftDesc";
        public const string RightAmPmGroupName = "rightDesc";

        public const string DECADEUNIT = "10Y";

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

        // Non Constants
        public static readonly int MinYearNum = int.Parse(BaseDateTime.MinYearNum);
        public static readonly int MaxYearNum = int.Parse(BaseDateTime.MaxYearNum);
        public static readonly int MaxTwoDigitYearFutureNum = int.Parse(BaseDateTime.MaxTwoDigitYearFutureNum);
        public static readonly int MinTwoDigitYearPastNum = int.Parse(BaseDateTime.MinTwoDigitYearPastNum);
        public static readonly string[] DatePeriodTimexSplitter = { ",", "(", ")" };
    }
}
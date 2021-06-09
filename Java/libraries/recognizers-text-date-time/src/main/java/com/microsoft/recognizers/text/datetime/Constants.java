package com.microsoft.recognizers.text.datetime;

import com.microsoft.recognizers.text.datetime.resources.BaseDateTime;

public class Constants {

    public static final String SYS_DATETIME_DATE = "date";
    public static final String SYS_DATETIME_TIME = "time";
    public static final String SYS_DATETIME_DATEPERIOD = "daterange";
    public static final String SYS_DATETIME_DATETIME = "datetime";
    public static final String SYS_DATETIME_TIMEPERIOD = "timerange";
    public static final String SYS_DATETIME_DATETIMEPERIOD = "datetimerange";
    public static final String SYS_DATETIME_DURATION = "duration";
    public static final String SYS_DATETIME_SET = "set";
    public static final String SYS_DATETIME_DATETIMEALT = "datetimealt";
    public static final String SYS_DATETIME_TIMEZONE = "timezone";

    // SourceEntity Types
    public static final String SYS_DATETIME_DATETIMEPOINT = "datetimepoint";

    // Model Name
    public static final String MODEL_DATETIME = "datetime";

    // Multiple Duration Types
    public static final String MultipleDuration_Prefix = "multipleDuration";
    public static final String MultipleDuration_Type = MultipleDuration_Prefix + "Type";
    public static final String MultipleDuration_DateTime = MultipleDuration_Prefix + "DateTime";
    public static final String MultipleDuration_Date = MultipleDuration_Prefix + "Date";
    public static final String MultipleDuration_Time = MultipleDuration_Prefix + "Time";

    // DateTime Parse
    public static final String Resolve = "resolve";
    public static final String ResolveToPast = "resolveToPast";
    public static final String ResolveToFuture = "resolveToFuture";
    public static final String FutureDate = "futureDate";
    public static final String PastDate = "pastDate";
    public static final String ParseResult1 = "parseResult1";
    public static final String ParseResult2 = "parseResult2";

    // In the ExtractResult data
    public static final String Context = "context";
    public static final String ContextType_RelativePrefix = "relativePrefix";
    public static final String ContextType_RelativeSuffix = "relativeSuffix";
    public static final String ContextType_AmPm = "AmPm";
    public static final String SubType = "subType";

    // Comment - internal tag used during entity processing, never exposed to users. 
    // Tags are filtered out in BaseMergedDateTimeParser DateTimeResolution()
    public static final String Comment = "Comment";
    // AmPm time representation for time parser
    public static final String Comment_AmPm = "ampm";
    // Prefix early/late for time parser
    public static final String Comment_Early = "early";
    public static final String Comment_Late = "late";
    // Parse week of date format
    public static final String Comment_WeekOf = "WeekOf";
    public static final String Comment_MonthOf = "MonthOf";

    public static final String Comment_DoubleTimex = "doubleTimex";

    public static final String InvalidDateString = "0001-01-01";
    public static final String CompositeTimexDelimiter = "|";
    public static final String CompositeTimexSplit = "\\|";

    // Mod Value
    // "before" -> To mean "preceding in time". I.e. Does not include the extracted datetime entity in the resolution's ending point. Equivalent to "<"
    public static final String BEFORE_MOD = "before";

    // "after" -> To mean "following in time". I.e. Does not include the extracted datetime entity in the resolution's starting point. Equivalent to ">"
    public static final String AFTER_MOD = "after";

    // "since" -> Same as "after", but including the extracted datetime entity. Equivalent to ">="
    public static final String SINCE_MOD = "since";

    // "until" -> Same as "before", but including the extracted datetime entity. Equivalent to "<="
    public static final String UNTIL_MOD = "until";

    public static final String EARLY_MOD = "start";
    public static final String MID_MOD = "mid";
    public static final String LATE_MOD = "end";

    public static final String MORE_THAN_MOD = "more";
    public static final String LESS_THAN_MOD = "less";

    public static final String REF_UNDEF_MOD = "ref_undef";

    public static final String APPROX_MOD = "approx";

    // Invalid year
    public static final int InvalidYear = Integer.MIN_VALUE;
    public static final int InvalidMonth = Integer.MIN_VALUE;
    public static final int InvalidDay = Integer.MIN_VALUE;
    public static final int InvalidHour = Integer.MIN_VALUE;
    public static final int InvalidMinute = Integer.MIN_VALUE;
    public static final int InvalidSecond = Integer.MIN_VALUE;

    public static final int MinYearNum = BaseDateTime.MinYearNum;
    public static final int MaxYearNum = BaseDateTime.MaxYearNum;

    public static final int MaxTwoDigitYearFutureNum = BaseDateTime.MaxTwoDigitYearFutureNum;
    public static final int MinTwoDigitYearPastNum = BaseDateTime.MinTwoDigitYearPastNum;

    // These are some particular values for timezone recognition
    public static final int InvalidOffsetValue = -10000;
    public static final String UtcOffsetMinsKey = "utcOffsetMins";
    public static final String TimeZoneText = "timezoneText";
    public static final String TimeZone = "timezone";
    public static final String ResolveTimeZone = "resolveTimeZone";
    public static final int PositiveSign = 1;
    public static final int NegativeSign = -1;

    public static final int TrimesterMonthCount = 3;
    public static final int QuarterCount = 4;
    public static final int SemesterMonthCount = 6;
    public static final int WeekDayCount = 7;
    public static final int CenturyYearsCount = 100;
    public static final int MaxWeekOfMonth = 5;

    // hours of one half day
    public static final int HalfDayHourCount = 12;
    // hours of a half mid-day-duration
    public static final int HalfMidDayDurationHourCount = 2;

    // the length of four digits year, e.g., 2018
    public static final int FourDigitsYearLength = 4;

    // specifies the priority interpreting month and day order
    public static final String DefaultLanguageFallback_MDY = "MDY";
    public static final String DefaultLanguageFallback_DMY = "DMY";
    public static final String DefaultLanguageFallback_YMD = "YMD"; // ZH

    // Groups' names for named groups in regexes
    public static final String NextGroupName = "next";
    public static final String AmGroupName = "am";
    public static final String PmGroupName = "pm";
    public static final String ImplicitAmGroupName = "iam";
    public static final String ImplicitPmGroupName = "ipm";
    public static final String PrefixGroupName = "prefix";
    public static final String SuffixGroupName = "suffix";
    public static final String DescGroupName = "desc";
    public static final String SecondGroupName = "sec";
    public static final String MinuteGroupName = "min";
    public static final String HourGroupName = "hour";
    public static final String TimeOfDayGroupName = "timeOfDay";
    public static final String BusinessDayGroupName = "business";
    public static final String LeftAmPmGroupName = "leftDesc";
    public static final String RightAmPmGroupName = "rightDesc";

    public static final String DECADE_UNIT = "10Y";
    public static final String FORTNIGHT_UNIT = "2W";

    // Timex
    public static final String[] DatePeriodTimexSplitter = { ",", "(", ")" };
    public static final String TimexYear = "Y";
    public static final String TimexMonth = "M";
    public static final String TimexMonthFull = "MON";
    public static final String TimexWeek = "W";
    public static final String TimexDay = "D";
    public static final String TimexBusinessDay = "BD";
    public static final String TimexWeekend = "WE";
    public static final String TimexHour = "H";
    public static final String TimexMinute = "M";
    public static final String TimexSecond = "S";
    public static final char TimexFuzzy = 'X';
    public static final String TimexFuzzyYear = "XXXX";
    public static final String TimexFuzzyMonth = "XX";
    public static final String TimexFuzzyWeek = "WXX";
    public static final String TimexFuzzyDay = "XX";
    public static final String DateTimexConnector = "-";
    public static final String TimeTimexConnector = ":";
    public static final String GeneralPeriodPrefix = "P";
    public static final String TimeTimexPrefix = "T";

    // Timex of TimeOfDay
    public static final String EarlyMorning = "TDA";
    public static final String Morning = "TMO";
    public static final String Afternoon = "TAF";
    public static final String Evening = "TEV";
    public static final String Daytime = "TDT";
    public static final String Night = "TNI";
    public static final String BusinessHour = "TBH";
}

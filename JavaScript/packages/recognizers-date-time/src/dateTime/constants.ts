import { BaseDateTime } from "../resources/baseDateTime";

export class Constants {
    static readonly SYS_DATETIME_DATE: string = "date";
    static readonly SYS_DATETIME_TIME: string = "time";
    static readonly SYS_DATETIME_DATEPERIOD: string = "daterange";
    static readonly SYS_DATETIME_DATETIME: string = "datetime";
    static readonly SYS_DATETIME_TIMEPERIOD: string = "timerange";
    static readonly SYS_DATETIME_DATETIMEPERIOD: string = "datetimerange";
    static readonly SYS_DATETIME_DURATION: string = "duration";
    static readonly SYS_DATETIME_SET: string = "set";

    // SourceEntity Types
    static readonly SYS_DATETIME_DATETIMEPOINT = "datetimepoint";

    // key
    static readonly TimexKey: string = "timex";
    static readonly ModKey: string = "Mod";
    static readonly SourceEntity: string = "sourceEntity";
    static readonly TypeKey: string = "type";
    static readonly IsLunarKey: string = "isLunar";
    static readonly ResolveKey: string = "resolve";
    static readonly ResolveToPastKey: string = "resolveToPast";
    static readonly ResolveToFutureKey: string = "resolveToFuture";
    static readonly Decade: string = "decade";
    static readonly Century: string = "century"
    static readonly RelCentury: string = "relcentury"
    
    static readonly CommentKey: string = "Comment";
    static readonly CommentAmPm: string = "ampm";

    // Tag to mark cases where the specifc resolution timex depends on future or past values.
    static readonly Comment_DoubleTimex: string = "DoubleTimex";

    static readonly SemesterMonthCount: number = 6;
    static readonly TrimesterMonthCount: number = 3;
    static readonly QuarterCount: number = 4;
    static readonly FourDigitsYearLength: number = 4;
    static readonly MaxMonth: number = 11;
    static readonly MinMonth: number = 0;

    // specifies the priority interpreting month and day order
    static readonly DefaultLanguageFallback_MDY: string = 'MDY';
    static readonly DefaultLanguageFallback_DMY: string = 'DMY';
    static readonly DefaultLanguageFallback_YMD: string = "YMD"; // ZH

    static readonly MinYearNum: number = parseInt(BaseDateTime.MinYearNum);
    static readonly MaxYearNum: number = parseInt(BaseDateTime.MaxYearNum);

    static readonly MaxTwoDigitYearFutureNum: number = parseInt(BaseDateTime.MaxTwoDigitYearFutureNum);
    static readonly MinTwoDigitYearPastNum: number = parseInt(BaseDateTime.MinTwoDigitYearPastNum);

    // Mod Value
    // "before" -> To mean "preceding in time". I.e. Does not include the extracted datetime entity in the resolution's ending point. Equivalent to "<"
    static readonly BEFORE_MOD: string = 'before';

    // "after" -> To mean "following in time". I.e. Does not include the extracted datetime entity in the resolution's starting point. Equivalent to ">"
    static readonly AFTER_MOD: string = 'after';

    // "since" -> Same as "after", but including the extracted datetime entity. Equivalent to ">="
    static readonly SINCE_MOD: string = 'since';

    // "until" -> Same as "before", but including the extracted datetime entity. Equivalent to "<="
    static readonly UNTIL_MOD: string = 'until';

    static readonly EARLY_MOD: string = 'start';
    static readonly MID_MOD: string = 'mid';
    static readonly LATE_MOD: string = 'end';

    static readonly MORE_THAN_MOD: string = 'more';
    static readonly LESS_THAN_MOD: string = 'less';

    static readonly REF_UNDEF_MOD: string = 'ref_undef';

    // Timex
    static readonly TimexYear: string = "Y";
    static readonly TimexMonth: string = "M";
    static readonly TimexMonthFull: string = "MON";
    static readonly TimexWeek: string = "W";
    static readonly TimexDay: string = "D";
    static readonly TimexBusinessDay: string = "BD";
    static readonly TimexWeekend: string = "WE";
    static readonly TimexHour: string = "H";
    static readonly TimexMinute: string = "M";
    static readonly TimexSecond: string = "S";
    static readonly TimexFuzzy: string = 'X';
    static readonly TimexFuzzyYear: string = "XXXX";
    static readonly TimexFuzzyMonth: string = "XX";
    static readonly TimexFuzzyWeek: string = "WXX";
    static readonly TimexFuzzyDay: string = "XX";
    static readonly DateTimexConnector: string = "-";
    static readonly TimeTimexConnector: string = ":";
    static readonly GeneralPeriodPrefix: string = "P";
    static readonly TimeTimexPrefix: string = "T";
    static readonly ByDay: string = "ByDay";
    static readonly ByWeek: string = "ByWeek";
    static readonly ByMonth: string = "ByMonth";
    static readonly ByYear: string = "ByYear";

    static readonly DatePeriodTimexTypeToTimexSuffix: ReadonlyMap<string, string> = new Map<string, string>([[Constants.ByDay, Constants.TimexDay], [Constants.ByWeek, Constants.TimexWeek], [Constants.ByMonth, Constants.TimexMonth], [Constants.ByYear, Constants.TimexYear]]);

    // Timex of TimeOfDay
    static readonly EarlyMorning: string = "TDA";
    static readonly Morning: string = "TMO";
    static readonly MidDay: string = "TMI";
    static readonly Afternoon: string = "TAF";
    static readonly Evening: string = "TEV";
    static readonly Daytime: string = "TDT";
    static readonly Night: string = "TNI";
    static readonly BusinessHour: string = "TBH";

    static readonly InvalidDateString: string = "0001-01-01";

    static readonly CompositeTimexDelimiter: string = "|";

    // Invalid year
    static readonly InvalidYear: number = Number.MIN_VALUE;
    public readonly InvalidMonth: number = Number.MIN_VALUE;
    public readonly InvalidDay: number = Number.MIN_VALUE;
    public readonly InvalidHour: number = Number.MIN_VALUE;
    public readonly InvalidMinute: number = Number.MIN_VALUE;
    public readonly InvalidSecond: number = Number.MIN_VALUE;
}

export class TimeTypeConstants {
    static readonly DATE: string = "date";
    static readonly START_DATE: string = "startDate";
    static readonly END_DATE: string = "endDate";
    static readonly DATETIME: string = "dateTime";
    static readonly START_DATETIME: string = "startDateTime";
    static readonly END_DATETIME: string = "endDateTime";
    static readonly DURATION: string = "duration";
    static readonly SET: string = "set";
    static readonly TIME: string = "time";
    static readonly VALUE: string = "value";
    static readonly START_TIME: string = "startTime";
    static readonly END_TIME: string = "endTime";

    static readonly START: string = "start";
    static readonly END: string = "end";

    static readonly beforeMod: string = "before";
    static readonly afterMod: string = "after";
    static readonly sinceMod: string = "since";
    static readonly moreThanMod: string = "more";
    static readonly lessThanMod: string = "less";
}
import { BaseDateTime } from "../resources/baseDateTime";
import { EnglishDateTime } from "../resources/englishDateTime";
import { ChineseDateTime } from "../resources/chineseDateTime";

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
    
    static readonly CommentKey: string = "Comment";
    static readonly CommentAmPm: string = "ampm";

    static readonly SemesterMonthCount: number = 6;
    static readonly TrimesterMonthCount: number = 3;
    static readonly FourDigitsYearLength: number = 4;
    static readonly MaxMonth: number = 11;
    static readonly MinMonth: number = 0;

    static readonly DefaultLanguageFallback_MDY: string = 'MDY';
    static readonly DefaultLanguageFallback_DMY: string = 'DMY';

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

    static readonly EarlyMorning: string = "TDA";
    static readonly Morning: string = "TMO";
    static readonly MidDay: string = "TMI";
    static readonly Afternoon: string = "TAF";
    static readonly Evening: string = "TEV";
    static readonly Daytime: string = "TDT";
    static readonly Night: string = "TNI";
    static readonly BusinessHour: string = "TBH";
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
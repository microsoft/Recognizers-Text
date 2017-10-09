export class Constants {
    static readonly SYS_DATETIME_DATE: string = "date";
    static readonly SYS_DATETIME_TIME: string = "time";
    static readonly SYS_DATETIME_DATEPERIOD: string = "daterange";
    static readonly SYS_DATETIME_DATETIME: string = "datetime";
    static readonly SYS_DATETIME_TIMEPERIOD: string = "timerange";
    static readonly SYS_DATETIME_DATETIMEPERIOD: string = "datetimerange";
    static readonly SYS_DATETIME_DURATION: string = "duration";
    static readonly SYS_DATETIME_SET: string = "set";

    // key
    static readonly TimexKey: string = "timex";
    static readonly CommentKey: string = "Comment";
    static readonly ModKey: string = "Mod";
    static readonly TypeKey: string = "type";
    static readonly IsLunarKey: string = "isLunar";
    static readonly ResolveKey: string = "resolve";
    static readonly ResolveToPastKey: string = "resolveToPast";
    static readonly ResolveToFutureKey: string = "resolveToFuture";
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
}
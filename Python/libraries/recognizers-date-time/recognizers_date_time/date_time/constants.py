from ..resources import BaseDateTime

class Constants:
    SYS_DATETIME_DATE: str = 'date'
    SYS_DATETIME_TIME: str = 'time'
    SYS_DATETIME_DATEPERIOD: str = 'daterange'
    SYS_DATETIME_DATETIME: str = 'datetime'
    SYS_DATETIME_TIMEPERIOD: str = 'timerange'
    SYS_DATETIME_DATETIMEPERIOD: str = 'datetimerange'
    SYS_DATETIME_DURATION: str = 'duration'
    SYS_DATETIME_SET: str = 'set'
    SYS_DATETIME_MERGED: str = 'datetimeV2'

    #keys
    TimexKey: str = 'timex'
    CommentKey: str = 'Comment'
    ModKey: str = 'Mod'
    TypeKey: str = 'type'
    IsLunarKey: str = 'isLunar'
    ResolveKey: str = 'resolve'
    ResolveToPastKey: str = 'resolveToPast'
    ResolveToFutureKey: str = 'resolveToFuture'

    SemesterMonthCount: int = 6
    TrimesterMonthCount: int = 3

    DefaultLanguageFallback_MDY: str = 'MDY'
    DefaultLanguageFallback_DMY: str = 'DMY'

    MaxTwoDigitYearFutureNum: int = int(BaseDateTime.MaxTwoDigitYearFutureNum)
    MinTwoDigitYearPastNum: int = int(BaseDateTime.MinTwoDigitYearPastNum)

class TimeTypeConstants:
    DATE: str = 'date'
    START_DATE: str = 'startDate'
    END_DATE: str = 'endDate'
    DATETIME: str = 'dateTime'
    START_DATETIME: str = 'startDateTime'
    END_DATETIME: str = 'endDateTime'
    DURATION: str = 'duration'
    SET: str = 'set'
    TIME: str = 'time'
    VALUE: str = 'value'
    START_TIME: str = 'startTime'
    END_TIME: str = 'endTime'

    START: str = 'start'
    END: str = 'end'

    # Mod Value
    # "before" -> To mean "preceding in time". I.e. Does not include the extracted datetime entity in the resolution's ending point. Equivalent to "<"
    BEFORE_MOD: str = 'before'
    # "after" -> To mean "following in time". I.e. Does not include the extracted datetime entity in the resolution's starting point. Equivalent to ">"
    AFTER_MOD: str = 'after'
    # "since" -> Same as "after", but including the extracted datetime entity. Equivalent to ">="
    SINCE_MOD: str = 'since'
    # "until" -> Same as "before", but including the extracted datetime entity. Equivalent to "<="
    UNTIL_MOD: str = 'until'
    EARLY_MOD: str = 'start'
    MID_MOD: str = 'mid'
    LATE_MOD: str = 'end'
    MORE_THAN_MOD: str = 'more'
    LESS_THAN_MOD: str = 'less'
    REF_UNDEF_MOD: str = 'ref_undef'

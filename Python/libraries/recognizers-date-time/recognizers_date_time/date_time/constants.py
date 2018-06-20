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

    BEFORE_MOD: str = 'before'
    AFTER_MOD: str = 'after'
    SINCE_MOD: str = 'since'

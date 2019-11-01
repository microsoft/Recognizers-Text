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
    SYS_DATETIME_TIMEZONE: str = 'timezone'

    SYS_DATETIME_MERGED: str = 'datetimeV2'

    # SourceEntity Types
    SYS_DATETIME_DATETIMEPOINT: str = 'datetimepoint'

    # Multiple Duration Types
    MULTIPLE_DURATION_PREFIX = 'multipleDuration'
    MULTIPLE_DURATION_DATE = MULTIPLE_DURATION_PREFIX + 'Date'
    MULTIPLE_DURATION_TIME = MULTIPLE_DURATION_PREFIX + 'Time'
    MULTIPLE_DURATION_DATE_TIME = MULTIPLE_DURATION_PREFIX + 'DateTime'

    # keys
    TIMEX_KEY: str = 'timex'
    COMMENT_KEY: str = 'Comment'
    MOD_KEY: str = 'Mod'
    SOURCE_TYPE: str = 'sourceEntity'
    TYPE_KEY: str = 'type'
    IS_LUNAR_KEY: str = 'isLunar'
    RESOLVE_KEY: str = 'resolve'
    RESOLVE_TO_PAST_KEY: str = 'resolveToPast'
    RESOLVE_TO_FUTURE_KEY: str = 'resolveToFuture'

    SEMESTER_MONTH_COUNT: int = 6
    TRIMESTER_MONTH_COUNT: int = 3
    QUARTER_COUNT: int = 4
    FOUR_DIGITS_YEAR_LENGTH: int = 4
    MIN_MONTH: int = 1
    MAX_MONTH: int = 12
    INVALID_YEAR = -2147483648

    MIN_YEAR_NUM: int = int(BaseDateTime.MinYearNum)
    MAX_YEAR_NUM: int = int(BaseDateTime.MaxYearNum)

    DEFAULT_LANGUAGE_FALLBACK_MDY: str = 'MDY'
    DEFAULT_LANGUAGE_FALLBACK_DMY: str = 'DMY'

    MAX_TWO_DIGIT_YEAR_FUTURE_NUM: int = int(BaseDateTime.MaxTwoDigitYearFutureNum)
    MIN_TWO_DIGIT_YEAR_PAST_NUM: int = int(BaseDateTime.MinTwoDigitYearPastNum)

    # Timex
    TIMEX_YEAR: str = "Y"
    TIMEX_MONTH: str = "M"
    TIMEX_MONTH_FULL: str = "MON"
    TIMEX_WEEK: str = "W"
    TIMEX_DAY: str = "D"
    TIMEX_BUSINESS_DAY: str = "BD"
    TIMEX_WEEKEND: str = "WE"
    TIMEX_HOUR: str = "H"
    TIMEX_MINUTE: str = "M"
    TIMEX_SECOND: str = "S"
    TIMEX_FUZZY: str = 'X'
    TIMEX_FUZZY_YEAR: str = "XXXX"
    TIMEX_FUZZY_MONTH: str = "XX"
    TIMEX_FUZZY_WEEK: str = "WXX"
    TIMEX_FUZZY_DAY: str = "XX"
    DATE_TIMEX_CONNECTOR: str = "-"
    TIME_TIMEX_CONNECTOR: str = ":"
    GENERAL_PERIOD_PREFIX: str = "P"
    TIME_TIMEX_PREFIX: str = "T"

    EARLY_MORNING: str = "TDA"
    MORNING: str = "TMO"
    MID_DAY: str = "TMI"
    AFTERNOON: str = "TAF"
    EVENING: str = "TEV"
    DAYTIME: str = "TDT"
    NIGHT: str = "TNI"
    BUSINESS_HOUR = "TBH"

    # Groups' names for named groups in regexes
    NEXT_GROUP_NAME = "next"
    AM_GROUP_NAME = 'am'
    PM_GROUP_NAME = 'pm'
    AM_PM_GROUP_NAME = 'ampm'
    IMPLICIT_AM_GROUP_NAME = 'iam'
    IMPLICIT_PM_GROUP_NAME = 'ipm'
    PREFIX_GROUP_NAME = 'prefix'
    SUFFIX_GROUP_NAME = 'suffix'
    SUFFIX_NUM_GROUP_NAME = 'suffix_num'
    DESC_GROUP_NAME = 'desc'
    LEFT_DESC_GROUP_NAME = 'leftDesc'
    RIGHT_DESC_GROUP_NAME = 'rightDesc'
    SECOND_GROUP_NAME = 'sec'
    MINUTE_GROUP_NAME = 'min'
    HOUR_GROUP_NAME = 'hour'
    DAY_GROUP_NAME = 'day'
    WEEK_GROUP_NAME = 'week'
    WEEKDAY_GROUP_NAME = 'weekday'
    MONTH_GROUP_NAME = 'month'
    YEAR_GROUP_NAME = 'year'
    HOUR_NUM_GROUP_NAME = 'hournum'
    TENS_GROUP_NAME = 'tens'

    TIME_OF_DAY_GROUP_NAME = 'timeOfDay'
    BUSINESS_DAY_GROUP_NAME = 'business'
    LEFT_AM_PM_GROUP_NAME = 'leftDesc'
    RIGHT_AM_PM_GROUP_NAME = 'rightDesc'
    HOLIDAY_GROUP_NAME = 'holiday'

    REL_MONTH = 'relmonth'
    FIRST_TWO_YEAR_NUM = 'firsttwoyearnum'
    LAST_TWO_YEAR_NUM = 'lasttwoyearnum'
    YEAR_CHINESE = 'yearchs'
    OTHER = 'other'
    YEAR_RELATIVE = 'yearrel'
    DAY_OF_MONTH = 'DayOfMonth'

    NEW_TIME = 'newTime'
    ENGLISH_TIME = 'engtime'

    WEEK_OF = 'WeekOf'
    MONTH_OF = 'MonthOf'

    ORDER = 'order'
    ORDER_QUARTER = 'orderQuarter'

    NUM = 'num'
    NUMBER = 'number'
    MIN_NUM = 'minnum'

    MID = 'mid'
    MIDDAY = 'midday'
    MID_AFTERNOON = 'midafternoon'
    MID_MORNING = 'midmorning'
    MID_NIGHT = 'midnight'

    CARDINAL = 'cardinal'

    SEAS = 'seas'
    SEASON = 'season'

    UNIT = 'unit'
    UNIT_Y = 'Y'
    UNIT_D = 'D'
    UNIT_M = 'M'
    UNIT_W = 'W'
    UNIT_MON = 'MON'
    UNIT_S = 'S'
    UNIT_H = 'H'
    UNIT_T = 'T'
    UNIT_P = 'P'

    # Prefix
    EARLY_PREFIX = 'EarlyPrefix'
    LATE_PREFIX = 'LatePrefix'
    MID_PREFIX = 'MidPrefix'

    REL_EARLY = 'RelEarly'
    REL_LATE = 'RelLate'
    EARLY = 'early'
    LATE = 'late'

    HALF = 'half'

    # Holidays
    # These should not be constants, they should go on the resources files for English
    FATHERS = 'fathers'
    MOTHERS = 'mothers'
    THANKS_GIVING_DAY = 'thanksgivingday'
    THANKS_GIVING = 'thanksgiving'
    BLACK_FRIDAY = 'blackfriday'
    MARTIN_LUTHER_KING = 'martinlutherking'
    WASHINGTON_BIRTHDAY = 'washingtonsbirthday'
    LABOUR = 'labour'
    CANBERRA = 'canberra'
    COLUMBUS = 'columbus'
    MEMORIAL = 'memorial'


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
    # "before" -> To mean "preceding in time". I.e.
    # Does not include the extracted datetime entity in the resolution's ending point. Equivalent to "<"
    BEFORE_MOD: str = 'before'
    # "after" -> To mean "following in time". I.e.
    # Does not include the extracted datetime entity in the resolution's starting point. Equivalent to ">"
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

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

    EARLY_MORNING: str = "TDA"
    MORNING: str = "TMO"
    MID_DAY: str = "TMI"
    AFTERNOON: str = "TAF"
    EVENING: str = "TEV"
    DAYTIME: str = "TDT"
    NIGHT: str = "TNI"
    BUSINESS_HOUR = "TBH"

    # Groups' names for named groups in regexes
    next_group_name = "next"

    next_group_name = 'next'
    am_group_name = 'am'
    pm_group_name = 'pm'
    am_pm_group_name = 'ampm'
    implicit_am_group_name = 'iam'
    implicit_pm_group_name = 'ipm'
    prefix_group_name = 'prefix'
    suffix_group_name = 'suffix'
    suffix_num_group_name = 'suffix_num'
    desc_group_name = 'desc'
    left_desc_group_name = 'leftDesc'
    right_desc_group_name = 'rightDesc'
    second_group_name = 'sec'
    minute_group_name = 'min'
    hour_group_name = 'hour'
    day_group_name = 'day'
    week_group_name = 'week'
    WEEKDAY_GROUP_NAME = 'weekday'
    month_group_name = 'month'
    year_group_name = 'year'
    hour_num_group_name = 'hournum'

    time_of_day_group_name = 'timeOfDay'
    business_day_group_name = 'business'
    left_am_pm_group_name = 'leftDesc'
    right_am_pm_group_name = 'rightDesc'
    holiday_group_name = 'holiday'

    rel_month = 'relmonth'
    first_two_year_num = 'firsttwoyearnum'
    last_two_year_num = 'lasttwoyearnum'
    year_chinese = 'yearchs'
    OTHER = 'other'
    year_relative = 'yearrel'

    week_of = 'WeekOf'
    month_of = 'MonthOf'

    order = 'order'
    order_quarter = 'orderQuarter'

    num = 'num'
    number = 'number'
    num = 'num'

    cardinal = 'cardinal'

    seas = 'seas'
    season = 'season'
    rel_month = 'relmonth'

    unit = 'unit'
    unit_Y = 'Y'
    unit_D = 'D'
    unit_M = 'M'
    unit_W = 'W'
    unit_MON = 'MON'
    unit_S = 'S'
    unit_H = 'H'
    unit_T = 'T'
    UNIT = 'unit'

    # Prefix
    early_prefix = 'EarlyPrefix'
    late_prefix = 'LatePrefix'
    mid_prefix = 'MidPrefix'

    rel_early = 'RelEarly'
    rel_late = 'RelLate'
    early = 'early'
    late = 'late'

    half = 'half'

    # Holidays
    fathers = 'fathers'
    mothers = 'mothers'
    thanks_giving_day = 'thanksgivingday'
    thanks_giving = 'thanksgiving'
    black_friday = 'blackfriday'
    martin_luther_king = 'martinlutherking'
    washington_birthday = 'washingtonsbirthday'
    labour = 'labour'
    canberra = 'canberra'
    columbus = 'columbus'
    memorial = 'memorial'


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

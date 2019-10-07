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
    multiple_duration_prefix = 'multipleDuration'
    multiple_duration_date = multiple_duration_prefix + 'Date'
    multiple_duration_time = multiple_duration_prefix + 'Time'
    multiple_duration_date_time = multiple_duration_prefix + 'DateTime'

    # keys
    timex_key: str = 'timex'
    comment_key: str = 'Comment'
    mod_key: str = 'Mod'
    source_type: str = 'sourceEntity'
    type_key: str = 'type'
    is_lunar_key: str = 'isLunar'
    resolve_key: str = 'resolve'
    resolve_to_past_key: str = 'resolveToPast'
    resolve_to_future_key: str = 'resolveToFuture'

    semester_month_count: int = 6
    trimester_month_count: int = 3
    quarter_count: int = 4
    four_digits_year_length: int = 4
    min_month: int = 1
    max_month: int = 12
    invalid_year = -2147483648

    min_year_num: int = int(BaseDateTime.MinYearNum)
    max_year_num: int = int(BaseDateTime.MaxYearNum)

    default_language_fallback_mdy: str = 'MDY'
    default_language_fallback_dmy: str = 'DMY'

    max_two_digit_year_future_num: int = int(BaseDateTime.MaxTwoDigitYearFutureNum)
    min_two_digit_year_past_num: int = int(BaseDateTime.MinTwoDigitYearPastNum)

    early_morning: str = "TDA"
    morning: str = "TMO"
    mid_day: str = "TMI"
    afternoon: str = "TAF"
    evening: str = "TEV"
    daytime: str = "TDT"
    night: str = "TNI"
    business_hour = "TBH"

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
    second_group_name = 'sec'
    minute_group_name = 'min'
    hour_group_name = 'hour'
    day_group_name = 'day'
    week_group_name = 'week'
    month_group_name = 'month'
    year_group_name = 'year'
    hour_num_group_name = 'hournum'
    time_of_day_group_name = 'timeOfDay'
    business_day_group_name = 'business'
    left_am_pm_group_name = 'leftDesc'
    right_am_pm_group_name = 'rightDesc'
    holiday_group_name = 'holiday'

    first_two_year_num = 'firsttwoyearnum'
    last_two_year_num = 'lasttwoyearnum'

    week_of = 'WeekOf'
    month_of = 'MonthOf'

    order = 'order'
    order_quarter = 'orderQuarter'

    number = 'number'
    num = 'num'

    cardinal = 'cardinal'

    rel_month = 'relmonth'

    season = 'seas'

    unit_Y = 'Y'
    unit_D = 'D'
    unit_M = 'M'
    unit_W = 'W'
    unit_MON = 'MON'
    unit_S = 'S'
    unit_H = 'H'
    unit_T = 'T'

    # Prefix
    early_prefix = 'EarlyPrefix'
    late_prefix = 'LatePrefix'
    mid_prefix = 'MidPrefix'

    rel_early = 'RelEarly'
    rel_late = 'RelLate'
    early = 'early'
    late = 'late'

    unit = 'unit'

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

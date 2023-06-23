#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from ..resources import BaseDateTime


class Constants:
    WRITTEN_TIME = 'writtentime'
    SYS_DATETIME_DATE: str = 'date'
    SYS_DATETIME_TIME: str = 'time'
    SYS_DATETIME_DATEPERIOD: str = 'daterange'
    SYS_DATETIME_DATETIME: str = 'datetime'
    SYS_DATETIME_TIMEPERIOD: str = 'timerange'
    SYS_DATETIME_DATETIMEPERIOD: str = 'datetimerange'
    SYS_DATETIME_DURATION: str = 'duration'
    SYS_DATETIME_SET: str = 'set'
    SYS_DATETIME_TIMEZONE: str = 'timezone'
    SYS_DATETIME_DATETIMEALT: str = 'datetimealt'

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
    INVALID_MONTH = -2147483648
    INVALID_HOUR = -2147483648
    INVALID_MINUTE = -2147483648
    INVALID_SECOND = -2147483648

    COMMENT_KEY: str = 'Comment'
    COMMENT_AMPM = 'ampm'
    COMMENT_AM = 'am'
    COMMENT_DOUBLETIMEX = "doubleTimex"

    # Default boundaries for time of day resolution
    EARLY_MORNING_BEGIN_HOUR = 4
    EARLY_MORNING_END_HOUR = 8
    MORNING_BEGIN_HOUR = 8
    MORNING_END_HOUR = 12
    MID_DAY_BEGIN_HOUR = 11
    MID_DAY_END_HOUR = 13
    AFTERNOON_BEGIN_HOUR = 12
    AFTERNOON_END_HOUR = 16
    EVENING_BEGIN_HOUR = 16
    EVENING_END_HOUR = 20
    DAYTIME_BEGIN_HOUR = 8
    DAYTIME_END_HOUR = 18
    NIGHTTIME_BEGIN_HOUR = 0
    NIGHTTIME_END_HOUR = 8
    BUSINESS_BEGIN_HOUR = 8
    BUSINESS_END_HOUR = 18
    NIGHT_BEGIN_HOUR = 20
    NIGHT_END_HOUR = 23
    NIGHT_END_MINUTE = 59
    MEAL_TIME_BREAKFAST_BEGIN_HOUR = 8
    MEAL_TIME_BREAKFAST_END_HOUR = 12
    MEAL_TIME_BRUNCH_BEGIN_HOUR = 8
    MEAL_TIME_BRUNCH_END_HOUR = 12
    MEAL_TIME_LUNCH_BEGIN_HOUR = 11
    MEAL_TIME_LUNCH_END_HOUR = 13
    MEAL_TIME_DINNER_BEGIN_HOUR = 16
    MEAL_TIME_DINNER_END_HOUR = 20



    # Failed connector extraction
    INVALID_CONNECTOR_CODE = -1

    MIN_YEAR_NUM: int = int(BaseDateTime.MinYearNum)
    MAX_YEAR_NUM: int = int(BaseDateTime.MaxYearNum)

    HALF_DAY_HOUR_COUNT = 12
    DAY_HOUR_COUNT = 24
    DAY_HOUR_START = 0
    HOUR_SECOND_COUNT = 3600
    MINUTE_SECOND_COUNT = 60
    HALF_MID_DAY_DURATION_HOUR_COUNT = 2
    WEEK_DAY_COUNT = 7
    CENTURY_YEARS_COUNT = 100

    # specifies the priority interpreting month and day order
    DEFAULT_LANGUAGE_FALLBACK_MDY: str = 'MDY'
    DEFAULT_LANGUAGE_FALLBACK_DMY: str = 'DMY'
    DEFAULT_LANGUAGE_FALLBACK_YMD: str = 'YMD'  # ZH

    MAX_TWO_DIGIT_YEAR_FUTURE_NUM: int = int(BaseDateTime.MaxTwoDigitYearFutureNum)
    MIN_TWO_DIGIT_YEAR_PAST_NUM: int = int(BaseDateTime.MinTwoDigitYearPastNum)

    BASE_YEAR_PAST_CENTURY = 1900
    BASE_YEAR_CURRENT_CENTURY = 2000

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
    TIMEX_FUZZY_TWO_DIGIT_YEAR = "XX"
    TIMEX_FUZZY_MONTH: str = "XX"
    TIMEX_FUZZY_WEEK: str = "WXX"
    TIMEX_FUZZY_DAY: str = "XX"
    DATE_TIMEX_CONNECTOR: str = "-"
    TIME_TIMEX_CONNECTOR: str = ":"
    TIMEX_SEPARATOR: str = ","
    GENERAL_PERIOD_PREFIX: str = "P"
    TIME_TIMEX_PREFIX: str = "T"

    EARLY_MORNING: str = "TDA"
    MORNING: str = "TMO"
    MID_DAY: str = "TMI"
    AFTERNOON: str = "TAF"
    EVENING: str = "TEV"
    DAYTIME: str = "TDT"
    NIGHT: str = "TNI"
    BUSINESS_HOUR: str = "TBH"
    MEALTIME_BREAKFAST: str = "TMEB"
    MEALTIME_BRUNCH: str = "TBH"
    MEALTIME_LUNCH: str = "TMEL"
    MEALTIME_DINNER: str = "TMED"

    INVALID_DATE_STRING = "0001-01-01"
    COMPOSTIE_TIMEX_DELIMITER = "|"

    # Timex non-constant
    DURATION_UNIT_CHAR = ['D', 'W', 'M', 'Y', 'B']

    # Groups' names for named groups in regexes
    NEXT_GROUP_NAME = "next"
    LAST_GROUP_NAME = "last"
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
    MONTH_FROM_GROUP_NAME = "monthFrom"
    MONTH_TO_GROUP_NAME = "monthTo"
    YEAR_GROUP_NAME = 'year'
    THIS_YEAR_GROUP_NAME = 'thisyear'
    THIS_MONTH_GROUP_NAME = 'thismonth'
    FULL_YEAR_GROUP_NAME = 'fullyear'
    HOUR_NUM_GROUP_NAME = 'hournum'
    TENS_GROUP_NAME = 'tens'
    YEAR_CJK_GROUP_NAME = 'yearCJK'
    UNIT_OF_YEAR_GROUP_NAME = 'uoy'
    LATER_GROUP_NAME = 'later'
    FEW_GROUP_NAME = 'few'
    LESS_GROUP_NAME = 'less'
    MORE_GROUP_NAME = 'more'
    SPECIFIC_END_OF_GROUP_NAME = "SpecificEndOf"
    TOMORROW_GROUP_NAME = "tomorrow"
    YESTERDAY_GROUP_NAME = "yesterday"
    REST_OF_GROUP_NAME = "restof"
    TO_DATE_GROUP_NAME = "toDate"
    HALF_GROUP_NAME = "half"
    FIRST_HALF_GROUP_NAME = 'firstHalf'
    SECOND_HALF_GROUP_NAME = 'secondHalf'
    HALF_TAG_GROUP_NAME = "halfTag"
    UNIT_GROUP_NAME = "unit"
    WITHIN_GROUP_NAME = "within"
    EARLY_PREFIX_GROUP_NAME = "EarlyPrefix"
    MID_PREFIX_GROUP_NAME = "MidPrefix"
    LATE_PREFIX_GROUP_NAME = "LatePrefix"
    SPECIAL_GROUP_NAME = 'special'
    HALF_GROUP_NAME = 'half'
    QUARTER_GROUP_NAME = 'quarter'
    THREE_QUARTER_GROUP_NAME = 'threequarter'

    TIME_OF_DAY_GROUP_NAME = 'timeOfDay'
    BUSINESS_DAY_GROUP_NAME = 'business'
    LEFT_AM_PM_GROUP_NAME = 'leftDesc'
    RIGHT_AM_PM_GROUP_NAME = 'rightDesc'
    MEALTIME_GROUP_NAME = 'mealTime'
    HOLIDAY_GROUP_NAME = 'holiday'
    ANOTHER_GROUP_NAME = 'another'

    REL_MONTH = 'relmonth'
    FIRST_TWO_YEAR_NUM = 'firsttwoyearnum'
    LAST_TWO_YEAR_NUM = 'lasttwoyearnum'
    YEAR_CHINESE = 'yearCJK'
    OTHER = 'other'
    YEAR_RELATIVE = 'yearrel'
    FOUR_DIGIT_YEAR_GROUP_NAME = "FourDigitYear"
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

    #  Include the date mentioned, to make "before" -> "until" or "after" -> "since". Such as "on or earlier than 1/1/2016".
    INCLUDE_GROUP_NAME = "include"

    CARDINAL = 'cardinal'

    DECADE_UNIT = '10Y'
    FORTNIGHT_UNIT = '2W'
    QUARTER_UNIT = '3MON'
    WEEKEND_UNIT = 'WE'

    DECADE = 'decade'
    CENTURY = 'century'
    REL_CENTURY = 'relcentury'

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
    COMMENT_EARLY = 'early'
    COMMENT_LATE = 'late'
    COMMENT_WEEK_OF = "WeekOf"
    COMMENT_MONTH_OF = "MonthOf"

    HALF = 'half'

    HAS_MOD = 'mod'
    LESS_THAN_MOD = 'less'
    MORE_THAN_MOD = 'more'

    BEFORE_MOD = "before"
    AFTER_MOD = "after"
    UNTIL_MOD = "until"
    SINCE_MOD = "since"
    APPROX_MOD = "approx"
    EARLY_MOD = "start"
    MID_MOD = "mid"
    LATE_MOD = "end"

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

    AGO_LABEL = "ago"
    LATER_LABEL = "later"

    # These are some particular values for timezone recognition
    INVALID_OFFSET_VALUE = -10000
    UTC_OFFSET_MINS_KEY = "utcOffsetMins"
    POSITIVE_SIGN = 1
    NEGATIVE_SIGN = -1
    RESOLVE_TIMEZONE = "resolveTimeZone"
    TIMEZONE_TEXT = "timezoneText"


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
    DATETIME_ALT = "dateTimeAlt"

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

    APPROX_MOD: str = 'approx'

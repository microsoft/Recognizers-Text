from typing import Dict, List, Tuple
from datetime import datetime

from datedelta import datedelta

from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.utilities import TimeOfDayResolution, DateUtils, \
    DateTimeFormatUtil, RangeTimexComponents, DateTimeResolutionKey
from datatypes_timex_expression.timex_helpers import TimexHelpers

date_period_timex_type_to_suffix = {
    0: Constants.TIMEX_DAY,
    1: Constants.TIMEX_WEEK,
    2: Constants.TIMEX_MONTH,
    3: Constants.TIMEX_YEAR,
}


class TimexUtil:

    @staticmethod
    def merge_timex_alternatives(timex1: str, timex2: str) -> str:
        if timex1 == timex2:
            return timex1
        return f"{timex1}{Constants.COMPOSTIE_TIMEX_DELIMITER}{timex2}"

    @staticmethod
    def parse_time_of_day(tod: str) -> TimeOfDayResolution:
        result = TimeOfDayResolution()

        if tod == Constants.EARLY_MORNING:
            result.timex = Constants.EARLY_MORNING
            result.begin_hour = 4
            result.end_hour = 8
        elif tod == Constants.MORNING:
            result.timex = Constants.MORNING
            result.begin_hour = 8
            result.end_hour = 12
        elif tod == Constants.MID_DAY:
            result.timex = Constants.MID_DAY
            result.begin_hour = 11
            result.end_hour = 13
        elif tod == Constants.AFTERNOON:
            result.timex = Constants.AFTERNOON
            result.begin_hour = 12
            result.end_hour = 16
        elif tod == Constants.EVENING:
            result.timex = Constants.EVENING
            result.begin_hour = 16
            result.end_hour = 20
        elif tod == Constants.DAYTIME:
            result.timex = Constants.DAYTIME
            result.begin_hour = 8
            result.end_hour = 18
        elif tod == Constants.BUSINESS_HOUR:
            result.timex = Constants.BUSINESS_HOUR
            result.begin_hour = 8
            result.end_hour = 18
        elif tod == Constants.NIGHT:
            result.timex = Constants.NIGHT
            result.begin_hour = 20
            result.end_hour = 23
            result.end_min = 59
        elif tod == Constants.MEALTIME_BREAKFAST:
            result.timex = Constants.MEALTIME_BREAKFAST
            result.begin_hour = 8
            result.end_hour = 12
        elif tod == Constants.MEALTIME_BRUNCH:
            result.timex = Constants.MEALTIME_BRUNCH
            result.begin_hour = 8
            result.end_hour = 12
        elif tod == Constants.MEALTIME_LUNCH:
            result.timex = Constants.MEALTIME_LUNCH
            result.begin_hour = 11
            result.end_hour = 13
        elif tod == Constants.MEALTIME_DINNER:
            result.timex = Constants.MEALTIME_DINNER
            result.begin_hour = 16
            result.end_hour = 20

        return result

    @staticmethod
    def set_timex_with_context(timex: str, context) -> str:
        result = timex.replace(Constants.TIMEX_FUZZY_YEAR, str(context.year))
        return result

    @staticmethod
    def generate_date_period_timex_unit_count(begin, end, timex_type, equal_duration_length=True):
        unit_count = 'XX'

        if equal_duration_length:
            if timex_type == 0:
                unit_count = (end - begin).days

            if timex_type == 1:
                unit_count = (end - begin).days / 7
            if timex_type == 2:
                unit_count = ((end.year - begin.year) * 12) + (end.month - begin.month)
            if timex_type == 3:
                unit_count = (end.year - begin.year) + ((end.mont - begin.month) / 12.0)
        return unit_count

    @staticmethod
    def generate_date_period_timex_str(begin, end, timex_type, timex1, timex2):
        boundary_valid = DateUtils.is_valid_datetime(begin) and DateUtils.is_valid_datetime(end)
        unit_count = TimexUtil.generate_date_period_timex_unit_count(begin, end, timex_type) if boundary_valid else "X"
        return f"({timex1},{timex2},P{unit_count}{date_period_timex_type_to_suffix[timex_type]})"

    @staticmethod
    def generate_date_period_timex_with_diff(begin: datetime, end_date: datetime, duration_unit):

        diff = 0
        if duration_unit == Constants.TIMEX_WEEK:
            diff = Constants.WEEK_DAY_COUNT - (Constants.WEEK_DAY_COUNT if begin.day == 0 else begin.isoweekday())
            end_date = begin + datedelta(days=diff)

        if duration_unit == Constants.TIMEX_MONTH_FULL:
            end_date = DateUtils.safe_create_from_min_value(begin.year, begin.month, 1)
            end_date = end_date + datedelta(months=1) - datedelta(days=1)
            diff - end_date.day - begin.day + 1

        if duration_unit == Constants.TIMEX_YEAR:
            end_date = DateUtils.safe_create_from_min_value(begin.year, 12, 1)
            end_date = end_date + datedelta(months=1) - datedelta(days=1)
            diff = DateUtils.day_of_year(end_date) - DateUtils.day_of_year(begin) + 1

        duration_timex = Constants.GENERAL_PERIOD_PREFIX + diff + Constants.TIMEX_DAY

        return f'({DateTimeFormatUtil.luis_date(begin.year, begin.month, begin.day)},' \
               f'{DateTimeFormatUtil.luis_date(end_date.year, end_date.month, end_date.day)},{duration_timex})'

    @staticmethod
    def generate_date_period_timex(begin, end, timex_type, alternative_begin: datetime = None,
                                   alternative_end: datetime = None, has_year: bool = False):

        # If the year is not specified, the combined range timex will use fuzzy years.
        if not has_year:
            begin.year = -1
            end.year = -1

        alternative = False
        if alternative_begin is None and alternative_end is None:
            alternative_begin = datetime.now()
            alternative_end = datetime.now()
        else:
            alternative = True

        equal_duration_length = (end - begin).days == (alternative_end - alternative_begin).days or \
                                datetime.now() == alternative_end == alternative_begin
        unit_count = TimexUtil.generate_date_period_timex_unit_count(begin, end, timex_type, equal_duration_length)
        date_period_timex = f'P{unit_count}{date_period_timex_type_to_suffix[timex_type]}'

        if alternative:
            f'({DateTimeFormatUtil.luis_date_from_datetime_with_alternative(begin, alternative_begin)},' \
            f'{DateTimeFormatUtil.luis_date_from_datetime_with_alternative(end, alternative_end)},{date_period_timex})'
        else:
            return f'({DateTimeFormatUtil.luis_date(begin.year, begin.month, begin.day)},' \
                   f'{DateTimeFormatUtil.luis_date(end.year, end.month, end.day)},{date_period_timex})'

    @staticmethod
    def generate_date_period_timex_with_duration(begin_date: datetime, end_date: datetime, duration_timex: str):
        return f'({DateTimeFormatUtil.luis_date(begin_date.year, begin_date.month, begin_date.day)},' \
               f'{DateTimeFormatUtil.luis_date(end_date.year, end_date.month, end_date.day)},{duration_timex})'

    @staticmethod
    def generate_split_date_time_period_timex(date_timex: str, time_range_timex: str):
        split = time_range_timex.split(Constants.TIME_TIMEX_PREFIX[0])
        timex: str = None
        if len(split) == 4:
            timex = split[0] + date_timex + Constants.TIME_TIMEX_PREFIX + split[1] + date_timex + \
                Constants.TIME_TIMEX_PREFIX + split[2] + Constants.TIME_TIMEX_PREFIX + split[3]
        elif len(split) == 2:
            timex = date_timex + time_range_timex

        return timex

    @staticmethod
    def generate_relative_unit_date_time_period_timex(begin_date_time: datetime, end_date_time: datetime,
                                                      reference_time: datetime, unit_str: str, swift: int):
        prefix: str = Constants.GENERAL_PERIOD_PREFIX + Constants.TIME_TIMEX_PREFIX
        duration_timex = ''
        if unit_str == Constants.TIMEX_DAY:
            end_date_time = DateUtils.safe_create_from_value(begin_date_time.year,
                                                             begin_date_time.month,
                                                             begin_date_time.day)
            end_date_time = end_date_time + timedelta(days=1, seconds=-1)
            duration_timex = prefix + (end_date_time - begin_date_time).total_seconds() + Constants.TIMEX_SECOND

        elif unit_str == Constants.TIMEX_HOUR:
            begin_date_time = begin_date_time if swift > 0 else reference_time + timedelta(hours=swift)
            end_date_time = reference_time + timedelta(hours=swift) if swift > 0 else end_date_time
            duration_timex = prefix + "1" + Constants.TIMEX_HOUR

        elif unit_str == Constants.TIMEX_MINUTE:
            begin_date_time = begin_date_time if swift > 0 else reference_time + timedelta(hours=swift)
            end_date_time = reference_time + timedelta(hours=swift) if swift > 0 else end_date_time
            duration_timex = prefix + "1" + Constants.TIMEX_MINUTE

        elif unit_str == Constants.TIMEX_SECOND:
            begin_date_time = begin_date_time if swift > 0 else reference_time + timedelta(hours=swift)
            end_date_time = reference_time + timedelta(hours=swift) if swift > 0 else end_date_time
            duration_timex = prefix + "1" + Constants.TIMEX_SECOND
        else:
            return ''

        return TimexUtil.generate_date_time_period_timex(begin_date_time, end_date_time, duration_timex)
    @staticmethod
    def _process_double_timex(resolution_dic: Dict[str, object], future_key: str, past_key: str, origin_timex: str):
        timexes = origin_timex.split(Constants.COMPOSTIE_TIMEX_DELIMITER)
        if not future_key in resolution_dic or not past_key in resolution_dic or len(timexes) != 2:
            return
        future_resolution = resolution_dic[future_key]
        past_resolution = resolution_dic[past_key]
        future_resolution[Constants.TIMEX_KEY] = timexes[0]
        past_resolution[Constants.TIMEX_KEY] = timexes[1]

    @staticmethod
    def _has_double_timex(comment: str):
        return comment == Constants.COMMENT_DOUBLETIMEX

    @staticmethod
    def is_range_timex(timex: str) -> bool:
        return timex and timex.startswith("(")

    @staticmethod
    def get_range_timex_components(range_timex: str) -> RangeTimexComponents:
        range_timex = range_timex.replace('(', '').replace(')', '')
        components = range_timex.split(',')
        result = RangeTimexComponents()
        if len(components) == 3:
            result.begin_timex = components[0]
            result.end_timex = components[1]
            result.duration_timex = components[2]
            result.is_valid = True

        return result

    @staticmethod
    def combine_date_and_time_timex(date_timex: str, time_timex: str):
        return f'{date_timex}{time_timex}'

    @staticmethod
    def generate_date_time_period_timex(begin_timex: str, end_timex: str, duration_timex: str):
        return f'({begin_timex},{end_timex},{duration_timex})'

    @staticmethod
    def parse_number_from_duration_timex(timex: str):
        number_str = timex[
                     timex.index(Constants.GENERAL_PERIOD_PREFIX) + 1:
                     timex.index(Constants.DURATION_UNIT_CHAR - 1)]

        return float(number_str)

    @staticmethod
    def generate_year_timex(date: any = None, special_year_prefixes=None):
        if not date:
            year_timex = Constants.TIMEX_FUZZY_YEAR
        else:
            if type(date) == datetime:
                year_timex = f'{date.year}'
            elif type(date) == int:
                # For when the year is used directly
                year_timex = DateTimeFormatUtil.luis_date(date, 1, 1)

        if not special_year_prefixes:
            return year_timex
        else:
            return special_year_prefixes + year_timex

    @staticmethod
    def parse_hour_from_time_timex(timex: str) -> int:
        start = timex.index(Constants.TIME_TIMEX_PREFIX) + 1
        end = timex.index(Constants.TIME_TIMEX_CONNECTOR)
        if not end > 0:
            end = len(timex)
        hour = int(timex[start:end - start])
        return hour

    @staticmethod
    def parse_hours_from_time_period_timex(timex: str) -> Tuple[str, str]:
        hour1 = 0
        hour2 = 0
        time_list = timex.split(Constants.TIMEX_SEPARATOR[0])
        if len(time_list) > 2:
            hour1 = TimexUtil.parse_hour_from_time_timex(time_list[0])
            hour2 = TimexUtil.parse_hour_from_time_timex(time_list[1])
        return tuple(hour1, hour2)

    @staticmethod
    def generate_date_time_timex(date_time: datetime) -> str:
        return DateTimeFormatUtil.luis_date_time(date_time)

    @staticmethod
    def has_double_timex(comment: str) -> bool:
        return comment == Constants.COMMENT_DOUBLETIMEX

    @staticmethod
    def process_double_timex(resolution_dict: Dict, future_key: str, past_key: str, origin_timex: str) -> Dict:
        timexes = origin_timex.split(Constants.COMPOSTIE_TIMEX_DELIMITER)
        if future_key not in resolution_dict or past_key not in resolution_dict or len(timexes) != 2:
            return resolution_dict

        future_resolution = resolution_dict[future_key]
        past_resolution = resolution_dict[past_key]
        future_resolution[DateTimeResolutionKey.timex] = timexes[0]
        past_resolution[DateTimeResolutionKey.timex] = timexes[1]

    @staticmethod
    def generate_week_timex(monday=None):
        if monday is None:
            return f'{Constants.TIMEX_FUZZY_YEAR}{Constants.DATE_TIMEX_CONNECTOR}{Constants.TIMEX_FUZZY_WEEK}'
        else:
            return DateTimeFormatUtil.to_iso_week_timex(monday)

    @staticmethod
    def generate_weekday_timex(weekday: int):
        return f'{Constants.TIMEX_FUZZY_YEAR}{Constants.DATE_TIMEX_CONNECTOR}{Constants.TIMEX_FUZZY_WEEK}' \
               f'{Constants.DATE_TIMEX_CONNECTOR}{weekday}'


    @staticmethod
    def generate_decade_timex(begin_year, total_last_year, decade, input_century) -> str:

        if input_century:
            begin_str = DateTimeFormatUtil.luis_date(begin_year, 1, 1)
            end_str = DateTimeFormatUtil.luis_date(begin_year + total_last_year, 1, 1)

        else:
            begin_year_str = Constants.TIMEX_FUZZY_TWO_DIGIT_YEAR + decade
            begin_str = DateTimeFormatUtil.luis_date(-1, 1, 1)
            begin_str = begin_str.replace(Constants.TIMEX_FUZZY_YEAR, begin_year_str)

            end_year_str = Constants.TIMEX_FUZZY_TWO_DIGIT_YEAR + f'{((decade + total_last_year) % 100):02d}'
            end_str = DateTimeFormatUtil.luis_date(-1, 1, 1)
            end_str = end_str.replace(Constants.TIMEX_FUZZY_YEAR, end_year_str)

        return f'({begin_str},{end_str},{Constants.GENERAL_PERIOD_PREFIX}{total_last_year}{Constants.TIMEX_YEAR})'

    @staticmethod
    def generate_week_of_month_timex(year: int, month: int, week_num: int) -> str:
        week_timex = TimexUtil.generate_week_timex(week_num)
        month_timex = DateTimeFormatUtil.luis_date(year, month, 1)

        return f'{month_timex}-{week_timex}'

    @staticmethod
    def generate_week_of_year_timex(year: int, week_num: int) -> str:
        week_timex = TimexUtil.generate_week_timex(week_num)
        year_timex = DateTimeFormatUtil.luis_date(year, 1, 1)

        return f'{year_timex}-{week_timex}'

    @staticmethod
    def generate_weekend_timex(date: datetime = None):
        if date is None:
            return f'{Constants.TIMEX_FUZZY_YEAR}{Constants.DATE_TIMEX_CONNECTOR}{Constants.TIMEX_FUZZY_WEEK}' \
                   f'{Constants.DATE_TIMEX_CONNECTOR}{Constants.TIMEX_WEEKEND}'
        else:
            return f'{DateTimeFormatUtil.to_iso_week_timex(date)}{Constants.DATE_TIMEX_CONNECTOR}' \
                   f'{Constants.TIMEX_WEEKEND}'

    @staticmethod
    def generate_month_timex(date: datetime = None):
        if date is None:
            return f'{Constants.TIMEX_FUZZY_YEAR}{Constants.DATE_TIMEX_CONNECTOR}{Constants.TIMEX_FUZZY_MONTH}'
        else:
            return f'{date.year:D4}{Constants.DATE_TIMEX_CONNECTOR}{date.month:D2}'
    @staticmethod
    def generate_duration_timex(number: float, unit_str: str, is_less_than_day: bool) -> str:
        if Constants.TIMEX_BUSINESS_DAY != unit_str:
            if unit_str == Constants.DECADE_UNIT:
                number = number * 10
                unit_str = Constants.TIMEX_YEAR
            elif unit_str == Constants.FORTNIGHT_UNIT:
                number = number * 2
                unit_str = Constants.TIMEX_WEEK
            elif unit_str == Constants.WEEKEND_UNIT:
                unit_str = Constants.TIMEX_WEEKEND
            else:
                unit_str = unit_str[0:1]

        if is_less_than_day:
            return Constants.GENERAL_PERIOD_PREFIX + Constants.TIME_TIMEX_PREFIX + str(number) + unit_str
        else:
            return Constants.GENERAL_PERIOD_PREFIX + str(number) + unit_str

    @staticmethod
    def generate_compound_duration_timex(unit_to_timex_components: Dict[str, str],
                                         unit_value_map: Dict[str, str]) -> str:
        unit_list: List[str] = list(unit_to_timex_components.keys())
        unit_list.sort(key=lambda x: unit_value_map[x])

        return TimexHelpers.generate_compound_duration_timex(unit_list)

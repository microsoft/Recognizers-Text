from typing import Dict
from datetime import datetime
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.utilities import TimeOfDayResolution, DateUtils, \
    DateTimeFormatUtil, RangeTimexComponents, DateTimeResolutionKey


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
                unit_count = (end - begin).days/7
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
    def generate_date_period_timex(begin, end, timex_type, alternative_begin=datetime.now(), alternative_end=datetime.now()):
        equal_duration_length = (end - begin).days == (alternative_end - alternative_begin).days or datetime.now() == alternative_end == alternative_begin
        unit_count = TimexUtil.generate_date_period_timex_unit_count(begin, end, timex_type, equal_duration_length)
        date_period_timex = f'P{unit_count}{date_period_timex_type_to_suffix[timex_type]}'

        return f'({DateTimeFormatUtil.luis_date(begin.year, begin.month, begin.day)},' \
               f'{DateTimeFormatUtil.luis_date(end.year, end.month, end.day)},{date_period_timex})'

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
    def parse_hour_from_time_timex(timex: str) -> int:
        start = timex.index(Constants.TIME_TIMEX_PREFIX) + 1
        end = timex.index(Constants.TIME_TIMEX_CONNECTOR)
        if not end > 0:
            end = len(timex)
        hour = int(timex[start:end-start])
        return hour

    @staticmethod
    def generate_date_time_timex(date_time: datetime) -> str:
        return DateTimeFormatUtil.luis_date_time(date_time)

    @staticmethod
    def has_double_timex(comment: str) -> bool:
        return comment == Constants.COMMENT_DOUBLETIMEX

    @staticmethod
    def process_double_timex(resolution_dict: Dict, future_key: str, past_key: str, origin_timex:str) -> Dict:
        timexes = origin_timex.split(Constants.COMPOSTIE_TIMEX_DELIMITER)
        if future_key not in resolution_dict or past_key not in resolution_dict or len(timexes) != 2:
            return resolution_dict

        future_resolution = resolution_dict[future_key]
        past_resolution = resolution_dict[past_key]
        future_resolution[DateTimeResolutionKey.timex] = timexes[0]
        past_resolution[DateTimeResolutionKey.timex] = timexes[1]


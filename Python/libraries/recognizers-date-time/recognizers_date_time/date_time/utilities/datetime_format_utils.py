from typing import List, Pattern, Union
from datetime import datetime
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.utilities import DateTimeResolutionResult


class DateTimeFormatUtil:
    HourTimeRegex = RegExpUtility.get_safe_reg_exp(r'(?<!P)T\d{2}')

    @staticmethod
    def to_str(num: Union[int, float], size: int) -> str:
        format_ = f'{{0:0{size}d}}'
        return str.format(format_, num)

    @staticmethod
    def luis_date(year: int, month: int, day: int) -> str:
        if year == -1:
            if month == -1:
                return f'XXXX-XX-{day:02d}'
            return f'XXXX-{month:02d}-{day:02d}'
        return f'{year:04d}-{month:02d}-{day:02d}'

    @staticmethod
    def luis_date_from_datetime(date: datetime) -> str:
        return DateTimeFormatUtil.luis_date(date.year, date.month, date.day)

    @staticmethod
    def luis_date_from_datetime_with_alternative(date: datetime, alternative_date: datetime = None) -> str:
        year = date.year
        month = date.month
        day = date.day

        if alternative_date:
            if alternative_date.year != year:
                year = -1
            if alternative_date.month != month:
                month = -1
            if alternative_date.day != day:
                day = -1

        return DateTimeFormatUtil.luis_date(year, month, day)

    @staticmethod
    def luis_time(hour: int, minute: int, second: int = Constants.INVALID_SECOND) -> str:
        if second == Constants.INVALID_SECOND:
            return f'{hour:02d}:{minute:02d}'
        else:
            return f'{hour:02d}:{minute:02d}:{second:02d}'

    @staticmethod
    def luis_time_from_datetime(time: datetime) -> str:
        return DateTimeFormatUtil.luis_time(time.hour, time.minute, time.second)

    @staticmethod
    def luis_date_time(time: datetime) -> str:
        return DateTimeFormatUtil.luis_date_from_datetime(time) + 'T' + DateTimeFormatUtil.luis_time_from_datetime(time)

    @staticmethod
    def luis_date_short_time(time: datetime, timex: str = None) -> str:
        has_min = False if timex is None else Constants.TIME_TIMEX_CONNECTOR in timex
        has_sec = False if timex is None else len(timex.split(Constants.TIME_TIMEX_CONNECTOR)) > 2

        return DateTimeFormatUtil.luis_date_from_datetime(time) + DateTimeFormatUtil.format_short_time(time, has_min,
                                                                                                       has_sec)

    @staticmethod
    def format_short_time(time: datetime, has_min: bool = False, has_sec: bool = False) -> str:
        hour = time.hour
        min = time.minute if has_min or time.minute > 0 else Constants.INVALID_MINUTE
        sec = time.second if has_sec or time.second > 0 else Constants.INVALID_SECOND
        return DateTimeFormatUtil.short_time(hour, min, sec)

    @staticmethod
    def short_time(hour: int, minute: int = Constants.INVALID_MINUTE, second: int = Constants.INVALID_SECOND) -> str:
        if minute == Constants.INVALID_MINUTE and second == Constants.INVALID_SECOND:
            return f'{Constants.TIME_TIMEX_PREFIX}{hour:02d}'
        else:
            return f'{Constants.TIME_TIMEX_PREFIX}{DateTimeFormatUtil.luis_time(hour, minute, second)}'

    @staticmethod
    def luis_time_span(begin_time: datetime, end_time: datetime) -> str:
        timex_builder = f'{Constants.GENERAL_PERIOD_PREFIX}{Constants.TIME_TIMEX_PREFIX}'
        span = end_time - begin_time
        total_days = span.days
        total_seconds = span.seconds
        total_hours, total_seconds = divmod(total_seconds, Constants.HOUR_SECOND_COUNT)
        total_minutes, total_seconds = divmod(total_seconds, Constants.MINUTE_SECOND_COUNT)

        if total_days > 0 or total_hours > 0:
            timex_builder += f'{total_days * Constants.DAY_HOUR_COUNT + total_hours}H'
        if total_minutes > 0:
            timex_builder += f'{total_minutes}M'
        if total_seconds > 0:
            timex_builder += f'{total_seconds}S'

        return str(timex_builder)

    @staticmethod
    def format_date(date: datetime) -> str:
        return f'{date.year:04d}-{date.month:02d}-{date.day:02d}'

    @staticmethod
    def format_time(time: datetime) -> str:
        return f'{time.hour:02d}:{time.minute:02d}:{time.second:02d}'

    @staticmethod
    def format_date_time(date_time: datetime) -> str:
        return DateTimeFormatUtil.format_date(date_time) + ' ' + DateTimeFormatUtil.format_time(date_time)

    @staticmethod
    def all_str_to_pm(source: str) -> str:
        matches = list(regex.finditer(
            DateTimeFormatUtil.HourTimeRegex, source))
        split: List[str] = list()
        last_position = 0

        for match in matches:
            if last_position != match.start():
                split.append(source[last_position:match.start()])

            split.append(source[match.start():match.end()])
            last_position = match.end()

        if source[:last_position]:
            split.append(source[last_position:])

        for index, value in enumerate(split):
            if regex.search(DateTimeFormatUtil.HourTimeRegex, value):
                split[index] = DateTimeFormatUtil.to_pm(value)

        return ''.join(split)

    @staticmethod
    def to_pm(source: str) -> str:
        result = ''

        if source.startswith(Constants.UNIT_T):
            result = Constants.UNIT_T
            source = source[1:]

        split = source.split(':')
        hour = int(split[0])
        hour = 0 if hour == 12 else hour + 12
        split[0] = f'{hour:02d}'
        return result + ':'.join(split)

    @staticmethod
    def parse_dynasty_year(year_str: str, dynasty_year_regex: Pattern, dynasty_start_year: str,
                           dynasty_year_map: dict, integer_extractor, number_parser):
        dynasty_year_match = regex.search(dynasty_year_regex, year_str)
        if dynasty_year_match and dynasty_year_match.start() == 0 and len(dynasty_year_match.group()) == len(year_str):
            # handle "康熙元年" refer to https://zh.wikipedia.org/wiki/%E5%B9%B4%E5%8F%B7
            dynasty_str = RegExpUtility.get_group(dynasty_year_match, "dynasty")
            bias_year_str = RegExpUtility.get_group(dynasty_year_match, "biasYear")
            basic_year = dynasty_year_map[dynasty_str]
            if bias_year_str == dynasty_start_year:
                bias_year = 1
            else:
                er = next(iter(integer_extractor.extract(bias_year_str)), None)
                bias_year = int(number_parser.parse(er).value)
            year = int(basic_year + bias_year - 1)
            return year
        return None

    @staticmethod
    def resolve_end_of_day(timex_prefix: str, future_date: datetime, past_date: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        result.timex = timex_prefix + "T23:59:59"
        result.future_value = future_date
        result.past_value = past_date

        return result

    @staticmethod
    def to_iso_week_timex(date: datetime) -> str:
        year, week_num = date.isocalendar()[:2]
        return f"{year}-W{week_num:02}"

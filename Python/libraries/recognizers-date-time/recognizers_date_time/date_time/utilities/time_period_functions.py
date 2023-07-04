from typing import Match
from datetime import datetime, timedelta

from recognizers_text import ExtractResult

from ..constants import Constants
from recognizers_date_time.date_time.utilities import DateTimeResolutionResult, DateUtils, TimeFunctions, \
    DateTimeExtra, TimeResult
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.data_structures import PeriodType


class TimePeriodFunctions:

    @staticmethod
    def handle(time_parser: DateTimeParser, extra: DateTimeExtra, reference: datetime,
               time_func: TimeFunctions) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        # Left is a time
        left_entity = next(iter(extra.named_entity['left']), '')

        #  下午四点十分到五点十分
        if extra.data_type == PeriodType.FullTime:
            left_result = TimePeriodFunctions.get_parse_time_result(
                left_entity, extra.match, reference, time_parser)
        else:
            #  下午四到五点
            left_result = time_func.get_short_left(left_entity)

        # Right is a time
        right_entity = next(iter(extra.named_entity['right']), '')
        right_result = TimePeriodFunctions.get_parse_time_result(
            right_entity, extra.match, reference, time_parser)


        span_hour = right_result.hour - left_result.hour
        if span_hour < 0 or (span_hour == 0 and left_result.minute > right_result.minute):
            span_hour += Constants.DAY_HOUR_COUNT

        # the right side doesn't contain desc while the left side does
        if right_result.low_bound == -1 and \
                left_result.low_bound != -1 and \
                right_result.hour <= Constants.HALF_DAY_HOUR_COUNT and \
                span_hour > Constants.HALF_DAY_HOUR_COUNT:
            right_result.hour += Constants.HALF_DAY_HOUR_COUNT

        # the left side doesn't contain desc while the right side does
        if left_result.low_bound == -1 and \
                right_result.low_bound != -1 and \
                left_result.hour <= Constants.HALF_DAY_HOUR_COUNT and \
                span_hour > Constants.HALF_DAY_HOUR_COUNT:
            left_result.hour += Constants.HALF_DAY_HOUR_COUNT

        #  No 'am' or 'pm' indicator
        if left_result.low_bound == right_result.low_bound == -1 and \
            left_result.hour <= Constants.HALF_DAY_HOUR_COUNT and \
            right_result.hour <= Constants.HALF_DAY_HOUR_COUNT:

            if span_hour > Constants.HALF_DAY_HOUR_COUNT:
                if left_result.hour > right_result.hour:
                    if left_result.hour == Constants.HALF_DAY_HOUR_COUNT:
                        left_result.hour -= Constants.HALF_DAY_HOUR_COUNT
                    else:
                        right_result.hour += Constants.HALF_DAY_HOUR_COUNT
            result.comment = Constants.COMMENT_AMPM

        day = reference.day
        month = reference.month
        year = reference.year
        right_swift_day = 0
        left_swift_day = 0

        #  determine if the left side time is smaller than the right side, if yes, add one day
        hour = left_result.hour if left_result.hour > 0 else 0
        minute = left_result.minute if left_result.minute > 0 else 0
        second = left_result.second if left_result.second > 0 else 0

        #  handle cases with time like 25時 which resolve to the next day
        if hour > Constants.DAY_HOUR_COUNT:
            hour -= Constants.DAY_HOUR_COUNT
            left_swift_day += 1

        left_time = DateUtils.safe_create_from_min_value(year, month, day, hour, minute, second)

        hour = right_result.hour if right_result.hour > 0 else 0
        minute = right_result.minute if right_result.minute > 0 else 0
        second = right_result.second if right_result.second > 0 else 0

        # handle cases with time like 25時 which resolve to the next day
        if hour > Constants.DAY_HOUR_COUNT:
            hour -= Constants.DAY_HOUR_COUNT
            right_swift_day += 1

        right_time = DateUtils.safe_create_from_min_value(reference.year, reference.month, reference.day,
                                                          hour, minute, second)

        if right_result.hour < left_result.hour:
            right_time += timedelta(days=1)

        left_timex = TimePeriodFunctions.build_timex(left_result)
        right_timex = TimePeriodFunctions.build_timex(right_result)
        span_timex = TimePeriodFunctions.build_span(left_result, right_result)

        result.timex = f'({left_timex},{right_timex},{span_timex})'

        right_time += timedelta(days=right_swift_day)
        left_time += timedelta(days=left_swift_day)

        result.future_value = [left_time, right_time]
        result.past_value = [left_time, right_time]
        result.success = True

        return result

    @staticmethod
    def build_timex(time_result: TimeResult) -> str:
        timex = 'T'
        if time_result.hour >= 0:
            timex = f'{timex}{time_result.hour:02d}'
        if time_result.minute >= 0:
            timex = f'{timex}:{time_result.minute:02d}'
        if time_result.second >= 0:
            timex = f'{timex}:{time_result.second:02d}'

        return timex

    @staticmethod
    def build_span(left: TimeResult, right: TimeResult) -> str:
        left = TimePeriodFunctions.sanitize_time_result(left)
        right = TimePeriodFunctions.sanitize_time_result(right)

        span_hour = right.hour - left.hour
        span_min = right.minute - left.minute
        span_sec = right.second - left.second

        if span_sec < 0:
            span_sec += 60
            span_min -= 1

        if span_min < 0:
            span_min += 60
            span_hour -= 1

        if span_hour < 0:
            span_hour += 24

        span_timex = f'PT'
        if span_hour > 0:
            span_timex += f'{span_hour}H'
        if span_min > 0:
            span_timex += f'{span_min}M'
            if span_sec > 0:
                span_timex += f'{span_sec}S'

        return span_timex

    @staticmethod
    def sanitize_time_result(source: TimeResult) -> TimeResult:
        return TimeResult(
            source.hour,
            0 if source.minute == -1 else source.minute,
            0 if source.second == -1 else source.second)

    @staticmethod
    def get_parse_time_result(entity: str, match: Match, reference: datetime,
                              time_parser: DateTimeParser) -> TimeResult:
        match_str: str = match.group()

        extract_result = ExtractResult()
        extract_result.start = match.start() + match_str.find(entity)
        extract_result.length = len(entity)
        extract_result.text = entity
        extract_result.type = Constants.SYS_DATETIME_TIME

        result = time_parser.parse(extract_result, reference)
        return result.data

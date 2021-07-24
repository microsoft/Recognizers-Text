#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Optional, Match
from datetime import datetime, timedelta
import regex

from recognizers_text import RegExpUtility, ExtractResult

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import Constants
from ..parsers import DateTimeParseResult
from ..utilities import TimeTypeConstants, DateTimeFormatUtil, DateTimeResolutionResult, DateUtils, TimexUtil
from ..base_timeperiod import BaseTimePeriodParser
from .base_date_time_extractor import DateTimeExtra, TimeResult, TimeResolutionUtils
from .timeperiod_extractor import TimePeriodType
from .timeperiod_parser_config import ChineseTimePeriodParserConfiguration


class ChineseTimePeriodParser(BaseTimePeriodParser):
    def __init__(self):
        super().__init__(ChineseTimePeriodParserConfiguration())
        self.day_description_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.TimeDayDescRegex)
        self.only_digit_match = RegExpUtility.get_safe_reg_exp(r'\d+')
        self.numbers_map = ChineseDateTime.TimeNumberDictionary
        self.low_bound_map = ChineseDateTime.TimeLowBoundDesc

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult(source)
        extra: DateTimeExtra = source.data

        if not extra:
            return result

        if source.type is self.parser_type_name:
            inner_result = self.parse_chinese_time_of_day(
                source.text, reference)

            if inner_result.success is False:
                inner_result = self.parse_time_period(extra, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.START_TIME] = DateTimeFormatUtil.format_time(
                    inner_result.future_value[0])
                inner_result.future_resolution[TimeTypeConstants.END_TIME] = DateTimeFormatUtil.format_time(
                    inner_result.future_value[1])
                inner_result.past_resolution[TimeTypeConstants.START_TIME] = DateTimeFormatUtil.format_time(
                    inner_result.past_value[0])
                inner_result.past_resolution[TimeTypeConstants.END_TIME] = DateTimeFormatUtil.format_time(
                    inner_result.past_value[1])

                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    def parse_chinese_time_of_day(self, text: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        day = reference.day
        month = reference.month
        year = reference.year

        parameters = self.get_matched_timex_range(text)
        if parameters['matched'] is False:
            return DateTimeResolutionResult()

        result.timex = parameters['timex']
        result.future_value = result.past_value = [
            DateUtils.safe_create_from_min_value(
                year, month, day, parameters['begin_hour'], 0, 0),
            DateUtils.safe_create_from_min_value(
                year, month, day, parameters['end_hour'], parameters['end_min'], 0)
        ]

        result.success = True
        return result

    def get_matched_timex_range(self, text: str) -> dict:
        trimmed_text = text.strip()
        begin_hour = 0
        end_hour = 0
        end_min = 0

        time_of_day = ""
        if any(trimmed_text.endswith(o) for o in ChineseDateTime.MorningTermList):
            time_of_day = Constants.MORNING
        elif any(trimmed_text.endswith(o) for o in ChineseDateTime.MidDayTermList):
            time_of_day = Constants.MID_DAY
        elif any(trimmed_text.endswith(o) for o in ChineseDateTime.AfternoonTermList):
            time_of_day = Constants.AFTERNOON
        elif any(trimmed_text.endswith(o) for o in ChineseDateTime.EveningTermList):
            time_of_day = Constants.EVENING
        elif any(trimmed_text == o for o in ChineseDateTime.DaytimeTermList):
            time_of_day = Constants.DAYTIME
        elif any(trimmed_text.endswith(o) for o in ChineseDateTime.NightTermList):
            time_of_day = Constants.NIGHT
        else:
            timex = None
            matched = False

            return {'matched': matched, 'timex': timex, 'begin_hour': begin_hour,
                    'end_hour': end_hour, 'end_min': end_min}

        parse_result = TimexUtil.parse_time_of_day(time_of_day)
        timex = parse_result.timex
        begin_hour = parse_result.begin_hour
        end_hour = parse_result.end_hour
        end_min = parse_result.end_min

        matched = True
        return {'matched': matched, 'timex': timex, 'begin_hour': begin_hour,
                'end_hour': end_hour, 'end_min': end_min}

    def parse_time_period(self, extra: DateTimeExtra, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        left_entity = next(iter(extra.named_entity['left']), '')
        left_result: TimeResult = None
        if extra.data_type == TimePeriodType.FullTime:
            left_result = self.get_parse_time_result(
                left_entity, extra.match, reference)
        else:
            left_result = self.get_short_left(left_entity)

        right_entity = next(iter(extra.named_entity['right']), '')
        right_result = self.get_parse_time_result(
            right_entity, extra.match, reference)

        # the right side doesn't contain desc while the left side does
        if right_result.low_bound == -1 and left_result.low_bound != -1 and right_result.hour <= left_result.low_bound:
            right_result.hour += 12

        left_date = self.build_date(left_result, reference)
        right_date = self.build_date(right_result, reference)

        if right_date.hour < left_date.hour:
            right_date += timedelta(days=1)

        result.future_value = [left_date, right_date]
        result.past_value = [left_date, right_date]

        left_timex = self.build_timex(left_result)
        right_timex = self.build_timex(right_result)
        span_timex = self.build_span(left_result, right_result)

        result.timex = f'({left_timex},{right_timex},{span_timex})'
        result.success = True

        return result

    def get_parse_time_result(self, entity: str, match: Match, reference: datetime) -> TimeResult:
        match_str: str = match.group()

        extract_result = ExtractResult()
        extract_result.start = match.start() + match_str.find(entity)
        extract_result.length = len(entity)
        extract_result.text = entity
        extract_result.type = Constants.SYS_DATETIME_TIME

        result = self.config.time_parser.parse(extract_result, reference)
        return result.data

    def get_short_left(self, source: str) -> TimeResult:
        description: str = ''
        if regex.match(self.day_description_regex, source):
            description = source[:-1]

        hour = TimeResolutionUtils.match_to_value(
            self.only_digit_match, self.numbers_map, source[-1])
        time_result = TimeResult(hour, -1, -1)
        TimeResolutionUtils.add_description(
            time_result, self.low_bound_map, description)
        return time_result

    def build_date(self, time: TimeResult, reference: datetime) -> datetime:
        hour = self.__min_with_floor(time.hour)
        minute = self.__min_with_floor(time.minute)
        second = self.__min_with_floor(time.second)
        return DateUtils.safe_create_from_min_value(reference.year, reference.month, reference.day, hour, minute, second)

    def __min_with_floor(self, value: int) -> int:
        return value if value > 0 else 0

    def build_timex(self, time_result: TimeResult) -> str:
        timex = 'T'
        if time_result.hour >= 0:
            timex += f'{time_result.hour:02d}'
            if time_result.minute >= 0:
                timex += f':{time_result.minute:02d}'
                if time_result.second >= 0:
                    timex += f':{time_result.second:02d}'

        return timex

    def build_span(self, left: TimeResult, right: TimeResult) -> str:
        left = self.sanitize_time_result(left)
        right = self.sanitize_time_result(right)

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
        if span_hour != 0:
            span_timex += f'{span_hour}H'
        if span_min != 0:
            span_timex += f'{span_min}M'
            if span_sec != 0:
                span_timex += f'{span_sec}S'

        return span_timex

    def sanitize_time_result(self, source: TimeResult) -> TimeResult:
        return TimeResult(
            source.hour,
            0 if source.minute == -1 else source.minute,
            0 if source.second == -1 else source.second)

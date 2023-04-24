#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Optional
from datetime import datetime, timedelta

from recognizers_text import RegExpUtility

from ...resources.japanese_date_time import JapaneseDateTime
from ..constants import TimeTypeConstants, Constants
from ..utilities import DateTimeFormatUtil, DateTimeResolutionResult, DateUtils
from ..extractors import ExtractResult
from ..parsers import DateTimeParseResult
from ..base_time import BaseTimeParser
from .base_date_time_extractor import DateTimeExtra, TimeResult, TimeResolutionUtils
from .time_extractor import JapaneseTimeExtractor, TimeType


class JapaneseTimeParser(BaseTimeParser):
    def __init__(self):
        super().__init__(None)
        self.only_digit_match = RegExpUtility.get_safe_reg_exp('\\d+')
        self.numbers_map = JapaneseDateTime.TimeNumberDictionary
        self.low_bound_map = JapaneseDateTime.TimeLowBoundDesc
        self.function_map = {
            TimeType.JapaneseTime: self.handle_japanese,
            TimeType.DigitTime: self.handle_digit,
            TimeType.LessTime: self.handle_less
        }
        self.inner_extractor = JapaneseTimeExtractor()

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        extra: DateTimeExtra = source.data

        if not extra:
            inner_result = next(iter(self.inner_extractor.extract(
                source.text, reference)), ExtractResult())
            extra = inner_result.data

        time_result = self.function_map[extra.data_type](extra)
        parse_result = self.pack_time_result(extra, time_result, reference)

        if parse_result.success:
            parse_result.future_resolution[TimeTypeConstants.TIME] = DateTimeFormatUtil.format_time(
                parse_result.future_value)
            parse_result.past_resolution[TimeTypeConstants.TIME] = DateTimeFormatUtil.format_time(
                parse_result.past_value)

        result = DateTimeParseResult(source)
        result.value = parse_result
        result.data = time_result
        result.timex_str = parse_result.timex if parse_result is not None else ''
        result.resolution_str = ''

        return result

    def handle_less(self, extra: DateTimeExtra) -> TimeResult:
        hour = self.match_to_value(next(iter(extra.named_entity['hour']), ''))
        quarter = self.match_to_value(
            next(iter(extra.named_entity['quarter']), ''))
        has_half = next(iter(extra.named_entity['half']), '') == ''
        minute = 30 if not has_half else quarter * 15 if quarter != -1 else 0
        second = self.match_to_value(next(iter(extra.named_entity['sec']), ''))
        less = self.match_to_value(next(iter(extra.named_entity['min']), ''))

        _all = hour * 60 + minute - less
        if _all < 0:
            _all = _all + 1440

        return TimeResult(_all / 60, _all % 60, second)

    def handle_digit(self, extra: DateTimeExtra) -> TimeResult:
        hour = self.match_to_value(next(iter(extra.named_entity['hour']), ''))
        minute = self.match_to_value(next(iter(extra.named_entity['min']), ''))
        second = self.match_to_value(next(iter(extra.named_entity['sec']), ''))

        return TimeResult(hour, minute, second)

    def handle_japanese(self, extra: DateTimeExtra) -> TimeResult:
        hour = self.match_to_value(next(iter(extra.named_entity['hour']), ''))
        quarter = self.match_to_value(
            next(iter(extra.named_entity['quarter']), ''))
        has_half = next(iter(extra.named_entity['half']), '') == ''
        minute = 30 if not has_half else quarter * 15 if quarter != - \
            1 else self.match_to_value(next(iter(extra.named_entity['min']), ''))
        second = self.match_to_value(next(iter(extra.named_entity['sec']), ''))

        return TimeResult(hour, minute, second)

    def pack_time_result(self, extra: DateTimeExtra, time_result: TimeResult,
                         reference_time: datetime) -> DateTimeResolutionResult:
        date_time_result = DateTimeResolutionResult()

        day_description = next(iter(extra.named_entity['daydesc']), '')
        no_desc = day_description.strip() == ''

        if no_desc:
            date_time_result.comment = 'ampm'
        else:
            self.add_description(time_result, day_description)

        hour = time_result.hour if (time_result.hour > 0 and time_result.hour != Constants.DAY_HOUR_COUNT) else 0
        minute = self._min_with_floor(time_result.minute)
        second = self._min_with_floor(time_result.second)

        print(f"------ hour {type(hour)} - {type(time_result.hour)} - {hour} - {time_result.hour}")
        print(f"------ minute {type(minute)} - {type(time_result.minute)} - {minute} - {time_result.minute}")
        print(f"------ second {type(second)} - {type(time_result.second)} - {second} - {time_result.second}")

        timex = 'T'

        if time_result.hour >= 0:
            timex = f'{timex}{hour:02d}'
            if time_result.minute >= 0:
                timex = f'{timex}:{minute:02d}'
                if time_result.second >= 0:
                    if time_result.minute < 0:
                        timex = f'{timex}:{minute:02d}'
                    timex = f'{timex}:{second:02d}'

        if hour == Constants.DAY_HOUR_COUNT:
            hour = 0

        # handle cases with time like 25時 (the hour is normalized in the past/future values)
        if time_result.hour > Constants.DAY_HOUR_COUNT:
            hour = time_result.hour - Constants.DAY_HOUR_COUNT
            reference_time = reference_time + timedelta(days=1)
            if no_desc:
                date_time_result.comment = Constants.COMMENT_AM
                no_desc = False

        if no_desc and (hour <= Constants.HALF_DAY_HOUR_COUNT) and (hour > Constants.DAY_HOUR_COUNT):
            date_time_result.comment = Constants.COMMENT_AMPM

        day = reference_time.day
        month = reference_time.month
        year = reference_time.year

        date_time_result.future_value = DateUtils.safe_create_from_min_value(
            year, month, day, hour, minute, second)
        date_time_result.past_value = DateUtils.safe_create_from_min_value(
            year, month, day, hour, minute, second)
        date_time_result.timex = timex
        date_time_result.success = True

        return date_time_result

    def _min_with_floor(self, source: int) -> int:
        return source if source > 0 else 0

    def match_to_value(self, source: str) -> int:
        return TimeResolutionUtils.match_to_value(self.only_digit_match, self.numbers_map, source)

    def add_description(self, time_result: TimeResult, description: str):
        TimeResolutionUtils.add_description(
            time_result, self.low_bound_map, description)

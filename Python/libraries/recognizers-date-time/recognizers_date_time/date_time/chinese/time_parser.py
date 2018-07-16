from typing import Optional
from datetime import datetime

from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import TimeTypeConstants
from ..utilities import FormatUtil, DateTimeResolutionResult, DateUtils
from ..extractors import ExtractResult
from ..parsers import DateTimeParseResult
from ..base_time import BaseTimeParser
from .base_date_time_extractor import DateTimeExtra, TimeResult, TimeResolutionUtils
from .time_extractor import ChineseTimeExtractor, TimeType

class ChineseTimeParser(BaseTimeParser):
    def __init__(self):
        super().__init__(None)
        self.only_digit_match = RegExpUtility.get_safe_reg_exp('\\d+')
        self.numbers_map = ChineseDateTime.TimeNumberDictionary
        self.low_bound_map = ChineseDateTime.TimeLowBoundDesc
        self.function_map = {
            TimeType.ChineseTime: self.handle_chinese,
            TimeType.DigitTime: self.handle_digit,
            TimeType.LessTime: self.handle_less
        }
        self.inner_extractor = ChineseTimeExtractor()

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        extra: DateTimeExtra = source.data

        if not extra:
            inner_result = next(iter(self.inner_extractor.extract(source.text, reference)), ExtractResult())
            extra = inner_result.data

        time_result = self.function_map[extra.data_type](extra)
        parse_result = self.pack_time_result(extra, time_result, reference)

        if parse_result.success:
            parse_result.future_resolution[TimeTypeConstants.TIME] = FormatUtil.format_time(parse_result.future_value)
            parse_result.past_resolution[TimeTypeConstants.TIME] = FormatUtil.format_time(parse_result.past_value)

        result = DateTimeParseResult(source)
        result.value = parse_result
        result.data = time_result
        result.timex_str = parse_result.timex if parse_result is not None else ''
        result.resolution_str = ''

        return result

    def handle_less(self, extra: DateTimeExtra) -> TimeResult:
        hour = self.match_to_value(next(iter(extra.named_entity['hour']), ''))
        quarter = self.match_to_value(next(iter(extra.named_entity['quarter']), ''))
        has_half = next(iter(extra.named_entity['half']), '') == ''
        minute = 30 if not has_half else quarter * 15 if quarter != -1 else 0
        second = self.match_to_value(next(iter(extra.named_entity['sec']), ''))
        less = self.match_to_value(next(iter(extra.named_entity['min']), ''))

        _all = hour * 60 + minute - less
        if _all < 0:
            _all = _all + 1440

        return TimeResult(_all / 60, _all % 60, second)

    def handle_digit(self, extra: DateTimeExtra) -> TimeResult:
        print(extra.named_entity)
        hour = self.match_to_value(next(iter(extra.named_entity['hour']), ''))
        minute = self.match_to_value(next(iter(extra.named_entity['min']), ''))
        second = self.match_to_value(next(iter(extra.named_entity['sec']), ''))

        return TimeResult(hour, minute, second)

    def handle_chinese(self, extra: DateTimeExtra) -> TimeResult:
        hour = self.match_to_value(next(iter(extra.named_entity['hour']), ''))
        quarter = self.match_to_value(next(iter(extra.named_entity['quarter']), ''))
        has_half = next(iter(extra.named_entity['half']), '') == ''
        minute = 30 if not has_half else quarter * 15 if quarter != -1 else self.match_to_value(next(iter(extra.named_entity['min']), ''))
        second = self.match_to_value(next(iter(extra.named_entity['sec']), ''))

        return TimeResult(hour, minute, second)

    def pack_time_result(self, extra: DateTimeExtra, time_result: TimeResult, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        day_description = next(iter(extra.named_entity['daydesc']), '')
        no_desc = day_description.strip() == ''

        if no_desc:
            result.comment = 'ampm'
        else:
            self.add_description(time_result, day_description)

        hour = self._min_with_floor(time_result.hour)
        minute = self._min_with_floor(time_result.minute)
        second = self._min_with_floor(time_result.second)

        day = reference.day
        month = reference.month
        year = reference.year

        timex = 'T'
        if time_result.hour >= 0:
            timex = f'{timex}{time_result.hour:02d}'
            if time_result.minute >= 0:
                timex = f'{timex}:{time_result.minute:02d}'
                if time_result.second >= 0:
                    timex = f'{timex}:{time_result.second:02d}'

        if hour == 24:
            hour = 0

        result.future_value = DateUtils.safe_create_from_min_value(year, month, day, hour, minute, second)
        result.past_value = DateUtils.safe_create_from_min_value(year, month, day, hour, minute, second)
        result.timex = timex
        result.success = True

        return result

    def _min_with_floor(self, source: int) -> int:
        return source if source > 0 else 0

    def match_to_value(self, source: str) -> int:
        return TimeResolutionUtils.match_to_value(self.only_digit_match, self.numbers_map, source)

    def add_description(self, time_result: TimeResult, description: str):
        TimeResolutionUtils.add_description(time_result, self.low_bound_map, description)

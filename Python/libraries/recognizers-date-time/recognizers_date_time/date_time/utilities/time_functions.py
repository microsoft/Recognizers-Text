from datetime import datetime
from typing import Dict, Pattern
import regex

from recognizers_text import RegExpUtility
from recognizers_date_time.date_time.utilities import DateTimeResolutionResult, DateUtils, TimeResult, DateTimeExtra
from recognizers_date_time.date_time.constants import Constants


class TimeFunctions:
    def __init__(self, number_dictionary: Dict[str,int], low_bound_desc: Dict[str, int], day_desc_regex: Pattern):
        self.number_dictionary = number_dictionary
        self.low_bound_desc = low_bound_desc
        self.day_desc_regex = day_desc_regex
        self.only_digit_match = RegExpUtility.get_safe_reg_exp('\\d+')

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

        return TimeResult(int(_all / 60), _all % 60, second)

    def handle_kanji(self, extra: DateTimeExtra) -> TimeResult:
        hour = self.match_to_value(next(iter(extra.named_entity['hour']), ''))
        quarter = self.match_to_value(next(iter(extra.named_entity['quarter']), ''))
        has_half = next(iter(extra.named_entity['half']), '') == ''
        minute = 30 if not has_half else quarter * 15 if quarter != - \
            1 else self.match_to_value(next(iter(extra.named_entity['min']), ''))
        second = self.match_to_value(next(iter(extra.named_entity['sec']), ''))

        return TimeResult(hour, minute, second)

    def handle_digit(self, extra: DateTimeExtra) -> TimeResult:
        hour = self.match_to_value(next(iter(extra.named_entity['hour']), ''))
        minute = self.match_to_value(next(iter(extra.named_entity['min']), ''))
        second = self.match_to_value(next(iter(extra.named_entity['sec']), ''))

        return TimeResult(hour, minute, second)

    def pack_time_result(self, extra: DateTimeExtra, time_result: TimeResult,
                         reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        #  Find if there is a description
        day_desc = next(iter(extra.named_entity['daydesc']), '')
        no_desc = True

        if day_desc:
            self.add_desc(time_result, day_desc)
            no_desc = False

        # Hours > 24 (e.g. 25時 which resolves to the next day) are kept unnormalized in the timex
        # to avoid ambiguity in other entities. For example, "on the 30th at 25" is resolved to
        # "XXXX-XX-30T25" because with "XXXX-XX-30+1T01" it is not known if the day should be "31" or "01".
        hour = self._min_with_floor(time_result.hour)
        if hour == Constants.DAY_HOUR_COUNT:
            hour = 0

        minute = self._min_with_floor(time_result.minute)
        second = self._min_with_floor(time_result.second)

        day = reference.day
        month = reference.month
        year = reference.year

        timex = 'T'
        if time_result.hour >= 0:
            timex = f'{timex}{hour:02d}'
        if time_result.minute >= 0:
            timex = f'{timex}:{minute:02d}'
        if time_result.second >= 0:
            if time_result.minute < 0:
                timex = f'{timex}:{minute:02d}'
            timex = f'{timex}:{second:02d}'

        # handle cases with time like 25時 (the hour is normalized in the past/future values)
        if hour > Constants.DAY_HOUR_COUNT:
            hour = time_result.hour - Constants.DAY_HOUR_COUNT
            if no_desc:
                result.comment = Constants.COMMENT_AM
                no_desc = False

        if no_desc and hour <= Constants.HALF_DAY_HOUR_COUNT and hour > Constants.DAY_HOUR_START:
            result.comment = Constants.COMMENT_AMPM

        result.future_value = DateUtils.safe_create_from_min_value(
            year, month, day, hour, minute, second)
        result.past_value = DateUtils.safe_create_from_min_value(
            year, month, day, hour, minute, second)
        result.timex = timex
        result.success = True

        return result

    #  Handle am/pm modifiers (e.g. "1 in the afternoon") and time of day (e.g. "mid-morning")
    def add_desc(self, result: TimeResult, day_desc: str):
        if not day_desc:
            return
        day_desc = self.normalise_day_desc(day_desc)

        if result.hour >= 0 and day_desc in self.low_bound_desc:
            if result.hour < self.low_bound_desc[day_desc] or (
                    result.hour == Constants.HALF_DAY_HOUR_COUNT and
                    self.low_bound_desc[day_desc] == Constants.DAY_HOUR_START):
                # cases like "1 in the afternoon", "12 midnight"
                result.hour += Constants.HALF_DAY_HOUR_COUNT
                result.low_bound = self.low_bound_desc[day_desc]

        elif result.hour < 0 and day_desc in self.low_bound_desc:
            # cases like "mid-morning", "mid-afternoon"
            result.low_bound = self.low_bound_desc[day_desc]
            result.hour = result.low_bound
        else:
            result.low_bound = 0

    def get_short_left(self, text: str) -> TimeResult:
        des = ""
        if regex.match(self.day_desc_regex, text):
            des = text[:-1]
        hour = self.match_to_value(text[-1])
        time_result = TimeResult(hour, -1, -1)
        self.add_desc(time_result, des)

        return time_result

    # Normalize cases like "p.m.", "p m" to canonical form "pm"
    def normalise_day_desc(self, day_desc: str):
        return day_desc.replace(" ", "").replace(".", "")

    def _min_with_floor(self, source: int) -> int:
        return source if source > 0 else 0

    def match_to_value(self, text: str) -> int:
        if not text.strip():
            return -1

        if regex.match(self.only_digit_match, text):
            return int(text)

        if len(text) == 1:
            return self.number_dictionary[text]

        value = 1
        for index, char in enumerate(text):
            if char == '十':
                value = value * 10
            elif index == 0:
                value = value * self.number_dictionary[char]
            else:
                value = value + self.number_dictionary[char]

        return value




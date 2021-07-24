#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List
from datetime import datetime
import regex

from recognizers_text import RegExpUtility, ExtractResult

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import Constants
from ..utilities import merge_all_tokens, Token
from ..base_datetimeperiod import BaseDateTimePeriodExtractor
from .datetimeperiod_extractor_config import ChineseDateTimePeriodExtractorConfiguration


class ChineseDateTimePeriodExtractor(BaseDateTimePeriodExtractor):
    def __init__(self):
        super().__init__(ChineseDateTimePeriodExtractorConfiguration())
        self.zhijian_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.ZhijianRegex)
        self.past_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.PastRegex)
        self.future_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.FutureRegex)

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.merge_date_and_time_period(source, reference))
        tokens.extend(self.merge_two_time_points(source, reference))
        tokens.extend(self.match_number_with_unit(source))
        tokens.extend(self.match_night(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def merge_date_and_time_period(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        extract_results_date = self.config.single_date_extractor.extract(source, reference)
        extract_results_time = self.config.single_time_extractor.extract(source, reference)
        time_results: List[ExtractResult] = list()
        j = 0
        for extract_result_date in extract_results_date:
            time_results.append(extract_result_date)

            while j < len(extract_results_time) and extract_results_time[j].start + extract_results_time[j].length\
                    <= extract_result_date.start:
                time_results.append(extract_results_time[j])
                j = j + 1

            while j < len(extract_results_time) and extract_results_time[j].overlap(extract_result_date):
                j = j + 1

        time_results += extract_results_time[j:]

        sorted(time_results, key=lambda x: x.start)

        index = 0
        while index < len(time_results) - 1:
            current = time_results[index]
            next_val = time_results[index + 1]
            if current.type == Constants.SYS_DATETIME_DATE and next_val.type == Constants.SYS_DATETIME_TIMEPERIOD:
                middle_begin = current.start + current.length
                middle_end = next_val.start
                middle_str = source[middle_begin:middle_end].strip()
                if not middle_str or regex.search(self.config.preposition_regex, middle_str):
                    period_begin = current.start
                    period_end = next_val.start + next_val.length
                    tokens.append(Token(period_begin, period_end))
                index = index + 1
            index = index + 1

        return tokens

    def merge_two_time_points(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()
        extract_results_datetime: List[ExtractResult] = self.config.single_date_time_extractor.extract(
            source, reference)
        extract_results_time: List[ExtractResult] = self.config.single_time_extractor.extract(
            source, reference)
        inner_marks: List[ExtractResult] = list()

        j = 0

        for er_datetime in extract_results_datetime:
            inner_marks.append(er_datetime)

            while j < len(extract_results_time) and extract_results_time[j].start + extract_results_time[j].length\
                    < er_datetime.start:
                inner_marks.append(extract_results_time[j])
                j += 1

            while j < len(extract_results_time) and extract_results_time[j].overlap(er_datetime):
                j += 1

        while j < len(extract_results_time):
            inner_marks.append(extract_results_time[j])
            j += 1
        inner_marks = sorted(inner_marks, key=lambda x: x.start)

        index = 0
        ceil = len(inner_marks) - 1

        while index < ceil:
            current_mark = inner_marks[index]
            next_mark = inner_marks[index + 1]

            if current_mark.type == Constants.SYS_DATETIME_TIME and next_mark.type == Constants.SYS_DATETIME_TIME:
                index += 1
                continue

            middle_begin = current_mark.start + current_mark.length
            middle_end = next_mark.start

            middle_str = source[middle_begin:middle_end].strip()
            match = regex.search(self.config.till_regex, middle_str)

            if match and match.start() == 0 and match.group() == middle_str:
                period_begin = current_mark.start
                period_end = next_mark.start + next_mark.length
                before_str = source[0:period_begin].strip()
                from_token_index = self.config.get_from_token_index(before_str)
                if from_token_index.matched:
                    period_begin = from_token_index.index
                tokens.append(Token(period_begin, period_end))
                index += 2
                continue

            if self.config.has_connector_token(middle_str):
                period_begin = current_mark.start
                period_end = next_mark.start + next_mark.length
                after_str = source[period_end:].strip()
                match = regex.search(self.zhijian_regex, after_str)
                if match:
                    tokens.append(
                        Token(period_begin, period_end + len(match.group())))
                    index += 2
                    continue

            index += 1

        return tokens

    def match_number_with_unit(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        durations: List[Token] = list()

        for extract_result in self.config.cardinal_extractor.extract(source):
            after_str = source[extract_result.start + extract_result.length:]
            followed_unit_match = regex.search(
                self.config.followed_unit, after_str)

            if followed_unit_match and followed_unit_match.start() == 0:
                durations.append(Token(extract_result.start, extract_result.start +
                                       extract_result.length + len(followed_unit_match.group())))

        for match in regex.finditer(self.config.time_unit_regex, source):
            durations.append(Token(match.start(), match.end()))

        for duration in durations:
            before_str = source[:duration.start].lower()

            if not before_str.strip():
                continue

            match = regex.search(self.past_regex, before_str)

            if match and not before_str[match.end():].strip():
                tokens.append(Token(match.start(), duration.end))
                continue

            match = regex.search(self.future_regex, before_str)

            if match and not before_str[match.end():].strip():
                tokens.append(Token(match.start(), duration.end))

        return tokens

    def match_night(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()

        matches = regex.finditer(
            self.config.specific_time_of_day_regex, source)
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))

        ers_date: List[ExtractResult] = self.config.single_date_extractor.extract(
            source, reference)

        for er in ers_date:
            after_str = source[er.start + er.length:]
            match = regex.search(self.config.time_of_day_regex, after_str)

            if match:
                middle_str = source[0:match.start()]
                if not middle_str.strip() or regex.search(self.config.preposition_regex, middle_str):
                    tokens.append(
                        Token(er.start, er.start + er.length + match.end()))

        return tokens

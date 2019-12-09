from abc import abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime, timedelta
from collections import namedtuple
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_number import BaseNumberExtractor
from .base_date import BaseDateExtractor
from .base_time import BaseTimeExtractor
from .base_datetime import BaseDateTimeExtractor
from .base_duration import BaseDurationExtractor
from .base_timeperiod import BaseTimePeriodExtractor
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, RegExpUtility, DateTimeFormatUtil, DateTimeResolutionResult, \
    DateUtils, RegExpUtility, DateTimeOptionsConfiguration, DateTimeOptions, TimexUtil


class MatchedTimeRange:
    def __init__(self, time_str: str = '', begin_hour: int = 0, end_hour: int = 0, end_min: int = 0,
                 success: bool = False, swift: int = 0):
        self.time_str = time_str
        self.begin_hour = begin_hour
        self.end_hour = end_hour
        self.end_min = end_min
        self.swift = swift
        self.success = success


MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])
BeginEnd = namedtuple('BeginEnd', ['begin', 'end'])


class DateTimePeriodExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_date_extractor(self) -> BaseDateExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_time_extractor(self) -> BaseTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_date_time_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> BaseDurationExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> BaseTimePeriodExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_cases_regexes(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def preposition_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def till_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def specific_time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def period_time_of_day_with_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def followed_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_combined_with_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def previous_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def next_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_time_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def rest_of_date_time_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def general_ending_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def middle_pause_regex(self) -> Pattern:
        raise NotImplementedError

    @abstractmethod
    def get_from_token_index(self, source: str) -> MatchedIndex:
        raise NotImplementedError

    @abstractmethod
    def get_between_token_index(self, source: str) -> MatchedIndex:
        raise NotImplementedError

    @abstractmethod
    def has_connector_token(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def token_before_date(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def within_next_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def future_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def pm_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def prefix_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> Pattern:
        raise NotImplementedError


class BaseDateTimePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIMEPERIOD

    def __init__(self, config: DateTimePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        date_ers = self.config.single_date_extractor.extract(source, reference)
        time_ers = self.config.single_time_extractor.extract(source, reference)

        tokens: List[Token] = list()
        tokens.extend(self.match_simple_cases(source, reference))
        tokens.extend(self.merge_two_time_points(source, reference, date_ers, time_ers))
        tokens.extend(self.match_duration(source, reference))
        tokens.extend(self.match_time_of_day(source, reference, date_ers))
        tokens.extend(self.match_relative_unit(source))
        tokens.extend(self.match_date_with_period_prefix(source, reference, date_ers))
        tokens.extend(self.merge_date_with_time_period_suffix(source, date_ers, time_ers))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        if (self.config.options & DateTimeOptions.ENABLE_PREVIEW) != 0:
            # When TimeZone is migrated enable it
            pass

        return result

    def match_date_with_period_prefix(self, source, reference, date_ers):
        result = []

        for date_er in date_ers:
            date_str_end = date_er.start + date_er.length
            before_str = source[0:date_er.start].strip()
            match = regex.match(self.config.prefix_day_regex, before_str)
            if match:
                result.append(Token(match.pos, date_str_end))
        return result

    def merge_date_with_time_period_suffix(self, text: str, date_ers: [ExtractResult], time_ers: [ExtractResult]):
        result = []

        if not any(date_ers):
            return result

        if not any(time_ers):
            return result

        extracted_results: [Match] = date_ers
        extracted_results.extend(time_ers)

        ers = sorted(extracted_results, key=lambda x: x.start)

        i = 0

        while i < len(extracted_results) - 1:

            j = i + 1
            while j < len(extracted_results) and extracted_results[i].overlap(extracted_results[j]):
                j += 1

            if j >= len(extracted_results):
                break

            if extracted_results[i].type == Constants.SYS_DATETIME_DATE and\
                    extracted_results[j].type == Constants.SYS_DATETIME_TIME:
                middle_begin = extracted_results[i].start + (extracted_results[i].length or 0)
                middle_end = extracted_results[j].start or 0
                if middle_begin > middle_end:
                    i = j + 1
                    continue

                middle_str = text[middle_begin: middle_end].strip()

                if self.is_valid_connector_for_date_and_time_period(middle_str):
                    begin = ers[i].start or 0
                    end = (ers[j].start or 0) + (ers[j].length or 0)
                    result.append(Token(begin, end))

                i = j + 1
                continue

            i = j

        idx = 0
        for idx in range(idx, len(result), 1):
            after_str = text[result[idx].end]
            match = self.config.suffix_regex.search(after_str)
            if match:
                result[idx] = Token(result[idx].start, result[idx].end + (match.end() - match.start()))

        return result

    def is_valid_connector_for_date_and_time_period(self, text: str):
        before_after_regexes = [self.config.before_regex, self.config.after_regex]
        for regexp in before_after_regexes:

            if RegExpUtility.is_exact_match(regexp, text, True):
                return True

        return False

    def match_time_of_day(self, source: str, reference: datetime, date_ers: [ExtractResult] = None):
        tokens: List[Token] = list()
        source = source.strip().lower()

        matches = regex.finditer(
            self.config.specific_time_of_day_regex, source)
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))

        ers_date: List[ExtractResult] = self.config.single_date_extractor.extract(
            source, reference)

        for extracted_result in ers_date:
            after_str = source[extracted_result.start + extracted_result.length:]
            match = regex.search(
                self.config.period_time_of_day_with_date_regex, after_str)

            if match:
                if not after_str[0:match.start()].strip():
                    tokens.append(
                        Token(extracted_result.start, extracted_result.start + extracted_result.length + match.end()))
                else:
                    pause_match = regex.search(
                        self.config.middle_pause_regex, after_str[0:match.start()].strip())
                    if pause_match:
                        suffix = after_str[match.end():].strip()

                        ending_match = regex.search(
                            self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(
                                Token(extracted_result.start, extracted_result.start + extracted_result.length +
                                      match.end()))

            before_str = source[0:extracted_result.start]
            match = regex.search(
                self.config.period_time_of_day_with_date_regex, before_str)

            if match:
                if not before_str[match.end():].strip():
                    middle_str = source[match.end():extracted_result.start]
                    if middle_str == ' ':
                        tokens.append(
                            Token(match.start(), extracted_result.start + extracted_result.length))
                else:
                    pause_match = regex.search(
                        self.config.middle_pause_regex, before_str[match.end():])
                    if pause_match:
                        suffix = source[extracted_result.start + extracted_result.length:].strip()

                        ending_match = regex.search(
                            self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(
                                Token(match.start(), extracted_result.start + extracted_result.length))

            # check whether there are adjacent time period strings, before or after
            for token in tokens:
                # try to extract a time period in before-string
                if token.start > 0:
                    before_str = source[0:token.start].strip()
                    if before_str:
                        ers_time = self.config.time_period_extractor.extract(
                            before_str, reference)

                        for er_time in ers_time:
                            middle_str = before_str[er_time.start +
                                                    er_time.length:].strip()
                            if not middle_str:
                                tokens.append(Token(er_time.start,
                                                    er_time.start + er_time.length + len(middle_str) + token.length))

                if token.start + token.length <= len(source):
                    after_str = source[token.start + token.length:]
                    if after_str:
                        ers_time = self.config.time_period_extractor.extract(
                            after_str, reference)
                        for er_time in ers_time:
                            middle_str = after_str[0:er_time.start]
                            if not middle_str:
                                token_end = token.start + token.length + \
                                    len(middle_str) + er_time.length
                                tokens.append(Token(token.start, token_end))
        return tokens

    def match_simple_cases(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()
        simple_cases_matches = list(filter(lambda t: t != [], map(lambda x: list(
            regex.finditer(x, source)), self.config.simple_cases_regexes)))

        for matches in simple_cases_matches:
            for match in matches:
                # has a date before?
                has_before_date = False

                before_str = source[0:match.start()].strip()
                if before_str:
                    er = next(reversed(self.config.single_date_extractor.extract(
                        before_str, reference)), None)
                    if er:
                        begin = er.start
                        end = er.start + er.length
                        middle_str = before_str[end:].strip()
                        if middle_str == '' or regex.search(self.config.preposition_regex, middle_str):
                            tokens.append(Token(begin, match.end()))
                            has_before_date = True

                followed_str = source[match.end():]
                if followed_str and not has_before_date:
                    extracted_result = next(iter(self.config.single_date_extractor.extract(
                        followed_str, reference)), None)
                    if extracted_result:
                        begin = extracted_result.start
                        end = extracted_result.start + extracted_result.length
                        middle_str = followed_str[0:begin].strip()
                        if middle_str == '' or regex.search(self.config.preposition_regex, middle_str):
                            tokens.append(
                                Token(match.start(), match.end() + end))
        return tokens

    def merge_two_time_points(self, source: str, reference: datetime, date_ers: [ExtractResult],
                              time_ers: [ExtractResult]) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()
        ers_datetime: List[ExtractResult] = self.config.single_date_time_extractor.extract(
            source, reference)
        ers_time: List[ExtractResult] = self.config.single_time_extractor.extract(
            source, reference)
        inner_marks: List[ExtractResult] = list()

        j = 0

        for er_datetime in ers_datetime:
            inner_marks.append(er_datetime)

            while j < len(ers_time) and ers_time[j].start + ers_time[j].length < er_datetime.start:
                inner_marks.append(ers_time[j])
                j += 1

            while j < len(ers_time) and ers_time[j].overlap(er_datetime):
                j += 1

        while j < len(ers_time):
            inner_marks.append(ers_time[j])
            j += 1
        inner_marks = sorted(inner_marks, key=lambda x: x.start)

        idx = 0
        ceil = len(inner_marks) - 1

        while idx < ceil:
            current_mark = inner_marks[idx]
            next_mark = inner_marks[idx + 1]

            if current_mark.type == Constants.SYS_DATETIME_TIME and next_mark.type == Constants.SYS_DATETIME_TIME:
                idx += 1
                continue

            middle_begin = current_mark.start + current_mark.length
            middle_end = next_mark.start

            middle_str = source[middle_begin:middle_end].strip()
            match = regex.search(self.config.till_regex, middle_str)

            if match and match.group() == middle_str:
                period_begin = current_mark.start
                period_end = next_mark.start + next_mark.length
                before_str = source[0:period_begin].strip()
                match_from = self.config.get_from_token_index(before_str)
                from_token_index = match_from if match_from.matched else self.config.get_between_token_index(
                    before_str)
                if from_token_index.matched:
                    period_begin = from_token_index.index
                tokens.append(Token(period_begin, period_end))
                idx += 2
                continue

            if self.config.has_connector_token(middle_str):
                period_begin = current_mark.start
                period_end = next_mark.start + next_mark.length
                before_str = source[0:period_begin].strip()
                between_token_index = self.config.get_between_token_index(
                    before_str)
                if between_token_index.matched:
                    period_begin = between_token_index.index
                    tokens.append(Token(period_begin, period_end))
                    idx += 2
                    continue

            idx += 1

        return tokens

    def match_duration(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()

        ers_duration: List[ExtractResult] = self.config.duration_extractor.extract(
            source, reference)
        durations: List[Token] = list()

        for extracted_result in ers_duration:
            if regex.search(self.config.time_unit_regex, extracted_result.text):
                durations.append(Token(extracted_result.start, extracted_result.start + extracted_result.length))

        for duration in durations:
            before_str = source[0:duration.start].strip()
            if before_str:
                match = regex.search(
                    self.config.previous_prefix_regex, before_str)
                if match and not before_str[match.end():]:
                    tokens.append(Token(match.start(), duration.end))
                    continue

                match = regex.search(self.config.next_prefix_regex, before_str)
                if match and not before_str[match.end():]:
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

        for extracted_result in ers_date:
            after_str = source[extracted_result.start + extracted_result.length:]
            match = regex.search(
                self.config.period_time_of_day_with_date_regex, after_str)

            if match:
                if not after_str[0:match.start()].strip():
                    tokens.append(
                        Token(extracted_result.start, extracted_result.start + extracted_result.length + match.end()))
                else:
                    pause_match = regex.search(
                        self.config.middle_pause_regex, after_str[0:match.start()].strip())
                    if pause_match:
                        suffix = after_str[match.end():].strip()

                        ending_match = regex.search(
                            self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(
                                Token(extracted_result.start, extracted_result.start +
                                      extracted_result.length + match.end()))

            before_str = source[0:extracted_result.start]
            match = regex.search(
                self.config.period_time_of_day_with_date_regex, before_str)

            if match:
                if not before_str[match.end():].strip():
                    middle_str = source[match.end():extracted_result.start]
                    if middle_str == ' ':
                        tokens.append(
                            Token(match.start(), extracted_result.start + extracted_result.length))
                else:
                    pause_match = regex.search(
                        self.config.middle_pause_regex, before_str[match.end():])
                    if pause_match:
                        suffix = source[extracted_result.start + extracted_result.length:].strip()

                        ending_match = regex.search(
                            self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(
                                Token(match.start(), extracted_result.start + extracted_result.length))

            # check whether there are adjacent time period strings, before or after
            for token in tokens:
                # try to extract a time period in before-string
                if token.start > 0:
                    before_str = source[0:token.start].strip()
                    if before_str:
                        ers_time = self.config.time_period_extractor.extract(
                            before_str, reference)

                        for er_time in ers_time:
                            middle_str = before_str[er_time.start +
                                                    er_time.length:].strip()
                            if not middle_str:
                                tokens.append(Token(er_time.start,
                                                    er_time.start + er_time.length + len(middle_str) + token.length))

                if token.start + token.length <= len(source):
                    after_str = source[token.start + token.length:]
                    if after_str:
                        ers_time = self.config.time_period_extractor.extract(
                            after_str, reference)
                        for er_time in ers_time:
                            middle_str = after_str[0:er_time.start]
                            if not middle_str:
                                token_end = token.start + token.length + \
                                    len(middle_str) + er_time.length
                                tokens.append(Token(token.start, token_end))

        return tokens

    @staticmethod
    def match_prefix_regex_in_segment(before_str: str, match: Match):
        return match and before_str[before_str.index(match.group()) + (match.end() - match.start())]

    def match_relative_unit(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        matches = list(regex.finditer(
            self.config.relative_time_unit_regex, source))

        if not matches:
            matches = list(regex.finditer(
                self.config.rest_of_date_time_regex, source))

        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens


class DateTimePeriodParserConfiguration:
    @property
    @abstractmethod
    def before_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def after_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def prefix_day_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def token_before_date(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> bool:
        raise NotImplementedError

    @property
    @abstractmethod
    def pure_number_from_to_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def pure_number_between_and_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def period_time_of_day_with_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def specific_time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def past_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def future_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_time_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def rest_of_date_time_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def numbers(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @abstractmethod
    def get_matched_time_range(self, source: str) -> MatchedTimeRange:
        raise NotImplementedError

    @abstractmethod
    def get_swift_prefix(self, source: str) -> int:
        raise NotImplementedError


class BaseDateTimePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIMEPERIOD

    def __init__(self, config: DateTimePeriodParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult(source)

        if source.type is self.parser_type_name:
            source_text = source.text.lower()

            inner_result = self.merge_date_and_time_periods(
                source_text, reference)

            if not inner_result.success:
                inner_result = self.merge_two_time_points(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_specific_time_of_day(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_duration(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_relative_unit(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_date_with_period_prefix(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_date_with_time_period_suffix(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.START_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.future_value[0])
                inner_result.future_resolution[TimeTypeConstants.END_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.future_value[1])
                inner_result.past_resolution[TimeTypeConstants.START_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.past_value[0])
                inner_result.past_resolution[TimeTypeConstants.END_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.past_value[1])
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    def is_before_or_after_mod(self, mod: str) -> bool:
        if not self.config.check_both_before_after:
            return mod and (mod == TimeTypeConstants.BEFORE_MOD or mod == TimeTypeConstants.AFTER_MOD)
        else:
            # matches with inclusive_mod_prepositions are also parsed here
            return mod and (mod == TimeTypeConstants.BEFORE_MOD or mod == TimeTypeConstants.AFTER_MOD or
                            mod == TimeTypeConstants.UNTIL_MOD or mod == TimeTypeConstants.SINCE_MOD)

    def merge_date_and_time_periods(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        trimmed_source = source.strip().lower()

        extracted_results = self.config.time_period_extractor.extract(trimmed_source, reference)
        if len(extracted_results) == 0:
            return self.parse_simple_cases(trimmed_source, reference)
        elif len(extracted_results) == 1:
            time_period_parse_result = self.config.time_period_parser.parse(extracted_results[0])
            time_period_resolution_result = time_period_parse_result.value

            if time_period_resolution_result is None:
                return self.parse_simple_cases(trimmed_source, reference)

            time_period_timex = time_period_resolution_result.timex

            # if it is a range type timex
            if TimexUtil.is_range_timex(time_period_timex):
                date_result = self.config.date_extractor.extract(trimmed_source.replace(extracted_results[0].text, ''), reference)

                # Check if token_before_date is null
                date_text = trimmed_source.replace(extracted_results[0].text, '').replace(self.config.token_before_date, ''). \
                    strip() if self.config.token_before_date else trimmed_source.replace(extracted_results[0].text, '').strip()
                if self.config.check_both_before_after:
                    token_list_before_date = list(self.config.token_before_date.split('|'))
                    for token in filter(lambda n: n, token_list_before_date):
                        date_text = date_text.replace(token, '').strip()

                # If only one Date is extracted and the Date text equals to the rest part of source text
                if len(date_result) == 1 and date_text == date_result[0].text:
                    parse_result = self.config.date_parser.parse(date_result[0], reference)

                    if parse_result.value:
                        future_time = parse_result.value.future_value
                        past_time = parse_result.value.past_value

                        date_timex = parse_result.timex_str
                    else:
                        return self.parse_simple_cases(trimmed_source, source)

                    range_timex_components = TimexUtil.get_range_timex_components(time_period_timex)

                    if range_timex_components.is_valid:
                        begin_timex = TimexUtil.combine_date_and_time_timex(date_timex, range_timex_components.begin_timex)
                        end_timex = TimexUtil.combine_date_and_time_timex(date_timex, range_timex_components.end_timex)
                        result.timex = TimexUtil.generate_date_time_period_timex(begin_timex, end_timex, range_timex_components.duration_timex)

                        time_period_future_value = (time_period_resolution_result.future_value.start, time_period_resolution_result.future_value.end)
                        begin_time = time_period_future_value[0]
                        end_time = time_period_future_value[1]

                        result.future_value = [
                            DateUtils.safe_create_from_min_value(future_time.year, future_time.month, future_time.day,
                                                                 begin_time.hour, begin_time.minute,
                                                                 begin_time.second),
                            DateUtils.safe_create_from_min_value(future_time.year, future_time.month, future_time.day,
                                                                 end_time.hour, end_time.minute,
                                                                 end_time.second)
                        ]

                        result.past_value = [
                            DateUtils.safe_create_from_min_value(past_time.year, past_time.month, past_time.day,
                                                                 begin_time.hour, begin_time.minute,
                                                                 begin_time.second),
                            DateUtils.safe_create_from_min_value(past_time.year, past_time.month, past_time.day,
                                                                 end_time.hour, end_time.minute,
                                                                 end_time.second)
                        ]

                        if time_period_resolution_result.comment and time_period_resolution_result.comment == Constants.COMMENT_AMPM:
                            # ampm comment is used for later set_parser_result to judge whether this parse result should
                            # have two parsing results.
                            # Cases like "from 10:30 to 11 on 1/1/2015" should have ampm comment, as it can be parsed
                            # to "10:30am to 11am" and also be parsed to "10:30pm to 11pm".
                            # Cases like "from 10:30 to 3 on 1/1/2015" should not have ampm comment.
                            if begin_time.hour < Constants.HALF_DAY_HOUR_COUNT and end_time.hour < Constants.HALF_DAY_HOUR_COUNT:
                                result.comment = Constants.COMMENT_AMPM

                        result.success = True
                        result.sub_date_time_entities = [parse_result, time_period_parse_result]

                        return result

                return self.parse_simple_cases(source, reference)

        return result

    def parse_simple_cases(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = regex.search(self.config.pure_number_from_to_regex, source)

        if not match:
            match = regex.search(
                self.config.pure_number_between_and_regex, source)

        if not match or match.start() != 0:
            return result

        hour_group = RegExpUtility.get_group_list(match, Constants.HOUR_GROUP_NAME)
        begin_hour = self.config.numbers.get(hour_group[0])

        if not begin_hour:
            begin_hour = int(hour_group[0])
        end_hour = self.config.numbers.get(hour_group[1])

        if not end_hour:
            end_hour = int(hour_group[1])

        extracted_result = next(iter(self.config.date_extractor.extract(
            source.replace(match.group(), ''), reference)), None)

        if not extracted_result:
            return result

        pr = self.config.date_parser.parse(extracted_result, reference)

        if not pr:
            return result

        date_result: DateTimeResolutionResult = pr.value
        future_date: datetime = date_result.future_value
        past_date: datetime = date_result.past_value
        date_str = pr.timex_str

        has_am = False
        has_pm = False
        am_str = RegExpUtility.get_group(match, Constants.AM_GROUP_NAME)
        pm_str = RegExpUtility.get_group(match, Constants.PM_GROUP_NAME)
        desc_str = RegExpUtility.get_group(match, Constants.DESC_GROUP_NAME)

        if am_str or desc_str.startswith('a'):
            if begin_hour >= 12:
                begin_hour = begin_hour - 12
            if end_hour >= 12:
                end_hour = end_hour - 12
            has_am = True

        if pm_str or desc_str.startswith('p'):
            if begin_hour < 12:
                begin_hour = begin_hour + 12
            if end_hour < 12:
                end_hour = end_hour + 12
            has_pm = True

        if not has_am and not has_pm and begin_hour <= 12 and end_hour <= 12:
            result.comment = Constants.AM_PM_GROUP_NAME

        begin_str = f'{date_str}T{begin_hour:02d}'
        end_str = f'{date_str}T{end_hour:02d}'

        result.timex = f'({begin_str},{end_str},PT{end_hour - begin_hour}H)'
        result.future_value = [
            DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day, begin_hour, 0,
                                                 0),
            DateUtils.safe_create_from_min_value(
                future_date.year, future_date.month, future_date.day, end_hour, 0, 0)
        ]
        result.past_value = [
            DateUtils.safe_create_from_min_value(
                past_date.year, past_date.month, past_date.day, begin_hour, 0, 0),
            DateUtils.safe_create_from_min_value(
                past_date.year, past_date.month, past_date.day, end_hour, 0, 0)
        ]

        result.success = True
        return result

    @staticmethod
    def get_two_points(begin_er: ExtractResult, end_er: ExtractResult, begin_parser: DateTimeParser,
                       end_parser: DateTimeParser, reference: datetime) -> BeginEnd:
        return BeginEnd(begin=begin_parser.parse(begin_er, reference), end=end_parser.parse(end_er, reference))

    def merge_two_time_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        prs: BeginEnd = None
        time_ers = self.config.time_extractor.extract(source, reference)
        datetime_ers = self.config.date_time_extractor.extract(
            source, reference)

        both_has_date = False
        begin_has_date = False
        end_has_date = False

        if len(datetime_ers) == 2:
            prs = self.get_two_points(datetime_ers[0], datetime_ers[1], self.config.date_time_parser,
                                      self.config.date_time_parser, reference)
            both_has_date = True
        elif len(datetime_ers) == 1 and len(time_ers) == 2:
            if datetime_ers[0].overlap(time_ers[0]):
                prs = self.get_two_points(datetime_ers[0], time_ers[1], self.config.date_time_parser,
                                          self.config.time_parser, reference)
                begin_has_date = True
            else:
                prs = self.get_two_points(time_ers[0], datetime_ers[0], self.config.time_parser,
                                          self.config.date_time_parser, reference)
                end_has_date = True
        elif len(datetime_ers) == 1 and len(time_ers) == 1:
            if time_ers[0].start < datetime_ers[0].start:
                prs = self.get_two_points(time_ers[0], datetime_ers[0], self.config.time_parser,
                                          self.config.date_time_parser, reference)
                end_has_date = True
            else:
                prs = self.get_two_points(datetime_ers[0], time_ers[0], self.config.date_time_parser,
                                          self.config.time_parser, reference)
                begin_has_date = True

        if prs is None or not prs.begin.value or not prs.end.value:
            return result

        begin: DateTimeResolutionResult = prs.begin.value
        end: DateTimeResolutionResult = prs.end.value

        future_begin: datetime = begin.future_value
        future_end: datetime = end.future_value
        past_begin: datetime = begin.past_value
        past_end: datetime = end.past_value

        if both_has_date:
            if future_begin > future_end:
                future_begin = past_begin

            if past_end < past_begin:
                past_end = future_end

            total_hours = DateUtils.total_hours(future_begin, future_end)
            result.timex = f'({prs.begin.timex_str},{prs.end.timex_str},PT{total_hours}H)'
        elif begin_has_date:
            future_end = self.get_datetime(future_begin, future_end)
            past_end = self.get_datetime(past_begin, past_end)
            total_hours = DateUtils.total_hours(future_begin, future_end)
            date_str = prs.begin.timex_str.split('T').pop()
            result.timex = f'({prs.begin.timex_str},{date_str}{prs.end.timex_str},PT{total_hours}H)'
        elif end_has_date:
            future_begin = self.get_datetime(future_end, future_begin)
            past_begin = self.get_datetime(past_end, past_begin)
            total_hours = DateUtils.total_hours(future_begin, future_end)
            date_str = prs.end.timex_str.split('T')[0]
            result.timex = f'({date_str}{prs.begin.timex_str},{prs.end.timex_str},PT{total_hours}H)'

        if begin.comment and begin.comment.endswith(Constants.AM_PM_GROUP_NAME) and end.comment and \
                end.comment.endswith(Constants.AM_PM_GROUP_NAME):
            result.comment = Constants.AM_PM_GROUP_NAME

        result.future_value = [future_begin, future_end]
        result.past_value = [past_begin, past_end]
        result.success = True
        result.sub_date_time_entities = [prs.begin, prs.end]

        return result

    @staticmethod
    def get_datetime(date: datetime, time: datetime) -> datetime:
        return DateUtils.safe_create_from_min_value(date.year, date.month, date.day, time.hour, time.minute,
                                                    time.second)

    def parse_specific_time_of_day(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        time_str = source
        has_early = False
        has_late = False

        match = regex.search(
            self.config.period_time_of_day_with_date_regex, source)
        if match:
            time_str = RegExpUtility.get_group(match, Constants.TIME_OF_DAY_GROUP_NAME)
            if RegExpUtility.get_group(match, Constants.EARLY):
                has_early = True
                result.comment = Constants.EARLY
                result.mod = TimeTypeConstants.EARLY_MOD
            elif RegExpUtility.get_group(match, Constants.LATE):
                has_late = True
                result.comment = Constants.LATE
                result.mod = TimeTypeConstants.LATE_MOD

        matched = self.config.get_matched_time_range(time_str)
        if not matched.success:
            return result

        if has_early:
            matched = MatchedTimeRange(matched.time_str, matched.begin_hour, matched.begin_hour + 2,
                                       0 if matched.end_min == 59 else matched.end_min, matched.success)
        elif has_late:
            matched = MatchedTimeRange(matched.time_str, matched.begin_hour + 2, matched.end_hour, matched.end_min,
                                       matched.success)

        match = list(self.config.specific_time_of_day_regex.finditer(source))
        if match and match[-1].start() == 0 and match[-1].group() == source:
            swift = self.config.get_swift_prefix(source)
            date = reference + timedelta(days=swift)
            result.timex = DateTimeFormatUtil.format_date(
                date) + matched.time_str
            result.future_value = [
                DateUtils.safe_create_from_min_value(
                    date.year, date.month, date.day, matched.begin_hour, 0, 0),
                DateUtils.safe_create_from_min_value(date.year, date.month, date.day, matched.end_hour, matched.end_min,
                                                     matched.end_min)
            ]
            result.past_value = [
                DateUtils.safe_create_from_min_value(
                    date.year, date.month, date.day, matched.begin_hour, 0, 0),
                DateUtils.safe_create_from_min_value(date.year, date.month, date.day, matched.end_hour, matched.end_min,
                                                     matched.end_min)
            ]
            result.success = True

            return result

        match = list(
            self.config.period_time_of_day_with_date_regex.finditer(source))
        if not match:
            return result

        before_str = source[0:match[-1].start()].strip()
        after_str = source[match[-1].end():].strip()
        extracted_result = next(iter(self.config.date_extractor.extract(
            before_str, reference)), None)

        # eliminate time period, if any
        time_period_er = next(
            iter(self.config.time_period_extractor.extract(before_str)), None)
        if time_period_er:
            before_str = before_str[time_period_er.start:time_period_er.start +
                                    time_period_er.length].strip()
        else:
            time_period_er = next(
                iter(self.config.time_period_extractor.extract(after_str)), None)
            if time_period_er:
                after_str = after_str[time_period_er.start:time_period_er.start +
                                      time_period_er.length].strip()

        if not extracted_result or extracted_result.length != len(before_str):
            valid = False
            if extracted_result and extracted_result.start == 0:
                middle_str = before_str[extracted_result.start + extracted_result.length:]
                if not middle_str.replace(',', ''):
                    valid = True

            if not valid:
                extracted_result = next(
                    iter(self.config.date_extractor.extract(after_str)), None)
                if not extracted_result or extracted_result.length != len(after_str):
                    if extracted_result and extracted_result.start + extracted_result.length == len(after_str):
                        valid = True
                else:
                    valid = True

                if not valid:
                    return result

        has_specific_time_period = False
        if time_period_er:
            time_pr = self.config.time_period_parser.parse(time_period_er)
            if time_pr:
                period_future = time_pr.value.future_value
                period_past = time_pr.value.past_value

                if period_future.start == period_past.start and period_future.end == period_past.end:
                    begin_hour: datetime = period_future.start
                    end_hour: datetime = period_future.end
                    matched = MatchedTimeRange(matched.time_str, begin_hour.hour, end_hour.hour, matched.end_min,
                                               matched.success)
                else:
                    if period_future.start.hour >= matched.begin_hour or period_future.end.hour <= matched.end_hour:
                        begin_hour: datetime = period_future.start
                        end_hour: datetime = period_future.end
                        matched = MatchedTimeRange(matched.time_str, begin_hour.hour, end_hour.hour, matched.end_min,
                                                   matched.success)
                    else:
                        begin_hour: datetime = period_past.start
                        end_hour: datetime = period_past.end
                        matched = MatchedTimeRange(matched.time_str, begin_hour.hour, end_hour.hour, matched.end_min,
                                                   matched.success)

                has_specific_time_period = True

        parse_result = self.config.date_parser.parse(extracted_result, reference)
        if not parse_result:
            return result

        future_date: datetime = parse_result.value.future_value
        past_date: datetime = parse_result.value.past_value

        if not has_specific_time_period:
            result.timex = parse_result.timex_str + matched.time_str
        else:
            result.timex = f'({parse_result.timex_str}T{matched.begin_hour},{parse_result.timex_str}T{matched.end_hour},' \
                           f'PT{matched.end_hour - matched.begin_hour}H)'

        result.future_value = [
            DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day,
                                                 matched.begin_hour, 0, 0),
            DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day, matched.end_hour,
                                                 matched.end_min, matched.end_min)
        ]
        result.past_value = [
            DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day, matched.begin_hour, 0,
                                                 0),
            DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day, matched.end_hour,
                                                 matched.end_min, matched.end_min)
        ]
        result.success = True

        return result

    def parse_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        # for rest of datetime, it will be handled in next function
        if regex.search(self.config.rest_of_date_time_regex, source):
            return result

        extracted_result = self.config.duration_extractor.extract(source, reference)
        if len(extracted_result) == 1:
            parse_result = self.config.duration_parser.parse(extracted_result[0])

            before_str = source[0:parse_result.start].strip()
            after_str = source[parse_result.start + parse_result.length:].strip()

            numbers_in_suffix = self.config.cardinal_extractor.extract(before_str)
            numbers_in_duration = self.config.cardinal_extractor.extract(extracted_result[0].text)

            # Handle cases like "2 upcoming days", "5 previous years"
            if any(numbers_in_suffix) and not any(numbers_in_duration):
                number_extracted_result = next(numbers_in_suffix)
                number_text = number_extracted_result.text
                duration_text = extracted_result[0].text
                combined_text = f'{number_text} {duration_text}'
                combined_duration_extracted_result = self.config.duration_extractor.extract(combined_text, reference)

                if any(combined_duration_extracted_result):
                    parse_result = self.config.duration_parser.parse(next(combined_duration_extracted_result))
                    start_index = number_extracted_result.start + number_extracted_result.length
                    before_str = before_str[start_index:].strip()

            if parse_result.value:
                swift_seconds = 0
                mod = ''
                duration_result = parse_result.value

                if duration_result.past_value is float and duration_result.future_value is float:
                    swift_seconds = int(float(duration_result.future_value))

                end_time = begin_time = reference

                if RegExpUtility.is_exact_match(self.config.previous_prefix_regex, before_str, True):
                    mod = TimeTypeConstants.BEFORE_MOD
                    begin_time = reference + timedelta(seconds=-swift_seconds)

                # Handle the "within (the) (next) xx seconds/minutes/hours" case
                # Should also habdle the multiple duration case like P1DT8H
                # Set the begin_time equal to reference time for now
                if RegExpUtility.is_exact_match(self.config.within_next_prefix_regex, before_str, True):
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                if self.config.check_both_before_after and RegExpUtility.is_exact_match(self.config.within_next_prefix_regex, after_str, True):
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                if RegExpUtility.is_exact_match(self.config.future_regex, before_str, True):
                    mod = TimeTypeConstants.AFTER_MOD
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                if RegExpUtility.is_exact_match(self.config.previous_prefix_regex, after_str, True):
                    mod = TimeTypeConstants.BEFORE_MOD
                    begin_time = reference + timedelta(seconds=-swift_seconds)
                if RegExpUtility.is_exact_match(self.config.future_regex, after_str, True):
                    mod = TimeTypeConstants.AFTER_MOD
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                if RegExpUtility.is_exact_match(self.config.future_suffix_regex, after_str, True):
                    mod = TimeTypeConstants.AFTER_MOD
                    end_time = begin_time + timedelta(seconds=swift_seconds)

                result.timex = f'({DateTimeFormatUtil.luis_date_from_datetime(begin_time)}T' \
                               f'{DateTimeFormatUtil.luis_date_from_datetime(begin_time)},' + \
                               f'{DateTimeFormatUtil.luis_date_from_datetime(end_time)}T' \
                               f'{DateTimeFormatUtil.luis_date_from_datetime(end_time)},' + \
                               f'{duration_result.timex})'

                result.future_value = result.past_value = (begin_time, end_time)
                result.success = True

                if not mod:
                    parse_result.value.mod = mod

                result.sub_date_time_entities = [parse_result]

                return result

        return result

    def get_valid_connector_mod_for_date_and_time_period(self, source: str, in_prefix: bool):
        mod = None

        # Item1 is the regexp to be tested
        # Item2 is the mod corresponding to an inclusive match (i.e. containing an inclusive_mod_prepositions,
        # e.g. "at or before3")
        # Item3 is the mod corresponding to a non-inclusive match (e.g "before 3")
        before_after_regex_tuple = [(self.config.before_regex, TimeTypeConstants.UNTIL_MOD,
                                     TimeTypeConstants.BEFORE_MOD),
                                    (self.config.after_regex, TimeTypeConstants.SINCE_MOD,
                                     TimeTypeConstants.AFTER_MOD)]

        for regexp in before_after_regex_tuple:
            match = regexp[0].match(source) if in_prefix else RegExpUtility.match_begin(regexp[0], source, True)
            if match and match.success:
                mod = regexp[2] if in_prefix else (regexp[1] if RegExpUtility.get_group(match, 'include') else
                                                   regexp[2])
                return mod

        return mod

    def parse_relative_unit(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = regex.search(self.config.relative_time_unit_regex, source)

        if not match:
            match = regex.search(self.config.rest_of_date_time_regex, source)

        if not match:
            return result

        src_unit = RegExpUtility.get_group(match, Constants.UNIT)
        unit_str = self.config.unit_map.get(src_unit, None)

        if not unit_str:
            return result

        swift = 1
        prefix_match = regex.search(self.config.past_regex, source)
        if prefix_match:
            swift = -1

        begin_time: datetime = reference
        end_time: datetime = reference

        if unit_str == 'D':
            end_time = DateUtils.safe_create_from_min_value(begin_time.year, begin_time.month,
                                                            begin_time.day) + timedelta(days=1, seconds=-1)
            difference = int((end_time - begin_time).total_seconds())
            pt_timex = f'PT{difference}S'
        elif unit_str == 'H':
            begin_time = begin_time + \
                timedelta(hours=0 if swift > 0 else swift)
            end_time = end_time + timedelta(hours=swift if swift > 0 else 0)
            pt_timex = 'PT1H'
        elif unit_str == 'M':
            begin_time = begin_time + \
                timedelta(minutes=0 if swift > 0 else swift)
            end_time = end_time + timedelta(minutes=swift if swift > 0 else 0)
            pt_timex = 'PT1M'
        elif unit_str == 'S':
            begin_time = begin_time + \
                timedelta(seconds=0 if swift > 0 else swift)
            end_time = end_time + timedelta(seconds=swift if swift > 0 else 0)
            pt_timex = 'PT1S'
        else:
            return result

        luis_date_begin = DateTimeFormatUtil.luis_date_from_datetime(
            begin_time)
        luis_time_begin = DateTimeFormatUtil.luis_time_from_datetime(
            begin_time)
        luis_date_end = DateTimeFormatUtil.luis_date_from_datetime(end_time)
        luis_time_end = DateTimeFormatUtil.luis_time_from_datetime(end_time)

        result.timex = f'({luis_date_begin}T{luis_time_begin},{luis_date_end}T{luis_time_end},{pt_timex})'
        result.future_value = [begin_time, end_time]
        result.past_value = [begin_time, end_time]
        result.success = True

        return result

    def parse_date_with_period_prefix(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        date_result = self.config.date_extractor.extract(source)
        if len(date_result) > 0:
            before_str = source[0: date_result[-1].start]
            match = self.config.prefix_day_regex.match(before_str)

            # Check also after_str
            if not match and self.config.check_both_before_after:
                after_str = source[date_result[-1].start + date_result[-1].length: len(source) - date_result[-1].start + date_result[-1].length]
                match = self.config.prefix_day_regex.match(after_str)

            if match:
                parse_result = self.config.date_parser.parse(date_result[-1], reference)
                if parse_result.value:
                    start_time = parse_result.value.future_value
                    start_time = datetime(start_time.year, start_time.month, start_time.day)
                    end_time = start_time

                    if RegExpUtility.get_group(match, Constants.EARLY_PREFIX):
                        end_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        result.mod = TimeTypeConstants.EARLY_MOD
                    elif RegExpUtility.get_group(match, Constants.MID_PREFIX):
                        start_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT - Constants.HALF_MID_DAY_DURATION_HOUR_COUNT)
                        end_time = end_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT + Constants.HALF_MID_DAY_DURATION_HOUR_COUNT)
                        result.mod = TimeTypeConstants.MID_MOD
                    elif RegExpUtility.get_group(match, Constants.LATE_PREFIX):
                        start_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        end_time = end_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        result.mod = TimeTypeConstants.LATE_MOD
                    else:
                        return result

                    result.timex = parse_result.timex_str
                    result.past_value = result.future_value = (start_time, end_time)
                    result.success = True

        return result

    def parse_date_with_time_period_suffix(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        date_extract_result = next(self.config.date_extractor.extract(source), None)
        time_extract_result = next(self.config.time_extractor.extract(source), None)

        if date_extract_result and time_extract_result:
            date_str_end = int(date_extract_result.start + date_extract_result.length)
            time_str_end = int(time_extract_result.start + time_extract_result.length)

            if date_str_end < time_extract_result.start:
                mid_str = source[date_str_end:time_extract_result.start - date_str_end].strip()
                after_str = source[time_str_end:]

                mod_str = self.get_valid_connector_mod_for_date_and_time_period(mid_str, True)

                # Check also after_str
                if mod_str and self.config.check_both_before_after:
                    mod_str = self.get_valid_connector_mod_for_date_and_time_period(after_str, False) if\
                        len(mid_str) <= 4 else None

                if mod_str:
                    date_parse_result = self.config.date_parser.parse(date_extract_result, reference)
                    time_parse_result = self.config.time_parser.parse(time_extract_result, reference)

                    if date_parse_result and time_parse_result:
                        time_resolution_result = time_parse_result.value
                        date_resolution_result = date_parse_result.value
                        future_date_value = date_resolution_result.future_value
                        past_date_value = date_resolution_result.past_value
                        future_time_value = time_resolution_result.future_value
                        past_time_value = time_resolution_result.past_value

                        result.comment = time_resolution_result.comment
                        result.timex = f'{date_parse_result.timex_str}{time_parse_result.timex_str}'

                        result.future_value = DateUtils.safe_create_from_min_value(future_date_value.year,
                                                                                   future_date_value.month,
                                                                                   future_date_value.day,
                                                                                   future_time_value.hour,
                                                                                   future_time_value.minute,
                                                                                   future_time_value.second)
                        result.past_value = DateUtils.safe_create_from_min_value(past_date_value.year,
                                                                                 past_date_value.month,
                                                                                 past_date_value.day,
                                                                                 past_time_value.hour,
                                                                                 past_time_value.minute,
                                                                                 past_time_value.second)

                        result.mod = mod_str
                        result.sub_date_time_entities = [date_parse_result, time_parse_result]
                        result.success = True

        return result

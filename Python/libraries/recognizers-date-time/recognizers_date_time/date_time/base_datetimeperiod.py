from abc import ABC, abstractmethod
from typing import List, Optional, Pattern
from datetime import datetime
from collections import namedtuple
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_number import BaseNumberExtractor
from .base_date import BaseDateExtractor
from .base_time import BaseTimeExtractor
from .base_datetime import BaseDateTimeExtractor
from .base_duration import BaseDurationExtractor
from .base_timeperiod import BaseTimePeriodExtractor
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, RegExpUtility

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])

class DateTimePeriodExtractorConfiguration:
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
    def past_prefix_regex(self) -> Pattern:
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

class BaseDateTimePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIMEPERIOD

    def __init__(self, config: DateTimePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.match_simple_cases(source, reference))
        tokens.extend(self.merge_two_time_points(source, reference))
        tokens.extend(self.match_duration(source, reference))
        tokens.extend(self.match_night(source, reference))
        tokens.extend(self.match_relative_unit(source))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def match_simple_cases(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()
        simple_cases_matches = list(map(lambda x: list(regex.finditer(x, source)), self.config.simple_cases_regexes))
        for matches in simple_cases_matches:
            for match in matches:
                # has a date before?
                has_before_date = False
                before_str = source[0:match.start()].strip()
                if before_str:
                    er = next(iter(self.config.single_date_extractor.extract(before_str, reference)), None)
                    if er:
                        begin = er.start
                        end = er.start + er.length
                        middle_str = before_str[end:].strip()
                        if middle_str == '' or regex.search(self.config.preposition_regex, middle_str):
                            tokens.append(Token(begin, match.end()))
                            has_before_date = True
                followed_str = source[match.end():]
                if followed_str and not has_before_date:
                    er = next(iter(self.config.single_date_extractor.extract(followed_str, reference)), None)
                    if er:
                        begin = er.start
                        end = er.start + er.length
                        middle_str = followed_str[0:er.start].strip()
                        if middle_str == '' or regex.search(self.config.preposition_regex, middle_str):
                            tokens.append(Token(match.start(), match.end() + end))

        return tokens

    def merge_two_time_points(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()
        ers_datetime: List[ExtractResult] = self.config.single_date_time_extractor.extract(source, reference)
        ers_time: List[ExtractResult] = self.config.single_time_extractor.extract(source, reference)
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
                from_token_index = match_from if match_from.matched else self.config.get_between_token_index(before_str)
                if from_token_index.matched:
                    period_begin = from_token_index.index
                tokens.append(Token(period_begin, period_end))
                idx += 2
                continue
            if self.config.has_connector_token(middle_str):
                period_begin = current_mark.start
                period_end = next_mark.start + next_mark.length
                before_str = source[0:period_begin].strip()
                between_token_index = self.config.get_between_token_index(before_str)
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

        ers_duration: List[ExtractResult] = self.config.duration_extractor.extract(source, reference)
        durations: List[Token] = list()
        for er in ers_duration:
            if regex.search(self.config.time_unit_regex, er.text):
                durations.append(Token(er.start, er.start + er.length))
        
        for duration in durations:
            before_str = source[0:duration.start].strip()
            if before_str:
                match = regex.search(self.config.past_prefix_regex, before_str)
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

        matches = regex.finditer(self.config.specific_time_of_day_regex, source)
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))

        ers_date: List[ExtractResult] = self.config.single_date_extractor.extract(source, reference)
        for er in ers_date:
            after_str = source[er.start + er.length:]
            match = regex.search(self.config.period_time_of_day_with_date_regex, after_str)
            if match:
                if not after_str[0:match.start()].strip():
                    tokens.append(Token(er.start, er.start + er.length + match.end()))
                else:
                    pause_match = regex.search(self.config.middle_pause_regex, after_str[0:match.start()].strip())
                    if pause_match:
                        suffix = after_str[match.end():].strip()

                        ending_match = regex.search(self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(Token(er.start, er.start + er.length + match.end()))

            before_str = source[0:er.start]
            match = regex.search(self.config.period_time_of_day_with_date_regex, before_str)
            if match:
                if not before_str[match.end():].strip():
                    middle_str = source[match.end():er.start]
                    if middle_str == ' ':
                        tokens.append(Token(match.start(), er.start + er.length))
                else:
                    pause_match = regex.search(self.config.middle_pause_regex, before_str[match.end():])
                    if pause_match:
                        suffix = source[er.start + er.length:].strip()

                        ending_match = regex.search(self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(Token(match.start(), er.start + er.length))
            # check whether there are adjacent time period strings, before or after
            for token in tokens:
                # try to extract a time period in before-string
                if token.start > 0:
                    before_str = source[0:token.start].strip()
                    if before_str:
                        ers_time = self.config.time_period_extractor.extract(before_str, reference)
                        for er in ers_time:
                            middle_str = before_str[er.start + er.length:].strip()
                            if not middle_str:
                                tokens.append(Token(er.start, er.start + er.length + len(middle_str) + token.length))

                if token.start + token.length <= len(source):
                    after_str = source[token.start + token.length:]
                    if after_str:
                        ers_time = self.config.time_period_extractor.extract(after_str, reference)
                        for er in ers_time:
                            middle_str = after_str[0:er.start]
                            if not middle_str:
                                token_end = token.start + token.length + len(middle_str) + er.length
                                tokens.append(Token(token.start, token_end))

        return tokens

    def match_relative_unit(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        matches = list(regex.finditer(self.config.relative_time_unit_regex, source))
        if not matches:
            matches = list(regex.finditer(self.config.rest_of_date_time_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

class DateTimePeriodParserConfiguration:
    pass

class BaseDateTimePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIMEPERIOD

    def __init__(self, config: DateTimePeriodParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass

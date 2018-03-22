from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Match, Dict
from datetime import datetime
from collections import namedtuple

from recognizers_text.extractor import ExtractResult
from recognizers_number.number import BaseNumberParser, BaseNumberExtractor
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult 
from .base_date import BaseDateParser
from .base_duration import BaseDurationParser
from .utilities import Token, merge_all_tokens

MatchedIndex = namedtuple('MatchedIndex',['matched', 'index'])

class DatePeriodExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def simple_cases_regexes(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def till_regex(self) -> Pattern:
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
    def past_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def future_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def in_connector_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def range_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_point_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self) -> BaseNumberParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
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

class BaseDatePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    def __init__(self, config: DatePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if not reference:
            reference = datetime.now()
        tokens = []
        tokens += self.match_simple_cases(source)
        tokens += self.merge_two_time_points(source, reference)
        tokens += self.match_duration(source, reference)
        tokens += self.single_time_point_with_patterns(source, reference)
        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        return result

    def match_simple_cases(self, source: str) -> List[ExtractResult]:
        tokens = []
        for regexp in self.config.simple_cases_regexes:
            matches = regexp.finditer(source)
            for match in matches:
                add_token = True
                match_year = self.config.year_regex.search(match.group())
                if match_year and len(match_year.group()) == len(match.group()):
                    year_str = match_year.group('year')
                    if not year_str:
                        year = self.__get_year_from_text(match_year)
                        if not (year >= 1500 and year <= 2000):
                            add_token = False
                if add_token:
                    tokens.append(Token(match.start(), match.end()))
        return tokens

    def __get_year_from_text(self, match) -> int:
        first_two_year_num_str = match.group('firsttwoyearnum')
        if first_two_year_num_str:
            er = ExtractResult()
            er.text = first_two_year_num_str
            er.start = match.start('firsttwoyearnum')
            er.length = match.end('firsttwoyearnum') - er.start
            first_two_year_num = self.config.number_parser.parse(er).value

            last_two_year_num = 0
            last_two_year_num_str = match.group('lasttwoyearnum')
            if last_two_year_num_str:
                er.text = last_two_year_num_str
                er.start = match.start('lasttwoyearnum')
                er.length = match.end('lasttwoyearnum') - er.start
                last_two_year_num = self.config.number_parser.parse(er).value

            if first_two_year_num < 100 and last_two_year_num == 0 or first_two_year_num < 100 and first_two_year_num % 10 == 0 and len(last_two_year_num_str.strip().split(' ')) == 1:
                return -1

            if first_two_year_num >= 100:
                return first_two_year_num + last_two_year_num
            else:
                return first_two_year_num * 100 + last_two_year_num
        else:
            return -1

    def merge_two_time_points(self, source: str, reference: datetime) -> List[ExtractResult]:
        tokens = []
        er = self.config.date_point_extractor.extract(source, reference)
        if len(er) <= 1:
            return tokens
        idx = 0
        while idx < len(er) - 1:
            middle_begin = er[idx].start + (er[idx].length or 0)
            middle_end = er[idx + 1].start or 0
            if middle_begin >= middle_end:
                idx += 1
                continue
            middle_str = source[middle_begin:middle_end].strip().lower()
            match = self.config.till_regex.search(middle_str)
            if match and match.group() and match.start() == 0 and match.end() - match.start() == len(middle_str):
                period_begin = er[idx].start
                period_end = (er[idx + 1].start or 0) + (er[idx + 1].length or 0)
                before_str = source[0:period_begin].strip().lower()
                from_token_index = self.config.get_from_token_index(before_str)
                between_token_index = self.config.get_between_token_index(before_str)
                if from_token_index.matched or between_token_index.matched:
                    period_begin = from_token_index.index if from_token_index.matched else between_token_index.index
                tokens.append(Token(period_begin, period_end))
                idx += 2
                continue
            if self.config.has_connector_token(middle_str):
                period_begin = er[idx].start or 0
                period_end = (er[idx + 1].start or 0) + (er[idx + 1].length or 0)
                before_str = source[0:period_begin].strip().lower()
                between_token_index = self.config.get_between_token_index(before_str)
                if between_token_index.matched:
                    period_begin = between_token_index.index
                    tokens.append(Token(period_begin, period_end))
                    idx += 2
                    continue
            idx += 1
        return tokens

    def match_duration(self, source: str, reference: datetime) -> List[ExtractResult]:
        tokens = []
        durations = []
        for duration_ex in self.config.duration_extractor.extract(source, reference):
            match = self.config.date_unit_regex.search(duration_ex.text)
            if match:
                durations.append(Token(duration_ex.start, duration_ex.start + duration_ex.length))
        for duration in durations:
            before_str = source[0:duration.start].lower()
            if not before_str:
                break
            match = self.config.past_regex.search(before_str)
            if self.__match_regex_in_prefix(before_str, match):
                tokens.append(Token(match.start(), duration.end))
                break
            match = self.config.future_regex.search(before_str)
            if self.__match_regex_in_prefix(before_str, match):
                tokens.append(Token(match.start(), duration.end))
                break
            match = self.config.in_connector_regex.search(before_str)
            if self.__match_regex_in_prefix(before_str, match):
                range_str = source[duration.start:duration.start + duration.length]
                range_match = self.config.range_unit_regex.search(range_str)
                if range_match:
                    tokens.append(Token(match.start(), duration.end))
                break
        return tokens

    def single_time_point_with_patterns(self, source: str, reference: datetime) -> List[ExtractResult]:
        tokens = []
        ers = self.config.date_point_extractor.extract(source, reference)
        if len(ers) < 1:
            return tokens
        for er in ers:
            if er.start and er.length:
                before_str = source[0:er.start]
                tokens += self.__get_token_for_regex_matching(before_str, self.config.week_of_regex, er)
                tokens += self.__get_token_for_regex_matching(before_str, self.config.month_of_regex, er)
        return tokens

    def __match_regex_in_prefix(self, source: str, match: Match) -> bool:
        return match and source[match.end()]

    def __get_token_for_regex_matching(self, source: str, regexp: Pattern, er: ExtractResult) -> List[Token]:
        tokens = []
        match = regexp.search(source)
        if match and source.strip().endswith(match.group().strip()):
            start_index = source.rfind(match.group())
            tokens.append(Token(start_index, er.start + er.length))
        return tokens

class DatePeriodParserConfiguration(ABC):
    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> BaseDateParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> BaseDurationParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_front_between_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def between_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_front_simple_cases_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_cases_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def one_word_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_with_year(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_num_with_year(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
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
    def in_connector_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def quarter_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def quarter_regex_year_front(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def all_half_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def season_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def which_week_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def rest_of_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def later_early_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_with_week_day_range_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def token_before_date(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_of_month(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_year(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_map(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def season_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def get_swift_day_or_month(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def get_swift_year(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def is_future(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_year_to_date(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_month_to_date(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_week_only(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_weekend(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_month_only(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_last_cardinal(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_year_only(self, source: str) -> bool:
        raise NotImplementedError

class BaseDatePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    def __init__(self, config: DatePeriodParserConfiguration, include_end: bool = False):
        self.config = config
        self.include_end = include_end

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass

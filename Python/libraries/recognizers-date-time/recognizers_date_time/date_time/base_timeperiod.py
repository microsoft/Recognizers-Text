from abc import ABC, abstractmethod
from typing import List, Optional, Pattern
from datetime import datetime
from collections import namedtuple
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, get_tokens_from_regex, DateTimeResolutionResult

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])

class TimePeriodExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def simple_cases_regex(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def till_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def general_ending_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> Extractor:
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

class BaseTimePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEPERIOD

    def __init__(self, config: TimePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens = self.match_simple_cases(source)
        tokens.extend(self.merge_two_time_points(source, reference))
        tokens.extend(self.match_night(source))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def match_simple_cases(self, source: str) -> List[Token]:
        result: List[Token] = list()
        for pattern in self.config.simple_cases_regex:
            for match in regex.finditer(pattern, source):
                pm = RegExpUtility.get_group(match, 'pm')
                am = RegExpUtility.get_group(match, 'am')
                desc = RegExpUtility.get_group(match, 'desc')
                if pm or am or desc:
                    result.append(Token(match.start(), match.end()))
        return result

    def merge_two_time_points(self, source: str, reference: datetime) -> List[Token]:
        result: List[Token] = list()
        time_ers = self.config.single_time_extractor.extract(source, reference)
        num_ers = self.config.integer_extractor.extract(source)

        # Check if it is an ending number
        if num_ers:
            time_numbers: List[ExtractResult] = list()

            # check if it is a ending number
            ending_number = False
            num = num_ers[-1]
            if num.start + num.length == len(source):
                ending_number = True
            else:
                after = source[num.start + num.length:]
                if regex.search(self.config.general_ending_regex, after) is not None:
                    ending_number = True
            if ending_number:
                time_numbers.append(num)

            i = 0
            j = 0
            while i < len(num_ers):
                # find subsequent time point
                num_end = num_ers[i].start + num_ers[i].length
                while j < len(time_ers) and time_ers[j].start <= num_end:
                    j += 1
                if j >= len(time_ers):
                    break
                # check connector string
                middle = source[num_end:time_ers[j].start]
                match = regex.search(self.config.till_regex, middle)
                if match is not None and match.group() == middle.strip():
                    time_numbers.append(num_ers[i])
                i += 1

            # check overlap
            for time_num in time_numbers:
                overlap: bool = any(map(time_num.overlap, time_ers)) #TODO test
                if not overlap:
                    time_ers.append(time_num)

            time_ers = sorted(time_ers, key=lambda x: x.start) #TODO test

        # merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
        i = 0
        while i < len(time_ers)-1:
            middle_begin = time_ers[i].start + time_ers[i].length
            middle_end = time_ers[i+1].start
            middle: str = source[middle_begin:middle_end].strip().lower()
            match = regex.search(self.config.till_regex, middle)
            # handle "{TimePoint} to {TimePoint}"
            if match is not None and match.start() == 0 and match.group() == middle:
                period_begin = time_ers[i].start
                period_end = time_ers[i+1].start + time_ers[i+1].length

                # handle "from"
                before = source[0:period_begin].strip().lower()
                from_index: MatchedIndex = self.config.get_from_token_index(before)
                if from_index.matched:
                    period_begin = from_index.index

                result.append(Token(period_begin, period_end))
                i += 2
                continue
            # handle "between {TimePoint} and {TimePoint}"
            if self.config.has_connector_token(middle):
                period_begin = time_ers[i].start
                period_end = time_ers[i+1].start + time_ers[i+1].length

                # handle "between"
                before = source[0:period_begin].strip().lower()
                between_index: MatchedIndex = self.config.get_between_token_index(before)
                if between_index.matched:
                    period_begin = between_index.index
                    result.append(Token(period_begin, period_end))
                    i += 2
                    continue
            i += 1
        return result

    def match_night(self, source: str) -> List[Token]:
        return get_tokens_from_regex(self.config.time_of_day_regex, source)

class TimePeriodParserConfiguration:
    pass

class BaseTimePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEPERIOD

    def __init__(self, config: TimePeriodParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass

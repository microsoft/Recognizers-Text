from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime
import regex

from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, DateTimeResolutionResult, DateTimeUtilityConfiguration, AgoLaterUtil

class DateTimeExtractorConfiguration:
    @property
    @abstractmethod
    def date_point_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_point_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def now_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_today_after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_time_of_today_after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def night_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_today_before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_time_of_today_before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def the_end_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

    @abstractmethod
    def is_connector_token(self, source: str) -> bool:
        raise NotImplementedError

class BaseDateTimeExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: DateTimeExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.merge_date_and_time(source, reference))
        tokens.extend(self.basic_regex_match(source))
        tokens.extend(self.time_of_today_before(source, reference))
        tokens.extend(self.time_of_today_after(source, reference))
        tokens.extend(self.special_time_of_date(source, reference))
        tokens.extend(self.duration_with_before_and_after(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def merge_date_and_time(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers: List[ExtractResult] = self.config.date_point_extractor.extract(source, reference)
        if not ers:
            return tokens
        ers.extend(self.config.time_point_extractor.extract(source, reference))
        if len(ers) < 2:
            return tokens
        ers = sorted(ers, key=lambda x: x.start)
        i=0
        while i < len(ers)-1:
            j = i+1
            while j < len(ers) and ers[i].overlap(ers[j]):
                j += 1
            if j >= len(ers):
                break
            if ((ers[i].type is Constants.SYS_DATETIME_DATE and ers[j].type is Constants.SYS_DATETIME_TIME) or
                (ers[i].type is Constants.SYS_DATETIME_TIME and ers[j].type is Constants.SYS_DATETIME_DATE)):
                middle_begin = ers[i].start + ers[i].length
                middle_end = ers[j].start
                if middle_begin > middle_end:
                    i = j + 1
                    continue
                
                middle = source[middle_begin:middle_end].strip().lower()
                if self.config.is_connector_token(middle):
                    begin = ers[i].start
                    end = ers[j].start + ers[j].length
                    tokens.append(Token(begin, end))
                    i = j + 1
                    continue
            i = j
        
        tokens = list(map(lambda x: self.verify_end_token(source, x), tokens))
        return tokens

    def verify_end_token(self, source: str, token: Token) -> Token:
        after_str = source[token.end:]
        match = regex.search(self.config.suffix_regex, after_str)
        if match is not None:
            token.end += len(match.group())
        return token

    def basic_regex_match(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        matches: List[Match] = list(regex.finditer(self.config.now_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    def time_of_today_before(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.time_point_extractor.extract(source, reference)
        for er in ers:
            before = source[:er.start]
            inner_match = regex.search(self.config.night_regex, er.text)
            if inner_match is not None and inner_match.start() == 0:
                before = source[:er.start + len(inner_match.group())]
            if not before:
                continue
            match = regex.search(self.config.time_of_today_before_regex, before)
            if match is not None:
                begin = match.start()
                end = er.start + er.length
                tokens.append(Token(begin, end))
        matches: List[Match] = list(regex.finditer(self.config.simple_time_of_today_before_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    def time_of_today_after(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.time_point_extractor.extract(source, reference)
        for er in ers:
            after = source[er.start + er.length:]
            if not after:
                continue
            match = regex.search(self.config.time_of_today_after_regex, after)
            if match is not None:
                begin = er.start
                end = er.start + er.length + len(match.group())
                tokens.append(Token(begin, end))
        matches: List[Match] = list(regex.finditer(self.config.simple_time_of_today_after_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    def special_time_of_date(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.date_point_extractor.extract(source, reference)
        for er in ers:
            before = source[:er.start]
            before_match = regex.search(self.config.the_end_of_regex, before)
            if before_match is not None:
                tokens.append(Token(before_match.start(), er.start + er.length))
            else:
                after = source[er.start + er.length:]
                after_match = regex.search(self.config.the_end_of_regex, after)
                if after_match is not None:
                    tokens.append(Token(er.start, er.start + er.length + after_match.end()))
        return tokens

    def duration_with_before_and_after(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.duration_extractor.extract(source, reference)
        for er in ers:
            match = regex.search(self.config.unit_regex, er.text)
            if match is not None:
                tokens = AgoLaterUtil.extractor_duration_with_before_and_after(source, er, tokens, self.config.utility_configuration)
        return tokens

class DateTimeParserConfiguration:
    pass

class BaseDateTimeParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: DateTimeParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass

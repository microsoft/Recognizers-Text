from abc import abstractmethod
from typing import List, Optional, Pattern
from datetime import datetime
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_number.number.extractors import BaseNumberExtractor
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens

class DurationExtractorConfiguration:
    @property
    @abstractmethod
    def all_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def half_regex(self) -> Pattern:
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
    def an_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def in_exact_number_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_and_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_duration_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

class BaseDurationExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DURATION

    def __init__(self, config: DurationExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens = self.number_with_unit(source)
        tokens.extend(self.number_with_unit_and_suffix(source, tokens))
        tokens.extend(self.implicit_duration(source))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def number_with_unit(self, source: str) -> List[Token]:
        ers: List[ExtractResult] = self.config.cardinal_extractor.extract(source)
        result: List[Token] = list(filter(None, map(lambda x: self.__cardinal_to_token(x, source), ers)))
        result.extend(self.get_tokens_from_regex(self.config.number_combined_with_unit, source))
        result.extend(self.get_tokens_from_regex(self.config.an_unit_regex, source))
        result.extend(self.get_tokens_from_regex(self.config.in_exact_number_unit_regex, source))
        return result

    def number_with_unit_and_suffix(self, source: str, tokens: List[Token]) -> List[Token]:
        result: List[Token] = list(filter(None, map(lambda x: self.__base_to_token(x, source), tokens)))
        return result

    def implicit_duration(self, source: str) -> List[Token]:
        result: List[Token] = self.get_tokens_from_regex(self.config.all_regex, source)
        result.extend(self.get_tokens_from_regex(self.config.half_regex, source))
        result.extend(self.get_tokens_from_regex(self.config.relative_duration_unit_regex, source))
        return result

    def __cardinal_to_token(self, cardinal: ExtractResult, source: str) -> Optional[Token]:
        after = source[cardinal.start + cardinal.length:]
        match = regex.match(self.config.followed_unit, after)
        if match is not None:
            return Token(cardinal.start, cardinal.start + cardinal.length + len(match.group()))
        return None

    def __base_to_token(self, token: Token, source: str) -> Optional[Token]:
        after = source[token.start + token.length:]
        match = regex.match(self.config.followed_unit, after)
        if match is not None:
            return Token(token.start, token.start + token.length + len(match.group()))
        return None

    def get_tokens_from_regex(self, pattern: Pattern, source: str) -> List[Token]:
        return list(map(lambda x: Token(x.start(), x.end()), regex.finditer(pattern, source)))

class DurationParserConfiguration:
    pass

class BaseDurationParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DURATION

    def __init__(self, config: DurationParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass

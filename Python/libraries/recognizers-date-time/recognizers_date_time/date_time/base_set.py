from abc import ABC, abstractmethod
from typing import List, Optional, Pattern
from datetime import datetime
import regex

from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens

class SetExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def last_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def periodic_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def before_each_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def set_week_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def set_each_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

class BaseSetExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_SET

    def __init__(self, config: SetExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.match_each_unit(source))
        tokens.extend(self.match_periodic(source))
        # tokens.extend(self.match_each_duration(source, reference))
        # tokens.extend(self.time_everyday(source, reference))
        # tokens.extend(self.match_each(self.config.date_extractor, source, reference))
        # tokens.extend(self.match_each(self.config.time_extractor, source, reference))
        # tokens.extend(self.match_each(self.config.date_time_extractor, source, reference))
        # tokens.extend(self.match_each(self.config.date_period_extractor, source, reference))
        # tokens.extend(self.match_each(self.config.time_period_extractor, source, reference))
        # tokens.extend(self.match_each(self.config.date_time_period_extractor, source, reference))
        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def match_each_unit(self, source: str) -> List[Token]:
        for match in regex.finditer(self.config.each_unit_regex, source):
            yield Token(match.start(), match.end())

    def match_periodic(self, source: str) -> List[Token]:
        for match in regex.finditer(self.config.periodic_regex, source):
            yield Token(match.start(), match.end())


class SetParserConfiguration:
    pass

class BaseSetParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_SET

    def __init__(self, config: SetParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass

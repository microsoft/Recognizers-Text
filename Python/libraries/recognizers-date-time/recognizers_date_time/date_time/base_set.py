from abc import ABC, abstractmethod
from typing import List, Optional, Pattern
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult

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
        #TODO: code
        pass

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

from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import DateTimeUtilityConfiguration

class DateExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def date_regex_list(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def implicit_date_list(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_end(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def of_month(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def for_the_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_and_day_of_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_of_week(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def ordinal_extractor(self) -> BaseNumberExtractor:
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

    @property
    @abstractmethod
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

class BaseDateExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: DateExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        #TODO: code
        pass

class DateParserConfiguration:
    pass

class BaseDateParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: DateParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass

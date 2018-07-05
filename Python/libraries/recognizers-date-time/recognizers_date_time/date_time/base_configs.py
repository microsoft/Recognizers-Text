from typing import Dict
from abc import abstractmethod, ABC

from recognizers_number import BaseNumberExtractor, BaseNumberParser
from ..resources.base_date_time import BaseDateTime
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser
from .utilities import DateTimeUtilityConfiguration

class BaseDateParserConfiguration(ABC):
    @property
    @abstractmethod
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def ordinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self) -> BaseNumberParser:
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
    def duration_extractor(self) -> DateTimeExtractor:
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
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_year(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def numbers(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_value_map(self) -> Dict[str, int]:
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
    def cardinal_map(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    @abstractmethod
    def day_of_week(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def double_numbers(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

    def __init__(self):
        self._day_of_month = BaseDateTime.DayOfMonthDictionary

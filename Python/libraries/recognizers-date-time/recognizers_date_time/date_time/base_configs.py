from typing import Dict
from abc import abstractmethod, ABC

from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_date_time.date_time.base_datetime import BaseDateTimeExtractor, BaseDateTimeParser
from recognizers_date_time.date_time.base_date import BaseDateParser
from recognizers_date_time.date_time.base_time import BaseTimeParser
from recognizers_date_time.date_time.base_duration import BaseDurationParser
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodParser
from recognizers_date_time.date_time.base_timeperiod import BaseTimePeriodParser
from recognizers_date_time.date_time.base_datetimeperiod import BaseDateTimePeriodParser
from recognizers_date_time.date_time.utilities import DateTimeUtilityConfiguration

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
    def date_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> BaseDateParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_parser(self) -> BaseTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_parser(self) -> BaseDateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> BaseDurationParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_parser(self) -> BaseDatePeriodParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_parser(self) -> BaseTimePeriodParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_parser(self) -> BaseDateTimePeriodParser:
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
    @abstractmethod
    def day_of_month(self) -> Dict[str, int]:
        raise NotImplementedError

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

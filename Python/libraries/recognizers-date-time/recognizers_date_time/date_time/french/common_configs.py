from typing import Dict

from recognizers_number import BaseNumberExtractor, FrenchCardinalExtractor, FrenchIntegerExtractor, FrenchOrdinalExtractor, BaseNumberParser, FrenchNumberParserConfiguration

from ...resources import FrenchDateTime, BaseDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from ..base_date import BaseDateExtractor, BaseDateParser
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor, BaseDurationParser
from ..base_dateperiod import BaseDatePeriodExtractor, BaseDatePeriodParser
from ..base_timeperiod import BaseTimePeriodExtractor, BaseTimePeriodParser
from ..base_datetime import BaseDateTimeExtractor, BaseDateTimeParser
from ..base_datetimeperiod import BaseDateTimePeriodExtractor, BaseDateTimePeriodParser
from .base_configs import FrenchDateTimeUtilityConfiguration
from .duration_extractor_config import FrenchDurationExtractorConfiguration
from .date_extractor_config import FrenchDateExtractorConfiguration
from .time_extractor_config import FrenchTimeExtractorConfiguration
from .datetime_extractor_config import FrenchDateTimeExtractorConfiguration
from .dateperiod_extractor_config import FrenchDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import FrenchTimePeriodExtractorConfiguration
from .datetimeperiod_extractor_config import FrenchDateTimePeriodExtractorConfiguration
from .duration_parser_config import FrenchDurationParserConfiguration
from .date_parser_config import FrenchDateParserConfiguration
from .time_parser_config import FrenchTimeParserConfiguration
from .datetime_parser_config import FrenchDateTimeParserConfiguration
from .dateperiod_parser_config import FrenchDatePeriodParserConfiguration
from .timeperiod_parser_config import FrenchTimePeriodParserConfiguration
from .datetimeperiod_parser_config import FrenchDateTimePeriodParserConfiguration
from .parsers import FrenchTimeParser

class FrenchCommonDateTimeParserConfiguration(BaseDateParserConfiguration):
    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def date_period_extractor(self) -> DateTimeExtractor:
        return self._date_period_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def date_time_period_extractor(self) -> DateTimeExtractor:
        return self._date_time_period_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def date_period_parser(self) -> DateTimeParser:
        return self._date_period_parser

    @property
    def time_period_parser(self) -> DateTimeParser:
        return self._time_period_parser

    @property
    def date_time_period_parser(self) -> DateTimeParser:
        return self._date_time_period_parser

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def unit_value_map(self) -> Dict[str, int]:
        return self._unit_value_map

    @property
    def season_map(self) -> Dict[str, str]:
        return self._season_map

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def double_numbers(self) -> Dict[str, int]:
        return self._double_numbers

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        super().__init__()

        self._utility_configuration = FrenchDateTimeUtilityConfiguration()
        self._unit_map = FrenchDateTime.UnitMap
        self._unit_value_map = FrenchDateTime.UnitValueMap
        self._season_map = FrenchDateTime.SeasonMap
        self._cardinal_map = FrenchDateTime.CardinalMap
        self._day_of_week = FrenchDateTime.DayOfWeek
        self._month_of_year = FrenchDateTime.MonthOfYear
        self._numbers = FrenchDateTime.Numbers
        self._double_numbers = FrenchDateTime.DoubleNumbers
        self._cardinal_extractor = FrenchCardinalExtractor()
        self._integer_extractor = FrenchIntegerExtractor()
        self._ordinal_extractor = FrenchOrdinalExtractor()
        self._day_of_month = {**BaseDateTime.DayOfMonthDictionary, **FrenchDateTime.DayOfMonth}
        self._number_parser = BaseNumberParser(FrenchNumberParserConfiguration())
        self._date_extractor = BaseDateExtractor(FrenchDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(FrenchTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(FrenchDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(FrenchDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(FrenchTimePeriodExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(FrenchDateTimeExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(FrenchDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(FrenchDurationParserConfiguration(self))
        self._date_parser = BaseDateParser(FrenchDateParserConfiguration(self))
        self._time_parser = FrenchTimeParser(FrenchTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(FrenchDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(FrenchTimePeriodParserConfiguration(self))
        self._date_time_parser = BaseDateTimeParser(FrenchDateTimeParserConfiguration(self))
        self._date_time_period_parser = BaseDateTimePeriodParser(FrenchDateTimePeriodParserConfiguration(self))

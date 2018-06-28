from typing import Dict

from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_number.number.spanish.parsers import SpanishNumberParserConfiguration
from recognizers_number.number.spanish.extractors import SpanishCardinalExtractor, SpanishIntegerExtractor, SpanishOrdinalExtractor

from ...resources.spanish_date_time import SpanishDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from ..base_date import BaseDateExtractor, BaseDateParser
from ..base_time import BaseTimeExtractor, BaseTimeParser
from ..base_duration import BaseDurationExtractor, BaseDurationParser
from ..base_dateperiod import BaseDatePeriodExtractor, BaseDatePeriodParser
from ..base_timeperiod import BaseTimePeriodExtractor, BaseTimePeriodParser
from ..base_datetime import BaseDateTimeExtractor, BaseDateTimeParser
from ..base_datetimeperiod import BaseDateTimePeriodExtractor, BaseDateTimePeriodParser

from .base_configs import SpanishDateTimeUtilityConfiguration
from .date_extractor_config import SpanishDateExtractorConfiguration
from .date_parser_config import SpanishDateParserConfiguration
from .time_extractor_config import SpanishTimeExtractorConfiguration
from .time_parser_config import SpanishTimeParserConfiguration
from .duration_extractor_config import SpanishDurationExtractorConfiguration
from .duration_parser_config import SpanishDurationParserConfiguration
from .dateperiod_extractor_config import SpanishDatePeriodExtractorConfiguration
from .dateperiod_parser_config import SpanishDatePeriodParserConfiguration
from .timeperiod_extractor_config import SpanishTimePeriodExtractorConfiguration
from .timeperiod_parser_config import SpanishTimePeriodParserConfiguration
from .datetime_extractor_config import SpanishDateTimeExtractorConfiguration
from .datetime_parser_config import SpanishDateTimeParserConfiguration
from .datetimeperiod_extractor_config import SpanishDateTimePeriodExtractorConfiguration
from .datetimeperiod_parser_config import SpanishDateTimePeriodParserConfiguration

class SpanishCommonDateTimeParserConfiguration(BaseDateParserConfiguration):
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
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

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
        BaseDateParserConfiguration.__init__(self)

        self._utility_configuration = SpanishDateTimeUtilityConfiguration()

        self._unit_map = SpanishDateTime.UnitMap
        self._unit_value_map = SpanishDateTime.UnitValueMap
        self._season_map = SpanishDateTime.SeasonMap
        self._cardinal_map = SpanishDateTime.CardinalMap
        self._day_of_week = SpanishDateTime.DayOfWeek
        self._month_of_year = SpanishDateTime.MonthOfYear
        self._numbers = SpanishDateTime.Numbers
        self._double_numbers = SpanishDateTime.DoubleNumbers

        self._cardinal_extractor = SpanishCardinalExtractor()
        self._integer_extractor = SpanishIntegerExtractor()
        self._ordinal_extractor = SpanishOrdinalExtractor()

        self._number_parser = BaseNumberParser(SpanishNumberParserConfiguration())
        self._date_extractor = BaseDateExtractor(SpanishDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(SpanishTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(SpanishDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(SpanishDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(SpanishTimePeriodExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(SpanishDateTimeExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(SpanishDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(SpanishDurationParserConfiguration(self))
        self._date_parser = BaseDateParser(SpanishDateParserConfiguration(self))
        self._time_parser = BaseTimeParser(SpanishTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(SpanishDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(SpanishTimePeriodParserConfiguration(self))
        self._date_time_parser = BaseDateTimeParser(SpanishDateTimeParserConfiguration(self))
        self._date_time_period_parser = BaseDateTimePeriodParser(SpanishDateTimePeriodParserConfiguration(self))

from typing import Dict

from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from recognizers_number.number.english.extractors import EnglishCardinalExtractor, EnglishIntegerExtractor, EnglishOrdinalExtractor

from ...resources.english_date_time import BaseDateTime, EnglishDateTime
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

from .base_configs import EnglishDateTimeUtilityConfiguration
from .date_extractor_config import EnglishDateExtractorConfiguration
from .date_parser_config import EnglishDateParserConfiguration
from .time_extractor_config import EnglishTimeExtractorConfiguration
from .time_parser_config import EnglishTimeParserConfiguration
from .parsers import EnglishTimeParser
from .duration_extractor_config import EnglishDurationExtractorConfiguration
from .duration_parser_config import EnglishDurationParserConfiguration
from .dateperiod_extractor_config import EnglishDatePeriodExtractorConfiguration
from .dateperiod_parser_config import EnglishDatePeriodParserConfiguration
from .timeperiod_extractor_config import EnglishTimePeriodExtractorConfiguration
from .timeperiod_parser_config import EnglishTimePeriodParserConfiguration
from .datetime_extractor_config import EnglishDateTimeExtractorConfiguration
from .datetime_parser_config import EnglishDateTimeParserConfiguration
from .datetimeperiod_extractor_config import EnglishDateTimePeriodExtractorConfiguration
from .datetimeperiod_parser_config import EnglishDateTimePeriodParserConfiguration

class EnglishCommonDateTimeParserConfiguration(BaseDateParserConfiguration):
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
        self._utility_configuration = EnglishDateTimeUtilityConfiguration()
        self._unit_map = EnglishDateTime.UnitMap
        self._unit_value_map = EnglishDateTime.UnitValueMap
        self._season_map = EnglishDateTime.SeasonMap
        self._cardinal_map = EnglishDateTime.CardinalMap
        self._day_of_week = EnglishDateTime.DayOfWeek
        self._month_of_year = EnglishDateTime.MonthOfYear
        self._numbers = EnglishDateTime.Numbers
        self._double_numbers = EnglishDateTime.DoubleNumbers
        self._cardinal_extractor = EnglishCardinalExtractor()
        self._integer_extractor = EnglishIntegerExtractor()
        self._ordinal_extractor = EnglishOrdinalExtractor()
        self._day_of_month = {**BaseDateTime.DayOfMonthDictionary, **EnglishDateTime.DayOfMonth}
        self._number_parser = BaseNumberParser(EnglishNumberParserConfiguration())
        self._date_extractor = BaseDateExtractor(EnglishDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(EnglishTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(EnglishDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(EnglishDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(EnglishTimePeriodExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(EnglishDateTimeExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(EnglishDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(EnglishDurationParserConfiguration(self))
        self._date_parser = BaseDateParser(EnglishDateParserConfiguration(self))
        self._time_parser = EnglishTimeParser(EnglishTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(EnglishDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(EnglishTimePeriodParserConfiguration(self))
        self._date_time_parser = BaseDateTimeParser(EnglishDateTimeParserConfiguration(self))
        self._date_time_period_parser = BaseDateTimePeriodParser(EnglishDateTimePeriodParserConfiguration(self))

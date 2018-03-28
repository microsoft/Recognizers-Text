from typing import Dict

from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from recognizers_number.number.english.extractors import EnglishCardinalExtractor, EnglishIntegerExtractor, EnglishOrdinalExtractor
from recognizers_date_time.date_time.base_time import BaseTimeParser, BaseTimeExtractor
from recognizers_date_time.date_time.base_date import BaseDateParser, DateTimeUtilityConfiguration, BaseDateExtractor
from recognizers_date_time.date_time.base_datetime import BaseDateTimeParser, BaseDateTimeExtractor
from recognizers_date_time.date_time.base_configs import BaseDateParserConfiguration
from recognizers_date_time.date_time.base_duration import BaseDurationParser, BaseDurationExtractor
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodParser, BaseDatePeriodExtractor
from recognizers_date_time.date_time.base_timeperiod import BaseTimePeriodParser, BaseTimePeriodExtractor
from recognizers_date_time.date_time.base_datetimeperiod import BaseDateTimePeriodParser
from recognizers_date_time.date_time.english.base_configs import EnglishDateTimeUtilityConfiguration
from recognizers_date_time.date_time.english.date_extractor_config import EnglishDateExtractorConfiguration
from recognizers_date_time.date_time.english.time_extractor_config import EnglishTimeExtractorConfiguration
from recognizers_date_time.date_time.english.duration_extractor_config import EnglishDurationExtractorConfiguration
from recognizers_date_time.date_time.english.duration_parser_config import EnglishDurationParserConfiguration
from recognizers_date_time.date_time.english.dateperiod_extractor_config import EnglishDatePeriodExtractorConfiguration
from recognizers_date_time.date_time.english.timeperiod_extractor_config import EnglishTimePeriodExtractorConfiguration
from recognizers_date_time.resources.english_date_time import EnglishDateTime, BaseDateTime

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
    def date_extractor(self) -> BaseDateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> BaseDateTimeExtractor:
        return self._time_extractor

    @property
    def date_time_extractor(self) -> BaseDateTimeExtractor:
        return self._date_time_extractor

    @property
    def duration_extractor(self) -> BaseDateTimeExtractor:
        return self._duration_extractor

    @property
    def date_period_extractor(self) -> BaseDateTimeExtractor:
        return self._date_period_extractor

    @property
    def time_period_extractor(self) -> BaseDateTimeExtractor:
        return self._time_period_extractor

    @property
    def date_time_period_extractor(self) -> BaseDateTimeExtractor:
        return self._date_time_period_extractor

    @property
    def date_parser(self) -> BaseDateParser:
        return self._date_parser

    @property
    def time_parser(self) -> BaseTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> BaseDateTimeParser:
        return self._date_time_parser

    @property
    def duration_parser(self) -> BaseDurationParser:
        return self._duration_parser

    @property
    def date_period_parser(self) -> BaseDatePeriodParser:
        return self._date_period_parser

    @property
    def time_period_parser(self) -> BaseTimePeriodParser:
        return self._time_period_parser

    @property
    def date_time_period_parser(self) -> BaseDateTimePeriodParser:
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
        self._date_time_extractor = None #BaseDateTimeExtractor(EnglishDateTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(EnglishDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(EnglishDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(EnglishTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = None #BaseDateTimePeriodExtractor(EnglishDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(EnglishDurationParserConfiguration(self))
        self._date_parser = None #BaseDateParser(EnglishDateParserConfiguration(self))
        self._time_parser = None #EnglishTimeParser(EnglishTimeParserConfiguration(self))
        self._date_time_parser = None #BaseDateTimeParser(EnglishDateTimeParserConfiguration(self))
        self._date_period_parser = None #BaseDatePeriodParser(EnglishDatePeriodParserConfiguration(self))
        self._time_period_parser = None #BaseTimePeriodParser(EnglishTimePeriodParserConfiguration(self))
        self._date_time_period_parser = None #BaseDateTimePeriodParser(EnglishDateTimePeriodParserConfiguration(self))
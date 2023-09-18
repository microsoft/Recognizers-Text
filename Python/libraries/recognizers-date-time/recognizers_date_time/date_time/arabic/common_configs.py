from typing import Dict, Pattern

from recognizers_number import BaseNumberExtractor, ArabicCardinalExtractor, ArabicIntegerExtractor, \
    ArabicOrdinalExtractor, BaseNumberParser, ArabicNumberParserConfiguration

from recognizers_date_time.resources import ArabicDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from recognizers_date_time.date_time.base_date import BaseDateExtractor, BaseDateParser
from recognizers_date_time.date_time.base_time import BaseTimeExtractor
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodExtractor, BaseDatePeriodParser
from recognizers_date_time.date_time.base_timeperiod import BaseTimePeriodExtractor, BaseTimePeriodParser
from recognizers_date_time.date_time.base_timezone import BaseTimeZoneParser
from recognizers_date_time.date_time.arabic.base_configs import ArabicDateTimeUtilityConfiguration
from recognizers_date_time.date_time.arabic.date_extractor_config import ArabicDateExtractorConfiguration
from recognizers_date_time.date_time.arabic.time_extractor_config import ArabicTimeExtractorConfiguration
from recognizers_date_time.date_time.arabic.dateperiod_extractor_config import ArabicDatePeriodExtractorConfiguration
from recognizers_date_time.date_time.arabic.timeperiod_extractor_config import ArabicTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.arabic.date_parser_config import ArabicDateParserConfiguration
from recognizers_date_time.date_time.arabic.time_parser_config import ArabicTimeParserConfiguration
from recognizers_date_time.date_time.arabic.dateperiod_parser_config import ArabicDatePeriodParserConfiguration
from recognizers_date_time.date_time.arabic.timeperiod_parser_config import ArabicTimePeriodParserConfiguration
from recognizers_date_time.date_time.arabic.parsers import ArabicTimeParser


class ArabicCommonDateTimeParserConfiguration(BaseDateParserConfiguration):
    @property
    def time_zone_parser(self) -> DateTimeParser:
        return self._time_zone_parser

    @property
    def check_both_before_after(self) -> Pattern:
        return self._check_both_before_after

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
    def special_year_prefixes(self) -> Dict[str, str]:
        return self._special_year_prefixes_map

    @property
    def written_decades(self) -> Dict[str, int]:
        return self._written_decades

    @property
    def special_decade_cases(self) -> Dict[str, int]:
        return self._special_decade_cases

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        super().__init__()
        self._utility_configuration = ArabicDateTimeUtilityConfiguration()

        self._unit_value_map = ArabicDateTime.UnitValueMap
        self._season_map = ArabicDateTime.SeasonMap
        self._special_year_prefixes_map = ArabicDateTime.SpecialYearPrefixesMap
        self._cardinal_map = ArabicDateTime.CardinalMap
        self._day_of_week = ArabicDateTime.DayOfWeek
        self._month_of_year = ArabicDateTime.MonthOfYear
        self._numbers = ArabicDateTime.Numbers
        self._double_numbers = ArabicDateTime.DoubleNumbers
        self._written_decades = ArabicDateTime.WrittenDecades
        self._special_decade_cases = ArabicDateTime.SpecialDecadeCases

        self._check_both_before_after = ArabicDateTime.CheckBothBeforeAfter

        self._unit_map = ArabicDateTime.UnitMap
        self._time_zone_parser = BaseTimeZoneParser()
        self._utility_configuration = ArabicDateTimeUtilityConfiguration()

        self._cardinal_extractor = ArabicCardinalExtractor()
        self._integer_extractor = ArabicIntegerExtractor()
        self._ordinal_extractor = ArabicOrdinalExtractor()

        self._number_parser = BaseNumberParser(ArabicNumberParserConfiguration())

        #  Do not change order. The order of initialization can lead to side-effects
        self._date_extractor = BaseDateExtractor( ArabicDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(ArabicTimeExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(ArabicDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(ArabicTimePeriodExtractorConfiguration())

        self._date_parser = BaseDateParser(ArabicDateParserConfiguration(self))
        self._time_parser = ArabicTimeParser(ArabicTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(ArabicDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(ArabicTimePeriodParserConfiguration(self))

        # Set to None until supported
        self._duration_extractor = None
        self._date_time_extractor = None
        self._date_time_extractor = None
        self._date_time_parser = None
        self._date_time_period_parser = None
        self._date_time_period_extractor = None
        self._duration_parser = None

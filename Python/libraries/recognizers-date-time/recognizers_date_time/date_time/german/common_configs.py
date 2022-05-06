#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict, Pattern

from recognizers_number import BaseNumberExtractor, GermanCardinalExtractor, GermanIntegerExtractor, GermanOrdinalExtractor, BaseNumberParser, GermanNumberParserConfiguration

from ...resources import GermanDateTime, BaseDateTime
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
from ..base_timezone import BaseTimeZoneParser
from .base_configs import GermanDateTimeUtilityConfiguration
from .duration_extractor_config import GermanDurationExtractorConfiguration
from .date_extractor_config import GermanDateExtractorConfiguration
from .time_extractor_config import GermanTimeExtractorConfiguration
from .datetime_extractor_config import GermanDateTimeExtractorConfiguration
from .dateperiod_extractor_config import GermanDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import GermanTimePeriodExtractorConfiguration
from .datetimeperiod_extractor_config import GermanDateTimePeriodExtractorConfiguration
from .duration_parser_config import GermanDurationParserConfiguration
from .date_parser_config import GermanDateParserConfiguration
from .time_parser_config import GermanTimeParserConfiguration
from .datetime_parser_config import GermanDateTimeParserConfiguration
from .dateperiod_parser_config import GermanDatePeriodParserConfiguration
from .timeperiod_parser_config import GermanTimePeriodParserConfiguration
from .datetimeperiod_parser_config import GermanDateTimePeriodParserConfiguration
from .parsers import GermanTimeParser


class GermanCommonDateTimeParserConfiguration(BaseDateParserConfiguration):
    @property
    def time_zone_parser(self) -> DateTimeParser:
        self._time_zone_parser

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
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        super().__init__()
        self._time_zone_parser = BaseTimeZoneParser()
        self._utility_configuration = GermanDateTimeUtilityConfiguration()
        self._unit_map = GermanDateTime.UnitMap
        self._unit_value_map = GermanDateTime.UnitValueMap
        self._season_map = GermanDateTime.SeasonMap
        self._cardinal_map = GermanDateTime.CardinalMap
        self._day_of_week = GermanDateTime.DayOfWeek
        self._month_of_year = GermanDateTime.MonthOfYear
        self._numbers = GermanDateTime.Numbers
        self._double_numbers = GermanDateTime.DoubleNumbers
        self._check_both_before_after = GermanDateTime.CheckBothBeforeAfter

        self._cardinal_extractor = GermanCardinalExtractor()
        self._integer_extractor = GermanIntegerExtractor()
        self._ordinal_extractor = GermanOrdinalExtractor()

        self._day_of_month = {
            **BaseDateTime.DayOfMonthDictionary, **GermanDateTime.DayOfMonth}
        self._number_parser = BaseNumberParser(
            GermanNumberParserConfiguration())
        self._date_extractor = BaseDateExtractor(
            GermanDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            GermanTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            GermanDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            GermanDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            GermanTimePeriodExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            GermanDateTimeExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            GermanDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(
            GermanDurationParserConfiguration(self))
        self._date_parser = BaseDateParser(GermanDateParserConfiguration(self))
        self._time_parser = GermanTimeParser(
            GermanTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(
            GermanDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(
            GermanTimePeriodParserConfiguration(self))
        self._date_time_parser = BaseDateTimeParser(
            GermanDateTimeParserConfiguration(self))
        self._date_time_period_parser = BaseDateTimePeriodParser(
            GermanDateTimePeriodParserConfiguration(self))

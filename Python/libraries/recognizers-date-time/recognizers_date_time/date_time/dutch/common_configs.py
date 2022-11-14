#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict, Pattern

from recognizers_number import BaseNumberExtractor, DutchCardinalExtractor, DutchIntegerExtractor, \
    DutchOrdinalExtractor, BaseNumberParser, DutchNumberParserConfiguration

from ...resources import DutchDateTime, BaseDateTime
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
from .base_configs import DutchDateTimeUtilityConfiguration
from .duration_extractor_config import DutchDurationExtractorConfiguration
from .date_extractor_config import DutchDateExtractorConfiguration
from .time_extractor_config import DutchTimeExtractorConfiguration
from .datetime_extractor_config import DutchDateTimeExtractorConfiguration
from .dateperiod_extractor_config import DutchDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import DutchTimePeriodExtractorConfiguration
from .datetimeperiod_extractor_config import DutchDateTimePeriodExtractorConfiguration
from .duration_parser_config import DutchDurationParserConfiguration
from .date_parser_config import DutchDateParserConfiguration
from .time_parser_config import DutchTimeParserConfiguration
from .datetime_parser_config import DutchDateTimeParserConfiguration
from .dateperiod_parser_config import DutchDatePeriodParserConfiguration
from .timeperiod_parser_config import DutchTimePeriodParserConfiguration
from .datetimeperiod_parser_config import DutchDateTimePeriodParserConfiguration
from .parsers import DutchTimeParser


class DutchCommonDateTimeParserConfiguration(BaseDateParserConfiguration):
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
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        super().__init__()
        self._time_zone_parser = BaseTimeZoneParser()
        self._utility_configuration = DutchDateTimeUtilityConfiguration()
        self._unit_map = DutchDateTime.UnitMap
        self._unit_value_map = DutchDateTime.UnitValueMap
        self._season_map = DutchDateTime.SeasonMap
        self._cardinal_map = DutchDateTime.CardinalMap
        self._day_of_week = DutchDateTime.DayOfWeek
        self._month_of_year = DutchDateTime.MonthOfYear
        self._numbers = DutchDateTime.Numbers
        self._double_numbers = DutchDateTime.DoubleNumbers
        self._check_both_before_after = DutchDateTime.CheckBothBeforeAfter

        self._cardinal_extractor = DutchCardinalExtractor()
        self._integer_extractor = DutchIntegerExtractor()
        self._ordinal_extractor = DutchOrdinalExtractor()

        self._day_of_month = {
            **BaseDateTime.DayOfMonthDictionary, **DutchDateTime.DayOfMonth}
        self._number_parser = BaseNumberParser(
            DutchNumberParserConfiguration())
        self._date_extractor = BaseDateExtractor(
            DutchDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            DutchTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            DutchDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            DutchDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            DutchTimePeriodExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            DutchDateTimeExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            DutchDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(
            DutchDurationParserConfiguration(self))
        self._date_parser = BaseDateParser(DutchDateParserConfiguration(self))
        self._time_parser = DutchTimeParser(
            DutchTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(
            DutchDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(
            DutchTimePeriodParserConfiguration(self))
        self._date_time_parser = BaseDateTimeParser(
            DutchDateTimeParserConfiguration(self))
        self._date_time_period_parser = BaseDateTimePeriodParser(
            DutchDateTimePeriodParserConfiguration(self))
